from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.schemas.response import ResponseCreate, ResponseRead
from app.services.response_service import ResponseService

router = APIRouter()


def get_response_service(db: Session = Depends(get_db)) -> ResponseService:
    return ResponseService(db)


@router.post("/", response_model=ResponseRead, status_code=status.HTTP_201_CREATED)
def create_response(
    payload: ResponseCreate,
    service: ResponseService = Depends(get_response_service),
):
    try:
        response = service.submit_response(payload)
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))
    return response


@router.get("/", response_model=list[ResponseRead])
def list_responses(db: Session = Depends(get_db)):
    from app.models.response import Response
    return db.query(Response).order_by(Response.created_at.desc()).all()