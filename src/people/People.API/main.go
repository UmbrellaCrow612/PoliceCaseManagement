package main

import (
	"people/api/db"
	"people/api/router"
)

func main() {
	db.Init() 
	r := router.SetupRouter()

	r.RunTLS(":8080", "localhost.pem", "localhost-key.pem")
}
