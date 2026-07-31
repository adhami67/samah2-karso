from datetime import datetime
from typing import Optional, List
from pydantic import Field
from app.schemas.base import ORMModel

class WorkspaceCreate(ORMModel):
    name: str = Field(min_length=1, max_length=200, description="نام حوزه")
    description: Optional[str] = Field(None, description="توضیحات")

class WorkspaceUpdate(ORMModel):
    name: Optional[str] = Field(None, min_length=1, max_length=200, description="نام جدید حوزه")
    description: Optional[str] = Field(None, description="توضیحات جدید")
    status: Optional[str] = Field(None, description="وضعیت جدید (active/inactive)")

class WorkspaceMemberRead(ORMModel):
    user_id: str
    username: str
    full_name: Optional[str] = None
    invite_time: Optional[datetime] = None
    join_time: Optional[datetime] = None
    is_active: bool = True

class WorkspaceRead(ORMModel):
    id: str
    name: str
    description: Optional[str]
    status: str
    owner_user_id: str
    created_at: datetime
    updated_at: datetime
    members: Optional[List[WorkspaceMemberRead]] = None
    children: Optional[List["WorkspaceRead"]] = None  # برای زیرحوزه‌ها (خودارجاعی)

    class Config:
        from_attributes = True