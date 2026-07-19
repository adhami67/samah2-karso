from pydantic import BaseModel
from typing import Optional
from datetime import datetime


class AssignmentCreate(BaseModel):
    assignee_id: str
    role: Optional[str] = "executor"


class AssignmentRead(BaseModel):
    id: str
    activity_id: str
    assignee_id: str
    assignee_type: str
    role: str
    created_at: datetime

    class Config:
        from_attributes = True