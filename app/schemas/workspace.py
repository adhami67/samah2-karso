from pydantic import Field
from app.schemas.base import ORMModel

class WorkspaceCreate(ORMModel):
    name: str = Field(min_length=1, max_length=200)
    description: str | None = None

class WorkspaceUpdate(ORMModel):
    name: str | None = Field(default=None, min_length=1, max_length=200)
    description: str | None = None
    status: str | None = None

class WorkspaceRead(ORMModel):
    id: str
    name: str
    description: str | None
    status: str
