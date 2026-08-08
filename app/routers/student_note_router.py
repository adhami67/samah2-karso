# app/routers/student_note_router.py
from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from typing import Optional
from app.db.deps import get_db
from app.services.student_note_service import StudentNoteService
from app.schemas.student_note import StudentNoteCreate, StudentNoteRead, StudentNoteUpdate
from app.utils.deps import get_current_user
from app.models.security import User

router = APIRouter(prefix="/api/notes", tags=["Student Notes"])

def get_note_service(db: Session = Depends(get_db)) -> StudentNoteService:
    return StudentNoteService(db)

@router.post("/", response_model=StudentNoteRead, status_code=status.HTTP_201_CREATED)
def create_note(
    payload: StudentNoteCreate,
    service: StudentNoteService = Depends(get_note_service),
    current_user: User = Depends(get_current_user)
):
    return service.create_note(
        student_id=payload.student_id,
        teacher_id=str(current_user.id),
        content=payload.content,
        category=payload.category,
        is_private=payload.is_private
    )

@router.get("/student/{student_id}", response_model=list[StudentNoteRead])
def get_student_notes(
    student_id: str,
    include_private: bool = False,
    service: StudentNoteService = Depends(get_note_service),
    current_user: User = Depends(get_current_user)
):
    return service.get_student_notes(
        student_id=student_id,
        teacher_id=str(current_user.id),
        include_private=include_private
    )

@router.patch("/{note_id}", response_model=StudentNoteRead)
def update_note(
    note_id: str,
    payload: StudentNoteUpdate,
    service: StudentNoteService = Depends(get_note_service),
    current_user: User = Depends(get_current_user)
):
    note = service.update_note(note_id, payload.content)
    if not note:
        raise HTTPException(status_code=404, detail="یادداشت یافت نشد")
    return note

@router.delete("/{note_id}", status_code=status.HTTP_204_NO_CONTENT)
def delete_note(
    note_id: str,
    service: StudentNoteService = Depends(get_note_service),
    current_user: User = Depends(get_current_user)
):
    if not service.delete_note(note_id):
        raise HTTPException(status_code=404, detail="یادداشت یافت نشد")