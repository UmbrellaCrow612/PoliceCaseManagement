package handlers

import (
	"net/http"
	"people/api/services"

	"github.com/gin-gonic/gin"
	"gorm.io/gorm"
)

// Public: Handles the function to run on a specific people endpoint
type PersonHandler struct {
	service services.PersonService
}

func NewPersonHandler(service services.PersonService) *PersonHandler {
	return &PersonHandler{service: service}
}

func (h *PersonHandler) HandleGetPersonById(c *gin.Context) {
	personId := c.Param("personId")
	person, err := h.service.GetById(personId)

	if err != nil {
		if err == gorm.ErrRecordNotFound {
			c.JSON(http.StatusNotFound, gin.H{"error": "Person not found"})
		} else {
			c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		}
		return
	}

	c.JSON(http.StatusOK, person)
}


func(h *PersonHandler) HandleCreatePerson(c *gin.Context){

}

func(h *PersonHandler) HandlePutPerson(c *gin.Context){

}

func(h *PersonHandler) HandleDeletePerson(c *gin.Context){

}