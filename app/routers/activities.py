from typing import Optional
from datetime import datetime
from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.activity import Activity
from app.schemas.activity import ActivityCreate, ActivityRead, ActivityUpdate
from app.schemas.assignment import AssignmentCreate, AssignmentRead
from app.services.activity_service import ActivityService
from app.services.workflow import can_transition_activity
from app.utils.deps import get_current_user

router = APIRouter()


@router.post("/", response_model=ActivityRead, status_code=status.HTTP_201_CREATED)
def create_activity(
    payload: ActivityCreate,
    db: Session = Depends(get_db),
    current_user=Depends(get_current_user),
):
    service = ActivityService(db)
    try:
        activity = service.create_activity(payload, str(current_user.id))
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))
    return activity


@router.get("/", response_model=list[ActivityRead])
def list_activities(
    db: Session = Depends(get_db),
    status: Optional[str] = None,
    assignee_id: Optional[str] = None,
    workspace_id: Optional[str] = None,
    due_before: Optional[datetime] = None,
    due_after: Optional[datetime] = None,
    search: Optional[str] = None,
):
    query = db.query(Activity)

    if status:
        query = query.filter(Activity.status == status)
    if workspace_id:
        query = query.filter(Activity.workspace_id == workspace_id)
    if due_before:
        query = query.filter(Activity.due_at <= due_before)
    if due_after:
        query = query.filter(Activity.due_at >= due_after)
    if search:
        query = query.filter(Activity.title.ilike(f"%{search}%"))
    if assignee_id:
        # برای فیلتر بر اساس مجری، باید با Assignment جوین بزنیم
        from app.models.assignment import Assignment
        query = query.join(Activity.assignments).filter(
            Assignment.assignee_id == assignee_id
        ).distinct()

    return query.order_by(Activity.created_at.desc()).all()


@router.get("/{activity_id}", response_model=ActivityRead)
def get_activity(activity_id: str, db: Session = Depends(get_db)):
    item = db.get(Activity, activity_id)
    if not item:
        raise HTTPException(status_code=404, detail="Activity not found")
    return item


@router.patch("/{activity_id}", response_model=ActivityRead)
def update_activity(
    activity_id: str,
    payload: ActivityUpdate,
    db: Session = Depends(get_db),
):
    activity = db.get(Activity, activity_id)
    if not activity:
        raise HTTPException(status_code=404, detail="Activity not found")

    if payload.title is not None:
        activity.title = payload.title
    if payload.description is not None:
        activity.description = payload.description
    if payload.activity_type is not None:
        activity.activity_type = payload.activity_type
    if payload.due_at is not None:
        activity.due_at = payload.due_at

    if payload.status is not None and payload.status != activity.status:
        if not can_transition_activity(activity.status, payload.status):
            raise HTTPException(
                status_code=400,
                detail=f"Transition from '{activity.status}' to '{payload.status}' is not allowed."
            )
        activity.status = payload.status

    db.commit()
    db.refresh(activity)
    return activity


@router.post("/{activity_id}/assign", response_model=AssignmentRead, status_code=status.HTTP_201_CREATED)
def assign_activity(
    activity_id: str,
    payload: AssignmentCreate,
    db: Session = Depends(get_db),
    current_user=Depends(get_current_user),
):
    activity = db.get(Activity, activity_id)
    if not activity:
        raise HTTPException(status_code=404, detail="Activity not found")

    service = ActivityService(db)
    assignment = service.assign_to_user(
        activity=activity,
        assignee_id=str(payload.assignee_id),
        role=payload.role or "executor",
    )
    return assignment