# Running 

- Have go installed
- have `mkcert` installed
- For first time run `mkcert localhost`
- Then downloa deps `go mod tidy`
- Then run the API with `go run main.go`



# Generating proto files

Run from `People.API`

make sure that `gen` folder exists if not make it

```bash
protoc --proto_path=../../proto --go_out=gen --go_opt=paths=source_relative --go-grpc_out=gen --go-grpc_opt=paths=source_relative common/person.proto
```

# Style guide

- Make `repositories` for model db access 
- Make `services` for each model that handles it's buisness logic
- Make a `handler` for each group of related API endpoints
- Follow a `DI` style code strcuture 
- Code intlisense comment each function and field by first stating it's public or private status by either being a first capital letter or not
then useful information about it or just if it is public or private
- Use error codes in error - have a constats of error message for certain business logic operation and be checked when propergated up and code comment what can be produced
only write codes for specific conditions in business logic that can happen for example a user trys to send a sms two factor auth but they havent confirmed there phone number 
there would be a error coder for this etc
```go
var (
	// ErrRecordNotFound record not found error
	ErrRecordNotFound = logger.ErrRecordNotFound
)

if err == ErrRecordNotFound 
```

# Testing 


## GRPC 

- Use postman 
- `New` `gRPC`
- Import proto file for the server to test
- Invoke the method on the endpoint to test