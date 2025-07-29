package services

import (
	"people/api/db/repositories"
	"people/api/models"
)

type personService struct {
	repo repositories.PersonRepository
}

func NewPersonService(repo repositories.PersonRepository) PersonService {
	return &personService{repo: repo}
}

// GetById implements PersonService.
func (p *personService) GetById(personId string) (*models.Person, error) {
	return p.repo.GetByID(personId)
}

// Public: Exists implements PersonService.
func (p *personService) Exists(personId string) (bool, error) {
	return p.repo.Exists(personId)
}