from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.security import Permission, Role, User
from app.schemas.security import PermissionCreate, PermissionRead, RoleCreate, RoleRead, UserCreate, UserRead
from app.utils.security import hash_password
from app.utils.deps import get_current_user, require_role

router = APIRouter()

@router.post("/permissions", response_model=PermissionRead, status_code=201)
def create_permission(payload: PermissionCreate, db: Session = Depends(get_db), user: User = Depends(require_role("system_admin"))):
    item = Permission(code=payload.code, name=payload.name)
    db.add(item)
    db.commit()
    db.refresh(item)
    return item

@router.get("/permissions", response_model=list[PermissionRead])
def list_permissions(db: Session = Depends(get_db), user: User = Depends(require_role("system_admin"))):
    return db.query(Permission).order_by(Permission.created_at.desc()).all()

@router.post("/roles", response_model=RoleRead, status_code=201)
def create_role(payload: RoleCreate, db: Session = Depends(get_db), user: User = Depends(require_role("system_admin"))):
    permissions = db.query(Permission).filter(Permission.id.in_(payload.permission_ids)).all() if payload.permission_ids else []
    item = Role(code=payload.code, name=payload.name, permissions=permissions)
    db.add(item)
    db.commit()
    db.refresh(item)
    return item

@router.get("/roles", response_model=list[RoleRead])
def list_roles(db: Session = Depends(get_db), user: User = Depends(require_role("system_admin"))):
    return db.query(Role).order_by(Role.created_at.desc()).all()

@router.post("/users", response_model=UserRead, status_code=201)
def create_user(payload: UserCreate, db: Session = Depends(get_db), user: User = Depends(require_role("system_admin"))):
    existing = db.query(User).filter(User.national_id == payload.national_id).first()
    if existing:
        raise HTTPException(status_code=400, detail="National ID already exists")
    roles = db.query(Role).filter(Role.id.in_(payload.role_ids)).all() if payload.role_ids else []
    item = User(
        national_id=payload.national_id,
        full_name=payload.full_name,
        password_hash=hash_password(payload.password),
        roles=roles,
    )
    db.add(item)
    db.commit()
    db.refresh(item)
    return item

@router.get("/users", response_model=list[UserRead])
def list_users(db: Session = Depends(get_db), user: User = Depends(require_role("system_admin"))):
    return db.query(User).order_by(User.created_at.desc()).all()

@router.get("/me")
def me(current_user: User = Depends(get_current_user)):
    return {
        "id": current_user.id,
        "national_id": current_user.national_id,
        "full_name": current_user.full_name,
        "roles": [role.code for role in current_user.roles],
    }
