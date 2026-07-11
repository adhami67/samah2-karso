from app.schemas.base import ORMModel

class PermissionCreate(ORMModel):
    code: str
    name: str

class PermissionRead(ORMModel):
    id: str
    code: str
    name: str

class RoleCreate(ORMModel):
    code: str
    name: str
    permission_ids: list[str] = []

class RoleRead(ORMModel):
    id: str
    code: str
    name: str
    permission_ids: list[str] = []

class UserCreate(ORMModel):
    national_id: str
    full_name: str
    password: str
    role_ids: list[str] = []

class UserRead(ORMModel):
    id: str
    national_id: str
    full_name: str
    is_active: bool
    role_ids: list[str] = []
