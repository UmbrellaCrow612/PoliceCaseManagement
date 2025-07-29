package repositories

import "people/api/models"

type PersonRepository interface {

	// Public: Get a person by there ID or error
	GetByID(id string) (*models.Person, error)

	// Public: Check if a person exists
	Exists(personId string) (bool, error)
}
