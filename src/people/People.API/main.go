package main

import (
	"log"
	"os"
	"people/api/db/repositories"
	"people/api/handlers"
	"people/api/middleware"
	"people/api/models"
	"people/api/services"

	"github.com/gin-gonic/gin"
	"github.com/joho/godotenv"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

func main() {

	err := godotenv.Load()
	if err != nil {
		log.Fatal("Error loading .env file")
	}

	port := os.Getenv("PORT")
	if port == "" {
		port = "8080"
		log.Println("PORT not found; defaulting to 8080")
	}

	jwt_issuer := os.Getenv("JWT_ISSUER")
	if jwt_issuer == "" {
		log.Fatal("JWT issuer missing")
	}

	jwt_secret := os.Getenv("JWT_SECRET")
	if jwt_secret == "" {
		log.Fatal("JWT secret missing")
	}

	jwt_aud := os.Getenv("JWT_AUDIENCE")
	if jwt_aud == "" {
		log.Fatal("JWT aud missing")
	}

	dsn := os.Getenv("DB_CONNECTION")
	if dsn == "" {
		log.Fatal(".env DB_CONNECTION not found")
	}

	db, err := gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		log.Fatalf("Failed to connect to DB: %v", err)
	}

	if err := db.AutoMigrate(&models.Person{}); err != nil {
		log.Fatalf("AutoMigrate failed: %v", err)
	}

	personRepo := repositories.NewPersonRepository(db)
	personService := services.NewPersonService(personRepo)
	personHandler := handlers.NewPersonHandler(personService)

	router := gin.Default()

	router.Use(middleware.Jwt(jwt_secret, jwt_issuer, jwt_aud))

	router.GET("/people/:personId", personHandler.HandleGetPersonById)

	router.Run(":" + port)
}
