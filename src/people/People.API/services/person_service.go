package services

import (
	"people/api/db/repositories"
	"people/api/models"
)

type PersonService struct {
	repo repositories.PersonRepository
}

func NewPersonService(repo repositories.PersonRepository) *PersonService {
	return &PersonService{repo: repo}
}

func (s *PersonService) GetPersonByID(id string) (*models.Person, error) {
	return s.repo.GetByID(id)
}


func (s *PersonService) Exists(id string) (bool, error) {
	return false, nil
}

