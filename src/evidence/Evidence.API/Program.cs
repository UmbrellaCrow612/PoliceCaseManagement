using Caching;
using Evidence.API;
using Evidence.API.Grpc;
using Evidence.Application;
using Evidence.Infrastructure;
using Scalar.AspNetCore;
using Validator;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddInfrastructure(config);
builder.Services.AddApplication(config);
builder.Services.AddBaseAuthorization(config);
builder.Services.AddCaching(config);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddGrpc();

builder.Services.AddValidators();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<GrpcEvidenceServiceImp>();

app.MapControllers();

app.Run();
