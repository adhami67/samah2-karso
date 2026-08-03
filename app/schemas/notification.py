from typing import Optional
from pydantic import BaseModel
from datetime import datetime


class NotificationCreate(BaseModel):
    user_id: str
    title: str
    body: Optional[str] = None
    link: Optional[str] = None


class NotificationResponse(BaseModel):
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
    count: int