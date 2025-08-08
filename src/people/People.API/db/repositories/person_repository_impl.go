package repositories

import (
	"context"
	"math"
	"people/api/models"
	valueobjects "people/api/value_objects"

	"gorm.io/gorm"
)

type personRepository struct {
	db  *gorm.DB
	ctx *context.Context
}

// Public: Handles DB operations for a person
func NewPersonRepository(db *gorm.DB, ctx *context.Context) PersonRepository {
	return &personRepository{db: db, ctx: ctx}
}

// Public: GetByID implements PersonRepository.
func (p *personRepository) GetByID(id string) (*models.Person, error) {
	person, err := gorm.G[models.Person](p.db).Where("id = ?", id).First(*p.ctx)
	if err != nil {
		return nil, err
	}

	return &person, nil
}

// Public: EmailTaken checks if a person with the given email already exists.
func (p *personRepository) EmailTaken(email string) (bool, error) {
	var exists bool

	err := gorm.G[models.Person](p.db).Raw("SELECT EXISTS ( SELECT 1 FROM people  WHERE email = ? AND deleted_at IS NULL )", email).Scan(*p.ctx, &exists)
	if err != nil {
		return false, err
	}

	return exists, nil
}

// Public: PhoneNumberTaken checks if a person with the given phone number already exists.
func (p *personRepository) PhoneNumberTaken(phoneNumber string) (bool, error) {
	var exists bool

	err := gorm.G[models.Person](p.db).Raw("SELECT EXISTS ( SELECT 1 FROM people  WHERE phone_number = ? AND deleted_at IS NULL )", phoneNumber).Scan(*p.ctx, &exists)
	if err != nil {
		return false, err
	}

	return exists, nil
}

// Create implements PersonRepository.
func (p *personRepository) Create(person *models.Person) error {
	err := gorm.G[models.Person](p.db).Create(*p.ctx, person)
	return err
}

// Search implements PersonRepository.
func (p *personRepository) Search(query *valueobjects.SearchPersonQuery) (*valueobjects.PaginatedResult[models.Person], error) {
	var queryBuilder gorm.ChainInterface[models.Person] = gorm.G[models.Person](p.db)

	if query.FirstName != "" {
		queryBuilder = queryBuilder.Where("first_name LIKE ?", "%"+query.FirstName+"%")
	}
	if query.LastName != "" {
		queryBuilder = queryBuilder.Where("last_name LIKE ?", "%"+query.LastName+"%")
	}
	if query.DateOfBirth != nil {
		queryBuilder = queryBuilder.Where("DATE(date_of_birth) = ?", query.DateOfBirth.Format("2006-01-02"))
	}
	if query.PhoneNumber != "" {
		queryBuilder = queryBuilder.Where("phone_number LIKE ?", "%"+query.PhoneNumber+"%")
	}
	if query.Email != "" {
		queryBuilder = queryBuilder.Where("email LIKE ?", "%"+query.Email+"%")
	}

	totalRecords, err := queryBuilder.Count(*p.ctx, "*")
	if err != nil {
		return nil, err
	}

	pageNumber := query.PageNumber
	pageSize := query.PageSize

	if pageNumber < 1 {
		pageNumber = 1
	}
	if pageSize < 1 {
		pageSize = 10
	}

	offset := (pageNumber - 1) * pageSize

	people, err := queryBuilder.
		Limit(pageSize).
		Offset(offset).
		Find(*p.ctx)

	if err != nil {
		return nil, err
	}

	totalPages := int(math.Ceil(float64(totalRecords) / float64(pageSize)))

	result := &valueobjects.PaginatedResult[models.Person]{
		Data:            people,
		HasNextPage:     pageNumber < totalPages,
		HasPreviousPage: pageNumber > 1,
		Pagination: valueobjects.PaginationMeta{
			PageNumber:   pageNumber,
			PageSize:     pageSize,
			TotalPages:   totalPages,
			TotalRecords: int(totalRecords),
		},
	}

	return result, nil
}
