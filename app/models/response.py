from sqlalchemy import ForeignKey, String, Text
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel


class Response(TimestampedUUIDModel):
    __tablename__ = "responses"

    work_receiver_id: Mapped[str] = mapped_column(ForeignKey("work_receivers.id"), nullable=False, index=True)
    body: Mapped[str | None] = mapped_column(Text, nullable=True)
    status: Mapped[str] = mapped_column(String(40), nullable=False, default="submitted")

    # روابط
    work_receiver = relationship("WorkReceiver", back_populates="responses")
    evaluation = relationship("Evaluation", back_populates="response", uselist=False, cascade="all, delete-orphan")