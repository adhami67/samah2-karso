from fastapi import APIRouter, Depends, HTTPException, Query
from sqlalchemy.orm import Session
from typing import Optional
from datetime import datetime
from app.db.deps import get_db
from app.services.report_service import ReportService
from app.schemas.report import UserStatsResponse, SystemStatsResponse, PerformanceReportResponse
from app.utils.deps import get_current_user
from app.models.security import User

router = APIRouter(prefix="/reports", tags=["Reports"])


def get_report_service(db: Session = Depends(get_db)) -> ReportService:
    return ReportService(db)


@router.get("/my/stats", response_model=UserStatsResponse)
def get_my_stats(
    service: ReportService = Depends(get_report_service),
    current_user: User = Depends(get_current_user),
):
    """دریافت آمار عملکرد کاربر فعلی"""
    return service.get_user_stats(str(current_user.id))


@router.get("/system/stats", response_model=SystemStatsResponse)
def get_system_stats(
    service: ReportService = Depends(get_report_service),
    # می‌توانید نقش مدیر را بررسی کنید
):
    """دریافت آمار کلی سیستم (فقط مدیران)"""
    return service.get_system_stats()


@router.get("/today", response_model=list)
def get_today_activities(
    service: ReportService = Depends(get_report_service),
    current_user: User = Depends(get_current_user),
):
    """کارهای امروز من"""
    return service.get_today_activities(str(current_user.id))


@router.get("/overdue", response_model=list)
def get_overdue_activities(
    service: ReportService = Depends(get_report_service),
    current_user: User = Depends(get_current_user),
):
    """کارهای عقب‌افتاده من"""
    return service.get_overdue_activities(str(current_user.id))


@router.get("/performance", response_model=PerformanceReportResponse)
def get_performance_report(
    start_date: Optional[datetime] = Query(None),
    end_date: Optional[datetime] = Query(None),
    service: ReportService = Depends(get_report_service),
):
    """گزارش عملکرد کلی در بازه زمانی"""
    return service.get_performance_report(start_date, end_date)