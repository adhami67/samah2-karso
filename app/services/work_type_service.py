from sqlalchemy.orm import Session
from app.models.work_type import WorkType
from app.schemas.work_type import WorkTypeCreate, WorkTypeUpdate
from app.core.exceptions import NotFoundError, ConflictError

class WorkTypeService:
    def __init__(self, db: Session):
        self.db = db

    def create(self, data: WorkTypeCreate) -> WorkType:
        # بررسی وجود کد تکراری
        existing = self.db.query(WorkType).filter(WorkType.code == data.code).first()
        if existing:
            raise ConflictError(f"WorkType with code '{data.code}' already exists")

        work_type = WorkType(
            code=data.code,
            title=data.title,
            color=data.color,
            icon=data.icon,
            description=data.description,
        )
        self.db.add(work_type)
        self.db.commit()
        self.db.refresh(work_type)
        return work_type

    def get(self, work_type_id: str) -> WorkType:
        work_type = self.db.get(WorkType, work_type_id)
        if not work_type:
            raise NotFoundError(f"WorkType with id '{work_type_id}' not found")
        return work_type

    def list(self, skip: int = 0, limit: int = 100) -> list[WorkType]:
        return self.db.query(WorkType).filter(WorkType.is_deleted == False).offset(skip).limit(limit).all()

    def update(self, work_type_id: str, data: WorkTypeUpdate) -> WorkType:
        work_type = self.get(work_type_id)
        for key, value in data.dict(exclude_unset=True).items():
            setattr(work_type, key, value)
        self.db.commit()
        self.db.refresh(work_type)
        return work_type

    def delete(self, work_type_id: str) -> None:
        work_type = self.get(work_type_id)
        work_type.is_deleted = True
        work_type.deleted_at = datetime.now(timezone.utc)
        self.db.commit()