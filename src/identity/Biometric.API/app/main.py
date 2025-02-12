from fastapi import FastAPI, HTTPException
from pydantic import BaseModel

from api.v1 import face_id_routes

app = FastAPI(
    title="Biometrics API",
    description="API for biometric authentication and face ID setup.",
    version="1.0.0"
)

app.include_router(face_id_routes.router, prefix="/v1", tags=["face_id"])