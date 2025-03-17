README.md

# To run app:

- Open terminal in folder `Identity.API`
- run `dotnet run watch`
- go to URL defined and `/swagger/index.html` for swagger UI


Problems and solutions:

Thread used exeption:
It could be there is a process using that thread most likley the app running there and you try to 
run the cmd again:
- Run `netstat -ano | findstr :5185`

and if it lists it out kill it `taskkill /PID 3596 /F`

then rerun the start cmd