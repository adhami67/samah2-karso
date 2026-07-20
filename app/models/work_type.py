from datetime import datetime
from sqlalchemy import String, Boolean, DateTime, LargeBinary
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel

class WorkType(TimestampedUUIDModel):
    __tablename__ = "work_types"

    code: Mapped[str] = mapped_column(String(50), nullable=False, unique=True)  # WorkTypeCode
    title: Mapped[str] = mapped_column(String(255), nullable=False)  # WorkTypeTitle
    color: Mapped[str] = mapped_column(String(20), nullable=False)  # WorkTypeColor
    icon: Mapped[bytes] = mapped_column(LargeBinary, nullable=False)  # WorkTypeIcon
    description: Mapped[str | None] = mapped_column(String, nullable=True)  # Comment

    is_deleted: Mapped[bool] = mapped_column(Boolean, default=False)
    deleted_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    # روابط
    works = relationship("Work", back_populates="work_type")
    work_receivers = relationship("WorkReceiver", back_populates="work_type")