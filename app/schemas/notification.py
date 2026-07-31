from typing import Optional
from pydantic import BaseModel
from datetime import datetime


class NotificationCreate(BaseModel):
    """ایجاد اعلان جدید"""
    user_id: str
    title: str
    body: Optional[str] = None
    link: Optional[str] = None


class NotificationUpdate(BaseModel):
    """به‌روزرسانی اعلان (فقط برای خوانده‌شده)"""
    is_read: bool = True


class NotificationResponse(BaseModel):
    """خروجی اعلان"""
    id: str
    user_id: str
    title: str
    body: Optional[str]
    link: Optional[str]
    is_read: bool
    read_at: Optional[datetime]
    created_at: datetime

    class Config:
        from_attributes = True


class UnreadCountResponse(BaseModel):
    """تعداد اعلان‌های خوانده‌نشده"""
    count: int