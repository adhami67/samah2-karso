from fastapi import APIRouter, Depends, Query
from sqlalchemy.orm import Session
from typing import List, Optional
from app.db.deps import get_db
from app.models.timeline_event import TimelineEvent
from app.schemas.timeline_event import TimelineEventRead

router = APIRouter()

@router.get("/", response_model=List[TimelineEventRead])
def get_timeline_events(
    activity_id: Optional[str] = Query(None),
    db: Session = Depends(get_db)
):
    query = db.query(TimelineEvent)
    if activity_id:
        query = query.filter(TimelineEvent.activity_id == activity_id)
    return query.order_by(TimelineEvent.created_at.desc()).all()