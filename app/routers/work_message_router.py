from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.services.work_message_service import WorkMessageService
from app.schemas.work_message import (
    MessageCreate,
    MessageUpdate,
    MessageResponse,
    UnseenCountResponse,
)
from app.utils.deps import get_current_user
from app.models.security import User
from app.core.exceptions import NotFoundError

router = APIRouter(prefix="/messages", tags=["Messages"])


def get_message_service(db: Session = Depends(get_db)) -> WorkMessageService:
    return WorkMessageService(db)


@router.post("/", response_model=MessageResponse, status_code=status.HTTP_201_CREATED)
def send_message(
    work_id: str,
    payload: MessageCreate,
    service: WorkMessageService = Depends(get_message_service),
    current_user: User = Depends(get_current_user),
):
    """ارسال پیام در یک کار"""
    try:
        message = service.send_message(work_id, str(current_user.id), payload)
        return message
    except NotFoundError as e:
        raise HTTPException(status_code=404, detail=str(e))


@router.get("/work/{work_id}", response_model=list[MessageResponse])
def get_messages(
    work_id: str,
    service: WorkMessageService = Depends(get_message_service),
):
    """دریافت همه پیام‌های یک کار"""
    try:
        return service.get_messages_for_work(work_id)
    except NotFoundError as e:
        raise HTTPException(status_code=404, detail=str(e))


@router.get("/thread/{message_id}", response_model=list[MessageResponse])
def get_thread(
    message_id: str,
    service: WorkMessageService = Depends(get_message_service),
):
    """دریافت یک پیام و همه پاسخ‌های آن"""
    try:
        return service.get_thread(message_id)
    except NotFoundError as e:
        raise HTTPException(status_code=404, detail=str(e))


@router.post("/{message_id}/seen")
def mark_as_seen(
    message_id: str,
    service: WorkMessageService = Depends(get_message_service),
    current_user: User = Depends(get_current_user),
):
    """ثبت دیده شدن پیام توسط کاربر فعلی"""
    try:
        service.mark_as_seen(message_id, str(current_user.id))
        return {"message": "Marked as seen"}
    except NotFoundError as e:
        raise HTTPException(status_code=404, detail=str(e))


@router.get("/work/{work_id}/unseen", response_model=UnseenCountResponse)
def get_unseen_count(
    work_id: str,
    service: WorkMessageService = Depends(get_message_service),
    current_user: User = Depends(get_current_user),
):
    """تعداد پیام‌های دیده‌نشده برای کاربر فعلی در یک کار"""
    count = service.get_unseen_count(work_id, str(current_user.id))
    return UnseenCountResponse(work_id=work_id, unseen_count=count)


@router.put("/{message_id}", response_model=MessageResponse)
def update_message(
    message_id: str,
    payload: MessageUpdate,
    service: WorkMessageService = Depends(get_message_service),
    current_user: User = Depends(get_current_user),
):
    """ویرایش پیام (فقط توسط فرستنده و قبل از دیده شدن)"""
    try:
        return service.update_message(message_id, payload, str(current_user.id))
    except (NotFoundError, PermissionError) as e:
        raise HTTPException(status_code=400, detail=str(e))


@router.delete("/{message_id}", status_code=status.HTTP_204_NO_CONTENT)
def delete_message(
    message_id: str,
    service: WorkMessageService = Depends(get_message_service),
    current_user: User = Depends(get_current_user),
):
    """حذف نرم پیام (فقط توسط فرستنده)"""
    try:
        service.delete_message(message_id, str(current_user.id))
    except (NotFoundError, PermissionError) as e:
        raise HTTPException(status_code=400, detail=str(e))
    