# app/routers/activities.py
from typing import Optional
from datetime import datetime, timezone
from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from sqlalchemy import func
from app.db.deps import get_db
from app.models.work import Work
from app.models.work_receiver import WorkReceiver
from app.models.work_group import WorkGroup
from app.models.security import User
from app.schemas.activity import ActivityCreate, ActivityRead, ActivityUpdate
from app.schemas.assignment import AssignmentCreate, AssignmentRead
from app.services.activity_service import ActivityService
from app.services.workflow import can_transition_activity
from app.utils.deps import get_current_user
from app.core.constants import WORK_STATUS_DRAFT, WORK_STATUS_PUBLISHED

router = APIRouter()

@router.post("/", response_model=ActivityRead, status_code=status.HTTP_201_CREATED)
def create_activity(
    payload: ActivityCreate,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    service = ActivityService(db)
    try:
        activity = service.create_activity(payload, str(current_user.id))
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))
    return activity

@router.post("/{work_id}/publish", response_model=ActivityRead)
def publish_activity(
    work_id: str,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    service = ActivityService(db)
    try:
        activity = service.publish_activity(work_id, str(current_user.id))
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
    current_user: User = Depends(get_current_user),
):
    query = db.query(Work)

    if status:
        query = query.filter(Work.status == status)
    if workspace_id:
        query = query.filter(Work.work_group_id == workspace_id)
    if due_before:
        query = query.filter(Work.due_at <= due_before)
    if due_after:
        query = query.filter(Work.due_at >= due_after)
    if search:
        query = query.filter(Work.subject.ilike(f"%{search}%"))
    if assignee_id:
        query = query.join(Work.work_receivers).filter(
            WorkReceiver.receiver_user_id == assignee_id
        ).distinct()

    return query.order_by(Work.created_at.desc()).all()

@router.get("/{work_id}", response_model=ActivityRead)
def get_activity(
    work_id: str,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    item = db.get(Work, work_id)
    if not item or item.is_deleted:
        raise HTTPException(status_code=404, detail="کار مورد نظر یافت نشد")
    return item

@router.patch("/{work_id}", response_model=ActivityRead)
def update_activity(
    work_id: str,
    payload: ActivityUpdate,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    activity = db.get(Work, work_id)
    if not activity or activity.is_deleted:
        raise HTTPException(status_code=404, detail="کار مورد نظر یافت نشد")

    # فقط فرستنده یا مدیر حوزه می‌تواند ویرایش کند
    if activity.sender_user_id != current_user.id:
        workspace = db.get(WorkGroup, activity.work_group_id)
        if workspace.owner_user_id != current_user.id:
            raise HTTPException(status_code=403, detail="شما مجوز ویرایش این کار را ندارید")

    if payload.subject is not None:
        activity.subject = payload.subject
    if payload.description is not None:
        activity.description = payload.description
    if payload.due_at is not None:
        activity.due_at = payload.due_at

    if payload.status is not None and payload.status != activity.status:
        if not can_transition_activity(activity.status, payload.status):
            raise HTTPException(
                status_code=400,
                detail=f"تغییر وضعیت از '{activity.status}' به '{payload.status}' مجاز نیست"
            )
        activity.status = payload.status

    db.commit()
    db.refresh(activity)
    return activity

@router.post("/{work_id}/assign", response_model=AssignmentRead, status_code=status.HTTP_201_CREATED)
def assign_activity(
    work_id: str,
    payload: AssignmentCreate,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    # بررسی وجود کار
    activity = db.get(Work, work_id)
    if not activity or activity.is_deleted:
        raise HTTPException(status_code=404, detail="کار مورد نظر یافت نشد")

    service = ActivityService(db)
    try:
        assignment = service.assign_to_user(
            work_id=work_id,
            receiver_user_id=payload.receiver_user_id,
            private_note=payload.private_note,
            reply_deadline_time=payload.reply_deadline_time,
        )
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))
    return assignment

@router.get("/my/today", response_model=list[ActivityRead])
def my_today_activities(
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    today = datetime.now(timezone.utc).date()
    start_of_day = datetime(today.year, today.month, today.day, tzinfo=timezone.utc)
    end_of_day = datetime(today.year, today.month, today.day, 23, 59, 59, tzinfo=timezone.utc)

    return (
        db.query(Work)
        .join(Work.work_receivers)
        .filter(
            WorkReceiver.receiver_user_id == current_user.id,
            Work.due_at.between(start_of_day, end_of_day),
            Work.status.in_([WORK_STATUS_PUBLISHED, "in_progress"]),
        )
        .order_by(Work.due_at)
        .all()
    )

@router.get("/my/overdue", response_model=list[ActivityRead])
def my_overdue_activities(
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    now = datetime.now(timezone.utc)
    return (
        db.query(Work)
        .join(Work.work_receivers)
        .filter(
            WorkReceiver.receiver_user_id == current_user.id,
            Work.due_at < now,
            Work.status.in_([WORK_STATUS_PUBLISHED, "in_progress"]),
        )
        .order_by(Work.due_at)
        .all()
    )