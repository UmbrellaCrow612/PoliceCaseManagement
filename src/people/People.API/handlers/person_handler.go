package handlers

import (
	"net/http"
	"net/mail"
	apperrors "people/api/app_errors"
	"people/api/dtos"
	internalutils "people/api/internal_utils"
	"people/api/models"
	"people/api/services"
	valueobjects "people/api/value_objects"
	"strings"

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

func (h *PersonHandler) HandleCreatePerson(c *gin.Context) {
	var req dtos.CreatePersonDto

	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	req.FirstName = strings.TrimSpace(req.FirstName)
	req.LastName = strings.TrimSpace(req.LastName)
	req.Email = strings.ToLower(strings.TrimSpace(req.Email))
	req.PhoneNumber = strings.TrimSpace(req.PhoneNumber)

	_, err := mail.ParseAddress(req.Email)

	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": apperrors.ErrInvalidEmail.Error()})
		return
	}

	valid := internalutils.IsValidPhoneNumber(req.PhoneNumber)
	if !valid {
		c.JSON(http.StatusBadRequest, gin.H{"error": apperrors.ErrPhoneNumberInvalidFormat.Error()})
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

	dtoList := make([]dtos.PersonDto, len(result.Data))
	for i, person := range result.Data {
		dtoList[i] = *person.ToJson()
	}

	dtoResult := valueobjects.PaginatedResult[dtos.PersonDto]{
		Data:            dtoList,
		Pagination:      result.Pagination,
		HasNextPage:     result.HasNextPage,
		HasPreviousPage: result.HasPreviousPage,
	}

	c.JSON(http.StatusOK, dtoResult)
}

func (h *PersonHandler) HandleIsPhoneNumberTaken(c *gin.Context) {
	var req dtos.PhoneNumberTakenDto

	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	taken, err := h.service.IsPhoneNumberTaken(req.PhoneNumber)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	c.JSON(http.StatusOK, dtos.PhoneNumberTakenResponse{Taken: taken})
}

func (h *PersonHandler) HandleIsEmailTaken(c *gin.Context) {
	var req dtos.EmailTakenDto

	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	taken, err := h.service.IsEmailTaken(req.Email)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	c.JSON(http.StatusOK, dtos.EmailTakenResponse{Taken: taken})
}
