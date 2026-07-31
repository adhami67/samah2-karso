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

class RoleUpdate(BaseModel):
    name: Optional[str] = None
    permission_ids: Optional[List[str]] = None

class RoleRead(BaseModel):
    id: str
    code: str
    name: str
    permissions: List[PermissionRead] = []   # توجه: لیستی از اشیاء Permission، نه شناسه

    class Config:
        from_attributes = True

# ---------- User ----------
class UserCreate(BaseModel):
    national_id: str
    full_name: str
    password: str
    username: Optional[str] = None
    role_ids: List[str] = []
    grade: Optional[str] = None
    class_name: Optional[str] = None
    father_name: Optional[str] = None
    mother_name: Optional[str] = None
    parent_phone: Optional[str] = None
    phone: Optional[str] = None
    address: Optional[str] = None
    birth_date: Optional[str] = None
    gender: Optional[str] = None

class UserUpdate(BaseModel):
    full_name: Optional[str] = None
    username: Optional[str] = None
    password: Optional[str] = None
    is_active: Optional[bool] = None
    grade: Optional[str] = None
    class_name: Optional[str] = None
    father_name: Optional[str] = None
    mother_name: Optional[str] = None
    parent_phone: Optional[str] = None
    phone: Optional[str] = None
    address: Optional[str] = None
    birth_date: Optional[str] = None
    gender: Optional[str] = None
    role_ids: Optional[List[str]] = None

class UserRead(BaseModel):
    id: str
    national_id: str
    full_name: str
    username: Optional[str] = None
    is_active: bool
    roles: List[RoleRead] = []        # لیست نقش‌ها همراه با مجوزهایشان
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