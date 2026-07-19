from sqlalchemy.orm import Session
from app.models.assignment import Assignment
from app.models.response import Response
from app.models.timeline_event import TimelineEvent
from app.schemas.response import ResponseCreate
from app.services.workflow import can_transition_activity


class ResponseService:
    def __init__(self, db: Session):
        self.db = db

    def submit_response(self, data: ResponseCreate) -> Response:
        # ۱. بررسی وجود assignment
        assignment = self.db.get(Assignment, data.assignment_id)
        if not assignment:
            raise ValueError("Assignment not found")

        # ۲. دریافت Activity
        activity = assignment.activity
        if not activity:
            raise ValueError("Activity related to assignment not found")

        # ۳. مدیریت وضعیت needs_revision (اصلاح خودکار)
        if activity.status == "needs_revision":
            if not can_transition_activity(activity.status, "in_progress"):
                raise ValueError(
                    f"Cannot transition activity from '{activity.status}' to 'in_progress'"
                )
            activity.status = "in_progress"
            # ثبت رویداد اصلاح
            self.db.add(TimelineEvent(
                activity_id=activity.id,
                event_type="revision_started",
                note="User started revision based on evaluation feedback"
            ))

        # ۴. ثبت پاسخ جدید
        response = Response(
            assignment_id=data.assignment_id,
            body=data.body,
        )
        self.db.add(response)
        self.db.flush()

        # ۵. تغییر وضعیت به submitted (با رعایت قوانین)
        if not can_transition_activity(activity.status, "submitted"):
            raise ValueError(
                f"Cannot transition activity from '{activity.status}' to 'submitted'"
            )
        activity.status = "submitted"

        # ۶. ثبت رویداد پاسخ
        self.db.add(TimelineEvent(
            activity_id=activity.id,
            event_type="response_submitted",
            note=f"Response submitted by assignee for assignment {assignment.id}"
        ))

        # ۷. ذخیره همه تغییرات
        self.db.commit()
        self.db.refresh(response)
        return response