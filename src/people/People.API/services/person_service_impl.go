package services

import (
	apperrors "people/api/app_errors"
	"people/api/db/repositories"
	"people/api/models"
	valueobjects "people/api/value_objects"
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

// Public: Search implements PersonService.
func (p *personService) Search(query *valueobjects.SearchPersonQuery) (*valueobjects.PaginatedResult[models.Person], error) {
	return p.repo.Search(query)
}

// Public: IsPhoneNumberTaken implements PersonService.
func (p *personService) IsPhoneNumberTaken(phoneNumber string) (bool, error) {
	return p.repo.PhoneNumberTaken(phoneNumber)
}

// Public: IsEmailTaken implements PersonService.
func (p *personService) IsEmailTaken(email string) (bool, error) {
	return p.repo.EmailTaken(email)
}
