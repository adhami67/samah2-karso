# app/schemas/student_note.py
from datetime import datetime
from pydantic import BaseModel
from typing import Optional

class StudentNoteCreate(BaseModel):
    student_id: str
    content: str
    category: str = "general"
    is_private: bool = True

class StudentNoteUpdate(BaseModel):
    content: str

class StudentNoteRead(BaseModel):
    id: str
    student_id: str
    teacher_id: str
    content: str
    category: str
    is_private: bool
    created_at: datetime
    updated_at: datetime

    class Config:
        from_attributes = True