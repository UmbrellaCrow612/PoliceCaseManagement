package grpc_servers

import (
	"context"
	personv1 "people/api/gen/common"
	"people/api/services"
)

// personServiceServer implements the gRPC PersonServiceServer interface.
type personServiceServer struct {
	personv1.UnimplementedPersonServiceServer
	personService services.PersonService
}

// NewPersonServiceServer creates a new gRPC PersonServiceServer.
func NewPersonServiceServer(personService services.PersonService) personv1.PersonServiceServer {
	return &personServiceServer{
		personService: personService,
	}
}

func (s *personServiceServer) DoesPersonExist(ctx context.Context, req *personv1.DoesPersonExistRequest) (*personv1.DoesPersonExistResponse, error) {
	// Example call to your service layer (you will implement this)
	exists, err := s.personService.Exists(req.GetPersonId())
	if err != nil {
		return nil, err
	}
	return &personv1.DoesPersonExistResponse{Exists: exists}, nil
}

func (s *personServiceServer) GetPersonById(ctx context.Context, req *personv1.GetPersonByIdRequest) (*personv1.GetPersonByIdResponse, error) {
	person, err := s.personService.GetPersonByID(req.GetPersonId())
	if err != nil {
		return nil, err
	}
	return &personv1.GetPersonByIdResponse{
		PersonId:    person.ID,
		FirstName:   person.FirstName,
		LastName:    person.LastName,
		DateOfBirth: person.DateOfBirth.Format("2006-01-02"), // or appropriate formatting
		PhoneNumber: person.PhoneNumber,
		Email:       person.Email,
	}, nil
}
