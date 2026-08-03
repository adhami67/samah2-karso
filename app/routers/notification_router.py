from fastapi import APIRouter, Depends, Query
from sqlalchemy.orm import Session
from typing import List
from app.db.session import get_db
from app.utils.deps import get_current_user  # ✅ اصلاح مسیر
from app.models.notification import Notification
from app.models.security import User

router = APIRouter()

@router.get("")
def get_notifications(
    limit: int = Query(20, ge=1, le=100),
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """
    دریافت لیست اعلان‌های کاربر فعلی
    """
    notifications = db.query(Notification).filter(
        Notification.user_id == current_user.id,
        Notification.is_deleted == False
    ).order_by(Notification.created_at.desc()).limit(limit).all()
    return notifications

@router.get("/unread-count")
def get_unread_count(
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """
    تعداد اعلان‌های خوانده‌نشده
    """
    count = db.query(Notification).filter(
        Notification.user_id == current_user.id,
        Notification.is_read == False,
        Notification.is_deleted == False
    ).count()
    return {"unread_count": count}