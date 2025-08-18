package internalutils

import (
	"errors"
	"log"
	"os"

	"github.com/joho/godotenv"
)

// Public: Contains .env config data
type Config struct {

	// Public: The port that the HTTP server will run on will be in the format of example `8080`
	Port string

	// Public: Contains the JWT issuer claim
	JwtIssuer string

	// Public: Contains the JWT secret key to decode tokens and validate them
	JwtSecret string

	// Public: Contains the JWT audience claim
	JwtAudience string

	// Public: Contains the database connection string
	DSN string

	// Public: Contains the TCP port to run the gRPC server on
	TcpPort string

	// Public: Contaisn the RabbitMq connection string - the connection string has the user and password within it
	RabbitMqConnection string
}

// Public - Initialize all config values
func InitConfig() (*Config, error) {
	config := Config{}

	err := godotenv.Load()
	if err != nil {
		log.Fatal("error loading .env file")
	}

	port := os.Getenv("PORT")
	if port == "" {
		port = "8080"
		log.Println("port not found; defaulting to 8080")
	}
	config.Port = port

	jwtIssuer := os.Getenv("JWT_ISSUER")
	if jwtIssuer == "" {
		return &config, errors.New("failed to load jwt issuer from .env")
	}
	config.JwtIssuer = jwtIssuer

	jwtSecret := os.Getenv("JWT_SECRET")
	if jwtSecret == "" {
		return &config, errors.New("failed to load jwt secret from .env")
	}
	config.JwtSecret = jwtSecret

	jwtAudience := os.Getenv("JWT_AUDIENCE")
	if jwtAudience == "" {
		return &config, errors.New("failed to load jwt audience from .env")
	}
	config.JwtAudience = jwtAudience

	dsn := os.Getenv("DB_CONNECTION")
	if dsn == "" {
		return &config, errors.New("failed to load database connection string from .env")
	}
	config.DSN = dsn

	tcpPort := os.Getenv("TCP_PORT")
	if tcpPort == "" {
		log.Println("GRPC tcp port missing defaulting to 50051")
		tcpPort = "50051"
	}
	config.TcpPort = tcpPort

	rabbitMqConnection := os.Getenv("RABBITMQ_CONNECTION")
	if rabbitMqConnection == "" {
		return &config, errors.New("failed to load rabbit mq connection from .env")
	}
	config.RabbitMqConnection = rabbitMqConnection

	return &config, nil
}
