# Migrations


```bash
dotnet tool install --global dotnet-ef
```

from cases API project
```bash
dotnet ef migrations add InitialCreate --project ../Cases.Infrastructure --startup-project .
```


Running

- run docker for rabbit mq 
- run docker for redis