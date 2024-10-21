using PoliceCaseManagement.Application;
using PoliceCaseManagement.Application.DTOs.Cases;
using PoliceCaseManagement.Application.Interfaces;
using PoliceCaseManagement.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapPost("/cases", async (ICaseService caseService, CreateCaseDto caseDto) =>
{
    // Call the service to create a case
    var result = await caseService.CreateCaseAsync("1", caseDto);
    return Results.Created($"/cases", result);
});

app.Run();