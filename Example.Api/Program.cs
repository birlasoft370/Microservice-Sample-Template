using Example.Api;
using Example.Api.Core;
using Example.Common.Exceptions;
using Example.RepositoryHandler.MsSql.Dapper.CoreSql;
using Example.RepositoryHandler.MsSql.EF.CoreSql;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDapperRepositoryeServices(builder.Configuration);
builder.Services.AddSqlEfRepositoryServices(builder.Configuration);
builder.Services.ServiceCollectionExtension();

builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
    .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//for Globally Hand Data annotation and application level validations
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = ctx => new ValidationProblemDetailsResultHandler();
});

var app = builder.Build();

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
