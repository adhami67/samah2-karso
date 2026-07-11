from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.activity import Activity
from app.models.workspace import Workspace
from app.schemas.activity import ActivityCreate, ActivityRead, ActivityUpdate
from app.services.workflow import can_transition_activity
from app.utils.deps import get_current_user

router = APIRouter()

@router.post("/", response_model=ActivityRead, status_code=201)
def create_activity(payload: ActivityCreate, db: Session = Depends(get_db), current_user=Depends(get_current_user)):
    workspace = db.get(Workspace, payload.workspace_id)
    if not workspace:
        raise HTTPException(status_code=404, detail="Workspace not found")
    item = Activity(
        workspace_id=payload.workspace_id,
        created_by_user_id=current_user.id,
        title=payload.title,
        description=payload.description,
        activity_type=payload.activity_type,
        due_at=payload.due_at,
    )
    db.add(item)
    db.commit()
    db.refresh(item)
    return item

@router.get("/", response_model=list[ActivityRead])
def list_activities(db: Session = Depends(get_db)):
    return db.query(Activity).order_by(Activity.created_at.desc()).all()

@router.get("/{activity_id}", response_model=ActivityRead)
def get_activity(activity_id: str, db: Session = Depends(get_db)):
    item = db.get(Activity, activity_id)
    if not item:
        raise HTTPException(status_code=404, detail="Activity not found")
    return item

@router.patch("/{activity_id}", response_model=ActivityRead)
def update_activity(activity_id: str, payload: ActivityUpdate, db: Session = Depends(get_db)):
    item = db.get(Activity, activity_id)
    if not item:
        raise HTTPException(status_code=404, detail="Activity not found")
    if payload.title is not None:
        item.title = payload.title
    if payload.description is not None:
        item.description = payload.description
    if payload.activity_type is not None:
        item.activity_type = payload.activity_type
    if payload.due_at is not None:
        item.due_at = payload.due_at
    if payload.status is not None:
        if not can_transition_activity(item.status, payload.status) and payload.status != item.status:
            raise HTTPException(status_code=400, detail=f"Transition {item.status} -> {payload.status} is not allowed")
        item.status = payload.status
    db.commit()
    db.refresh(item)
    return item
