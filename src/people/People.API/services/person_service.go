package services

import (
	"people/api/models"
)

// Business contract for person service
type PersonService interface {

	// Get a person by there ID
	GetById(personId string) (*models.Person, error)

	// Public: Check if a person exists by there ID
	Exists(personId string) (bool, error)
}
