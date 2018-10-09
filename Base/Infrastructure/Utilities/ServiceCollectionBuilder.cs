using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Infrastructure.Data.Mappings;
using AutoMapper;
using MongoDB.Driver;

using Infrastructure.Data.Repositories;

namespace Infrastructure.Utilities
{
    public static class ServiceCollectionBuilder
    {
        public static IServiceCollection AddCommonDependencies(this IServiceCollection serviceCollection, IConfiguration configuration, ILogger logger)
        {
            serviceCollection
                .AddLogging()
                .AddConnections(configuration, logger)
                .AddServices()
                .AddModelEntityMapping()
                .AddRepoistories()
                .AddOptions();

            //.AddMemoryCache()

            return serviceCollection;

        }

        static IMappingExpression AddPatchMapping(this IMappingExpression expr)
        {
            expr.ForAllMembers(m => m.Condition((source, target, sourceValue, targetValue) => sourceValue != null));
            return expr;
        }

        static IServiceCollection AddModelEntityMapping(this IServiceCollection serviceCollection)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ModelEntityMapConfiguration());
            });

            var mapper = config.CreateMapper();
            return serviceCollection.AddSingleton(mapper);
        }

        static IServiceCollection AddConnections(this IServiceCollection serviceCollection, IConfiguration configuration, ILogger logger)
        {
            return serviceCollection
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<IMessageSerializer, JsonSerializer>()
                .AddTransient(a => logger)
                .AddTransient(a => configuration)
                .AddTransient(a =>
                {
                    return configuration["SSL_API:ValidateSSL"] == "false"
                        ? new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true })
                        : new HttpClient();
                })
                .AddTransient(a => CreateMongoDatabase(configuration, logger));
        }

        static IMongoDatabase CreateMongoDatabase(IConfiguration configuration, ILogger logger)
        {
            MongoDbMapping.RegisterMapping();
            var connectionString = configuration["ConnectionString:Mongo"];
            logger.LogDebug("Mongo connectionstring: {connectionString}", connectionString);
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(configuration["System:DefaultDatabase"]);
            return db;
        }

        static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<IUserService, UserService>()
                ;

        }

        static IServiceCollection AddRepoistories(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient(typeof(IMongoRepository<>), typeof(MongoRepository<>));
        }

        static T GetValue<T>(Dictionary<string, string> config, string key, T defaultValue = default(T))
        {
            if (config.ContainsKey(key))
                return ConvetValue(config[key], defaultValue);
            return defaultValue;
        }

        static T ConvetValue<T>(object value, T defaultValue)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
