# app/utils/deps.py
from typing import Optional, List
from fastapi import Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer
from jose import JWTError, jwt
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.security import User
from app.core.config import settings
from app.core.security import decode_token

oauth2_scheme = OAuth2PasswordBearer(tokenUrl="/api/auth/token")

async def get_current_user(
    token: str = Depends(oauth2_scheme),
    db: Session = Depends(get_db)
) -> User:
    """دریافت کاربر فعلی از روی توکن"""
    credentials_exception = HTTPException(
        status_code=status.HTTP_401_UNAUTHORIZED,
        detail="اعتبار توکن نامعتبر است",
        headers={"WWW-Authenticate": "Bearer"},
    )
    payload = decode_token(token)
    if payload is None:
        raise credentials_exception
    user_id: str = payload.get("sub")
    if user_id is None:
        raise credentials_exception

    user = db.get(User, user_id)
    if user is None:
        raise credentials_exception
    if not user.is_active:
        raise HTTPException(status_code=400, detail="کاربر غیرفعال است")
    return user

async def get_current_active_user(
    current_user: User = Depends(get_current_user),
) -> User:
    """بررسی فعال بودن کاربر"""
    if not current_user.is_active:
        raise HTTPException(status_code=400, detail="کاربر غیرفعال است")
    return current_user

def get_current_user_optional(
    token: str = Depends(oauth2_scheme),
    db: Session = Depends(get_db)
) -> Optional[User]:
    """دریافت کاربر در صورت وجود (اختیاری)"""
    try:
        return get_current_user(token, db)
    except HTTPException:
        return None

# ========== توابع بررسی نقش و مجوز (اصلاح شده) ==========

def require_role(required_role: str):
    """
    دکوریتور برای بررسی نقش کاربر.
    استفاده: @router.get("/admin", dependencies=[Depends(require_role("admin"))])
    """
    async def role_checker(current_user: User = Depends(get_current_user)):
        if not current_user.roles:
            raise HTTPException(status_code=403, detail="دسترسی غیرمجاز - کاربر هیچ نقشی ندارد")
        user_roles = [role.code for role in current_user.roles]
        if required_role not in user_roles:
            raise HTTPException(
                status_code=403,
                detail=f"دسترسی غیرمجاز - نقش '{required_role}' مورد نیاز است"
            )
        return current_user
    return role_checker  # ← فقط تابع را برگردان، نه Depends

def require_permission(required_permission: str):
    """
    دکوریتور برای بررسی مجوز کاربر.
    استفاده: @router.get("/admin", dependencies=[Depends(require_permission("system.manage"))])
    """
    async def permission_checker(current_user: User = Depends(get_current_user)):
        if not current_user.roles:
            raise HTTPException(status_code=403, detail="دسترسی غیرمجاز - کاربر هیچ نقشی ندارد")
        user_permissions = set()
        for role in current_user.roles:
            for perm in role.permissions:
                user_permissions.add(perm.code)
        if required_permission not in user_permissions:
            raise HTTPException(
                status_code=403,
                detail=f"دسترسی غیرمجاز - مجوز '{required_permission}' مورد نیاز است"
            )
        return current_user
    return permission_checker  # ← فقط تابع را برگردان، نه Depends