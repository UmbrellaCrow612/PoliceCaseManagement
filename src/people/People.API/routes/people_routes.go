package routes

import (
	"people/api/handlers"

	"github.com/gin-gonic/gin"
)

// Public: Adds all people related routes and there corsponding handlers
func AddPeopleRoutes(r *gin.Engine, h *handlers.PersonHandler) {
	people := r.Group("/people")
	people.POST("/", h.HandleCreatePerson)
	people.GET("/:personId", h.HandleGetPersonById)
	people.PUT("/:personId", h.HandlePutPerson)
	people.DELETE("/:personId", h.HandleDeletePerson)
}
