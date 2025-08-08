package valueobjects

import "time"

// Public: search person query object
type SearchPersonQuery struct {
    FirstName   string     `form:"firstName"`
    LastName    string     `form:"lastName"`
    DateOfBirth *time.Time `form:"dateOfBirth"`
    PhoneNumber string     `form:"phoneNumber"`
    Email       string     `form:"email"`
    PageNumber  int        `form:"pageNumber" binding:"required,min=1"`
    PageSize    int        `form:"pageSize" binding:"required,min=1,max=100"`  
}
