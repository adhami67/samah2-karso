from datetime import datetime, timedelta, timezone
from typing import Dict, List, Optional
from sqlalchemy.orm import Session
from sqlalchemy import func
from app.models.work import Work
from app.models.work_receiver import WorkReceiver
from app.models.timeline_event import TimelineEvent
from app.models.security import User
from app.models.work_group import WorkGroup
from app.core.constants import (
    WORK_STATUS_COMPLETED,
    WORK_STATUS_ARCHIVED,
    WORK_STATUS_IN_PROGRESS,
    WORK_STATUS_SUBMITTED,
    WORK_STATUS_NEEDS_REVISION,
    WORK_STATUS_APPROVED,
    WORK_STATUS_PUBLISHED,
    WORK_RECEIVER_STATUS_DONE,
    WORK_RECEIVER_STATUS_FINISHED,
)


class ReportService:
    def __init__(self, db: Session):
        self.db = db

    def get_user_stats(self, user_id: str) -> Dict:
        """آمار شخصی کاربر (منطبق با MySummary در داشبورد)"""
        base_query = self.db.query(Work).join(Work.work_receivers).filter(
            WorkReceiver.receiver_user_id == user_id,
            Work.is_deleted == False
        )

        total = base_query.count()
        in_progress = base_query.filter(Work.status == WORK_STATUS_IN_PROGRESS).count()
        submitted = base_query.filter(Work.status == WORK_STATUS_SUBMITTED).count()
        needs_revision = base_query.filter(Work.status == WORK_STATUS_NEEDS_REVISION).count()
        approved = base_query.filter(Work.status == WORK_STATUS_APPROVED).count()
        completed = base_query.filter(Work.status == WORK_STATUS_COMPLETED).count()

        avg_score = None
        try:
            avg_score = self.db.query(func.avg(WorkReceiver.score)).filter(
                WorkReceiver.receiver_user_id == user_id,
                WorkReceiver.score.isnot(None)
            ).scalar()
        except AttributeError:
            pass

        return {
            "total_activities": total,
            "in_progress": in_progress,
            "submitted": submitted,
            "needs_revision": needs_revision,
            "approved": approved,
            "completed": completed,
            "average_score": round(float(avg_score), 2) if avg_score else None,
        }

    def get_system_stats(self) -> Dict:
        """آمار کلی سیستم (برای صفحه گزارش‌های مدیریتی)"""
        total_users = self.db.query(User).filter(User.is_active == True).count()
        total_workspaces = self.db.query(WorkGroup).filter(WorkGroup.is_deleted == False).count()
        total_activities = self.db.query(Work).filter(Work.is_deleted == False).count()
        total_overdue = self.db.query(Work).filter(
            Work.due_at < datetime.now(timezone.utc),
            Work.status.in_([
                WORK_STATUS_PUBLISHED,
                WORK_STATUS_IN_PROGRESS,
                WORK_STATUS_SUBMITTED,
                WORK_STATUS_NEEDS_REVISION
            ]),
            Work.is_deleted == False
        ).count()

        return {
            "total_users": total_users,
            "total_workspaces": total_workspaces,
            "total_activities": total_activities,
            "total_overdue": total_overdue,
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

    def get_today_priorities(self, user_id: str) -> dict:
        """اولویت‌های امروز کاربر را برمی‌گرداند"""
        now = datetime.now(timezone.utc)
        today_start = now.replace(hour=0, minute=0, second=0, microsecond=0)
        today_end = today_start + timedelta(days=1)

        overdue = self.db.query(Work).join(Work.work_receivers).filter(
            WorkReceiver.receiver_user_id == user_id,
            Work.due_at < now,
            Work.status.in_([WORK_STATUS_PUBLISHED, WORK_STATUS_IN_PROGRESS, WORK_STATUS_SUBMITTED, WORK_STATUS_NEEDS_REVISION]),
            Work.is_deleted == False
        ).count()

        due_today = self.db.query(Work).join(Work.work_receivers).filter(
            WorkReceiver.receiver_user_id == user_id,
            Work.due_at.between(today_start, today_end),
            Work.status.in_([WORK_STATUS_PUBLISHED, WORK_STATUS_IN_PROGRESS, WORK_STATUS_SUBMITTED, WORK_STATUS_NEEDS_REVISION]),
            Work.is_deleted == False
        ).count()

        unread_messages = 0
        try:
            from app.models.work_message import WorkMessage
            unread_messages = self.db.query(WorkMessage).filter(
                WorkMessage.sender_user_id != user_id,
                WorkMessage.is_deleted == False
            ).count()
        except ImportError:
            pass

        return {
            "overdue": overdue,
            "due_today": due_today,
            "unread_messages": unread_messages,
            "classes_today": 0,
            "needs_review": 0,
        }