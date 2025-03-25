README.md

# To run app:

- Open terminal in folder `Identity.API`
- run `dotnet run --launch-profile https`
- go to URL defined and `/swagger/index.html` for swagger UI for example `https://localhost:7058/swagger/index.html`


Problems and solutions:

Thread used exeption:
It could be there is a process using that thread most likley the app running there and you try to 
run the cmd again:
- Run `netstat -ano | findstr :5185`

and if it lists it out kill it `taskkill /PID 3596 /F`

then rerun the start cmd


# Migrations

From the `Identity.API` project in terminal 

```bash
Project/Identity.API> dotnet ef migrations add InitialCreate --project ../Identity.Infrastructure --startup-project .
```

```
dotnet ed database update
```