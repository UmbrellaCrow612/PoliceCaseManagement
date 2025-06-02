var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("https://localhost:4200") // Your Angular frontend origin
                  .WithMethods("POST", "GET", "OPTIONS", "PUT", "DELETE")
                  .WithHeaders("Content-Type", "X-Device-Fingerprint", "Authorization") // Allowed request headers
                  .WithExposedHeaders("Content-Disposition")
                  .AllowCredentials();
        });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.MapReverseProxy();

app.Run();