﻿# Migrations


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
