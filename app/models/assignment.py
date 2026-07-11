from sqlalchemy import ForeignKey, String
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel

class Assignment(TimestampedUUIDModel):
    __tablename__ = "assignments"

    activity_id: Mapped[str] = mapped_column(ForeignKey("activities.id"), nullable=False, index=True)
    assignee_id: Mapped[str] = mapped_column(String(36), nullable=False, index=True)
    assignee_type: Mapped[str] = mapped_column(String(40), nullable=False, default="user")
    role: Mapped[str] = mapped_column(String(60), nullable=False, default="executor")
    status: Mapped[str] = mapped_column(String(40), nullable=False, default="assigned")

    activity = relationship("Activity", back_populates="assignments")
    responses = relationship("Response", back_populates="assignment", cascade="all, delete-orphan")
