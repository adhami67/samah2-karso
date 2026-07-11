from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.assignment import Assignment
from app.models.response import Response
from app.schemas.response import ResponseCreate, ResponseRead

router = APIRouter()

@router.post("/", response_model=ResponseRead, status_code=201)
def create_response(payload: ResponseCreate, db: Session = Depends(get_db)):
    assignment = db.get(Assignment, payload.assignment_id)
    if not assignment:
        raise HTTPException(status_code=404, detail="Assignment not found")
    item = Response(assignment_id=payload.assignment_id, body=payload.body)
    db.add(item)
    db.commit()
    db.refresh(item)
    return item

@router.get("/", response_model=list[ResponseRead])
def list_responses(db: Session = Depends(get_db)):
    return db.query(Response).order_by(Response.created_at.desc()).all()
