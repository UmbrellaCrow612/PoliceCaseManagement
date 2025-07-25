package services

import (
	"people/api/db"
	"people/api/models"
)

func GetAllUsers() []models.User {
    var users []models.User
    db.DB.Find(&users)
    return users
}

func CreateUser(name string) error {
    user := models.User{Name: name}
    return db.DB.Create(&user).Error
}