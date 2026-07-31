from typing import List, Optional
from pydantic import BaseModel

# ---------- Permission ----------
class PermissionCreate(BaseModel):
    code: str
    name: str

class PermissionRead(BaseModel):
    id: str
    code: str
    name: str

    class Config:
        from_attributes = True

# ---------- Role ----------
class RoleCreate(BaseModel):
    code: str
    name: str
    permission_ids: List[str] = []

class RoleRead(BaseModel):
    id: str
    code: str
    name: str
    permission_ids: List[str] = []

    class Config:
        from_attributes = True

# ---------- User ----------
class UserCreate(BaseModel):
    national_id: str
    full_name: str
    password: str
    username: Optional[str] = None          # ← برای لاگین با نام کاربری در آینده
    role_ids: List[str] = []
    # فیلدهای دانش‌آموزی (اختیاری)
    grade: Optional[str] = None
    class_name: Optional[str] = None
    father_name: Optional[str] = None
    mother_name: Optional[str] = None
    parent_phone: Optional[str] = None
    phone: Optional[str] = None
    address: Optional[str] = None
    birth_date: Optional[str] = None
    gender: Optional[str] = None

class UserRead(BaseModel):
    id: str
    national_id: str
    full_name: str
    username: Optional[str] = None
    is_active: bool
    # توجه: در اینجا به‌جای role_ids، از لیست RoleRead استفاده می‌کنیم
    roles: List[RoleRead] = []
    # فیلدهای دانش‌آموزی (اختیاری)
    grade: Optional[str] = None
    class_name: Optional[str] = None
    father_name: Optional[str] = None
    mother_name: Optional[str] = None
    parent_phone: Optional[str] = None
    phone: Optional[str] = None
    address: Optional[str] = None
    birth_date: Optional[str] = None
    gender: Optional[str] = None

    class Config:
        from_attributes = True