from sqlalchemy import String, Text, Boolean
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel
from app.core.constants import WORKGROUP_STATUS_ACTIVE

class WorkGroup(TimestampedUUIDModel):
    __tablename__ = "work_groups"

    name: Mapped[str] = mapped_column(String(200), nullable=False, index=True)  # WorkGroupName
    description: Mapped[str | None] = mapped_column(Text, nullable=True)  # Comment
    status: Mapped[str] = mapped_column(String(40), nullable=False, default=WORKGROUP_STATUS_ACTIVE)  # WorkGroupStateKindId

    owner_user_id: Mapped[str] = mapped_column(ForeignKey("users.id"), nullable=False, index=True)  # OwnerUserId

    is_deleted: Mapped[bool] = mapped_column(Boolean, default=False)
    deleted_at: Mapped[datetime | None] = mapped_column(DateTime(timezone=True), nullable=True)

    # روابط
    groups = relationship("WorkGroupMember", back_populates="work_group", cascade="all, delete-orphan")
    works = relationship("Work", back_populates="work_group", cascade="all, delete-orphan")

    owner_user = relationship("User", foreign_keys=[owner_user_id])