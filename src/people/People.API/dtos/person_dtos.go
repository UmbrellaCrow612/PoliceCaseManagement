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

// Public: create person dto
type CreatePersonDto struct {
	FirstName   string    `json:"firstName" binding:"required"`
	LastName    string    `json:"lastName" binding:"required"`
	DateOfBirth time.Time `json:"dateOfBirth" binding:"required"`
	PhoneNumber string    `json:"phoneNumber" binding:"required"`
	Email       string    `json:"email" binding:"required"`
}

type PhoneNumberTakenDto struct {
	PhoneNumber string `json:"phoneNumber" binding:"required"`
}

type PhoneNumberTakenResponse struct {
	Taken bool `json:"taken"`
}

type EmailTakenDto struct {
	Email string `json:"email" binding:"required"`
}

type EmailTakenResponse struct {
	Taken bool `json:"taken"`
}
