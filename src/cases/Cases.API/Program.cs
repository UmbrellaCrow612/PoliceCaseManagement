using Cache.Redis;
using Cases.API;
using Cases.Application;
using Cases.Infrastructure;
using Evidence.V1;
using Grpc.JwtInterceptor;
using Grpc.Net.ClientFactory;
using Microsoft.EntityFrameworkCore;
using Person.V1;
using Scalar.AspNetCore;
using User.V1;
using Validator;
using Serilog;
using SharedLogging;

var builder = WebApplication.CreateBuilder(args);

LoggingConfigurator.ConfigureLogging(builder);

var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddBaseAuthorization(config);
builder.Services.AddCasesInfrastructure(config);
builder.Services.AddCasesApplication(config);
builder.Services.AddRedis(config);

builder.Services.AddGrpcJwtInterceptor();
builder.Services.AddGrpcClient<EvidenceService.EvidenceServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:7078");
}).AddInterceptor<JwtForwardInterceptor>(InterceptorScope.Client);

builder.Services.AddGrpcClient<UserService.UserServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:7058");
}).AddInterceptor<JwtForwardInterceptor>(InterceptorScope.Client);

builder.Services.AddGrpcClient<PersonService.PersonServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:8084");
}).AddInterceptor<JwtForwardInterceptor>(InterceptorScope.Client);

builder.Services.AddValidators();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<Cases.Infrastructure.Data.CasesApplicationDbContext>();
        db.Database.Migrate();
    }

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
