package internalutils

import "regexp"

// Public: IsValidPhoneNumber strictly validates a phone number:
// - Starts with +
// - Followed by 8 to 15 digits
// - No spaces, dashes, parentheses, or any other characters
func IsValidPhoneNumber(phone string) bool {
	// Strict E.164 format (no spaces or symbols)
	regex := regexp.MustCompile(`^\+\d{8,15}$`)
	return regex.MatchString(phone)
}
