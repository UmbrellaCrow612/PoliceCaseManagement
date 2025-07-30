package services

import (
	apperrors "people/api/app_errors"
	"people/api/db/repositories"
	"people/api/models"
)

type personService struct {
	repo repositories.PersonRepository
}

func NewPersonService(repo repositories.PersonRepository) PersonService {
	return &personService{repo: repo}
}

// public: GetById implements PersonService.
func (p *personService) GetById(personId string) (*models.Person, error) {
	return p.repo.GetByID(personId)
}

// Public: Exists implements PersonService.
func (p *personService) Exists(personId string) (bool, error) {
	return p.repo.Exists(personId)
}

// Public: Create implements PersonService.
func (p *personService) Create(person *models.Person) error {
	emailTaken, err := p.repo.EmailTaken(person.Email)
	if err != nil {
		return err
	}
	if emailTaken {
		return apperrors.ErrEmailTaken
	}

	phoneNumberTaken, err := p.repo.PhoneNumberTaken(person.PhoneNumber)
	if err != nil {
		return err
	}
	if phoneNumberTaken {
		return apperrors.ErrPhoneNumberTaken
	}

	err = p.repo.Create(person)
	if err != nil {
		return err
	}

	return nil
}
