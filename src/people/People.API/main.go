package main

import (
	"log"
	"net"
	"people/api/db/repositories"
	personv1 "people/api/gen/common"
	grpcimpl "people/api/grpc_impl"
	"people/api/handlers"
	internalutils "people/api/internal_utils"
	"people/api/middleware"
	"people/api/models"
	"people/api/services"

	"github.com/gin-gonic/gin"
	"google.golang.org/grpc"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

func main() {
	config, err := internalutils.InitConfig()
	if err != nil {
		log.Fatal(err.Error())
	}

	db, err := gorm.Open(postgres.Open(config.DSN), &gorm.Config{})
	if err != nil {
		log.Fatalf("Failed to connect to DB: %v", err)
	}

	if err := db.AutoMigrate(&models.Person{}); err != nil {
		log.Fatalf("AutoMigrate failed: %v", err)
	}

	personRepo := repositories.NewPersonRepository(db)
	personService := services.NewPersonService(personRepo)
	personHandler := handlers.NewPersonHandler(personService)

	go func() {
		lis, err := net.Listen("tcp", ":"+config.TcpPort)
		if err != nil {
			log.Fatalf("failed to listen: %v", err)
		}

		grpcServer := grpc.NewServer()

		personGrpcServerImpla := grpcimpl.NewPersonServerImpl(personService)

		personv1.RegisterPersonServiceServer(grpcServer, personGrpcServerImpla)

		log.Println("gRPC server listening on " + config.TcpPort)
		if err := grpcServer.Serve(lis); err != nil {
			log.Fatalf("failed to serve: %v", err)
		}
	}()

	router := gin.Default()

	router.Use(middleware.Jwt(config.JwtSecret, config.JwtIssuer, config.JwtAudience))

	people := router.Group("/people")
	people.POST("/", personHandler.HandleCreatePerson)
	people.GET("/:personId", personHandler.HandleGetPersonById)
	people.PUT("/:personId", personHandler.HandlePutPerson)
	people.DELETE("/:personId", personHandler.HandleDeletePerson)

	router.Run(":" + config.Port)
}
