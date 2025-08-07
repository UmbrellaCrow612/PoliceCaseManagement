package services

import (
	"people/api/models"
	valueobjects "people/api/value_objects"
)

// Business contract for person service
type PersonService interface {

	// Get a person by there ID
	GetById(personId string) (*models.Person, error)

	// Public: Create a person
	Create(person *models.Person) error

	// Public: Search people
	Search(query *valueobjects.SearchPersonQuery) (*valueobjects.PaginatedResult[models.Person], error)

	// Public: Check if a phone number is taken by another person
	IsPhoneNumberTaken(phoneNumber string) (bool, error)

	// Public: Check is a email is taken by another person
	IsEmailTaken(email string) (bool, error)
}
