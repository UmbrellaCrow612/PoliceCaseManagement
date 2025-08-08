package models

import (
	"people/api/dtos"
	"strconv"
	"time"

	"gorm.io/gorm"
)

// Public: Person model
type Person struct {
	gorm.Model
	FirstName   string
	LastName    string
	DateOfBirth time.Time
	PhoneNumber string
	Email       string
}

// Public: A helper method that allows you to return the JSON representation of a person
func (p *Person) ToJson() *dtos.PersonDto {
	return &dtos.PersonDto{
		Id: strconv.FormatUint(uint64(p.Model.ID), 10),
		CreatedAt: p.Model.CreatedAt,
		UpdatedAt: p.Model.UpdatedAt,
		FirstName: p.FirstName,
		LastName: p.LastName,
		DateOfBirth: p.DateOfBirth,
		PhoneNumber: p.PhoneNumber,
		Email: p.Email,
	}
}
