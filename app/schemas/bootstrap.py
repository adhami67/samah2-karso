from app.schemas.base import ORMModel

class BootstrapStatusRead(ORMModel):
    permissions_count: int
    roles_count: int
    users_count: int
    is_ready: bool

class BootstrapSetupRead(ORMModel):
    created_permissions: int
    created_roles: int
    created_admin_user: bool
    admin_national_id: str
    admin_full_name: str
