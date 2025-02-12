from fastapi import FastAPI, HTTPException
from pydantic import BaseModel

app = FastAPI(
    title="Biometrics API",
    description="API for biometric authentication and face ID setup.",
    version="1.0.0"
)

class SetupResponse(BaseModel):
    message: str

@app.get(
    "/biometrics/users/{userId}/setup/face-id",
    summary="Initialize Face ID Setup",
    description=(
        "This endpoint allows a user to initialize Face ID setup for biometric authentication. "
        "It validates the user ID before starting the process."
    ),
    response_model=SetupResponse,
    tags=["Biometrics"],
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
