package handlers

import (
	"net/http"
	"net/mail"
	apperrors "people/api/app_errors"
	"people/api/models"
	"people/api/services"
	valueobjects "people/api/value_objects"
	"time"

	"github.com/gin-gonic/gin"
	"gorm.io/gorm"
)

// Private: create person dto
type createPersonDto struct {
	FirstName   string    `json:"firstName" binding:"required"`
	LastName    string    `json:"lastName" binding:"required"`
	DateOfBirth time.Time `json:"dateOfBirth" binding:"required"`
	PhoneNumber string    `json:"phoneNumber" binding:"required"`
	Email       string    `json:"email" binding:"required"`
}

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

func (h *PersonHandler) HandleCreatePerson(c *gin.Context) {
	var req createPersonDto

	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	_, err := mail.ParseAddress(req.Email)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": apperrors.ErrInvalidEmail.Error()})
		return
	}

	person := models.Person{
		FirstName:   req.FirstName,
		LastName:    req.LastName,
		DateOfBirth: req.DateOfBirth,
		PhoneNumber: req.PhoneNumber,
		Email:       req.Email,
	}

	err = h.service.Create(&person)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	c.JSON(http.StatusCreated, person)
}

func (h *PersonHandler) HandlePutPerson(c *gin.Context) {

}

func (h *PersonHandler) HandleDeletePerson(c *gin.Context) {

}

func (h *PersonHandler) HandleSearchPeople(c *gin.Context) {
	var req valueobjects.SearchPersonQuery

	if err := c.ShouldBindQuery(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	result, err := h.service.Search(&req)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	c.JSON(http.StatusOK, result)
}