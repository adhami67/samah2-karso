from datetime import datetime
from sqlalchemy import DateTime, ForeignKey, Boolean
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel

class WorkGroupMember(TimestampedUUIDModel):
    __tablename__ = "work_group_members"

    work_group_id: Mapped[str] = mapped_column(ForeignKey("work_groups.id"), nullable=False, index=True)
    member_user_id: Mapped[str] = mapped_column(ForeignKey("users.id"), nullable=False, index=True)

    invite_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)
    join_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)
    leave_time: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    is_deleted: Mapped[bool] = mapped_column(Boolean, default=False)
    deleted_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    work_group = relationship("WorkGroup", back_populates="groups")
    member_user = relationship("User", foreign_keys=[member_user_id])