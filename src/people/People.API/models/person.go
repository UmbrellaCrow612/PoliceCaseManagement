package models

import (
	"time"

	"gorm.io/gorm"
)

// Public: Person model
type Person struct {
	gorm.Model
	FirstName   string    `json:"firstName"`
	LastName    string    `json:"lastName"`
	DateOfBirth time.Time `json:"dateOfBirth"`
	PhoneNumber string    `json:"phoneNumber"`
	Email       string    `json:"email"`
}


func (Person) TableName() string {
	return "people"
}
