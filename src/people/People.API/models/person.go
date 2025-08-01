package models

import (
	"time"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

type Person struct {
	ID          string    `gorm:"primaryKey" json:"id"`
	FirstName   string    `json:"first_name"`
	LastName    string    `json:"last_name"`
	DateOfBirth time.Time `json:"date_of_birth"`
	PhoneNumber string    `gorm:"uniqueIndex" json:"phone_number"` 
	Email       string    `gorm:"uniqueIndex" json:"email"`       
}

func (p *Person) BeforeCreate(tx *gorm.DB) (err error) {
    if p.ID == "" {
        p.ID = uuid.NewString()
    }
    return
}

func (Person) TableName() string {
	return "people"
}