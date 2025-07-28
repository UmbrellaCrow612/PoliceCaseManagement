package repositories

import (
	"gorm.io/gorm"
	"people/api/models"
)

type personRepository struct {
	db *gorm.DB
}

func NewPersonRepository(db *gorm.DB) PersonRepository {
	return &personRepository{db: db}
}

func (r *personRepository) GetByID(id string) (*models.Person, error) {
	var person models.Person
	result := r.db.First(&person, "id = ?", id)
	if result.Error != nil {
		return nil, result.Error
	}
	return &person, nil
}
