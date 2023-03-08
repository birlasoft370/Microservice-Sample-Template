// Copyright © CompanyName. All Rights Reserved.
using Example.Repository.Example;
using Example.RepositoryHandler.MsSql.Dapper.Example;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.RepositoryHandler.MsSql.Dapper.CoreSql
{
    public static class RepositoryDapperSqlServiceRegistration
    {
        public static IServiceCollection AddDapperRepositoryeServices(this IServiceCollection services, IConfiguration configuration)
        {
            DapperRetryExtension.Settings = new DapperRetryExtensionSettings()
            {
                RetryAttempts = Convert.ToInt32(configuration.GetSection("Resilience:MaxRetryCount")?.Value ?? "3"),
                TimeOut = Convert.ToInt32(configuration.GetSection("Resilience:MaxRetryDelay")?.Value ?? "30")
            };

            services.AddScoped<IExampleOperations, ExampleOperations>();
           // services.AddScoped<IExampleOperatons, ExampleOperations>();

            return services;
        }
    }
}
