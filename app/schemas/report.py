from typing import Optional, Dict
from pydantic import BaseModel
from datetime import datetime


class UserStatsResponse(BaseModel):
    """آمار کاربر"""
    total_assigned: int
    completed: int
    in_progress: int
    done: int
    average_completion_time: Optional[str]


class SystemStatsResponse(BaseModel):
    """آمار سیستم"""
    total_works: int
    total_receivers: int
    status_distribution: Dict[str, int]


class PerformanceReportResponse(BaseModel):
    """گزارش عملکرد"""
    total_works: int
    completed: int
    completion_rate: float
    average_completion_time: Optional[str]