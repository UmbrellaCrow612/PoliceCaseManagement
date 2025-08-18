package services

import (
	"context"
	"log"
	apperrors "people/api/app_errors"
	"people/api/db/repositories"
	personeventsv1 "people/api/gen/person/events/v1"
	"people/api/models"
	valueobjects "people/api/value_objects"
	"strconv"

	"github.com/rabbitmq/amqp091-go"
	"google.golang.org/protobuf/encoding/protojson"
)

type personService struct {
	repo    repositories.PersonRepository
	queue   *amqp091.Queue
	channel *amqp091.Channel
	ctx     *context.Context
}

func NewPersonService(repo repositories.PersonRepository, queue *amqp091.Queue, channel *amqp091.Channel, ctx *context.Context) PersonService {
	return &personService{repo: repo, queue: queue, channel: channel, ctx: ctx}
}

// public: GetById implements PersonService.
func (p *personService) GetById(personId string) (*models.Person, error) {
	return p.repo.GetByID(personId)
}

// Public: Create implements PersonService.
func (p *personService) Create(person *models.Person) error {
	emailTaken, err := p.repo.EmailTaken(person.Email)
	if err != nil {
		return err
	}
	if emailTaken {
		return apperrors.ErrEmailTaken
	}

	phoneNumberTaken, err := p.repo.PhoneNumberTaken(person.PhoneNumber)
	if err != nil {
		return err
	}
	if phoneNumberTaken {
		return apperrors.ErrPhoneNumberTaken
	}

	err = p.repo.Create(person)
	if err != nil {
		return err
	}

	event := &personeventsv1.PersonCreated{
		PersonId:    strconv.FormatUint(uint64(person.ID), 10),
		FirstName:   person.FirstName,
		LastName:    person.LastName,
		Email:       person.Email,
		PhoneNumber: person.PhoneNumber,
	}

	body, err := protojson.Marshal(event)
	if err != nil {
		log.Printf("Failed to marshal PersonCreated event: %v", err)
		return nil // don’t block creation if event fails
	}

	err = p.channel.PublishWithContext(
		*p.ctx,
		"",           // exchange
		p.queue.Name, // routing key (queue name)
		false,
		false,
		amqp091.Publishing{
			ContentType: "application/json",
			Body:        body,
			Type:        "PersonCreated",
		},
	)
	if err != nil {
		log.Printf("Failed to publish PersonCreated event: %v", err)
		// optionally return err if you want to fail the whole request
	}

	return nil
}

// Public: Search implements PersonService.
func (p *personService) Search(query *valueobjects.SearchPersonQuery) (*valueobjects.PaginatedResult[models.Person], error) {
	return p.repo.Search(query)
}

// Public: IsPhoneNumberTaken implements PersonService.
func (p *personService) IsPhoneNumberTaken(phoneNumber string) (bool, error) {
	return p.repo.PhoneNumberTaken(phoneNumber)
}

// Public: IsEmailTaken implements PersonService.
func (p *personService) IsEmailTaken(email string) (bool, error) {
	return p.repo.EmailTaken(email)
}
