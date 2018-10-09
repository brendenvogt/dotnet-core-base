using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Infrastructure.Utilities;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.MemoryStorage;

namespace Jobs
{
    public class Startup
    {
        readonly ILogger _logger;
        readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("JobsLog");
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCommonDependencies(_configuration, _logger)
                    .AddHangfire(x => x.UseMemoryStorage(new MemoryStorageOptions { FetchNextJobTimeout = TimeSpan.FromDays(365 * 100) }));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            GlobalConfiguration.Configuration.UseMongoStorage(_configuration["ConnectionString:Mongo"], _configuration["System:DefaultDatabase"]);

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            //queue
            //BackgroundJob.Enqueue<QueueProcessJob<ImageAssetProcessResponse>>(a => a.Run(null, JobCancellationToken.Null));

            //recurring
            //RecurringJob.AddOrUpdate<SampleJob>(a => a.Run(), Cron.Minutely());

            //background
            //BackgroundJob.Schedule<MongoDbInitializer>(a => a.Initialize(), TimeSpan.FromDays(365 * 360));

        }
    }
}
