from typing import Optional
from pydantic import BaseModel
from datetime import datetime


class AttachmentResponse(BaseModel):
    """خروجی فایل پیوست"""
    id: str
    work_id: Optional[str]
    work_message_id: Optional[str]
    file_name: str
    file_path: str
    file_size: int
    mime_type: str
    uploaded_by_user_id: Optional[str]
    description: Optional[str]
    created_at: datetime
    is_deleted: bool

    class Config:
        from_attributes = True


class AttachmentUploadResponse(BaseModel):
    """پاسخ پس از آپلود فایل"""
    id: str
    file_name: str
    file_size: int
    message: str = "File uploaded successfully"