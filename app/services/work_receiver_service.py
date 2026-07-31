from datetime import datetime, timezone
from typing import Optional, List
from sqlalchemy.orm import Session
from app.models.work import Work
from app.models.work_receiver import WorkReceiver
from app.models.timeline_event import TimelineEvent  # ← جایگزین WorkStatusChangeHistory
from app.core.constants import (
    WORK_STATUS_DRAFT,
    WORK_STATUS_PUBLISHED,
    WORK_STATUS_IN_PROGRESS,
    WORK_STATUS_SUBMITTED,
    WORK_STATUS_NEEDS_REVISION,
    WORK_STATUS_APPROVED,
    WORK_STATUS_COMPLETED,
    WORK_RECEIVER_STATUS_ASSIGNED,
    WORK_RECEIVER_STATUS_SEEN,
    WORK_RECEIVER_STATUS_IN_PROGRESS,
    WORK_RECEIVER_STATUS_DONE,
    WORK_RECEIVER_STATUS_FINISHED,
)
from app.core.exceptions import NotFoundError, InvalidStateTransitionError
from app.services.work_service import WorkService


class WorkReceiverService:
    def __init__(self, db: Session):
        self.db = db
        self.work_service = WorkService(db)

    def assign(self, work_id: str, receiver_user_id: str, assigned_by_user_id: str) -> WorkReceiver:
        work = self.work_service.get(work_id)

        if work.status not in [WORK_STATUS_DRAFT, WORK_STATUS_PUBLISHED]:
            raise InvalidStateTransitionError(
                f"Cannot assign work in '{work.status}' status"
            )

        if work.status == WORK_STATUS_DRAFT:
            work.status = WORK_STATUS_PUBLISHED

        receiver = WorkReceiver(
            work_id=work.id,
            receiver_user_id=receiver_user_id,
            status=WORK_RECEIVER_STATUS_ASSIGNED,
        )
        self.db.add(receiver)
        self.db.flush()

        # ثبت در TimelineEvent
        self._add_timeline_event(work.id, "assigned", f"Work assigned to {receiver_user_id} by {assigned_by_user_id}")

        self.db.commit()
        self.db.refresh(receiver)
        return receiver

    def mark_as_seen(self, work_receiver_id: str, user_id: str) -> WorkReceiver:
        receiver = self._get_receiver(work_receiver_id)
        if receiver.status != WORK_RECEIVER_STATUS_ASSIGNED:
            raise InvalidStateTransitionError(
                f"Cannot mark as seen from status '{receiver.status}'"
            )
        receiver.status = WORK_RECEIVER_STATUS_SEEN
        receiver.seen_time = datetime.now(timezone.utc)

        self.db.commit()
        self.db.refresh(receiver)
        return receiver

    def mark_as_in_progress(self, work_receiver_id: str, user_id: str) -> WorkReceiver:
        receiver = self._get_receiver(work_receiver_id)
        work = receiver.work

        if receiver.status not in [WORK_RECEIVER_STATUS_ASSIGNED, WORK_RECEIVER_STATUS_SEEN]:
            raise InvalidStateTransitionError(
                f"Cannot start work from status '{receiver.status}'"
            )

        receiver.status = WORK_RECEIVER_STATUS_IN_PROGRESS
        receiver.in_progress_time = datetime.now(timezone.utc)

        if work.status == WORK_STATUS_PUBLISHED:
            work.status = WORK_STATUS_IN_PROGRESS
            self._add_timeline_event(work.id, "started", f"Work started by {user_id}")

        self.db.commit()
        self.db.refresh(receiver)
        return receiver

    def submit_response(self, work_receiver_id: str, body: str, user_id: str) -> WorkReceiver:
        receiver = self._get_receiver(work_receiver_id)
        work = receiver.work

        if work.status not in [WORK_STATUS_IN_PROGRESS, WORK_STATUS_NEEDS_REVISION]:
            raise InvalidStateTransitionError(
                f"Cannot submit response for work in '{work.status}' status"
            )

        # ذخیره پاسخ (در صورت وجود فیلد response_body)
        setattr(receiver, 'response_body', body)

        receiver.status = WORK_RECEIVER_STATUS_DONE
        receiver.done_time = datetime.now(timezone.utc)

        work.status = WORK_STATUS_SUBMITTED
        self._add_timeline_event(work.id, "response_submitted", f"Response submitted by {user_id}")

        self.db.commit()
        self.db.refresh(receiver)
        return receiver

    def evaluate_response(
        self,
        work_receiver_id: str,
        score: int,
        note: str,
        decision: str,
        evaluator_user_id: str
    ) -> WorkReceiver:
        receiver = self._get_receiver(work_receiver_id)
        work = receiver.work

        if work.status != WORK_STATUS_SUBMITTED:
            raise InvalidStateTransitionError(
                f"Cannot evaluate work in '{work.status}' status"
            )

        if decision == "approved":
            work.status = WORK_STATUS_APPROVED
        elif decision == "needs_revision":
            work.status = WORK_STATUS_NEEDS_REVISION
        else:
            raise ValueError(f"Invalid decision: {decision}")

        receiver.status = WORK_RECEIVER_STATUS_FINISHED
        receiver.finished_time = datetime.now(timezone.utc)
        receiver.finished_by_user_id = evaluator_user_id

        self._add_timeline_event(
            work.id,
            "evaluated",
            f"Evaluation: {decision} - Score: {score} - {note} by {evaluator_user_id}"
        )

        self.db.commit()
        self.db.refresh(receiver)
        return receiver

    def complete_work(self, work_id: str, user_id: str) -> Work:
        work = self.work_service.get(work_id)
        if work.status != WORK_STATUS_APPROVED:
            raise InvalidStateTransitionError(
                f"Cannot complete work in '{work.status}' status"
            )
        work.status = WORK_STATUS_COMPLETED
        self._add_timeline_event(work.id, "completed", f"Work completed by {user_id}")
        self.db.commit()
        self.db.refresh(work)
        return work

    def _get_receiver(self, work_receiver_id: str) -> WorkReceiver:
        receiver = self.db.get(WorkReceiver, work_receiver_id)
        if not receiver or receiver.is_deleted:
            raise NotFoundError(f"WorkReceiver with id '{work_receiver_id}' not found")
        return receiver

    def _add_timeline_event(self, work_id: str, event_type: str, note: str) -> None:
        """ثبت رویداد در TimelineEvent"""
        event = TimelineEvent(
            activity_id=work_id,
            event_type=event_type,
            note=note,
        )
        self.db.add(event)
        self.db.flush()