from datetime import datetime, timezone
from typing import Optional, List
from sqlalchemy.orm import Session
from app.models.work import Work
from app.models.work_receiver import WorkReceiver
from app.models.work_group import WorkGroup
from app.models.security import User
from app.models.timeline_event import TimelineEvent
from app.schemas.activity import ActivityCreate
from app.core.constants import (
    WORK_STATUS_DRAFT,
    WORK_RECEIVER_STATUS_ASSIGNED,
)


class ActivityService:
    def __init__(self, db: Session):
        self.db = db

    def create_activity(self, data: ActivityCreate, current_user_id: str) -> Work:
        workspace = self.db.get(WorkGroup, data.work_group_id)
        if workspace is None:
            raise ValueError("حوزه مورد نظر یافت نشد")

        receivers = self.db.query(User).filter(User.id.in_(data.receiver_user_ids)).all()
        if len(receivers) != len(data.receiver_user_ids):
            raise ValueError("یکی از کاربران مجری یافت نشد")

        work = Work(
            work_group_id=data.work_group_id,
            work_type_id=data.work_type_id,
            work_priority_id=data.work_priority_id,
            parent_work_id=data.parent_work_id,
            subject=data.subject,
            description=data.description,
            due_at=data.due_at,
            sender_user_id=current_user_id,
            created_by_user_id=current_user_id,
            status=WORK_STATUS_DRAFT,
        )
        self.db.add(work)
        self.db.flush()

        for user_id in data.receiver_user_ids:
            receiver = WorkReceiver(
                work_id=work.id,
                receiver_user_id=user_id,
                status=WORK_RECEIVER_STATUS_ASSIGNED,
            )
            self.db.add(receiver)

        event = TimelineEvent(
            activity_id=work.id,
            event_type="activity_created",
            note="کار جدید ایجاد شد",
        )
        self.db.add(event)

        # ایجاد اعلان برای همه مجریان
        from app.services.notification_service import NotificationService
        from app.schemas.notification import NotificationCreate
        notif_service = NotificationService(self.db)
        for user_id in data.receiver_user_ids:
            notif_service.create(NotificationCreate(
                user_id=user_id,
                title="ارجاع جدید",
                body=f"فعالیت '{data.subject}' به شما ارجاع شد",
                link=f"/activities/{work.id}"
            ))

        self.db.commit()
        self.db.refresh(work)
        return work

    def publish_activity(self, work_id: str, user_id: str) -> Work:
        work = self.db.get(Work, work_id)
        if not work:
            raise ValueError("کار مورد نظر یافت نشد")
        if work.sender_user_id != user_id:
            raise ValueError("فقط فرستنده کار می‌تواند آن را منتشر کند")
        if work.status != WORK_STATUS_DRAFT:
            raise ValueError("فقط کارهای در وضعیت پیش‌نویس قابل انتشار هستند")

        work.status = "published"
        work.sent_at = datetime.now(timezone.utc)

        event = TimelineEvent(
            activity_id=work.id,
            event_type="activity_published",
            note="کار منتشر شد",
        )
        self.db.add(event)

        self.db.commit()
        self.db.refresh(work)
        return work

    def assign_to_user(
        self,
        work_id: str,
        receiver_user_id: str,
        private_note: Optional[str] = None,
        reply_deadline_time: Optional[datetime] = None,
    ) -> WorkReceiver:
        work = self.db.get(Work, work_id)
        if not work:
            raise ValueError("کار مورد نظر یافت نشد")

        user = self.db.get(User, receiver_user_id)
        if not user:
            raise ValueError("کاربر مورد نظر یافت نشد")

        receiver = WorkReceiver(
            work_id=work_id,
            receiver_user_id=receiver_user_id,
            status=WORK_RECEIVER_STATUS_ASSIGNED,
            private_note=private_note,
            reply_deadline_time=reply_deadline_time,
        )
        self.db.add(receiver)

        event = TimelineEvent(
            activity_id=work.id,
            event_type="assignment_created",
            note=f"کار به کاربر {user.full_name} ارجاع شد",
        )
        self.db.add(event)

        # ایجاد اعلان برای کاربر جدید
        from app.services.notification_service import NotificationService
        from app.schemas.notification import NotificationCreate
        notif_service = NotificationService(self.db)
        notif_service.create(NotificationCreate(
            user_id=receiver_user_id,
            title="ارجاع جدید",
            body=f"فعالیت '{work.subject}' به شما ارجاع شد",
            link=f"/activities/{work_id}"
        ))

        self.db.commit()
        self.db.refresh(receiver)
        return receiver

    def get_user_activities(self, user_id: str, status: Optional[str] = None):
        query = self.db.query(Work).join(Work.work_receivers).filter(
            WorkReceiver.receiver_user_id == user_id
        )
        if status:
            query = query.filter(Work.status == status)
        return query.order_by(Work.due_at).all()

    def get_sent_activities(self, user_id: str):
        return self.db.query(Work).filter(
            Work.sender_user_id == user_id
        ).order_by(Work.created_at.desc()).all()

    def update_status(self, work_id: str, new_status: str, user_id: str) -> Work:
        work = self.db.get(Work, work_id)
        if not work:
            raise ValueError("کار مورد نظر یافت نشد")

        from app.services.workflow import can_transition_activity
        if not can_transition_activity(work.status, new_status):
            raise ValueError(f"تغییر وضعیت از '{work.status}' به '{new_status}' مجاز نیست")

        work.status = new_status

        event = TimelineEvent(
            activity_id=work.id,
            event_type="status_changed",
            note=f"وضعیت به {new_status} تغییر کرد",
        )
        self.db.add(event)

        self.db.commit()
        self.db.refresh(work)
        return work