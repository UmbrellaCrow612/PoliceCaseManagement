package repositories

import (
	"people/api/models"
	valueobjects "people/api/value_objects"
)

type PersonRepository interface {

	// Public: Get a person by there ID or error
	GetByID(id string) (*models.Person, error)

	// Public: Check if a phone number is taken
	PhoneNumberTaken(phoneNumber string) (bool, error)

	// Public: Check if a email is taken
	EmailTaken(email string) (bool, error)

	// Public: Create a person
	Create(person *models.Person) error

	// Public: Search users
	Search(query *valueobjects.SearchPersonQuery) (*valueobjects.PaginatedResult[models.Person], error)
}
