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