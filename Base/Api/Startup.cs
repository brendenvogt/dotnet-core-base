using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using FluentValidation.AspNetCore;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Newtonsoft.Json;

using Auth;
using Api.Validators;
using Infrastructure.Utilities;
using Core.Utilities;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            //configuration
            Configuration = configuration;

            //logging
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            _logger = CreateLoggger(loggerFactory);
        }

        public IConfiguration Configuration { get; }
        readonly ILogger _logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCommonDependencies(Configuration, _logger);

            services.AddCors()
                .AddMemoryCache()
                .AddMvc(o => o.Filters.Add(new ValidationFilter()))
                .AddJsonOptions(o =>
                {
                    o.SerializerSettings.ContractResolver = new ExcludePropertyNameResolver(new[] { "PasswordHash", "UserId" });
                    o.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                })
                .AddFluentValidation(v => v.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.UseAuthentication(CreateAuthenticationOptions());
            services.AddAuthorization(o => o.AddPolicy("Admin", policy => policy.RequireRole("Admin")));
            services.AddAuthorization(o => o.AddPolicy("Manager", policy => policy.RequireRole("Manager")));
            services.AddAuthorization(o => o.AddPolicy("CustomerService", policy => policy.RequireRole("CustomerService")));

            if (Configuration["EnableSwagger"] == "true")
                services.AddSwaggerGen(CreateSwaggerSetup());
        }

        Action<SwaggerGenOptions> CreateSwaggerSetup()
        {
            return c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "API", Version = "v1" });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "api.xml");
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            };
        }

        ILogger CreateLoggger(ILoggerFactory loggerFactory)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.LiterateConsole()
                .CreateLogger();
            loggerFactory.AddSerilog();

            return loggerFactory.CreateLogger<Startup>();
        }

        AuthOptions CreateAuthenticationOptions()
        {
            var options = new AuthOptions
            {
                SecretKey = Configuration["Security:SecretKey"],
                EncryptionKey = Configuration["Security:EncryptionKey"]
            };
            return options;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)//, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
                .UseStaticFiles()
                .UseAuthentication()
                .UseMvcWithDefaultRoute();

            if (Configuration["EnableSwagger"] == "true")
                app.UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API"); });
        }
    }
}
