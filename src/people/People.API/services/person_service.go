package services

import (
	"people/api/models"
	valueobjects "people/api/value_objects"
)

// Business contract for person service
type PersonService interface {

	// Get a person by there ID
	GetById(personId string) (*models.Person, error)

	// Public: Check if a person exists by there ID
	Exists(personId string) (bool, error)

	// Public: Create a person
	Create(person *models.Person) error

	// Public: Search people
	Search(query *valueobjects.SearchPersonQuery) (*valueobjects.PaginatedResult[models.Person], error)
}
