from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.security import Permission, Role, User
from app.schemas.security import (
    PermissionCreate, PermissionRead,
    RoleCreate, RoleRead, RoleUpdate,
    UserCreate, UserRead, UserUpdate
)
from app.utils.security import hash_password
from app.utils.deps import get_current_user, require_role

router = APIRouter()

# ---------- مجوزها ----------
@router.post("/permissions", response_model=PermissionRead, status_code=201)
def create_permission(
    payload: PermissionCreate,
    db: Session = Depends(get_db),
    current_user: User = Depends(require_role("system_admin"))
):
    item = Permission(code=payload.code, name=payload.name)
    db.add(item)
    db.commit()
    db.refresh(item)
    return item

@router.get("/permissions", response_model=list[PermissionRead])
def list_permissions(
    db: Session = Depends(get_db),
    current_user: User = Depends(require_role("system_admin"))
):
    return db.query(Permission).order_by(Permission.created_at.desc()).all()

# ---------- نقش‌ها ----------
@router.post("/roles", response_model=RoleRead, status_code=201)
def create_role(
    payload: RoleCreate,
    db: Session = Depends(get_db),
    current_user: User = Depends(require_role("system_admin"))
):
    permissions = (
        db.query(Permission)
        .filter(Permission.id.in_(payload.permission_ids))
        .all()
    ) if payload.permission_ids else []
    item = Role(code=payload.code, name=payload.name, permissions=permissions)
    db.add(item)
    db.commit()
    db.refresh(item)
    return item

@router.get("/roles", response_model=list[RoleRead])
def list_roles(
    db: Session = Depends(get_db),
    current_user: User = Depends(require_role("system_admin"))
):
    return db.query(Role).order_by(Role.created_at.desc()).all()

@router.patch("/roles/{role_id}", response_model=RoleRead)
def update_role(
    role_id: str,
    payload: RoleUpdate,
    db: Session = Depends(get_db),
    current_user: User = Depends(require_role("system_admin"))
):
    role = db.get(Role, role_id)
    if not role:
        raise HTTPException(status_code=404, detail="نقش یافت نشد")

    if payload.name is not None:
        role.name = payload.name

    if payload.permission_ids is not None:
        perms = db.query(Permission).filter(Permission.id.in_(payload.permission_ids)).all()
        role.permissions = perms

    db.commit()
    db.refresh(role)
    return role

@router.delete("/roles/{role_id}", status_code=204)
def delete_role(
    role_id: str,
    db: Session = Depends(get_db),
    current_user: User = Depends(require_role("system_admin"))
):
    role = db.get(Role, role_id)
    if not role:
        raise HTTPException(status_code=404, detail="نقش یافت نشد")

    # جلوگیری از حذف نقش‌های سیستمی
    if role.code in ["system_admin", "school_admin", "teacher", "student", "parent"]:
        raise HTTPException(status_code=400, detail="نقش‌های سیستمی قابل حذف نیستند")

    db.delete(role)
    db.commit()
    # در صورت نیاز به return، می‌توانید Response(None, status_code=204) برگردانید

# ---------- کاربران ----------
@router.post("/users", response_model=UserRead, status_code=201)
def create_user(
    payload: UserCreate,
    db: Session = Depends(get_db),
    current_user: User = Depends(require_role("system_admin"))
):
    existing = db.query(User).filter(User.national_id == payload.national_id).first()
    if existing:
        raise HTTPException(status_code=400, detail="National ID already exists")

    username = payload.username or payload.national_id
    roles = (
        db.query(Role).filter(Role.id.in_(payload.role_ids)).all()
    ) if payload.role_ids else []

    user = User(
        username=username,
        national_id=payload.national_id,
        full_name=payload.full_name,
        password_hash=hash_password(payload.password),
        is_active=True,
        grade=payload.grade,
        class_name=payload.class_name,
        father_name=payload.father_name,
        mother_name=payload.mother_name,
        parent_phone=payload.parent_phone,
        phone=payload.phone,
        address=payload.address,
        birth_date=payload.birth_date,
        gender=payload.gender,
        roles=roles,
    )
    db.add(user)
    db.commit()
    db.refresh(user)
    return user

@router.get("/users", response_model=list[UserRead])
def list_users(
    db: Session = Depends(get_db),
    current_user: User = Depends(require_role("system_admin"))
):
    return db.query(User).order_by(User.created_at.desc()).all()

@router.patch("/users/{user_id}", response_model=UserRead)
def update_user(
    user_id: str,
    payload: UserUpdate,
    db: Session = Depends(get_db),
    current_user: User = Depends(require_role("system_admin"))
):
    user = db.get(User, user_id)
    if not user:
        raise HTTPException(status_code=404, detail="کاربر یافت نشد")
    
    for field in ["full_name", "username", "is_active", "grade", "class_name",
                  "father_name", "mother_name", "parent_phone", "phone",
                  "address", "birth_date", "gender"]:
        value = getattr(payload, field, None)
        if value is not None:
            setattr(user, field, value)
    
    if payload.password:
        user.password_hash = hash_password(payload.password)
    
    if payload.role_ids is not None:
        roles = db.query(Role).filter(Role.id.in_(payload.role_ids)).all()
        user.roles = roles
    
    db.commit()
    db.refresh(user)
    return user

@router.delete("/users/{user_id}", status_code=204)
def delete_user(
    user_id: str,
    db: Session = Depends(get_db),
    current_user: User = Depends(require_role("system_admin"))
):
    user = db.get(User, user_id)
    if not user:
        raise HTTPException(status_code=404, detail="کاربر یافت نشد")
    user.is_active = False
    db.commit()

@router.get("/me")
def me(current_user: User = Depends(get_current_user)):
    return {
        "id": current_user.id,
        "national_id": current_user.national_id,
        "full_name": current_user.full_name,
        "roles": [role.code for role in current_user.roles],
    }