# Grpc.JwtInterceptor

Helper util library that offers JWT support for grpc services written in c# and use GRPC

Offers Jwt bearer token forwarding in a request


Example usage change the Grpc client to match yours and add the AddInterceptor to the chain

```cs

builder.Service.AddGrpcJwtInterceptor();

builder.Services.AddGrpcClient<EvidenceService.EvidenceServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:7078");
}).AddInterceptor<JwtForwardInterceptor>(InterceptorScope.Client);
```