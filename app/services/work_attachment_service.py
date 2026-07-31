import os
import shutil
from typing import Optional, List
from datetime import datetime, timezone
from uuid import uuid4
from fastapi import UploadFile, HTTPException
from sqlalchemy.orm import Session
from app.models.work_attachment import WorkAttachment
from app.models.work import Work
from app.models.work_message import WorkMessage
from app.core.config import settings
from app.core.exceptions import NotFoundError, ValidationError


class WorkAttachmentService:
    def __init__(self, db: Session):
        self.db = db
        self.upload_dir = settings.UPLOAD_DIR  # باید در config تعریف شود

    # ========== آپلود فایل ==========

    async def upload_file(
        self,
        file: UploadFile,
        work_id: Optional[str] = None,
        message_id: Optional[str] = None,
        uploaded_by_user_id: Optional[str] = None,
        description: Optional[str] = None,
    ) -> WorkAttachment:
        """
        آپلود فایل و اتصال به کار یا پیام
        - حداقل یکی از work_id یا message_id باید پر شود.
        """
        # اعتبارسنجی
        if not work_id and not message_id:
            raise ValidationError("Either work_id or message_id must be provided")

        # بررسی وجود کار یا پیام
        if work_id:
            work = self.db.get(Work, work_id)
            if not work or work.is_deleted:
                raise NotFoundError(f"Work with id '{work_id}' not found")
        if message_id:
            message = self.db.get(WorkMessage, message_id)
            if not message or message.is_deleted:
                raise NotFoundError(f"Message with id '{message_id}' not found")

        # تولید نام فایل یکتا
        file_extension = os.path.splitext(file.filename)[1]
        unique_filename = f"{uuid4().hex}{file_extension}"
        file_path = os.path.join(self.upload_dir, unique_filename)

        # ذخیره فایل
        os.makedirs(self.upload_dir, exist_ok=True)
        with open(file_path, "wb") as buffer:
            shutil.copyfileobj(file.file, buffer)

        # ایجاد رکورد در دیتابیس
        attachment = WorkAttachment(
            work_id=work_id,
            work_message_id=message_id,
            file_name=file.filename,
            file_path=file_path,
            file_size=os.path.getsize(file_path),
            mime_type=file.content_type or "application/octet-stream",
            uploaded_by_user_id=uploaded_by_user_id,
            description=description,
        )
        self.db.add(attachment)
        self.db.commit()
        self.db.refresh(attachment)

        return attachment

    # ========== دریافت فایل‌ها ==========

    def get_attachments_for_work(self, work_id: str) -> List[WorkAttachment]:
        """دریافت همه فایل‌های پیوست یک کار"""
        work = self.db.get(Work, work_id)
        if not work or work.is_deleted:
            raise NotFoundError(f"Work with id '{work_id}' not found")
        return work.work_attachments

    def get_attachments_for_message(self, message_id: str) -> List[WorkAttachment]:
        """دریافت همه فایل‌های پیوست یک پیام"""
        message = self.db.get(WorkMessage, message_id)
        if not message or message.is_deleted:
            raise NotFoundError(f"Message with id '{message_id}' not found")
        return message.work_attachments

    def get_attachment(self, attachment_id: str) -> WorkAttachment:
        """دریافت یک فایل پیوست با شناسه"""
        attachment = self.db.get(WorkAttachment, attachment_id)
        if not attachment or attachment.is_deleted:
            raise NotFoundError(f"Attachment with id '{attachment_id}' not found")
        return attachment

    # ========== دانلود فایل ==========

    def get_file_path(self, attachment_id: str) -> str:
        """دریافت مسیر فایل برای دانلود"""
        attachment = self.get_attachment(attachment_id)
        if not os.path.exists(attachment.file_path):
            raise NotFoundError(f"File not found on disk: {attachment.file_path}")
        return attachment.file_path

    # ========== حذف فایل ==========

    def delete_attachment(self, attachment_id: str, user_id: str) -> None:
        """حذف نرم فایل پیوست (فقط توسط آپلودکننده)"""
        attachment = self.get_attachment(attachment_id)

        if attachment.uploaded_by_user_id and attachment.uploaded_by_user_id != user_id:
            raise PermissionError("You can only delete your own attachments")

        # حذف فایل از دیسک
        if os.path.exists(attachment.file_path):
            os.remove(attachment.file_path)

        # حذف نرم از دیتابیس
        attachment.is_deleted = True
        attachment.deleted_at = datetime.now(timezone.utc)
        self.db.commit()

    # ========== متدهای کمکی ==========

    def get_total_size_for_work(self, work_id: str) -> int:
        """دریافت مجموع حجم فایل‌های پیوست یک کار"""
        attachments = self.get_attachments_for_work(work_id)
        return sum(att.file_size for att in attachments if not att.is_deleted)

    def get_total_size_for_message(self, message_id: str) -> int:
        """دریافت مجموع حجم فایل‌های پیوست یک پیام"""
        attachments = self.get_attachments_for_message(message_id)
        return sum(att.file_size for att in attachments if not att.is_deleted)