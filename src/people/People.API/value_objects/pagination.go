package valueobjects

// Public: Generic paginated result object
type PaginatedResult[T any] struct {

	// Collection of data of type T
	Data []T `json:"data"`

	// Contaisn the meta data for pagination
	Pagination PaginationMeta `json:"pagination"`

	// Flag to indicate if a next page can be fetched
	HasNextPage bool `json:"hasNextPage"`

	// Flag to indicate if a previous page can be fetched
	HasPreviousPage bool `json:"hasPreviousPage"`
}

// Contaisn meta data for pagination
type PaginationMeta struct {

	// Current page number for the pagination fetched
	PageNumber int `json:"pageNumber"`

	// How many items are sent with the pagination
	PageSize int `json:"pageSize"`

	// How manyu totroal pages can be fetched for the pagination
	TotalPages int `json:"totalPages"`

	// Totoal count of all items for the pagination
	TotalRecords int `json:"totalRecords"`
}
