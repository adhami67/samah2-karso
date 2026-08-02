from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.work_group import WorkGroup
from app.models.work_group_member import WorkGroupMember
from app.models.security import User
from app.schemas.workspace import WorkspaceCreate, WorkspaceRead, WorkspaceUpdate, WorkspaceMemberRead
from app.utils.deps import get_current_user
from datetime import datetime
from typing import List

router = APIRouter(prefix="/api/workspaces", tags=["workspaces"])

# ---------- ایجاد حوزه ----------
@router.post("", response_model=WorkspaceRead, status_code=status.HTTP_201_CREATED)
def create_workspace_no_slash(
    payload: WorkspaceCreate,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """ایجاد حوزه جدید – بدون اسلش انتهایی"""
    return _create_workspace(payload, db, current_user)

@router.post("/", response_model=WorkspaceRead, status_code=status.HTTP_201_CREATED)
def create_workspace(
    payload: WorkspaceCreate,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """ایجاد حوزه جدید – با اسلش انتهایی"""
    return _create_workspace(payload, db, current_user)

def _create_workspace(payload: WorkspaceCreate, db: Session, current_user: User) -> WorkGroup:
    existing = db.query(WorkGroup).filter(
        WorkGroup.name == payload.name,
        WorkGroup.is_deleted == False
    ).first()
    if existing:
        raise HTTPException(status_code=400, detail="حوزه‌ای با این نام قبلاً ثبت شده است")

    workspace = WorkGroup(
        name=payload.name,
        description=payload.description,
        owner_user_id=current_user.id,
        status="active"
    )
    db.add(workspace)
    db.commit()
    db.refresh(workspace)

    # اضافه کردن خود کاربر به‌عنوان عضو
    member = WorkGroupMember(
        work_group_id=workspace.id,
        member_user_id=current_user.id,
        join_time=datetime.utcnow()
    )
    db.add(member)
    db.commit()

    return workspace

# ---------- دریافت لیست حوزه‌ها ----------
@router.get("", response_model=List[WorkspaceRead])
@router.get("/", response_model=List[WorkspaceRead])
def list_workspaces(
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """دریافت لیست حوزه‌هایی که کاربر عضو یا مدیر آن است"""
    subquery = db.query(WorkGroupMember.work_group_id).filter(
        WorkGroupMember.member_user_id == current_user.id,
        WorkGroupMember.is_deleted == False
    ).subquery()

    workspaces = db.query(WorkGroup).filter(
        WorkGroup.is_deleted == False,
        WorkGroup.status == "active",
        WorkGroup.id.in_(subquery)
    ).order_by(WorkGroup.created_at.desc()).all()

    return workspaces

# ---------- دریافت یک حوزه ----------
@router.get("/{workspace_id}", response_model=WorkspaceRead)
def get_workspace(
    workspace_id: str,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    workspace = db.get(WorkGroup, workspace_id)
    if not workspace or workspace.is_deleted:
        raise HTTPException(status_code=404, detail="حوزه مورد نظر یافت نشد")

    is_member = db.query(WorkGroupMember).filter(
        WorkGroupMember.work_group_id == workspace_id,
        WorkGroupMember.member_user_id == current_user.id,
        WorkGroupMember.is_deleted == False
    ).first()
    if not is_member and workspace.owner_user_id != current_user.id:
        raise HTTPException(status_code=403, detail="شما به این حوزه دسترسی ندارید")

    return workspace

# ---------- ویرایش حوزه ----------
@router.patch("/{workspace_id}", response_model=WorkspaceRead)
def update_workspace(
    workspace_id: str,
    payload: WorkspaceUpdate,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    workspace = db.get(WorkGroup, workspace_id)
    if not workspace or workspace.is_deleted:
        raise HTTPException(status_code=404, detail="حوزه مورد نظر یافت نشد")

    if workspace.owner_user_id != current_user.id:
        raise HTTPException(status_code=403, detail="فقط مدیر حوزه می‌تواند ویرایش کند")

    if payload.name is not None:
        workspace.name = payload.name
    if payload.description is not None:
        workspace.description = payload.description
    if payload.status is not None:
        workspace.status = payload.status

    db.commit()
    db.refresh(workspace)
    return workspace

# ---------- حذف حوزه ----------
@router.delete("/{workspace_id}", status_code=status.HTTP_204_NO_CONTENT)
def delete_workspace(
    workspace_id: str,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    workspace = db.get(WorkGroup, workspace_id)
    if not workspace or workspace.is_deleted:
        raise HTTPException(status_code=404, detail="حوزه مورد نظر یافت نشد")

    if workspace.owner_user_id != current_user.id:
        raise HTTPException(status_code=403, detail="فقط مدیر حوزه می‌تواند حذف کند")

    if workspace.works and any(not w.is_archived for w in workspace.works):
        raise HTTPException(status_code=400, detail="حوزه دارای کارهای فعال است و قابل حذف نیست")

    workspace.is_deleted = True
    workspace.deleted_at = datetime.utcnow()
    db.commit()

# ---------- مدیریت اعضا ----------
@router.post("/{workspace_id}/members", response_model=WorkspaceMemberRead, status_code=status.HTTP_201_CREATED)
def add_member(
    workspace_id: str,
    user_id: str,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    workspace = db.get(WorkGroup, workspace_id)
    if not workspace or workspace.is_deleted:
        raise HTTPException(status_code=404, detail="حوزه مورد نظر یافت نشد")

    if workspace.owner_user_id != current_user.id:
        raise HTTPException(status_code=403, detail="فقط مدیر حوزه می‌تواند عضو اضافه کند")

    user = db.get(User, user_id)
    if not user:
        raise HTTPException(status_code=404, detail="کاربر مورد نظر یافت نشد")

    existing = db.query(WorkGroupMember).filter(
        WorkGroupMember.work_group_id == workspace_id,
        WorkGroupMember.member_user_id == user_id,
        WorkGroupMember.is_deleted == False
    ).first()
    if existing:
        raise HTTPException(status_code=400, detail="این کاربر قبلاً عضو حوزه است")

    member = WorkGroupMember(
        work_group_id=workspace_id,
        member_user_id=user_id,
        invite_time=datetime.utcnow()
    )
    db.add(member)
    db.commit()
    db.refresh(member)

    member_data = WorkspaceMemberRead(
        user_id=user.id,
        username=user.username,
        full_name=getattr(user, "full_name", None),
        invite_time=member.invite_time,
        join_time=member.join_time,
        is_active=True
    )
    return member_data

@router.delete("/{workspace_id}/members/{user_id}", status_code=status.HTTP_204_NO_CONTENT)
def remove_member(
    workspace_id: str,
    user_id: str,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    workspace = db.get(WorkGroup, workspace_id)
    if not workspace or workspace.is_deleted:
        raise HTTPException(status_code=404, detail="حوزه مورد نظر یافت نشد")

    if workspace.owner_user_id != current_user.id and current_user.id != user_id:
        raise HTTPException(status_code=403, detail="شما مجوز حذف این عضو را ندارید")

    member = db.query(WorkGroupMember).filter(
        WorkGroupMember.work_group_id == workspace_id,
        WorkGroupMember.member_user_id == user_id,
        WorkGroupMember.is_deleted == False
    ).first()
    if not member:
        raise HTTPException(status_code=404, detail="عضو مورد نظر در این حوزه یافت نشد")

    if user_id == workspace.owner_user_id:
        raise HTTPException(status_code=400, detail="مدیر حوزه نمی‌تواند خود را حذف کند. ابتدا مدیریت را به دیگری واگذار کنید.")

    member.is_deleted = True
    member.deleted_at = datetime.utcnow()
    db.commit()

@router.get("/{workspace_id}/members", response_model=List[WorkspaceMemberRead])
def list_members(
    workspace_id: str,
    db: Session = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    workspace = db.get(WorkGroup, workspace_id)
    if not workspace or workspace.is_deleted:
        raise HTTPException(status_code=404, detail="حوزه مورد نظر یافت نشد")

    is_member = db.query(WorkGroupMember).filter(
        WorkGroupMember.work_group_id == workspace_id,
        WorkGroupMember.member_user_id == current_user.id,
        WorkGroupMember.is_deleted == False
    ).first()
    if not is_member and workspace.owner_user_id != current_user.id:
        raise HTTPException(status_code=403, detail="شما به این حوزه دسترسی ندارید")

    members = db.query(WorkGroupMember).filter(
        WorkGroupMember.work_group_id == workspace_id,
        WorkGroupMember.is_deleted == False
    ).all()

    result = []
    for m in members:
        user = db.get(User, m.member_user_id)
        if user:
            result.append(WorkspaceMemberRead(
                user_id=user.id,
                username=user.username,
                full_name=getattr(user, "full_name", None),
                invite_time=m.invite_time,
                join_time=m.join_time,
                is_active=True
            ))
    return result