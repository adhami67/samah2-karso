# app/services/student_note_service.py
from sqlalchemy.orm import Session
from app.models.student_note import StudentNote
from app.models.security import User
from typing import List, Optional
from datetime import datetime, timezone

class StudentNoteService:
    def __init__(self, db: Session):
        self.db = db

    def create_note(
        self,
        student_id: str,
        teacher_id: str,
        content: str,
        category: str = "general",
        is_private: bool = True
    ) -> StudentNote:
        note = StudentNote(
            student_id=student_id,
            teacher_id=teacher_id,
            content=content,
            category=category,
            is_private=is_private
        )
        self.db.add(note)
        self.db.commit()
        self.db.refresh(note)
        return note

    def get_student_notes(
        self,
        student_id: str,
        teacher_id: Optional[str] = None,
        include_private: bool = False
    ) -> List[StudentNote]:
        query = self.db.query(StudentNote).filter(
            StudentNote.student_id == student_id,
            StudentNote.is_deleted == False
        )
        if not include_private:
            query = query.filter(StudentNote.is_private == False)
        if teacher_id:
            query = query.filter(StudentNote.teacher_id == teacher_id)
        return query.order_by(StudentNote.created_at.desc()).all()

    def update_note(self, note_id: str, content: str) -> Optional[StudentNote]:
        note = self.db.query(StudentNote).filter(
            StudentNote.id == note_id,
            StudentNote.is_deleted == False
        ).first()
        if not note:
            return None
        note.content = content
        self.db.commit()
        self.db.refresh(note)
        return note

    def delete_note(self, note_id: str) -> bool:
        note = self.db.query(StudentNote).filter(
            StudentNote.id == note_id,
            StudentNote.is_deleted == False
        ).first()
        if not note:
            return False
        note.is_deleted = True
        note.deleted_at = datetime.now(timezone.utc)  # ✅ استفاده از datetime
        self.db.commit()
        return True