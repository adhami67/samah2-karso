from typing import Optional
from pydantic import BaseModel, Field
from datetime import datetime


# ========== ورودی‌ها ==========

class WorkAssignRequest(BaseModel):
    """درخواست ارجاع کار به گیرنده"""
    receiver_user_id: str = Field(..., description="شناسه کاربر گیرنده (مجری)")


class WorkStartRequest(BaseModel):
    """درخواست شروع انجام کار"""
    work_receiver_id: str = Field(..., description="شناسه ارجاع")


class ResponseSubmitRequest(BaseModel):
    """درخواست ثبت پاسخ"""
    work_receiver_id: str = Field(..., description="شناسه ارجاع")
    body: str = Field(..., min_length=1, description="متن پاسخ")


class EvaluationRequest(BaseModel):
    """درخواست ارزیابی پاسخ"""
    work_receiver_id: str = Field(..., description="شناسه ارجاع")
    score: int = Field(..., ge=0, le=100, description="امتیاز (۰ تا ۱۰۰)")
    note: str = Field(..., min_length=1, description="بازخورد")
    decision: str = Field(..., pattern="^(approved|needs_revision)$", description="تصمیم: approved یا needs_revision")


class WorkCompleteRequest(BaseModel):
    """درخواست خاتمه کار"""
    work_id: str = Field(..., description="شناسه کار")


class WorkArchiveRequest(BaseModel):
    """درخواست بایگانی کار"""
    work_id: str = Field(..., description="شناسه کار")


# ========== خروجی‌ها ==========

class WorkStatusSummaryResponse(BaseModel):
    """خلاصه وضعیت کار"""
    work_id: str
    status: str
    receivers_count: int
    responses_count: int
    last_event: Optional[str]


class WorkflowTimelineItem(BaseModel):
    """آیتم تاریخچه گردش کار"""
    time: datetime
    status: str
    changed_by: str
    note: str