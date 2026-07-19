from app.schemas.base import ORMModel
from datetime import datetime

class TimelineEventRead(ORMModel):
    id: str
    activity_id: str
    event_type: str
    note: str | None
    created_at: datetime
