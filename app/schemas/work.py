from typing import Optional
from pydantic import BaseModel, Field
from datetime import datetime


class WorkBase(BaseModel):
    """پایه مدل کار"""
    work_group_id: str = Field(..., description="شناسه گروه کاری")
    work_type_id: Optional[str] = Field(None, description="شناسه نوع کار")
    work_priority_id: Optional[str] = Field(None, description="شناسه اولویت کار")
    subject: str = Field(..., max_length=255, description="عنوان کار")
    description: Optional[str] = Field(None, description="توضیحات")
    due_at: Optional[datetime] = Field(None, description="زمان سررسید")


class WorkCreate(WorkBase):
    """ایجاد کار جدید"""
    pass


class WorkUpdate(BaseModel):
    """به‌روزرسانی کار"""
    work_type_id: Optional[str] = None
    work_priority_id: Optional[str] = None
    subject: Optional[str] = Field(None, max_length=255)
    description: Optional[str] = None
    due_at: Optional[datetime] = None
    status: Optional[str] = None  # برای تغییر وضعیت


class WorkFilter(BaseModel):
    """فیلترهای جستجوی کار"""
    work_group_id: Optional[str] = None
    status: Optional[str] = None
    sender_user_id: Optional[str] = None
    receiver_user_id: Optional[str] = None
    search: Optional[str] = None
    due_from: Optional[datetime] = None
    due_to: Optional[datetime] = None


class WorkRead(WorkBase):
    """خروجی کار"""
    id: str
    sender_user_id: str
    created_by_user_id: Optional[str]
    status: str
    sent_at: Optional[datetime]
    is_deleted: bool
    deleted_at: Optional[datetime]
    created_at: datetime
    updated_at: datetime

    class Config:
        from_attributes = True