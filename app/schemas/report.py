from pydantic import BaseModel
from typing import Optional


class UserSummary(BaseModel):
    total_activities: int
    completed: int
    in_progress: int
    submitted: int
    needs_revision: int
    approved: int
    average_score: Optional[float] = None


class WorkspaceSummary(BaseModel):
    workspace_name: str
    total_activities: int
    completed: int
    in_progress: int
    submitted: int
    needs_revision: int
    approved: int