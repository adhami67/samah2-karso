from datetime import datetime
from sqlalchemy import ForeignKey, String, Boolean, Text, DateTime, Integer
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel

class WorkAttachment(TimestampedUUIDModel):
    __tablename__ = "work_attachments"

    work_id: Mapped[str] = mapped_column(ForeignKey("works.id"), nullable=False, index=True)
    attachment_type: Mapped[str] = mapped_column(String(50), nullable=False, default="file")
    file_name: Mapped[str] = mapped_column(String(255), nullable=False)
    file_path: Mapped[str] = mapped_column(String(500), nullable=False)
    file_size: Mapped[int] = mapped_column(Integer, nullable=False, default=0)
    mime_type: Mapped[str] = mapped_column(String(100), nullable=False, default="application/octet-stream")
    description: Mapped[str | None] = mapped_column(Text, nullable=True)

    is_deleted: Mapped[bool] = mapped_column(Boolean, default=False)
    deleted_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    work = relationship("Work", back_populates="work_attachments")