from sqlalchemy import String, Text
from sqlalchemy.orm import Mapped, mapped_column
from app.models.base import TimestampedUUIDModel

class ActivityTemplate(TimestampedUUIDModel):
    __tablename__ = "activity_templates"

    name: Mapped[str] = mapped_column(String(200), nullable=False, index=True)
    body: Mapped[str | None] = mapped_column(Text, nullable=True)
    status: Mapped[str] = mapped_column(String(40), nullable=False, default="active")
