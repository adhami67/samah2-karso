from datetime import datetime
from sqlalchemy import ForeignKey, Boolean, String, DateTime
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel

class WorkPermission(TimestampedUUIDModel):
    __tablename__ = "work_permissions"

    work_id: Mapped[str] = mapped_column(ForeignKey("works.id"), nullable=False, index=True)

    finish_work_by_receiver: Mapped[bool] = mapped_column(Boolean, default=False)
    finish_work_by_follower: Mapped[bool] = mapped_column(Boolean, default=False)
    send_read_receipt: Mapped[bool] = mapped_column(Boolean, default=False)
    send_work_change_state_receipt: Mapped[bool] = mapped_column(Boolean, default=False)
    allow_seeing_previous_workflow: Mapped[bool] = mapped_column(Boolean, default=False)

    description: Mapped[str | None] = mapped_column(String, nullable=True)  # Comment

    is_deleted: Mapped[bool] = mapped_column(Boolean, default=False)
    deleted_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    # روابط
    work = relationship("Work", back_populates="work_permissions")