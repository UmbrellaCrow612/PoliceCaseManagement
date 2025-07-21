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

This is done when a core model fields change meaning it's DB fields needs to change as well so we need to migrate

# Building 

- If another internal class library has been removed or added the `docker` file needs to be modified; 
use visual studio built in docker file maker to build the file for you, right click on the `API` project and then `Add` then `container support`,
this wil make a docker file to build a docker img of the API
