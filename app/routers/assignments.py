from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.activity import Activity
from app.models.assignment import Assignment
from app.schemas.assignment import AssignmentCreate, AssignmentRead

router = APIRouter()

@router.post("/", response_model=AssignmentRead, status_code=201)
def create_assignment(payload: AssignmentCreate, db: Session = Depends(get_db)):
    activity = db.get(Activity, payload.activity_id)
    if not activity:
        raise HTTPException(status_code=404, detail="Activity not found")
    item = Assignment(
        activity_id=payload.activity_id,
        assignee_id=payload.assignee_id,
        assignee_type=payload.assignee_type,
        role=payload.role,
    )
    db.add(item)
    db.commit()
    db.refresh(item)
    return item

@router.get("/", response_model=list[AssignmentRead])
def list_assignments(db: Session = Depends(get_db)):
    return db.query(Assignment).order_by(Assignment.created_at.desc()).all()
