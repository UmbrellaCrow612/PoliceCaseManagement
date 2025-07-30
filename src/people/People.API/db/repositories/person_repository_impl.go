package repositories

import (
	"math"
	"people/api/models"
	valueobjects "people/api/value_objects"

	"gorm.io/gorm"
)

type personRepository struct {
	db *gorm.DB
}

// Public: Handles DB operations for a person
func NewPersonRepository(db *gorm.DB) PersonRepository {
	return &personRepository{db: db}
}

// Public: GetByID implements PersonRepository.
func (p *personRepository) GetByID(id string) (*models.Person, error) {
	var person models.Person
	result := p.db.First(&person, "id = ?", id)
	if result.Error != nil {
		return nil, result.Error
	}
	return &person, nil
}

// Public: Exists implements PersonRepository.
func (p *personRepository) Exists(personId string) (bool, error) {
	var exists bool
	err := p.db.
		Raw("SELECT EXISTS(SELECT 1 FROM people WHERE id = ?)", personId).
		Scan(&exists).Error
	if err != nil {
		return false, err
	}
	return exists, nil
}

// Public: EmailTaken checks if a person with the given email already exists.
func (p *personRepository) EmailTaken(email string) (bool, error) {
	var exists bool
	err := p.db.
		Raw("SELECT EXISTS(SELECT 1 FROM people WHERE email = ?)", email).
		Scan(&exists).Error
	if err != nil {
		return false, err
	}
	return exists, nil
}

// Public: PhoneNumberTaken checks if a person with the given phone number already exists.
func (p *personRepository) PhoneNumberTaken(phoneNumber string) (bool, error) {
	var exists bool
	err := p.db.
		Raw("SELECT EXISTS(SELECT 1 FROM people WHERE phone_number = ?)", phoneNumber).
		Scan(&exists).Error
	if err != nil {
		return false, err
	}
	return exists, nil
}

// Create implements PersonRepository.
func (p *personRepository) Create(person *models.Person) error {
	result := p.db.Create(person)
	return result.Error
}

// Search implements PersonRepository.
func (p *personRepository) Search(query *valueobjects.SearchPersonQuery) (*valueobjects.PaginatedResult[models.Person], error) {
	var people []models.Person
	var totalRecords int64

	dbQuery := p.db.Model(&models.Person{})

	// Apply filters if provided
	if query.FirstName != "" {
		dbQuery = dbQuery.Where("first_name ILIKE ?", "%"+query.FirstName+"%")
	}
	if query.LastName != "" {
		dbQuery = dbQuery.Where("last_name ILIKE ?", "%"+query.LastName+"%")
	}
	if query.DateOfBirth != nil {
		dbQuery = dbQuery.Where("date_of_birth = ?", query.DateOfBirth.Format("2006-01-02"))
	}
	if query.PhoneNumber != "" {
		dbQuery = dbQuery.Where("phone_number = ?", query.PhoneNumber)
	}
	if query.Email != "" {
		dbQuery = dbQuery.Where("email = ?", query.Email)
	}

	if err := dbQuery.Count(&totalRecords).Error; err != nil {
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

	err := dbQuery.
		Limit(pageSize).
		Offset(offset).
		Find(&people).Error
	if err != nil {
		return nil, err
	}

	totalPages := int(math.Ceil(float64(totalRecords) / float64(pageSize)))

	result := &valueobjects.PaginatedResult[models.Person]{
		PageNumber:      pageNumber,
		PageSize:        pageSize,
		Data:            people,
		HasNextPage:     pageNumber < totalPages,
		HasPreviousPage: pageNumber > 1,
		TotalPages:      totalPages,
		TotalRecords:    int(totalRecords),
	}

	return result, nil
}
