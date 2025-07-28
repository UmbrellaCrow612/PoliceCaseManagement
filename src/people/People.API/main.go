package main

import (
	"log"
	"net"
	"os"
	"people/api/db/repositories"
	personv1 "people/api/gen/common"
	grpc_servers "people/api/grpc"
	"people/api/handlers"
	"people/api/middleware"
	"people/api/models"
	"people/api/services"

	"github.com/gin-gonic/gin"
	"github.com/joho/godotenv"
	"google.golang.org/grpc"
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

	tcp_port := os.Getenv("TCP_PORT")
	if tcp_port == "" {
		log.Println("GRPC tcp port missing defaulting to 50051")
		tcp_port = "50051"
	}

	db, err := gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		log.Fatalf("Failed to connect to DB: %v", err)
	}

	if err := db.AutoMigrate(&models.Person{}); err != nil {
		log.Fatalf("AutoMigrate failed: %v", err)
	}

	// DI and services
	personRepo := repositories.NewPersonRepository(db)
	personService := services.NewPersonService(personRepo)
	personHandler := handlers.NewPersonHandler(personService)

	go func() {
		lis, err := net.Listen("tcp", ": "+tcp_port)
		if err != nil {
			log.Fatalf("failed to listen: %v", err)
		}

		grpcServer := grpc.NewServer()
		personv1.RegisterPersonServiceServer(grpcServer, grpc_servers.NewPersonServiceServer(*personService))

		log.Println("gRPC server listening on" + tcp_port)
		if err := grpcServer.Serve(lis); err != nil {
			log.Fatalf("failed to serve: %v", err)
		}
	}()

	router := gin.Default()

	router.Use(middleware.Jwt(jwt_secret, jwt_issuer, jwt_aud))

	people := router.Group("/people")
	people.POST("/", personHandler.HandleCreatePerson)
	people.GET("/:personId", personHandler.HandleGetPersonById)
	people.PUT("/:personId", personHandler.HandlePutPerson)
	people.DELETE("/:personId", personHandler.HandleDeletePerson)

	router.Run(":" + port)
}
