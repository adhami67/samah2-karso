# app/schemas/activity.py
from datetime import datetime
from typing import Optional, List
from pydantic import Field
from app.schemas.base import ORMModel

class ActivityCreate(ORMModel):
    work_group_id: str = Field(..., description="شناسه حوزه")
    work_type_id: str = Field(..., description="شناسه نوع کار")
    work_priority_id: str = Field(..., description="شناسه اولویت")
    parent_work_id: Optional[str] = Field(None, description="شناسه کار والد")
    
    subject: str = Field(min_length=1, max_length=255, description="عنوان کار")
    description: Optional[str] = Field(None, description="توضیحات")
    due_at: Optional[datetime] = Field(None, description="مهلت انجام")
    activity_type: Optional[str] = Field("task", description="نوع فعالیت")   # ← اضافه شد
    
    receiver_user_ids: List[str] = Field([], description="لیست شناسه‌های مجریان")  # ← حالا اختیاری با مقدار پیش‌فرض خالی

class ActivityUpdate(ORMModel):
    subject: Optional[str] = Field(None, min_length=1, max_length=255)
    description: Optional[str] = None
    status: Optional[str] = None
    due_at: Optional[datetime] = None
    activity_type: Optional[str] = None   # ← اضافه شد

class ActivityRead(ORMModel):
    id: str
    work_group_id: str
    work_type_id: str
    work_priority_id: str
    parent_work_id: Optional[str]
    subject: str
    description: Optional[str]
    status: str
    sender_user_id: str
    created_by_user_id: Optional[str]
    sent_at: Optional[datetime]
    due_at: Optional[datetime]
    created_at: datetime
    updated_at: datetime
    
    # فیلدهای اضافی برای نمایش (اختیاری)
    work_group_name: Optional[str] = None
    work_type_name: Optional[str] = None
    work_priority_name: Optional[str] = None
    sender_username: Optional[str] = None
    receivers: Optional[List["ActivityReceiverRead"]] = None

class ActivityReceiverRead(ORMModel):
    id: str
    receiver_user_id: str
    status: str
    seen_time: Optional[datetime]
    reply_time: Optional[datetime]
    reply_deadline_time: Optional[datetime]
    in_progress_time: Optional[datetime]
    done_time: Optional[datetime]
    finished_time: Optional[datetime]
    
    receiver_username: Optional[str] = None
    receiver_full_name: Optional[str] = None