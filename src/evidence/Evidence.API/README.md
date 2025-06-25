# Evidence API 

# Migration and DB

Make sure you have `dotnet` installed as well as `ef` extra tool installed

Run these commands from the `Evidence.API` project to run EF core migrations for

Run migrations script 

```bash
dotnet ef migrations add InitialCreate --project ../Evidence.Infrastructure --startup-project .
```

and 

```bash
dotnet ef database update
```