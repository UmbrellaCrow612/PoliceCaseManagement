using Cases.API;
using Cases.Application;
using Cases.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddBaseAuthorization(config);
builder.Services.AddCasesInfrastructure(config);
builder.Services.AddCasesApplication(config);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
