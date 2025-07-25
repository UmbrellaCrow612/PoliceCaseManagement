package routes

import (
	"people/api/controllers"

	"github.com/gin-gonic/gin"
)

type SearchPeopleParams struct {
	FirstName string `form:"firstName" binding:"required"`
}

func ResgisterPersonRoutes(r *gin.Engine) {
	personRoutes := r.Group("/people")
	{
		personRoutes.GET("/:personId", controllers.GetPersonById)
	}
}
