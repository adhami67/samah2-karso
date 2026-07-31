from sqlalchemy import String, Text, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel

class TimelineEvent(TimestampedUUIDModel):
    __tablename__ = "timeline_events"

    activity_id: Mapped[str] = mapped_column(ForeignKey("works.id"), nullable=False, index=True)
    event_type: Mapped[str] = mapped_column(String(80), nullable=False)
    note: Mapped[str | None] = mapped_column(Text, nullable=True)

    activity = relationship("Work", back_populates="timeline_events")