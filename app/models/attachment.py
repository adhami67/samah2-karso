from sqlalchemy import String
from sqlalchemy.orm import Mapped, mapped_column
from app.models.base import TimestampedUUIDModel

class Attachment(TimestampedUUIDModel):
    __tablename__ = "attachments"

    owner_type: Mapped[str] = mapped_column(String(50), nullable=False, index=True)
    owner_id: Mapped[str] = mapped_column(String(36), nullable=False, index=True)
    file_name: Mapped[str] = mapped_column(String(255), nullable=False)
    file_path: Mapped[str] = mapped_column(String(500), nullable=False)
