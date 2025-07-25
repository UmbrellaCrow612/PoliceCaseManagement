package router

import (
	"people/api/routes"

	"github.com/gin-gonic/gin"
)

func SetupRouter() *gin.Engine {
	r := gin.Default()

	routes.RegisterUserRoutes(r)

	return r
}
