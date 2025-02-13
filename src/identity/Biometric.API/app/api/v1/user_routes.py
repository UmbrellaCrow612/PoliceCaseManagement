from fastapi import APIRouter
from ..tags.v1_tags import V1tags

router = APIRouter()

@router.post(
    "/users",
    summary="Create a new user",
    description=(
        "This endpoint allows a user to be created."
    ),
    tags=[V1tags.user],
    responses={
        200: {"description": "User created"},
        400: {"description": "User could not be created, either invalid data shape sent or some indexed field is same as another user."},
        500: {"description": "Internal server error"},
    }
)
async def create_user():
     return {"message": f"Face ID setup initialized for user "}