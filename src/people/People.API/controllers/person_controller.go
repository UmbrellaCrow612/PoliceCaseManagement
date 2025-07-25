package controllers

import (
	"net/http"
	"people/api/db"
	"people/api/models"

	"github.com/gin-gonic/gin"
)

func GetPersonById(c *gin.Context) {
	personId := c.Param("personId")

	var person models.Person
	if err := db.DB.First(&person, "id = ?", personId).Error; err != nil {
		c.JSON(http.StatusNotFound, gin.H{"error": "Person not found"})
		return
	}

	c.JSON(http.StatusOK, person)
}