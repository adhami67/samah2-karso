# app/schemas/assignment.py
from datetime import datetime
from typing import Optional
from pydantic import Field
from app.schemas.base import ORMModel

class AssignmentCreate(ORMModel):
    receiver_user_id: str = Field(..., description="شناسه کاربر مجری")
    private_note: Optional[str] = Field(None, description="یادداشت خصوصی")
    reply_deadline_time: Optional[datetime] = Field(None, description="مهلت پاسخ")

class AssignmentUpdate(ORMModel):
    status: Optional[str] = None
    private_note: Optional[str] = None

class AssignmentRead(ORMModel):
    id: str
    work_id: str
    receiver_user_id: str
    status: str
    private_note: Optional[str]
    seen_time: Optional[datetime]
    reply_time: Optional[datetime]
    reply_deadline_time: Optional[datetime]
    in_progress_time: Optional[datetime]
    done_time: Optional[datetime]
    finished_time: Optional[datetime]
    finished_by_user_id: Optional[str]
    created_at: datetime
    updated_at: datetime
    
    receiver_username: Optional[str] = None
    receiver_full_name: Optional[str] = None