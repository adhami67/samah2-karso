from datetime import datetime, timedelta, timezone
from typing import Dict, List, Optional
from sqlalchemy.orm import Session
from sqlalchemy import func
from app.models.work import Work
from app.models.work_receiver import WorkReceiver
from app.models.timeline_event import TimelineEvent
from app.core.constants import (
    WORK_STATUS_COMPLETED,
    WORK_STATUS_ARCHIVED,
    WORK_STATUS_IN_PROGRESS,
    WORK_STATUS_SUBMITTED,
    WORK_STATUS_NEEDS_REVISION,
    WORK_STATUS_APPROVED,
    WORK_RECEIVER_STATUS_DONE,
    WORK_RECEIVER_STATUS_FINISHED,
)


class ReportService:
    def __init__(self, db: Session):
        self.db = db

    def get_user_stats(self, user_id: str) -> Dict:
        """آمار کلی برای یک کاربر (مجری)"""
        receivers = (
            self.db.query(WorkReceiver)
            .filter(WorkReceiver.receiver_user_id == user_id)
            .all()
        )

        total = len(receivers)
        completed = sum(1 for r in receivers if r.status == WORK_RECEIVER_STATUS_FINISHED)
        in_progress = sum(1 for r in receivers if r.status == WORK_RECEIVER_STATUS_IN_PROGRESS)
        done = sum(1 for r in receivers if r.status == WORK_RECEIVER_STATUS_DONE)

        avg_time = None
        finished_receivers = [r for r in receivers if r.finished_time and r.created_at]
        if finished_receivers:
            total_seconds = sum(
                (r.finished_time - r.created_at).total_seconds()
                for r in finished_receivers
            )
            avg_seconds = total_seconds / len(finished_receivers)
            avg_time = timedelta(seconds=avg_seconds)

        return {
            "total_assigned": total,
            "completed": completed,
            "in_progress": in_progress,
            "done": done,
            "average_completion_time": str(avg_time) if avg_time else None,
        }

    def get_system_stats(self) -> Dict:
        """آمار کلی سیستم برای مدیران"""
        total_works = self.db.query(Work).filter(Work.is_deleted == False).count()
        total_receivers = self.db.query(WorkReceiver).filter(WorkReceiver.is_deleted == False).count()

        status_counts = {}
        for status in [WORK_STATUS_IN_PROGRESS, WORK_STATUS_SUBMITTED, WORK_STATUS_NEEDS_REVISION, WORK_STATUS_APPROVED, WORK_STATUS_COMPLETED, WORK_STATUS_ARCHIVED]:
            count = self.db.query(Work).filter(Work.status == status, Work.is_deleted == False).count()
            status_counts[status] = count

        return {
            "total_works": total_works,
            "total_receivers": total_receivers,
            "status_distribution": status_counts,
        }

    def get_today_activities(self, user_id: str) -> List[Work]:
        """کارهایی که امروز باید انجام شوند (برای یک کاربر)"""
        today = datetime.now(timezone.utc).date()
        start_of_day = datetime.combine(today, datetime.min.time(), tzinfo=timezone.utc)
        end_of_day = datetime.combine(today, datetime.max.time(), tzinfo=timezone.utc)

        return (
            self.db.query(Work)
            .join(Work.work_receivers)
            .filter(
                WorkReceiver.receiver_user_id == user_id,
                Work.due_at >= start_of_day,
                Work.due_at <= end_of_day,
                Work.is_deleted == False,
                Work.status.in_([WORK_STATUS_IN_PROGRESS, WORK_STATUS_SUBMITTED, WORK_STATUS_NEEDS_REVISION]),
            )
            .order_by(Work.due_at.asc())
            .all()
        )

    def get_overdue_activities(self, user_id: str) -> List[Work]:
        """کارهای عقب‌افتاده برای یک کاربر"""
        now = datetime.now(timezone.utc)

        return (
            self.db.query(Work)
            .join(Work.work_receivers)
            .filter(
                WorkReceiver.receiver_user_id == user_id,
                Work.due_at < now,
                Work.is_deleted == False,
                Work.status.in_([WORK_STATUS_IN_PROGRESS, WORK_STATUS_SUBMITTED, WORK_STATUS_NEEDS_REVISION]),
            )
            .order_by(Work.due_at.asc())
            .all()
        )

    def get_performance_report(self, start_date: Optional[datetime] = None, end_date: Optional[datetime] = None) -> Dict:
        """گزارش عملکرد کلی در یک بازه زمانی"""
        query = self.db.query(Work).filter(Work.is_deleted == False)

        if start_date:
            query = query.filter(Work.created_at >= start_date)
        if end_date:
            query = query.filter(Work.created_at <= end_date)

        works = query.all()

        total = len(works)
        completed = sum(1 for w in works if w.status == WORK_STATUS_COMPLETED)
        avg_time = None

        completed_works = [w for w in works if w.status == WORK_STATUS_COMPLETED]
        if completed_works:
            total_seconds = 0
            count = 0
            for w in completed_works:
                # پیدا کردن رویداد تکمیل از TimelineEvent
                finished_event = (
                    self.db.query(TimelineEvent)
                    .filter(
                        TimelineEvent.activity_id == w.id,
                        TimelineEvent.event_type == "completed",
                    )
                    .order_by(TimelineEvent.created_at.desc())
                    .first()
                )
                if finished_event and w.created_at:
                    delta = finished_event.created_at - w.created_at
                    total_seconds += delta.total_seconds()
                    count += 1
            if count > 0:
                avg_time = timedelta(seconds=total_seconds / count)

        return {
            "total_works": total,
            "completed": completed,
            "completion_rate": (completed / total * 100) if total > 0 else 0,
            "average_completion_time": str(avg_time) if avg_time else None,
        }