// Copyright © CompanyName. All Rights Reserved.
using Example.CommandHandler.Example;
using Example.QueryHandler.Example;
using Example.QueryHandler.Examples;
using Example.Repository.Example;
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

            services.AddMediatR(option => option.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddAutoMapper(typeof(GetExampleByIdQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(option => option.RegisterServicesFromAssemblies(typeof(GetExampleByIdQueryHandler).GetTypeInfo().Assembly));

            services.AddAutoMapper(typeof(AddExampleCommandHandler).GetTypeInfo().Assembly); 
            services.AddMediatR(option => option.RegisterServicesFromAssemblies(typeof(AddExampleCommandHandler).GetTypeInfo().Assembly));

            services.AddTransient<IGetExamples, GetExamples>();
            services.AddTransient<IGetExampleById, GetExampleById>();
            services.AddTransient<IAddExample, AddExample>();
            services.AddTransient<IUpdateExample, UpdateExample>();
            services.AddTransient<IDeleteExample, DeleteExample>();
            services.AddTransient<IAddUpdateExample, AddUpdateExample>();
            return services;
        }
    }
}