package valueobjects

// Public: Generic paginated result object
type PaginatedResult[T any] struct {

	// Collection of data of type T
	Data []T

	// Contaisn the meta data for pagination
	Pagination PaginationMeta

	// Flag to indicate if a next page can be fetched
	HasNextPage bool

	// Flag to indicate if a previous page can be fetched
	HasPreviousPage bool
}

// Contaisn meta data for pagination
type PaginationMeta struct {

	// Current page number for the pagination fetched
	PageNumber int

	// How many items are sent with the pagination
	PageSize int

	// How manyu totroal pages can be fetched for the pagination
	TotalPages int

	// Totoal count of all items for the pagination
	TotalRecords int
}
