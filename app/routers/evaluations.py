from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.schemas.evaluation import EvaluationCreate, EvaluationRead
from app.services.evaluation_service import EvaluationService
from app.utils.deps import get_current_user

router = APIRouter()


def get_evaluation_service(db: Session = Depends(get_db)) -> EvaluationService:
    return EvaluationService(db)


@router.post("/", response_model=EvaluationRead, status_code=status.HTTP_201_CREATED)
def create_evaluation(
    payload: EvaluationCreate,
    service: EvaluationService = Depends(get_evaluation_service),
    current_user=Depends(get_current_user),
):
    try:
        evaluation = service.submit_evaluation(payload)
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))
    return evaluation


@router.get("/", response_model=list[EvaluationRead])
def list_evaluations(db: Session = Depends(get_db)):
    from app.models.evaluation import Evaluation
    return db.query(Evaluation).order_by(Evaluation.created_at.desc()).all()