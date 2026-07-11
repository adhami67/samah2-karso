from sqlalchemy import ForeignKey, Integer, String, Text
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel

class Evaluation(TimestampedUUIDModel):
    __tablename__ = "evaluations"

    response_id: Mapped[str] = mapped_column(ForeignKey("responses.id"), nullable=False, index=True)
    score: Mapped[int | None] = mapped_column(Integer, nullable=True)
    note: Mapped[str | None] = mapped_column(Text, nullable=True)
    status: Mapped[str] = mapped_column(String(40), nullable=False, default="approved")

    response = relationship("Response", back_populates="evaluation")
