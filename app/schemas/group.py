from pydantic import Field
from app.schemas.base import ORMModel

class GroupCreate(ORMModel):
    workspace_id: str
    name: str = Field(min_length=1, max_length=200)
    kind: str = "custom"
    description: str | None = None

class GroupRead(ORMModel):
    id: str
    workspace_id: str
    name: str
    kind: str
    description: str | None
