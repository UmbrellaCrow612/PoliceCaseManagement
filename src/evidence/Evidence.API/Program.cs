using Cache.Redis;
using Evidence.API;
using Evidence.API.Grpc;
using Evidence.Application;
using Evidence.Infrastructure;
using Grpc.JwtInterceptor;
using Grpc.Net.ClientFactory;
using Logging;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using User.V1;
using Validator;

SerilogExtensions.ConfigureSerilog();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

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
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<Evidence.Infrastructure.Data.EvidenceApplicationDbContext>();
        db.Database.Migrate();
    }

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<GrpcEvidenceServiceImp>();

app.MapControllers();

app.Run();
