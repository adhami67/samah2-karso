from datetime import datetime, timezone
from typing import Optional, List
from sqlalchemy.orm import Session
from app.models.work_message import WorkMessage
from app.models.work_message_seen_history import WorkMessageSeenHistory
from app.models.work import Work
from app.models.security import User
from app.core.exceptions import NotFoundError
from app.schemas.work_message import MessageCreate, MessageUpdate


class WorkMessageService:
    def __init__(self, db: Session):
        self.db = db

    # ========== ارسال پیام ==========

    def send_message(
        self,
        work_id: str,
        sender_user_id: str,
        data: MessageCreate
    ) -> WorkMessage:
        """ارسال پیام جدید در سطح یک کار"""
        # بررسی وجود کار
        work = self.db.get(Work, work_id)
        if not work or work.is_deleted:
            raise NotFoundError(f"Work with id '{work_id}' not found")

        # ایجاد پیام
        message = WorkMessage(
            work_id=work_id,
            sender_user_id=sender_user_id,
            subject=data.subject,
            body=data.body,
            parent_message_id=data.parent_message_id,  # برای پاسخ به پیام
            sent_at=datetime.now(timezone.utc),
        )
        self.db.add(message)
        self.db.flush()

        # ثبت تاریخچه دیده شدن برای فرستنده (خودش)
        self._mark_as_seen(message.id, sender_user_id)

        self.db.commit()
        self.db.refresh(message)
        return message

    # ========== دریافت پیام‌ها ==========

    def get_messages_for_work(self, work_id: str) -> List[WorkMessage]:
        """دریافت همه پیام‌های یک کار (به‌همراه پاسخ‌ها)"""
        work = self.db.get(Work, work_id)
        if not work or work.is_deleted:
            raise NotFoundError(f"Work with id '{work_id}' not found")

        return (
            self.db.query(WorkMessage)
            .filter(WorkMessage.work_id == work_id, WorkMessage.is_deleted == False)
            .order_by(WorkMessage.sent_at.asc())
            .all()
        )

    def get_thread(self, message_id: str) -> List[WorkMessage]:
        """دریافت یک پیام و همه پاسخ‌های آن (رشته گفتگو)"""
        message = self.db.get(WorkMessage, message_id)
        if not message or message.is_deleted:
            raise NotFoundError(f"Message with id '{message_id}' not found")

        # پیام اصلی و همه پاسخ‌ها (با استفاده از رابطه parent در مدل)
        return [message] + message.replies

    # ========== مدیریت دیده شدن ==========

    def mark_as_seen(self, message_id: str, user_id: str) -> None:
        """ثبت دیده شدن پیام توسط یک کاربر"""
        self._mark_as_seen(message_id, user_id)
        self.db.commit()

    def _mark_as_seen(self, message_id: str, user_id: str) -> None:
        """داخلی: ثبت دیده شدن پیام"""
        message = self.db.get(WorkMessage, message_id)
        if not message or message.is_deleted:
            raise NotFoundError(f"Message with id '{message_id}' not found")

        # جلوگیری از ثبت تکراری
        existing = (
            self.db.query(WorkMessageSeenHistory)
            .filter(
                WorkMessageSeenHistory.work_message_id == message_id,
                WorkMessageSeenHistory.receiver_user_id == user_id,
            )
            .first()
        )
        if existing:
            return

        seen = WorkMessageSeenHistory(
            work_message_id=message_id,
            receiver_user_id=user_id,
            seen_time=datetime.now(timezone.utc),
        )
        self.db.add(seen)

    def get_unseen_count(self, work_id: str, user_id: str) -> int:
        """دریافت تعداد پیام‌های دیده‌نشده برای یک کاربر در یک کار"""
        # همه پیام‌های کار
        messages = (
            self.db.query(WorkMessage)
            .filter(WorkMessage.work_id == work_id, WorkMessage.is_deleted == False)
            .all()
        )

        unseen = 0
        for msg in messages:
            # اگر پیام توسط کاربر دیده نشده باشد
            seen = (
                self.db.query(WorkMessageSeenHistory)
                .filter(
                    WorkMessageSeenHistory.work_message_id == msg.id,
                    WorkMessageSeenHistory.receiver_user_id == user_id,
                )
                .first()
            )
            if not seen:
                unseen += 1

        return unseen

    # ========== ویرایش و حذف ==========

    def update_message(self, message_id: str, data: MessageUpdate, user_id: str) -> WorkMessage:
        """ویرایش پیام (فقط توسط فرستنده و تا قبل از دیده شدن)"""
        message = self.db.get(WorkMessage, message_id)
        if not message or message.is_deleted:
            raise NotFoundError(f"Message with id '{message_id}' not found")

        # فقط فرستنده می‌تواند ویرایش کند
        if message.sender_user_id != user_id:
            raise PermissionError("You can only edit your own messages")

        # اگر پیام دیده شده باشد، نمی‌توان ویرایش کرد
        seen_count = (
            self.db.query(WorkMessageSeenHistory)
            .filter(WorkMessageSeenHistory.work_message_id == message_id)
            .count()
        )
        if seen_count > 0:
            raise PermissionError("Cannot edit a message that has been seen")

        if data.subject is not None:
            message.subject = data.subject
        if data.body is not None:
            message.body = data.body

        self.db.commit()
        self.db.refresh(message)
        return message

    def delete_message(self, message_id: str, user_id: str) -> None:
        """حذف نرم پیام (فقط توسط فرستنده)"""
        message = self.db.get(WorkMessage, message_id)
        if not message or message.is_deleted:
            raise NotFoundError(f"Message with id '{message_id}' not found")

        if message.sender_user_id != user_id:
            raise PermissionError("You can only delete your own messages")

        message.is_deleted = True
        message.deleted_at = datetime.now(timezone.utc)
        self.db.commit()