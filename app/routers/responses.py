# app/routers/responses.py
from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.response import Response as ResponseModel
from app.models.work_receiver import WorkReceiver
from app.models.security import User
from app.models.timeline_event import TimelineEvent
from app.schemas.response import ResponseCreate, ResponseRead
from app.utils.deps import get_current_user
from app.core.constants import WORK_RECEIVER_STATUS_DONE

router = APIRouter(prefix="/api/responses", tags=["responses"])

@router.post("/", response_model=ResponseRead, status_code=status.HTTP_201_CREATED)
def create_response(
    payload: ResponseCreate,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    # بررسی وجود ارجاع
    receiver = db.get(WorkReceiver, payload.work_receiver_id)
    if not receiver:
        raise HTTPException(status_code=404, detail="ارجاع مورد نظر یافت نشد")

    # فقط مجری (گیرنده کار) می‌تواند پاسخ دهد
    if receiver.receiver_user_id != current_user.id:
        raise HTTPException(status_code=403, detail="شما مجاز به پاسخ‌گویی به این ارجاع نیستید")

    # ایجاد پاسخ
    response = ResponseModel(
        work_receiver_id=payload.work_receiver_id,
        responder_user_id=current_user.id,
        content=payload.content,
    )
    db.add(response)

    # به‌روزرسانی وضعیت ارجاع به "انجام شده" (یا هر وضعیت دیگری که مد نظر است)
    # در اینجا فرض می‌کنیم پاسخ به معنای انجام کار است
    receiver.status = WORK_RECEIVER_STATUS_DONE
    receiver.reply_time = datetime.utcnow()

    # ثبت در تاریخچه
    event = TimelineEvent(
        activity_id=receiver.work_id,
        event_type="response_submitted",
        note=f"کار توسط {current_user.username} انجام و پاسخ داده شد",
    )
    db.add(event)

    db.commit()
    db.refresh(response)
    return response

@router.get("/work-receiver/{work_receiver_id}", response_model=list[ResponseRead])
def get_responses_by_receiver(
    work_receiver_id: str,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    # فقط مجری یا فرستنده کار می‌توانند پاسخ‌ها را ببینند
    receiver = db.get(WorkReceiver, work_receiver_id)
    if not receiver:
        raise HTTPException(status_code=404, detail="ارجاع یافت نشد")
    
    # اجازه دسترسی (ساده شده)
    work = receiver.work
    if work.sender_user_id != current_user.id and receiver.receiver_user_id != current_user.id:
        raise HTTPException(status_code=403, detail="شما دسترسی به این پاسخ‌ها ندارید")

    responses = db.query(ResponseModel).filter(
        ResponseModel.work_receiver_id == work_receiver_id
    ).order_by(ResponseModel.created_at).all()
    return responses