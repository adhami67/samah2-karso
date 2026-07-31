from datetime import datetime, timezone
from typing import List, Optional
from sqlalchemy.orm import Session
from app.models.notification import Notification
from app.models.security import User
from app.core.exceptions import NotFoundError
from app.schemas.notification import NotificationCreate, NotificationUpdate


class NotificationService:
    def __init__(self, db: Session):
        self.db = db

    # ========== ایجاد اعلان ==========

    def create(self, data: NotificationCreate) -> Notification:
        """ایجاد یک اعلان جدید"""
        notification = Notification(
            user_id=data.user_id,
            title=data.title,
            body=data.body,
            link=data.link,
        )
        self.db.add(notification)
        self.db.commit()
        self.db.refresh(notification)
        return notification

    def create_for_users(self, user_ids: List[str], data: NotificationCreate) -> List[Notification]:
        """ایجاد اعلان برای چند کاربر (ارسال انبوه)"""
        notifications = []
        for user_id in user_ids:
            notif = Notification(
                user_id=user_id,
                title=data.title,
                body=data.body,
                link=data.link,
            )
            self.db.add(notif)
            notifications.append(notif)
        self.db.commit()
        for n in notifications:
            self.db.refresh(n)
        return notifications

    # ========== دریافت اعلان‌ها ==========

    def get_user_notifications(self, user_id: str, unread_only: bool = False, limit: int = 50) -> List[Notification]:
        """دریافت اعلان‌های یک کاربر"""
        query = self.db.query(Notification).filter(
            Notification.user_id == user_id,
            Notification.is_deleted == False,
        )
        if unread_only:
            query = query.filter(Notification.is_read == False)
        return query.order_by(Notification.created_at.desc()).limit(limit).all()

    def get_unread_count(self, user_id: str) -> int:
        """تعداد اعلان‌های خوانده‌نشده"""
        return self.db.query(Notification).filter(
            Notification.user_id == user_id,
            Notification.is_read == False,
            Notification.is_deleted == False,
        ).count()

    # ========== مدیریت خوانده‌شده ==========

    def mark_as_read(self, notification_id: str, user_id: str) -> Notification:
        """علامت‌گذاری یک اعلان به عنوان خوانده‌شده"""
        notification = self.db.get(Notification, notification_id)
        if not notification or notification.is_deleted:
            raise NotFoundError(f"Notification with id '{notification_id}' not found")
        if notification.user_id != user_id:
            raise PermissionError("You can only mark your own notifications as read")

        notification.is_read = True
        notification.read_at = datetime.now(timezone.utc)
        self.db.commit()
        self.db.refresh(notification)
        return notification

    def mark_all_as_read(self, user_id: str) -> int:
        """علامت‌گذاری همه اعلان‌های یک کاربر به عنوان خوانده‌شده"""
        updated = self.db.query(Notification).filter(
            Notification.user_id == user_id,
            Notification.is_read == False,
            Notification.is_deleted == False,
        ).update({
            "is_read": True,
            "read_at": datetime.now(timezone.utc),
        })
        self.db.commit()
        return updated

    # ========== حذف ==========

    def delete(self, notification_id: str, user_id: str) -> None:
        """حذف نرم اعلان"""
        notification = self.db.get(Notification, notification_id)
        if not notification or notification.is_deleted:
            raise NotFoundError(f"Notification with id '{notification_id}' not found")
        if notification.user_id != user_id:
            raise PermissionError("You can only delete your own notifications")

        notification.is_deleted = True
        self.db.commit()