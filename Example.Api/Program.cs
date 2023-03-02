using Example.Api.Core;
using Example.RepositoryHandler.MsSql.EF.CoreSql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSqlEfRepositoryServices(builder.Configuration);
builder.Services.ServiceCollectionExtension();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
