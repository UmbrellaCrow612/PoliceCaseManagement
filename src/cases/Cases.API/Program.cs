using Cases.API;
using Cases.Application;
using Cases.Infrastructure;
using CORS;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddApplicationCors(config);
builder.Services.AddBaseAuthorization(config);
builder.Services.AddCasesInfrastructure(config);
builder.Services.AddCasesApplication(config);

builder.Services.AddValidators();

var app = builder.Build();

app.UseApplicationCors(config);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
