# app/models/work_attachment.py
from datetime import datetime
from sqlalchemy import ForeignKey, String, Boolean
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel

class WorkAttachment(TimestampedUUIDModel):
    __tablename__ = "work_attachments"

    work_id: Mapped[str] = mapped_column(ForeignKey("works.id"), nullable=False, index=True)
    attachment_kind_id: Mapped[str] = mapped_column(ForeignKey("lookup_values.id"), nullable=False)  # یا یک Enum
    table_name: Mapped[str] = mapped_column(String(100), nullable=False)  # مثلاً "Work", "WorkMessage"
    entity_id: Mapped[str | None] = mapped_column(String(36), nullable=True)  # برای اتصال به موجودیت‌های دیگر
    file_name: Mapped[str] = mapped_column(String(255), nullable=False)
    file_path: Mapped[str] = mapped_column(String(500), nullable=False)
    description: Mapped[str | None] = mapped_column(Text, nullable=True)  # Comment

    is_deleted: Mapped[bool] = mapped_column(Boolean, default=False)
    deleted_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    work = relationship("Work", back_populates="work_attachments")