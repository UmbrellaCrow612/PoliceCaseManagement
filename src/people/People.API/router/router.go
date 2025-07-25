package router

import (
	"log"
	"os"
	"people/api/middleware"
	"people/api/routes"

	"github.com/gin-gonic/gin"
)

func SetupRouter() *gin.Engine {
	r := gin.Default()

	secret := os.Getenv("JWT_SECRET")
	if secret == "" {
		log.Fatal("JWT JWT_SECRET missing")
	}

	issuer := os.Getenv("JWT_ISSUER")
	if issuer == "" {
		log.Fatal("JWT JWT_ISSUER missing")
	}

	audience := os.Getenv("JWT_AUDIENCE")
	if audience == "" {
		log.Fatal("JWT JWT_AUDIENCE missing")
	}

	r.Use(middleware.Jwt(secret, issuer, audience))

	routes.ResgisterPersonRoutes(r)

	return r
}
