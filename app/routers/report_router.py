from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.services.report_service import ReportService
from app.services.student_service import StudentService
from app.utils.deps import get_current_user
from app.models.security import User

router = APIRouter()


def get_report_service(db: Session = Depends(get_db)) -> ReportService:
    return ReportService(db)


# ✅ تابع جداگانه برای سرویس دانش‌آموزان
def get_student_service(db: Session = Depends(get_db)) -> StudentService:
    return StudentService(db)


@router.get("/my/stats", response_model=dict)
def my_stats(
    service: ReportService = Depends(get_report_service),
    current_user: User = Depends(get_current_user)
):
    """آمار شخصی کاربر (منطبق با MySummary در داشبورد)"""
    return service.get_user_stats(str(current_user.id))


@router.get("/system/stats", response_model=dict)
def system_stats(
    service: ReportService = Depends(get_report_service),
    current_user: User = Depends(get_current_user)
):
    """آمار کلی سیستم – برای مدیران"""
    return service.get_system_stats()


@router.get("/today-priorities")
def get_today_priorities(
    service: ReportService = Depends(get_report_service),
    current_user: User = Depends(get_current_user)
):
    """دریافت اولویت‌های امروز کاربر"""
    return service.get_today_priorities(str(current_user.id))


# ✅ اصلاح شده: استفاده از get_student_service به‌جای lambda
@router.get("/students/need-follow-up")
def get_students_needing_follow_up(
    service: StudentService = Depends(get_student_service),
    current_user: User = Depends(get_current_user)
):
    """دریافت لیست دانش‌آموزانی که نیاز به پیگیری دارند"""
    return service.get_students_needing_follow_up(str(current_user.id))