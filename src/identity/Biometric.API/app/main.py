from fastapi import FastAPI

from .api.v1 import face_id_routes, user_routes
from .models.base import Base
from .db.engine import engine

Base.metadata.create_all(bind=engine)

app = FastAPI(
    title="Biometrics API",
    description="API for biometric authentication and face ID setup.",
    version="1.0.0"
)

app.include_router(face_id_routes.router, prefix="/v1")
app.include_router(user_routes.router, prefix="/v1")
