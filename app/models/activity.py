from datetime import datetime
from sqlalchemy import DateTime, ForeignKey, String, Text
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel
from app.core.constants import ACTIVITY_STATUS_DRAFT

class Activity(TimestampedUUIDModel):
    __tablename__ = "activities"

    workspace_id: Mapped[str] = mapped_column(ForeignKey("workspaces.id"), nullable=False, index=True)
    created_by_user_id: Mapped[str | None] = mapped_column(ForeignKey("users.id"), nullable=True, index=True)

    title: Mapped[str] = mapped_column(String(250), nullable=False, index=True)
    description: Mapped[str | None] = mapped_column(Text, nullable=True)
    activity_type: Mapped[str] = mapped_column(String(80), nullable=False, default="general")
    status: Mapped[str] = mapped_column(String(40), nullable=False, default=ACTIVITY_STATUS_DRAFT)
    due_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    workspace = relationship("Workspace", back_populates="activities")
    assignments = relationship("Assignment", back_populates="activity", cascade="all, delete-orphan")
    timeline_events = relationship("TimelineEvent", back_populates="activity", cascade="all, delete-orphan")
