# app/services/student_service.py
from sqlalchemy.orm import Session
from app.models.security import User, Role  # ✅ اضافه کردن Role
from app.models.work import Work
from app.models.work_receiver import WorkReceiver
from app.core.constants import WORK_STATUS_IN_PROGRESS, WORK_STATUS_SUBMITTED, WORK_STATUS_NEEDS_REVISION
from datetime import datetime, timezone
from typing import List, Dict

class StudentService:
    def __init__(self, db: Session):
        self.db = db

    def get_students_needing_follow_up(self, teacher_id: str) -> List[Dict]:
        """
        دریافت لیست دانش‌آموزانی که نیاز به پیگیری دارند.
        معیارها:
        - فعالیت‌های انجام‌نشده (در وضعیت in_progress یا submitted یا needs_revision)
        - غیبت‌های اخیر (ساده‌سازی: فعالیت‌هایی که مهلت آنها گذشته)
        """
        now = datetime.now(timezone.utc)
        
        # پیدا کردن دانش‌آموزانی که فعالیت معوق دارند
        students = self.db.query(User).join(
            WorkReceiver, WorkReceiver.receiver_user_id == User.id
        ).join(
            Work, Work.id == WorkReceiver.work_id
        ).filter(
            Work.due_at < now,
            Work.status.in_([WORK_STATUS_IN_PROGRESS, WORK_STATUS_SUBMITTED, WORK_STATUS_NEEDS_REVISION]),
            Work.is_deleted == False
        ).distinct().all()

        result = []
        for student in students:
            # تعداد فعالیت‌های معوق این دانش‌آموز
            overdue_count = self.db.query(Work).join(
                WorkReceiver, WorkReceiver.work_id == Work.id
            ).filter(
                WorkReceiver.receiver_user_id == student.id,
                Work.due_at < now,
                Work.status.in_([WORK_STATUS_IN_PROGRESS, WORK_STATUS_SUBMITTED, WORK_STATUS_NEEDS_REVISION]),
                Work.is_deleted == False
            ).count()

            result.append({
                "id": student.id,
                "full_name": student.full_name,
                "national_id": student.national_id,
                "overdue_count": overdue_count,
                "last_activity": None,  # در آینده تکمیل می‌شود
                "status": "نیاز به پیگیری",
                "priority": "high" if overdue_count > 3 else "medium",
            })

        # مرتب‌سازی بر اساس تعداد فعالیت‌های معوق (بیشترین اولویت)
        result.sort(key=lambda x: x["overdue_count"], reverse=True)
        return result

    def get_all_students(self) -> List[Dict]:
        """
        دریافت لیست تمام دانش‌آموزان (برای استفاده در کامپوننت QuickNote)
        """
        # ✅ اصلاح: استفاده از Role.code == "student"
        students = self.db.query(User).join(User.roles).filter(
            User.is_active == True,
            Role.code == "student"  # یا Role.name == "شاگرد" اگر ترجیح می‌دهید
        ).all()

        return [
            {
                "id": str(s.id),
                "full_name": s.full_name,
                "national_id": s.national_id,
                "grade": s.grade,
                "class_name": s.class_name,
            }
            for s in students
        ]