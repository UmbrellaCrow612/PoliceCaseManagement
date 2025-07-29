package repositories

import (
	"people/api/models"

	"gorm.io/gorm"
)

type personRepository struct {
	db *gorm.DB
}

// Public: Handles DB operations for a person
func NewPersonRepository(db *gorm.DB) PersonRepository {
	return &personRepository{db: db}
}

// Public: GetByID implements PersonRepository.
func (p *personRepository) GetByID(id string) (*models.Person, error) {
	var person models.Person
	result := p.db.First(&person, "id = ?", id)
	if result.Error != nil {
		return nil, result.Error
	}
	return &person, nil
}

// Public: Exists implements PersonRepository.
func (p *personRepository) Exists(personId string) (bool, error) {
	var exists bool
	err := p.db.
		Raw("SELECT EXISTS(SELECT 1 FROM people WHERE id = ?)", personId).
		Scan(&exists).Error
	if err != nil {
		return false, err
	}
	return exists, nil
}
