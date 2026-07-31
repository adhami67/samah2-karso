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
from app.core.exceptions import NotFoundError

router = APIRouter(prefix="/notifications", tags=["Notifications"])


def get_notification_service(db: Session = Depends(get_db)) -> NotificationService:
    return NotificationService(db)


@router.get("/", response_model=List[NotificationResponse])
def get_notifications(
    unread_only: bool = False,
    limit: int = 50,
    service: NotificationService = Depends(get_notification_service),
    current_user: User = Depends(get_current_user),
):
    """دریافت اعلان‌های کاربر فعلی"""
    return service.get_user_notifications(str(current_user.id), unread_only, limit)


@router.get("/unread/count", response_model=UnreadCountResponse)
def get_unread_count(
    service: NotificationService = Depends(get_notification_service),
    current_user: User = Depends(get_current_user),
):
    """تعداد اعلان‌های خوانده‌نشده"""
    count = service.get_unread_count(str(current_user.id))
    return UnreadCountResponse(count=count)


@router.put("/{notification_id}/read", response_model=NotificationResponse)
def mark_as_read(
    notification_id: str,
    service: NotificationService = Depends(get_notification_service),
    current_user: User = Depends(get_current_user),
):
    """علامت‌گذاری اعلان به عنوان خوانده‌شده"""
    try:
        return service.mark_as_read(notification_id, str(current_user.id))
    except (NotFoundError, PermissionError) as e:
        raise HTTPException(status_code=400, detail=str(e))


@router.post("/read-all", status_code=status.HTTP_204_NO_CONTENT)
def mark_all_as_read(
    service: NotificationService = Depends(get_notification_service),
    current_user: User = Depends(get_current_user),
):
    """علامت‌گذاری همه اعلان‌ها به عنوان خوانده‌شده"""
    service.mark_all_as_read(str(current_user.id))
    return


@router.delete("/{notification_id}", status_code=status.HTTP_204_NO_CONTENT)
def delete_notification(
    notification_id: str,
    service: NotificationService = Depends(get_notification_service),
    current_user: User = Depends(get_current_user),
):
    """حذف اعلان (فقط توسط خود کاربر)"""
    try:
        service.delete(notification_id, str(current_user.id))
    except (NotFoundError, PermissionError) as e:
        raise HTTPException(status_code=400, detail=str(e))