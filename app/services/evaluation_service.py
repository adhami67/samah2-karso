from sqlalchemy.orm import Session
from app.models.evaluation import Evaluation
from app.models.response import Response
from app.models.timeline_event import TimelineEvent
from app.schemas.evaluation import EvaluationCreate
from app.services.workflow import can_transition_activity


class EvaluationService:
    def __init__(self, db: Session):
        self.db = db

    def submit_evaluation(self, data: EvaluationCreate) -> Evaluation:
        # ۱. بررسی وجود پاسخ
        response = self.db.get(Response, data.response_id)
        if not response:
            raise ValueError("Response not found")

        # ۲. جلوگیری از ارزیابی تکراری (هر پاسخ فقط یک ارزیابی)
        if response.evaluation is not None:
            raise ValueError("This response already has an evaluation")

        # ۳. دریافت Activity از طریق response -> assignment -> activity
        assignment = response.assignment
        if not assignment:
            raise ValueError("Assignment not found for this response")
        activity = assignment.activity
        if not activity:
            raise ValueError("Activity not found")

        # ۴. بررسی مجاز بودن انتقال وضعیت Activity
        target_status = data.status  # "approved" یا "needs_revision"
        if not can_transition_activity(activity.status, target_status):
            raise ValueError(
                f"Cannot transition activity from '{activity.status}' to '{target_status}'"
            )

        # ۵. ایجاد ارزیابی
        evaluation = Evaluation(
            response_id=data.response_id,
            score=data.score,
            note=data.note,
            status=target_status
        )
        self.db.add(evaluation)
        self.db.flush()

        # ۶. تغییر وضعیت Activity
        activity.status = target_status

        # ۷. ثبت TimelineEvent
        event = TimelineEvent(
            activity_id=activity.id,
            event_type="evaluation_submitted",
            note=f"Evaluation submitted with decision: {target_status}"
        )
        self.db.add(event)

        # ۸. ذخیره همه تغییرات
        self.db.commit()
        self.db.refresh(evaluation)
        return evaluation