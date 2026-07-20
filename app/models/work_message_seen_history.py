from datetime import datetime
from sqlalchemy import ForeignKey, DateTime, String, Boolean
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel

class WorkMessageSeenHistory(TimestampedUUIDModel):
    __tablename__ = "work_message_seen_histories"

    work_message_id: Mapped[str] = mapped_column(ForeignKey("work_messages.id"), nullable=False, index=True)
    receiver_user_id: Mapped[str] = mapped_column(ForeignKey("users.id"), nullable=False, index=True)

    seen_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)
    description: Mapped[str | None] = mapped_column(String, nullable=True)  # Comment

    is_deleted: Mapped[bool] = mapped_column(Boolean, default=False)
    deleted_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    # روابط
    work_message = relationship("WorkMessage", back_populates="seen_histories")
    receiver_user = relationship("User", foreign_keys=[receiver_user_id])