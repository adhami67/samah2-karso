from datetime import datetime
from sqlalchemy import String, Boolean, DateTime, LargeBinary
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel


class WorkType(TimestampedUUIDModel):
    __tablename__ = "work_types"

    code: Mapped[str] = mapped_column(String(50), nullable=False, unique=True)
    title: Mapped[str] = mapped_column(String(255), nullable=False)
    color: Mapped[str] = mapped_column(String(20), nullable=False)
    icon: Mapped[bytes] = mapped_column(LargeBinary, nullable=False)
    description: Mapped[str | None] = mapped_column(String, nullable=True)

    is_deleted: Mapped[bool] = mapped_column(Boolean, default=False)
    deleted_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    # فقط رابطه با Work
    works = relationship("Work", back_populates="work_type")
    # ❌ حذف: work_receivers = relationship("WorkReceiver", back_populates="work_type")