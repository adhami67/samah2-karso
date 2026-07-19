from sqlalchemy import ForeignKey, String
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel
from app.core.constants import ASSIGNMENT_STATUS_ASSIGNED

class Assignment(TimestampedUUIDModel):
    __tablename__ = "assignments"

    activity_id: Mapped[str] = mapped_column(ForeignKey("activities.id"), nullable=False, index=True)
    created_by_user_id: Mapped[str | None] = mapped_column(
        ForeignKey("users.id"),
        nullable=True,
        index=True,
    )
    assignee_id: Mapped[str] = mapped_column(String(36), nullable=False, index=True)
    assignee_type: Mapped[str] = mapped_column(String(40), nullable=False, default="user")
    role: Mapped[str] = mapped_column(String(60), nullable=False, default="executor")
    status: Mapped[str] = mapped_column(
        String(40),
        nullable=False,
        default=ASSIGNMENT_STATUS_ASSIGNED,
    )
    activity = relationship("Activity", back_populates="assignments")
    responses = relationship("Response", back_populates="assignment", cascade="all, delete-orphan")
