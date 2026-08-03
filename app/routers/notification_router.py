from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from typing import List
from app.db.deps import get_db
from app.services.notification_service import NotificationService
from app.schemas.notification import (
    NotificationCreate,
    NotificationResponse,
    UnreadCountResponse,
)
from app.utils.deps import get_current_user
from app.models.security import User

router = APIRouter(prefix="/api/notifications", tags=["Notifications"])


def get_notification_service(db: Session = Depends(get_db)) -> NotificationService:
    return NotificationService(db)


@router.get("/", response_model=List[NotificationResponse])
def get_notifications(
    unread_only: bool = False,
    limit: int = 50,
    service: NotificationService = Depends(get_notification_service),
    current_user: User = Depends(get_current_user),
):
    return service.get_user_notifications(str(current_user.id), unread_only, limit)


@router.get("/unread-count", response_model=UnreadCountResponse)
def get_unread_count(
    service: NotificationService = Depends(get_notification_service),
    current_user: User = Depends(get_current_user),
):
    count = service.get_unread_count(str(current_user.id))
    return UnreadCountResponse(count=count)


# ========== endpoint جدید برای ایجاد اعلان ==========
@router.post("/", response_model=NotificationResponse, status_code=status.HTTP_201_CREATED)
def create_notification(
    data: NotificationCreate,
    service: NotificationService = Depends(get_notification_service),
    current_user: User = Depends(get_current_user),
):
    """ایجاد یک اعلان جدید (برای تست یا استفاده داخلی)"""
    # توجه: در اینجا هر کاربری (حتی غیر ادمین) می‌تواند اعلان بسازد.
    # در نسخهٔ نهایی می‌توانید محدودیت نقش اضافه کنید.
    return service.create(data)