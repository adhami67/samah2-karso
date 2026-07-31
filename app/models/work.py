from datetime import datetime
from sqlalchemy import DateTime, ForeignKey, String, Text, Boolean
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel
from app.core.constants import WORK_STATUS_DRAFT

class Work(TimestampedUUIDModel):
    __tablename__ = "works"

    # کلیدهای خارجی
    work_group_id: Mapped[str] = mapped_column(ForeignKey("work_groups.id"), nullable=False, index=True)
    work_type_id: Mapped[str] = mapped_column(ForeignKey("work_types.id"), nullable=False, index=True)
    work_priority_id: Mapped[str] = mapped_column(ForeignKey("work_priorities.id"), nullable=False, index=True)
    parent_work_id: Mapped[str | None] = mapped_column(ForeignKey("works.id"), nullable=True, index=True)

    sender_user_id: Mapped[str] = mapped_column(ForeignKey("users.id"), nullable=False, index=True)
    created_by_user_id: Mapped[str | None] = mapped_column(ForeignKey("users.id"), nullable=True, index=True)

    subject: Mapped[str] = mapped_column(String(255), nullable=False, index=True)
    description: Mapped[str | None] = mapped_column(Text, nullable=True)
    status: Mapped[str] = mapped_column(String(40), nullable=False, default=WORK_STATUS_DRAFT)
    sent_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)
    due_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    is_deleted: Mapped[bool] = mapped_column(Boolean, default=False)
    deleted_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    # روابط
    work_group = relationship("WorkGroup", back_populates="works")
    work_type = relationship("WorkType", back_populates="works")
    work_priority = relationship("WorkPriority", back_populates="works")
    parent = relationship("Work", remote_side="Work.id", back_populates="children")
    children = relationship("Work", back_populates="parent")
    sender_user = relationship("User", foreign_keys=[sender_user_id])
    creator_user = relationship("User", foreign_keys=[created_by_user_id])

    work_receivers = relationship("WorkReceiver", back_populates="work", cascade="all, delete-orphan")
    work_messages = relationship("WorkMessage", back_populates="work", cascade="all, delete-orphan")
    work_attachments = relationship("WorkAttachment", back_populates="work", cascade="all, delete-orphan")
    work_permissions = relationship("WorkPermission", back_populates="work", cascade="all, delete-orphan")

    # اصلاح رابطه با TimelineEvent با مشخص کردن foreign_keys
    timeline_events = relationship(
        "TimelineEvent",
        foreign_keys="TimelineEvent.activity_id",  # ← مشخص کردن کلید خارجی
        back_populates="activity",
        cascade="all, delete-orphan"
    )