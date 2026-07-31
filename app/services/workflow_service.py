from typing import Optional
from sqlalchemy.orm import Session
from app.models.work import Work
from app.models.work_receiver import WorkReceiver
from app.core.constants import (
    WORK_STATUS_DRAFT,
    WORK_STATUS_PUBLISHED,
    WORK_STATUS_IN_PROGRESS,
    WORK_STATUS_SUBMITTED,
    WORK_STATUS_NEEDS_REVISION,
    WORK_STATUS_APPROVED,
    WORK_STATUS_COMPLETED,
    WORK_STATUS_ARCHIVED,
    WORK_RECEIVER_STATUS_DONE,
)
from app.core.exceptions import NotFoundError, InvalidStateTransitionError
from app.services.work_service import WorkService
from app.services.work_receiver_service import WorkReceiverService


class WorkflowService:
    def __init__(self, db: Session):
        self.db = db
        self.work_service = WorkService(db)
        self.receiver_service = WorkReceiverService(db)

    def assign_work(self, work_id: str, receiver_user_id: str, assigned_by_user_id: str) -> WorkReceiver:
        return self.receiver_service.assign(work_id, receiver_user_id, assigned_by_user_id)

    def start_work(self, work_receiver_id: str, user_id: str) -> WorkReceiver:
        return self.receiver_service.mark_as_in_progress(work_receiver_id, user_id)

    def submit_response(self, work_receiver_id: str, body: str, user_id: str) -> WorkReceiver:
        return self.receiver_service.submit_response(work_receiver_id, body, user_id)

    def evaluate_response(
        self,
        work_receiver_id: str,
        score: int,
        note: str,
        decision: str,
        evaluator_user_id: str
    ) -> WorkReceiver:
        return self.receiver_service.evaluate_response(
            work_receiver_id, score, note, decision, evaluator_user_id
        )

    def complete_work(self, work_id: str, user_id: str) -> Work:
        return self.receiver_service.complete_work(work_id, user_id)

    def archive_work(self, work_id: str, user_id: str) -> Work:
        work = self.work_service.get(work_id)
        if work.status not in [WORK_STATUS_COMPLETED, WORK_STATUS_APPROVED]:
            raise InvalidStateTransitionError(
                f"Cannot archive work in '{work.status}' status"
            )
        return self.work_service.change_status(work_id, WORK_STATUS_ARCHIVED, user_id, "Work archived")

    def get_work_status_summary(self, work_id: str) -> dict:
        work = self.work_service.get(work_id)
        receivers = work.work_receivers

        return {
            "work_id": work.id,
            "status": work.status,
            "receivers_count": len(receivers),
            "responses_count": sum(1 for r in receivers if r.status == WORK_RECEIVER_STATUS_DONE),
            "last_event": work.timeline_events[-1].note if work.timeline_events else None,
        }

    def get_workflow_timeline(self, work_id: str) -> list:
        work = self.work_service.get(work_id)
        return [
            {
                "time": event.created_at,
                "event_type": event.event_type,
                "note": event.note,
            }
            for event in work.timeline_events
        ]