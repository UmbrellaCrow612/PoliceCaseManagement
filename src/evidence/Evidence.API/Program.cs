using Cache.Redis;
using Evidence.API;
using Evidence.API.Grpc;
using Evidence.Application;
using Evidence.Infrastructure;
using Grpc.JwtInterceptor;
using Grpc.Net.ClientFactory;
using Scalar.AspNetCore;
using User.V1;
using Validator;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddInfrastructure(config);
builder.Services.AddApplication(config);
builder.Services.AddBaseAuthorization(config);
builder.Services.AddRedis(config);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddGrpc();

builder.Services.AddGrpcJwtInterceptor();
builder.Services.AddGrpcClient<UserService.UserServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:7058");
}).AddInterceptor<JwtForwardInterceptor>(InterceptorScope.Client);

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
