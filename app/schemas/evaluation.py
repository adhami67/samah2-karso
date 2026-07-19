from app.schemas.base import ORMModel

class EvaluationCreate(ORMModel):
    response_id: str
    score: int | None = None
    note: str | None = None
    status: str = "approved"   # "approved" یا "needs_revision"

class EvaluationRead(ORMModel):
    id: str
    response_id: str
    score: int | None
    note: str | None
    status: str