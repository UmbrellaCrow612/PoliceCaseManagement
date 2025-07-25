package db

import (
	"fmt"
	"log"
	"os"
	"people/api/models"

	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

var DB *gorm.DB

func Init() {
	dsn := os.Getenv("DB_CONNECTION")

	if dsn == "" {
		log.Fatalf(".env DB_CONNECTION missing")
	}

	var err error
	DB, err = gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		log.Fatalf("Failed to connect to database: %v", err)
	}

	fmt.Println("Connected to Postgres with GORM!")

	if err := DB.AutoMigrate(&models.Person{}); err != nil {
		log.Fatalf("AutoMigrate failed: %v", err)
	}
}
