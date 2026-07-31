# app/schemas/response.py
from datetime import datetime
from typing import Optional
from pydantic import Field
from app.schemas.base import ORMModel

class ResponseCreate(ORMModel):
    work_receiver_id: str = Field(..., description="شناسه ارجاع")
    content: str = Field(..., min_length=1, description="متن پاسخ")
    attachment_ids: Optional[list[str]] = Field(None, description="لیست شناسه پیوست‌ها")

class ResponseUpdate(ORMModel):
    content: Optional[str] = None

class ResponseRead(ORMModel):
    id: str
    work_receiver_id: str
    responder_user_id: str
    content: str
    created_at: datetime
    updated_at: datetime
    
    responder_username: Optional[str] = None