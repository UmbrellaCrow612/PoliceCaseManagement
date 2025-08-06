package models

import (
	"time"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

// Public: Person model
type Person struct {
	ID          string    `gorm:"primaryKey;column:id" json:"id"`
	FirstName   string    `gorm:"column:firstName" json:"firstName"`
	LastName    string    `gorm:"column:lastName" json:"lastName"`
	DateOfBirth time.Time `gorm:"column:dateOfBirth" json:"dateOfBirth"`
	PhoneNumber string    `gorm:"uniqueIndex;column:phoneNumber" json:"phoneNumber"`
	Email       string    `gorm:"uniqueIndex;column:email" json:"email"`
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