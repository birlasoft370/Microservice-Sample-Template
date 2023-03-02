using Example.Repository;
using Example.Repository.Example;
using Example.RepositoryHandler.MsSql.EF.Example;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.RepositoryHandler.MsSql.EF.CoreSql
{
    public static class RepositoryeServiceRegistration
    {
        public static IServiceCollection AddSqlEfRepositoryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("SqlConnectionString"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: Convert.ToInt32(configuration.GetSection("Resilience:MaxRetryCount").Value ?? "3"),
                    maxRetryDelay: TimeSpan.FromSeconds(Convert.ToInt32(configuration.GetSection("Resilience:MaxRetryDelay").Value ?? "20")),
                    errorNumbersToAdd: null);
                }));

            services.AddScoped(typeof(IAsyncRepositoryOperations<>), typeof(BaseRepositoryOperations<>));
            services.AddScoped<IExampleOperations, ExampleOperations>();

            return services;
        }
    }
}
