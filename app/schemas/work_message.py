from typing import Optional
from pydantic import BaseModel, Field
from datetime import datetime


class MessageCreate(BaseModel):
    """ایجاد پیام جدید"""
    subject: str = Field(..., max_length=255, description="عنوان پیام")
    body: Optional[str] = Field(None, description="متن پیام")
    parent_message_id: Optional[str] = Field(None, description="شناسه پیام والد (برای پاسخ)")


class MessageUpdate(BaseModel):
    """ویرایش پیام"""
    subject: Optional[str] = Field(None, max_length=255)
    body: Optional[str] = Field(None)


class MessageResponse(BaseModel):
    """خروجی پیام"""
    id: str
    work_id: str
    sender_user_id: str
    subject: str
    body: Optional[str]
    parent_message_id: Optional[str]
    sent_at: datetime
    is_deleted: bool
    replies: list["MessageResponse"] = []  # پاسخ‌ها

    class Config:
        from_attributes = True


class UnseenCountResponse(BaseModel):
    """تعداد پیام‌های دیده‌نشده"""
    work_id: str
    unseen_count: int