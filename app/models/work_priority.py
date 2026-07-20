from datetime import datetime
from sqlalchemy import Integer, String, Boolean, DateTime, LargeBinary
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel

class WorkPriority(TimestampedUUIDModel):
    __tablename__ = "work_priorities"

    priority_number: Mapped[int] = mapped_column(Integer, nullable=False)
    priority_title: Mapped[str] = mapped_column(String(255), nullable=False)
    priority_color: Mapped[str] = mapped_column(String(20), nullable=False)
    priority_icon: Mapped[bytes] = mapped_column(LargeBinary, nullable=False)
    description: Mapped[str | None] = mapped_column(String, nullable=True)  # Comment

    is_deleted: Mapped[bool] = mapped_column(Boolean, default=False)
    deleted_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    # روابط
    works = relationship("Work", back_populates="work_priority")
    work_receivers = relationship("WorkReceiver", back_populates="work_priority")