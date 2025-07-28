package repositories

import (
	"people/api/models"

	"gorm.io/gorm"
)

type personRepository struct {
	db *gorm.DB
}

func NewPersonRepository(db *gorm.DB) PersonRepository {
	return &personRepository{db: db}
}

// GetByID implements PersonRepository.
func (p *personRepository) GetByID(id string) (*models.Person, error) {
	var person models.Person
	result := p.db.First(&person, "id = ?", id)
	if result.Error != nil {
		return nil, result.Error
	}
	return &person, nil
}
