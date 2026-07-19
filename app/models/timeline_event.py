from sqlalchemy import ForeignKey, String, Text
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel

class TimelineEvent(TimestampedUUIDModel):
    __tablename__ = "timeline_events"

    activity_id: Mapped[str] = mapped_column(
        ForeignKey("activities.id"),
        nullable=False,
        index=True,
    )

    created_by_user_id: Mapped[str | None] = mapped_column(
        ForeignKey("users.id"),
        nullable=True,
        index=True,
    )

    event_type: Mapped[str] = mapped_column(
        String(80),
        nullable=False,
        index=True,
    )

    note: Mapped[str | None] = mapped_column(
        Text,
        nullable=True,
    )

    activity = relationship(
        "Activity",
        back_populates="timeline_events",
    )
