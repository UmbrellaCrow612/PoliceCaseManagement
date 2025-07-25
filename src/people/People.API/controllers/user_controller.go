package controllers

import (
	"net/http"
	"people/api/services"

	"github.com/gin-gonic/gin"
)

type CreateUserRequest struct {
	Name string `json:"name" binding:"required"`
}

func GetUsers(c *gin.Context) {
	users := services.GetAllUsers()
	c.JSON(http.StatusOK, gin.H{"users": users})
}

func CreateUser(c *gin.Context) {
	var req CreateUserRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	if err := services.CreateUser(req.Name); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	c.JSON(http.StatusCreated, gin.H{"message": "User created"})
}
