from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.group import Group
from app.models.workspace import Workspace
from app.schemas.group import GroupCreate, GroupRead

router = APIRouter()

@router.post("/", response_model=GroupRead, status_code=201)
def create_group(payload: GroupCreate, db: Session = Depends(get_db)):
    workspace = db.get(Workspace, payload.workspace_id)
    if not workspace:
        raise HTTPException(status_code=404, detail="Workspace not found")
    item = Group(workspace_id=payload.workspace_id, name=payload.name, kind=payload.kind, description=payload.description)
    db.add(item)
    db.commit()
    db.refresh(item)
    return item

@router.get("/", response_model=list[GroupRead])
def list_groups(db: Session = Depends(get_db)):
    return db.query(Group).order_by(Group.created_at.desc()).all()

@router.get("/{group_id}", response_model=GroupRead)
def get_group(group_id: str, db: Session = Depends(get_db)):
    item = db.get(Group, group_id)
    if not item:
        raise HTTPException(status_code=404, detail="Group not found")
    return item
