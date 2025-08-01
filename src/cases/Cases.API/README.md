# Migrations

```bash
dotnet tool install --global dotnet-ef
```

from cases API project
```bash
dotnet ef migrations add InitialCreate --project ../Cases.Infrastructure --startup-project .
```

Then:

```bash
dotnet ef database update
```


Running

- run docker for rabbit mq 
- run docker for redis

for migrations comment out `AddCahing` when making migrations as it will cause it to fail then uncomment when done


Style Guide:

- Each model has it's own service
- Each model has it's own controller


# GRPC Notes:

If a server or API exposes a implementation of a GRPC proto file i.e it's implementation for external clients it needs to validate the bearer token logic in the request i.e
use `[Authorize]` and also add JWT options to the DI when setting up JWT auth on the server example `AddJwtBearer` for that middleware to run.

If a server consumes a external client it needs to to forward the JWT bearer in the request to it, for example refer to adding interceptors to a GRPC client and ref file `JwtForwardInterceptor` for an example 

This can be different for diff languages for c# `Authorize` is built and forwarding is a bit simple refer to `JwtForwardInterceptor` and it's uses but the main idea stands