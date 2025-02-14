from typing import Annotated
from fastapi import APIRouter, File, HTTPException, UploadFile
from pydantic import BaseModel
from ..tags.v1_tags import V1tags
from deepface import DeepFace
import numpy as np
import cv2
import io
import asyncio
from concurrent.futures import ThreadPoolExecutor
from functools import partial

router = APIRouter()
# Create a thread pool executor
thread_pool = ThreadPoolExecutor(max_workers=4)  # Adjust number based on your CPU cores

class ComparisonResult(BaseModel):
    verified: bool
    distance: float
    threshold: float
    model: str
    detector_backend: str

def process_face_comparison(img1: np.ndarray, img2: np.ndarray):
    """
    Handle the CPU-intensive face comparison in a separate thread
    """
    return DeepFace.verify(
        img1_path=img1,
        img2_path=img2,
        enforce_detection=True,
        detector_backend="retinaface",
        model_name="VGG-Face",
        distance_metric="cosine"
    )

@router.post(
    "/biometrics/face/compare/",
    summary="Compare two photos to see if they are the same person or not",
    description=(
        "This endpoint allows a user to compare two faces and determine if they are the same. "
    ),
    tags=[V1tags.compare],
    response_model=ComparisonResult,
    responses={
        200: {"description": "Successfully compared the two faces"},
        400: {"description": "There was a problem with the two images sent"},
        500: {"description": "Internal server error"},
    }
)
async def compare(
    fileOne: Annotated[UploadFile, File()], 
    fileTwo: Annotated[UploadFile, File()]
    ):
    allowed_types = {"image/png"}

    if fileOne.content_type not in allowed_types or fileTwo.content_type not in allowed_types:
        raise HTTPException(status_code=400, detail="Only PNG images are allowed")
    
    try:
        # Read image files asynchronously
        contents1, contents2 = await asyncio.gather(
            fileOne.read(),
            fileTwo.read()
        )

        # Convert to numpy arrays
        nparr1 = np.frombuffer(contents1, np.uint8)
        nparr2 = np.frombuffer(contents2, np.uint8)

        # Decode images
        img1 = cv2.imdecode(nparr1, cv2.IMREAD_COLOR)
        img2 = cv2.imdecode(nparr2, cv2.IMREAD_COLOR)

        # Run face comparison in thread pool
        loop = asyncio.get_event_loop()
        result = await loop.run_in_executor(
            thread_pool,
            partial(process_face_comparison, img1, img2)
        )

        return ComparisonResult(
            verified=result["verified"],
            distance=float(result["distance"]),
            threshold=float(result["threshold"]),
            model=result["model"],
            detector_backend=result["detector_backend"]
        )

    except Exception as e:
        if "Face could not be detected" in str(e):
            raise HTTPException(
                status_code=400,
                detail="Could not detect faces in one or both images"
            )
        raise HTTPException(
            status_code=500,
            detail=f"An error occurred while processing the images: {str(e)}"
        )