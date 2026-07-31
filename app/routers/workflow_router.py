from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.services.workflow_service import WorkflowService
from app.schemas.workflow import (
    WorkAssignRequest,
    WorkStartRequest,
    ResponseSubmitRequest,
    EvaluationRequest,
    WorkCompleteRequest,
    WorkArchiveRequest,
    WorkStatusSummaryResponse,
    WorkflowTimelineItem,
)
from app.utils.deps import get_current_user
from app.models.security import User
from app.core.exceptions import NotFoundError, InvalidStateTransitionError

router = APIRouter(prefix="/workflow", tags=["Workflow"])


def get_workflow_service(db: Session = Depends(get_db)) -> WorkflowService:
    return WorkflowService(db)


# ========== ۱. ارجاع کار ==========

@router.post("/assign", status_code=status.HTTP_201_CREATED)
def assign_work(
    payload: WorkAssignRequest,
    work_id: str,
    service: WorkflowService = Depends(get_workflow_service),
    current_user: User = Depends(get_current_user),
):
    """ارجاع کار به یک گیرنده (مجری)"""
    try:
        receiver = service.assign_work(work_id, payload.receiver_user_id, str(current_user.id))
        return {"message": "Work assigned successfully", "receiver_id": receiver.id}
    except (NotFoundError, InvalidStateTransitionError) as e:
        raise HTTPException(status_code=400, detail=str(e))


# ========== ۲. شروع انجام کار ==========

@router.post("/start")
def start_work(
    payload: WorkStartRequest,
    service: WorkflowService = Depends(get_workflow_service),
    current_user: User = Depends(get_current_user),
):
    """شروع انجام کار توسط گیرنده"""
    try:
        receiver = service.start_work(payload.work_receiver_id, str(current_user.id))
        return {"message": "Work started", "status": receiver.status}
    except (NotFoundError, InvalidStateTransitionError) as e:
        raise HTTPException(status_code=400, detail=str(e))


# ========== ۳. ثبت پاسخ ==========

@router.post("/submit-response")
def submit_response(
    payload: ResponseSubmitRequest,
    service: WorkflowService = Depends(get_workflow_service),
    current_user: User = Depends(get_current_user),
):
    """ثبت پاسخ توسط مجری"""
    try:
        receiver = service.submit_response(payload.work_receiver_id, payload.body, str(current_user.id))
        return {"message": "Response submitted", "status": receiver.status}
    except (NotFoundError, InvalidStateTransitionError) as e:
        raise HTTPException(status_code=400, detail=str(e))


# ========== ۴. ارزیابی پاسخ ==========

@router.post("/evaluate")
def evaluate_response(
    payload: EvaluationRequest,
    service: WorkflowService = Depends(get_workflow_service),
    current_user: User = Depends(get_current_user),
):
    """ارزیابی پاسخ توسط مالک یا ارزیاب"""
    try:
        receiver = service.evaluate_response(
            payload.work_receiver_id,
            payload.score,
            payload.note,
            payload.decision,
            str(current_user.id),
        )
        return {"message": "Evaluation submitted", "status": receiver.status}
    except (NotFoundError, InvalidStateTransitionError, ValueError) as e:
        raise HTTPException(status_code=400, detail=str(e))


# ========== ۵. خاتمه کار ==========

@router.post("/complete")
def complete_work(
    payload: WorkCompleteRequest,
    service: WorkflowService = Depends(get_workflow_service),
    current_user: User = Depends(get_current_user),
):
    """خاتمه کار پس از تأیید نهایی"""
    try:
        work = service.complete_work(payload.work_id, str(current_user.id))
        return {"message": "Work completed", "status": work.status}
    except (NotFoundError, InvalidStateTransitionError) as e:
        raise HTTPException(status_code=400, detail=str(e))


# ========== ۶. بایگانی کار ==========

@router.post("/archive")
def archive_work(
    payload: WorkArchiveRequest,
    service: WorkflowService = Depends(get_workflow_service),
    current_user: User = Depends(get_current_user),
):
    """بایگانی کار (حذف نرم)"""
    try:
        work = service.archive_work(payload.work_id, str(current_user.id))
        return {"message": "Work archived", "status": work.status}
    except (NotFoundError, InvalidStateTransitionError) as e:
        raise HTTPException(status_code=400, detail=str(e))


# ========== ۷. خلاصه وضعیت کار ==========

@router.get("/{work_id}/summary", response_model=WorkStatusSummaryResponse)
def get_work_summary(
    work_id: str,
    service: WorkflowService = Depends(get_workflow_service),
):
    """دریافت خلاصه وضعیت یک کار"""
    try:
        return service.get_work_status_summary(work_id)
    except NotFoundError as e:
        raise HTTPException(status_code=404, detail=str(e))


# ========== ۸. تاریخچه گردش کار ==========

@router.get("/{work_id}/timeline", response_model=list[WorkflowTimelineItem])
def get_work_timeline(
    work_id: str,
    service: WorkflowService = Depends(get_workflow_service),
):
    """دریافت تاریخچه کامل یک کار"""
    try:
        return service.get_workflow_timeline(work_id)
    except NotFoundError as e:
        raise HTTPException(status_code=404, detail=str(e))