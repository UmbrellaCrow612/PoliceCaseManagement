package main

import (
	"log"
	"os"
	"people/api/db"
	"people/api/router"

	"github.com/joho/godotenv"
)

func main() {

	err := godotenv.Load()
	if err != nil {
		log.Fatal("Error loading .env file")
	}

	port := os.Getenv("PORT")
	if port == "" {
		port = ":8080"
		log.Println(".env PORT not found defaulting to :8080")
	}

	db.Init()
	
	r := router.SetupRouter()

	r.RunTLS(port, "localhost.pem", "localhost-key.pem")
}
