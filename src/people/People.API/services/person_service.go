package services

import (
	"people/api/db"
	"people/api/models"
)

func FindPersonById(id uint) (models.Person, bool) {
	var person models.Person
	result := db.DB.First(&person, id)
	if result.Error != nil {
		return models.Person{}, false
	}
	return person, true
}

