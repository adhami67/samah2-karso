from app.schemas.base import ORMModel

class AssignmentCreate(ORMModel):
    activity_id: str
    assignee_id: str
    assignee_type: str = "user"
    role: str = "executor"

class AssignmentRead(ORMModel):
    id: str
    activity_id: str
    assignee_id: str
    assignee_type: str
    role: str
    status: str
