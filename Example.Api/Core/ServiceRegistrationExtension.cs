// Copyright © CompanyName. All Rights Reserved.
using Example.QueryHandler.Examples;
using Example.Repository.Examples;
using Example.RepositoryHandler.MsSql.EF.Example;
using System.Reflection;

namespace Example.Api.Core
{
    public static class ServiceRegistrationExtension
    {
        public static IServiceCollection ServiceCollectionExtension(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(GetExamplesQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(option => option.RegisterServicesFromAssemblies(typeof(GetExamplesQueryHandler).GetTypeInfo().Assembly));
            //services.AddMediatR(option => option.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddTransient<IGetExamples, GetExamples>();
            return services;
        }
    }
}