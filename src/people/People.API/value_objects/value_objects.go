package valueobjects

import "time"

// Public: search person query object
type SearchPersonQuery struct {
    FirstName   string     `form:"firstName"`
    LastName    string     `form:"lastName"`
    DateOfBirth *time.Time `form:"dateOfBirth" time_format:"2006-01-02"`
    PhoneNumber string     `form:"phoneNumber"`
    Email       string     `form:"email"`
    PageNumber  int        `form:"pageNumber" binding:"required,min=1"`
    PageSize    int        `form:"pageSize" binding:"required,min=1,max=100"`  
}

// Public: Generic paginated result object
type PaginatedResult[T any] struct {
	PageNumber      int
	PageSize        int
	Data            []T
	HasNextPage     bool
	HasPreviousPage bool
	TotalPages      int
	TotalRecords    int
}
