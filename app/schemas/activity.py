from datetime import datetime
from pydantic import Field
from app.schemas.base import ORMModel

class ActivityCreate(ORMModel):
    workspace_id: str

    title: str = Field(min_length=1, max_length=250)

    description: str | None = None

    activity_type: str = "general"

    due_at: datetime | None = None

    assignee_id: str | None = None

    assignee_type: str = "user"   # user | group

    role: str = "executor"

class ActivityUpdate(ORMModel):
    title: str | None = Field(default=None, min_length=1, max_length=250)
    description: str | None = None
    activity_type: str | None = None
    status: str | None = None
    due_at: datetime | None = None

class ActivityRead(ORMModel):
    id: str
    workspace_id: str
    created_by_user_id: str | None
    title: str
    description: str | None
    activity_type: str
    status: str
    due_at: datetime | None
