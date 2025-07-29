package grpcimpl

import (
	"context"
	personv1 "people/api/gen/common"
	"people/api/services"
)

type PersonServerImpl struct {
	personv1.UnimplementedPersonServiceServer
	personService services.PersonService
}

func NewPersonServerImpl(s services.PersonService) *PersonServerImpl {
	return &PersonServerImpl{personService: s}
}

func (s *PersonServerImpl) DoesPersonExist(ctx context.Context, req *personv1.DoesPersonExistRequest) (*personv1.DoesPersonExistResponse, error) {
	exists, err := s.personService.Exists(req.GetPersonId())
	if err != nil {
		return &personv1.DoesPersonExistResponse{Exists: false}, err
	}

	return &personv1.DoesPersonExistResponse{
		Exists: exists,
	}, nil
}

func (s *PersonServerImpl) GetPersonById(ctx context.Context, req *personv1.GetPersonByIdRequest) (*personv1.GetPersonByIdResponse, error) {
	person, err := s.personService.GetById(req.GetPersonId())
	if err != nil {
		return nil, err
	}

	return &personv1.GetPersonByIdResponse{
		PersonId:    person.ID,
		FirstName:   person.FirstName,
		LastName:    person.LastName,
		DateOfBirth: person.DateOfBirth.Format("2006-01-02"),
		PhoneNumber: person.PhoneNumber,
		Email:       person.Email,
	}, nil
}
