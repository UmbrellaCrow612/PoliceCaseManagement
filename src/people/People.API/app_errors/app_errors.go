package apperrors

import "errors"

var (
	ErrEmailTaken       = errors.New("ERR_EMAIL_TAKEN")
	ErrPhoneNumberTaken = errors.New("ERR_PHONE_NUMBER_TAKEN")
	ErrNotFound         = errors.New("ERR_NOT_FOUND")
	ErrInvalidInput     = errors.New("ERR_INVALID_INPUT")
	ErrInvalidEmail     = errors.New("ERR_INVALID_EMAIL")
)