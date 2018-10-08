using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

using Infrastructure.Utilities;


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

            services.AddMvc();

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


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
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
