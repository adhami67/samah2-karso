from datetime import datetime, timezone
from typing import Optional, List
from sqlalchemy.orm import Session
from sqlalchemy import or_
from app.models.work import Work
from app.models.work_group import WorkGroup
from app.models.work_receiver import WorkReceiver
from app.models.timeline_event import TimelineEvent  # ← جایگزین WorkStatusChangeHistory
from app.models.work_type import WorkType
from app.models.work_priority import WorkPriority
from app.schemas.work import WorkCreate, WorkUpdate, WorkFilter
from app.core.constants import (
    WORK_STATUS_DRAFT,
    WORK_STATUS_PUBLISHED,
    WORK_STATUS_IN_PROGRESS,
    WORK_STATUS_SUBMITTED,
    WORK_STATUS_NEEDS_REVISION,
    WORK_STATUS_APPROVED,
    WORK_STATUS_COMPLETED,
    WORK_STATUS_ARCHIVED,
)
from app.core.exceptions import NotFoundError, InvalidStateTransitionError


class WorkService:
    def __init__(self, db: Session):
        self.db = db

    def create(self, data: WorkCreate, sender_user_id: str) -> Work:
        work_group = self.db.get(WorkGroup, data.work_group_id)
        if not work_group:
            raise NotFoundError(f"WorkGroup with id '{data.work_group_id}' not found")

        work = Work(
            work_group_id=data.work_group_id,
            work_type_id=data.work_type_id,
            work_priority_id=data.work_priority_id,
            sender_user_id=sender_user_id,
            created_by_user_id=sender_user_id,
            subject=data.subject,
            description=data.description,
            due_at=data.due_at,
            status=WORK_STATUS_DRAFT,
        )
        self.db.add(work)
        self.db.flush()

        # ثبت رویداد در TimelineEvent
        self._add_timeline_event(work.id, "created", f"Work created by {sender_user_id}")

        self.db.commit()
        self.db.refresh(work)
        return work

    def get(self, work_id: str) -> Work:
        work = self.db.get(Work, work_id)
        if not work or work.is_deleted:
            raise NotFoundError(f"Work with id '{work_id}' not found")
        return work

    def list(self, filters: WorkFilter, skip: int = 0, limit: int = 100) -> List[Work]:
        query = self.db.query(Work).filter(Work.is_deleted == False)

        if filters.work_group_id:
            query = query.filter(Work.work_group_id == filters.work_group_id)
        if filters.status:
            query = query.filter(Work.status == filters.status)
        if filters.sender_user_id:
            query = query.filter(Work.sender_user_id == filters.sender_user_id)
        if filters.receiver_user_id:
            query = query.join(Work.work_receivers).filter(
                WorkReceiver.receiver_user_id == filters.receiver_user_id
            )
        if filters.search:
            query = query.filter(
                or_(
                    Work.subject.ilike(f"%{filters.search}%"),
                    Work.description.ilike(f"%{filters.search}%")
                )
            )
        if filters.due_from:
            query = query.filter(Work.due_at >= filters.due_from)
        if filters.due_to:
            query = query.filter(Work.due_at <= filters.due_to)

        return query.order_by(Work.created_at.desc()).offset(skip).limit(limit).all()

    def update(self, work_id: str, data: WorkUpdate, user_id: str) -> Work:
        work = self.get(work_id)

        if work.status in [WORK_STATUS_COMPLETED, WORK_STATUS_ARCHIVED]:
            raise InvalidStateTransitionError(
                f"Cannot update work in '{work.status}' status"
            )

        if data.subject is not None:
            work.subject = data.subject
        if data.description is not None:
            work.description = data.description
        if data.work_type_id is not None:
            work.work_type_id = data.work_type_id
        if data.work_priority_id is not None:
            work.work_priority_id = data.work_priority_id
        if data.due_at is not None:
            work.due_at = data.due_at

        self.db.commit()
        self.db.refresh(work)
        self._add_timeline_event(work.id, "updated", f"Work updated by {user_id}")
        return work

    def delete(self, work_id: str, user_id: str) -> None:
        work = self.get(work_id)
        if work.status in [WORK_STATUS_COMPLETED, WORK_STATUS_ARCHIVED]:
            raise InvalidStateTransitionError(
                f"Cannot delete work in '{work.status}' status"
            )

        work.is_deleted = True
        work.deleted_at = datetime.now(timezone.utc)
        self.db.commit()
        self._add_timeline_event(work.id, "archived", f"Work archived by {user_id}")

    def change_status(self, work_id: str, new_status: str, user_id: str, note: Optional[str] = None) -> Work:
        work = self.get(work_id)

        if not self._can_transition(work.status, new_status):
            raise InvalidStateTransitionError(
                f"Transition from '{work.status}' to '{new_status}' is not allowed"
            )

        old_status = work.status
        work.status = new_status
        self.db.commit()
        self.db.refresh(work)

        self._add_timeline_event(
            work.id,
            "status_changed",
            note or f"Status changed from '{old_status}' to '{new_status}' by {user_id}"
        )
        return work

    def _can_transition(self, current: str, target: str) -> bool:
        transitions = {
            WORK_STATUS_DRAFT: [WORK_STATUS_PUBLISHED],
            WORK_STATUS_PUBLISHED: [WORK_STATUS_IN_PROGRESS, WORK_STATUS_ARCHIVED],
            WORK_STATUS_IN_PROGRESS: [WORK_STATUS_SUBMITTED, WORK_STATUS_ARCHIVED],
            WORK_STATUS_SUBMITTED: [WORK_STATUS_NEEDS_REVISION, WORK_STATUS_APPROVED],
            WORK_STATUS_NEEDS_REVISION: [WORK_STATUS_IN_PROGRESS],
            WORK_STATUS_APPROVED: [WORK_STATUS_COMPLETED],
            WORK_STATUS_COMPLETED: [WORK_STATUS_ARCHIVED],
            WORK_STATUS_ARCHIVED: [],
        }
        return target in transitions.get(current, [])

    def _add_timeline_event(self, work_id: str, event_type: str, note: str) -> None:
        event = TimelineEvent(
            activity_id=work_id,  # توجه: در مدل TimelineEvent فیلد activity_id است
            event_type=event_type,
            note=note,
        )
        self.db.add(event)
        self.db.flush()