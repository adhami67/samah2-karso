from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.response import Response
from app.models.evaluation import Evaluation
from app.schemas.evaluation import EvaluationCreate, EvaluationRead

router = APIRouter()

@router.post("/", response_model=EvaluationRead, status_code=201)
def create_evaluation(payload: EvaluationCreate, db: Session = Depends(get_db)):
    response = db.get(Response, payload.response_id)
    if not response:
        raise HTTPException(status_code=404, detail="Response not found")
    item = Evaluation(response_id=payload.response_id, score=payload.score, note=payload.note)
    db.add(item)
    db.commit()
    db.refresh(item)
    return item

@router.get("/", response_model=list[EvaluationRead])
def list_evaluations(db: Session = Depends(get_db)):
    return db.query(Evaluation).order_by(Evaluation.created_at.desc()).all()
