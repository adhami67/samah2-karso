from app.schemas.base import ORMModel

class ResponseCreate(ORMModel):
    assignment_id: str
    body: str | None = None

class ResponseRead(ORMModel):
    id: str
    assignment_id: str
    body: str | None
    status: str
