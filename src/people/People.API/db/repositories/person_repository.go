// repositories/person_repository.go
package repositories

import "people/api/models"

type PersonRepository interface {
	GetByID(id string) (*models.Person, error)
}
