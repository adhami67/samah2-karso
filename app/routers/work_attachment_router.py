from fastapi import APIRouter, Depends, HTTPException, status, File, UploadFile, Form
from fastapi.responses import FileResponse
from sqlalchemy.orm import Session
from typing import Optional
from app.db.deps import get_db
from app.services.work_attachment_service import WorkAttachmentService
from app.schemas.work_attachment import AttachmentResponse, AttachmentUploadResponse
from app.utils.deps import get_current_user
from app.models.security import User
from app.core.exceptions import NotFoundError, ValidationError

router = APIRouter(prefix="/attachments", tags=["Attachments"])


def get_attachment_service(db: Session = Depends(get_db)) -> WorkAttachmentService:
    return WorkAttachmentService(db)


@router.post("/upload", response_model=AttachmentUploadResponse, status_code=status.HTTP_201_CREATED)
async def upload_file(
    file: UploadFile = File(...),
    work_id: Optional[str] = Form(None),
    message_id: Optional[str] = Form(None),
    description: Optional[str] = Form(None),
    service: WorkAttachmentService = Depends(get_attachment_service),
    current_user: User = Depends(get_current_user),
):
    """آپلود فایل و اتصال به کار یا پیام"""
    try:
        attachment = await service.upload_file(
            file=file,
            work_id=work_id,
            message_id=message_id,
            uploaded_by_user_id=str(current_user.id),
            description=description,
        )
        return AttachmentUploadResponse(
            id=attachment.id,
            file_name=attachment.file_name,
            file_size=attachment.file_size,
        )
    except (NotFoundError, ValidationError) as e:
        raise HTTPException(status_code=400, detail=str(e))


@router.get("/work/{work_id}", response_model=list[AttachmentResponse])
def get_attachments_for_work(
    work_id: str,
    service: WorkAttachmentService = Depends(get_attachment_service),
):
    """دریافت همه فایل‌های پیوست یک کار"""
    try:
        return service.get_attachments_for_work(work_id)
    except NotFoundError as e:
        raise HTTPException(status_code=404, detail=str(e))


@router.get("/message/{message_id}", response_model=list[AttachmentResponse])
def get_attachments_for_message(
    message_id: str,
    service: WorkAttachmentService = Depends(get_attachment_service),
):
    """دریافت همه فایل‌های پیوست یک پیام"""
    try:
        return service.get_attachments_for_message(message_id)
    except NotFoundError as e:
        raise HTTPException(status_code=404, detail=str(e))


@router.get("/{attachment_id}/download")
def download_attachment(
    attachment_id: str,
    service: WorkAttachmentService = Depends(get_attachment_service),
):
    """دانلود فایل پیوست"""
    try:
        file_path = service.get_file_path(attachment_id)
        attachment = service.get_attachment(attachment_id)
        return FileResponse(
            path=file_path,
            filename=attachment.file_name,
            media_type=attachment.mime_type,
        )
    except NotFoundError as e:
        raise HTTPException(status_code=404, detail=str(e))


@router.delete("/{attachment_id}", status_code=status.HTTP_204_NO_CONTENT)
def delete_attachment(
    attachment_id: str,
    service: WorkAttachmentService = Depends(get_attachment_service),
    current_user: User = Depends(get_current_user),
):
    """حذف فایل پیوست (فقط توسط آپلودکننده)"""
    try:
        service.delete_attachment(attachment_id, str(current_user.id))
    except (NotFoundError, PermissionError) as e:
        raise HTTPException(status_code=400, detail=str(e))