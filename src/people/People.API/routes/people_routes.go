package routes

import (
	"people/api/handlers"

	"github.com/gin-gonic/gin"
)

// Public: Adds all people related routes and there corsponding handlers
func AddPeopleRoutes(r *gin.Engine, h *handlers.PersonHandler) {
	people := r.Group("/people")
	people.POST("", h.HandleCreatePerson)
	people.POST("/phone-numbers/is-taken", h.HandleIsPhoneNumberTaken)
	people.POST("/emails/is-taken", h.HandleIsEmailTaken)
	people.GET("/:personId", h.HandleGetPersonById)
	people.GET("/search", h.HandleSearchPeople)
	people.PUT("/:personId", h.HandlePutPerson)
	people.DELETE("/:personId", h.HandleDeletePerson)
}
