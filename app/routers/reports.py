from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from sqlalchemy import func
from app.db.deps import get_db
from app.models.activity import Activity
from app.models.assignment import Assignment
from app.models.response import Response
from app.models.evaluation import Evaluation
from app.models.workspace import Workspace
from app.schemas.report import UserSummary, WorkspaceSummary
from app.utils.deps import get_current_user
from app.models.security import User

router = APIRouter()


@router.get("/my-summary", response_model=UserSummary)
def my_summary(
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    user_id = current_user.id

    # فعالیت‌هایی که کاربر به عنوان مجری در آن‌ها مشارکت دارد
    assigned_activities = (
        db.query(Activity)
        .join(Activity.assignments)
        .filter(Assignment.assignee_id == user_id)
        .distinct()
    )

    total = assigned_activities.count()

    def count_by_status(status: str) -> int:
        return assigned_activities.filter(Activity.status == status).count()

    # میانگین امتیاز از ارزیابی‌های مربوط به پاسخ‌های این کاربر
    avg_score = (
        db.query(func.avg(Evaluation.score))
        .join(Response, Evaluation.response_id == Response.id)
        .join(Assignment, Response.assignment_id == Assignment.id)
        .filter(Assignment.assignee_id == user_id)
        .scalar()
    )

    return UserSummary(
        total_activities=total,
        completed=count_by_status("completed"),
        in_progress=count_by_status("in_progress"),
        submitted=count_by_status("submitted"),
        needs_revision=count_by_status("needs_revision"),
        approved=count_by_status("approved"),
        average_score=round(float(avg_score), 2) if avg_score else None,
    )


@router.get("/workspace/{workspace_id}", response_model=WorkspaceSummary)
def workspace_summary(
    workspace_id: str,
    db: Session = Depends(get_db),
):
    workspace = db.get(Workspace, workspace_id)
    if not workspace:
        raise HTTPException(status_code=404, detail="Workspace not found")

    activities = db.query(Activity).filter(Activity.workspace_id == workspace_id)

    def count_by_status(status: str) -> int:
        return activities.filter(Activity.status == status).count()

    return WorkspaceSummary(
        workspace_name=workspace.name,
        total_activities=activities.count(),
        completed=count_by_status("completed"),
        in_progress=count_by_status("in_progress"),
        submitted=count_by_status("submitted"),
        needs_revision=count_by_status("needs_revision"),
        approved=count_by_status("approved"),
    )