# app/models/work_receiver.py
from datetime import datetime
from sqlalchemy import DateTime, ForeignKey, String, Boolean, Text
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel
from app.core.constants import WORK_RECEIVER_STATUS_ASSIGNED

class WorkReceiver(TimestampedUUIDModel):
    __tablename__ = "work_receivers"

    work_id: Mapped[str] = mapped_column(ForeignKey("works.id"), nullable=False, index=True)
    receiver_user_id: Mapped[str] = mapped_column(ForeignKey("users.id"), nullable=False, index=True)
    status: Mapped[str] = mapped_column(String(40), nullable=False, default=WORK_RECEIVER_STATUS_ASSIGNED)
    private_note: Mapped[str | None] = mapped_column(Text, nullable=True)

    seen_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)
    reply_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)
    reply_deadline_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)
    forward_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)
    in_progress_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)
    done_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)
    done_deadline_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)
    finished_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)
    finished_by_user_id: Mapped[str | None] = mapped_column(ForeignKey("users.id"), nullable=True)
    finish_deadline_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    is_deleted: Mapped[bool] = mapped_column(Boolean, default=False)
    deleted_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    work = relationship("Work", back_populates="work_receivers")
    receiver_user = relationship("User", foreign_keys=[receiver_user_id])
    finished_by_user = relationship("User", foreign_keys=[finished_by_user_id])

    # اضافه کردن به کلاس WorkReceiver
    work_priority = relationship("WorkPriority", back_populates="work_receivers")
    work_type = relationship("WorkType", back_populates="work_receivers")