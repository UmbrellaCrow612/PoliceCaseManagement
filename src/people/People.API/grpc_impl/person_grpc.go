package grpcimpl

import (
	"context"
	"log"
	personv1 "people/api/gen/common"
	"people/api/services"
)

type PersonServerImpl struct {
	personv1.UnimplementedPersonServiceServer
	personService services.PersonService
}

func NewPersonServerImpla(s services.PersonService) *PersonServerImpl {
	return &PersonServerImpl{personService: s}
}

func (s *PersonServerImpl) DoesPersonExist(ctx context.Context, req *personv1.DoesPersonExistRequest) (*personv1.DoesPersonExistResponse, error) {
	log.Printf("Checking existence of person with ID: %s", req.GetPersonId())

	// Dummy logic - replace with real lookup
	exists := req.GetPersonId() == "12345"

	return &personv1.DoesPersonExistResponse{
		Exists: exists,
	}, nil
}

func (s *PersonServerImpl) GetPersonById(ctx context.Context, req *personv1.GetPersonByIdRequest) (*personv1.GetPersonByIdResponse, error) {
	log.Printf("Fetching person with ID: %s", req.GetPersonId())

	// Dummy data - replace with DB/service logic
	return &personv1.GetPersonByIdResponse{
		PersonId:    req.GetPersonId(),
		FirstName:   "Jane",
		LastName:    "Doe",
		DateOfBirth: "1990-01-01",
		PhoneNumber: "+441234567890",
		Email:       "jane.doe@example.com",
	}, nil
}
