from fastapi import Depends, FastAPI, HTTPException
from pydantic import BaseModel
from sqlalchemy import inspect

from api.v1 import face_id_routes
from sqlalchemy.orm import Session
from models.base import Base
from models.user import User
from db.engine import engine
from db.utils import get_db

Base.metadata.create_all(bind=engine)

app = FastAPI(
    title="Biometrics API",
    description="API for biometric authentication and face ID setup.",
    version="1.0.0"
)

app.include_router(face_id_routes.router, prefix="/v1", tags=["face_id"])

class UserCreate(BaseModel):
    name: str
    email: str

class UserResponse(UserCreate):
    id: int

    class Config:
        from_attributes = True  # For ORM mode


@app.post("/users/", response_model=UserResponse)
def create_user(user: UserCreate, db: Session = Depends(get_db)):
    db_user = User(name=user.name, email=user.email)
    db.add(db_user)
    db.commit()
    db.refresh(db_user)
    return db_user