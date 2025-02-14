from fastapi import APIRouter, HTTPException
from pydantic import BaseModel
from ..tags.v1_tags import V1tags

router = APIRouter()

class SetupResponse(BaseModel):
    message: str

@router.get(
    "/biometrics/users/{userId}/setup/face-id",
    summary="Initialize Face ID Setup",
    description=(
        "This endpoint allows a user to initialize Face ID setup for biometric authentication. "
        "It validates the user ID before starting the process."
    ),
    response_model=SetupResponse,
    tags=[V1tags.face_id],
    responses={
        200: {"description": "Face ID setup initialized successfully"},
        400: {"description": "Invalid user ID provided"},
        500: {"description": "Internal server error"},
    }
)
async def setup_face_id(userId: int):
    """
    Set up Face ID for a user.

    Args:
        userId (int): The unique ID of the user.

    Returns:
        dict: A success message if Face ID setup is initiated.

    Raises:
        HTTPException: If the user ID is invalid.
    """
    if userId <= 0:
        raise HTTPException(status_code=400, detail="Invalid user ID provided")

    return {"message": f"Face ID setup initialized for user {userId}"}

# For login in future you could receive a image of user face vector it then compare it with db 
# but just before that read the meta data of the file and make sure it was taken in the last 5 minutes for security 