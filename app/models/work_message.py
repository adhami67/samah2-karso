# app/models/work_message.py
from datetime import datetime
from sqlalchemy import DateTime, ForeignKey, String, Boolean, Text
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel

class WorkMessage(TimestampedUUIDModel):
    __tablename__ = "work_messages"

    work_id: Mapped[str] = mapped_column(ForeignKey("works.id"), nullable=False, index=True)
    parent_message_id: Mapped[str | None] = mapped_column(ForeignKey("work_messages.id"), nullable=True)
    sender_user_id: Mapped[str] = mapped_column(ForeignKey("users.id"), nullable=False, index=True)

    subject: Mapped[str] = mapped_column(String(255), nullable=False)
    body: Mapped[str | None] = mapped_column(Text, nullable=True)
    sent_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    is_deleted: Mapped[bool] = mapped_column(Boolean, default=False)
    deleted_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    work = relationship("Work", back_populates="work_messages")
    parent = relationship("WorkMessage", remote_side="WorkMessage.id", back_populates="replies")
    replies = relationship("WorkMessage", back_populates="parent")
    sender_user = relationship("User", foreign_keys=[sender_user_id])
    # اضافه کردن به کلاس WorkMessage
    seen_histories = relationship("WorkMessageSeenHistory", back_populates="work_message", cascade="all, delete-orphan")