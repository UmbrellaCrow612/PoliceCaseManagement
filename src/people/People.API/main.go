package main

import (
	"context"
	"log"
	"net"
	"people/api/db/repositories"
	personv1 "people/api/gen/person/v1"
	grpcimpl "people/api/grpc_impl"
	"people/api/handlers"
	internalutils "people/api/internal_utils"
	"people/api/middleware"
	grpcmiddleware "people/api/middleware/grpc"
	"people/api/models"
	"people/api/routes"
	"people/api/services"

	"github.com/gin-gonic/gin"
	"github.com/rabbitmq/amqp091-go"
	"google.golang.org/grpc"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
	"gorm.io/gorm/logger"
)

func main() {
	config, err := internalutils.InitConfig()
	if err != nil {
		log.Fatal(err.Error())
	}

	ctx := context.Background()

	conn, err := amqp091.Dial(config.RabbitMqConnection)
	if err != nil {
		log.Fatalf("Failed to connect to RabbitMQ: %v", err)
	}
	defer conn.Close()

	channel, err := conn.Channel()
	if err != nil {
		log.Fatalf("Failed to open channel: %v", err)
	}
	defer channel.Close()

	queue, err := channel.QueueDeclare(
		"people_events", // queue name
		true,            // durable
		false,           // delete when unused
		false,           // exclusive
		false,           // no-wait
		nil,             // args
	)

	if err != nil {
		log.Fatalf("Failed to declare queue: %v", err)
	}

	db, err := gorm.Open(postgres.Open(config.DSN), &gorm.Config{
		Logger: logger.Default.LogMode(logger.Info),
	})

	if err != nil {
		log.Fatalf("Failed to connect to DB: %v", err)
	}

	if err := db.AutoMigrate(&models.Person{}); err != nil {
		log.Fatalf("AutoMigrate failed: %v", err)
	}

	personRepo := repositories.NewPersonRepository(db, &ctx)
	personService := services.NewPersonService(personRepo, &queue, channel, &ctx)
	personHandler := handlers.NewPersonHandler(personService)

	go func() {
		lis, err := net.Listen("tcp", ":"+config.TcpPort)
		if err != nil {
			log.Fatalf("failed to listen: %v", err)
		}

		grpcServer := grpc.NewServer(grpc.ChainUnaryInterceptor(grpcmiddleware.LogGrpc, grpcmiddleware.JwtMiddleware(config.JwtSecret, config.JwtIssuer, config.JwtAudience)))

		personGrpcServerImpla := grpcimpl.NewPersonServerImpl(personService)

		personv1.RegisterPersonServiceServer(grpcServer, personGrpcServerImpla)

		log.Println("gRPC server listening on " + config.TcpPort)
		if err := grpcServer.Serve(lis); err != nil {
			log.Fatalf("failed to serve: %v", err)
		}
	}()

	router := gin.Default()

	router.Use(middleware.Jwt(config.JwtSecret, config.JwtIssuer, config.JwtAudience))

	routes.AddPeopleRoutes(router, personHandler)

	router.Run(":" + config.Port)
}
