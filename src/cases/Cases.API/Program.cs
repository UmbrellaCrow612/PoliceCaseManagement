using Cache.Redis;
using Cases.API;
using Cases.Application;
using Cases.Infrastructure;
using Evidence.V1;
using Grpc.JwtInterceptor;
using Grpc.Net.ClientFactory;
using Logging;
using Person.V1;
using Scalar.AspNetCore;
using Serilog;
using User.V1;
using Validator;

SerilogExtensions.ConfigureSerilog();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

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
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
