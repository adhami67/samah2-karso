from sqlalchemy import String, Text
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel
from app.core.constants import WORKSPACE_STATUS_ACTIVE

class Workspace(TimestampedUUIDModel):
    __tablename__ = "workspaces"

    name: Mapped[str] = mapped_column(String(200), nullable=False, index=True)
    description: Mapped[str | None] = mapped_column(Text, nullable=True)
    status: Mapped[str] = mapped_column(String(40), nullable=False, default=WORKSPACE_STATUS_ACTIVE)

    groups = relationship("Group", back_populates="workspace", cascade="all, delete-orphan")
    activities = relationship("Activity", back_populates="workspace", cascade="all, delete-orphan")
