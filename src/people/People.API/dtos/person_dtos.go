package dtos

import "time"

// Public: Shape a person model is sent across the wire
type PersonDto struct {
	Id          string    `json:"id"`
	CreatedAt   time.Time `json:"createdAt"`
	UpdatedAt   time.Time `json:"updatedAt"`
	FirstName   string    `json:"firstName"`
	LastName    string    `json:"lastName"`
	DateOfBirth time.Time `json:"dateOfBirth"`
	PhoneNumber string    `json:"phoneNumber"`
	Email       string    `json:"email"`
}
