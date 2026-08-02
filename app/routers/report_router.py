from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.utils.deps import get_current_user
from app.models.security import User
from app.services.report_service import ReportService

router = APIRouter()


@router.get("/my/stats", response_model=dict)
def my_stats(
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """آمار شخصی کاربر (منطبق با MySummary در داشبورد)"""
    service = ReportService(db)
    return service.get_user_stats(str(current_user.id))


@router.get("/system/stats", response_model=dict)
def system_stats(
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """آمار کلی سیستم – فقط برای مدیران"""
    service = ReportService(db)
    return service.get_system_stats()