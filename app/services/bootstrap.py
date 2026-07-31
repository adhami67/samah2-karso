from sqlalchemy.orm import Session
from app.core.constants import (
    ROLE_SYSTEM_ADMIN,
    ROLE_SCHOOL_ADMIN,
    ROLE_TEACHER,
    ROLE_STUDENT,
    ROLE_PARENT,
    ROLE_SUPERVISOR,
    PERMISSION_MANAGE_SYSTEM,
    PERMISSION_MANAGE_USERS,
    PERMISSION_MANAGE_ROLES,
    PERMISSION_MANAGE_PERMISSIONS,
    PERMISSION_MANAGE_WORKSPACES,
    PERMISSION_MANAGE_GROUPS,
    PERMISSION_MANAGE_ACTIVITIES,
    PERMISSION_VIEW_DASHBOARD,
)
from app.models.security import Permission, Role, User
from app.utils.security import hash_password
from app.core.config import settings

DEFAULT_PERMISSIONS = [
    (PERMISSION_MANAGE_SYSTEM, "Manage system"),
    (PERMISSION_MANAGE_USERS, "Manage users"),
    (PERMISSION_MANAGE_ROLES, "Manage roles"),
    (PERMISSION_MANAGE_PERMISSIONS, "Manage permissions"),
    (PERMISSION_MANAGE_WORKSPACES, "Manage workspaces"),
    (PERMISSION_MANAGE_GROUPS, "Manage groups"),
    (PERMISSION_MANAGE_ACTIVITIES, "Manage activities"),
    (PERMISSION_VIEW_DASHBOARD, "View dashboard"),
]

DEFAULT_ROLES = [
    (ROLE_SYSTEM_ADMIN, "System Admin", [
        PERMISSION_MANAGE_SYSTEM,
        PERMISSION_MANAGE_USERS,
        PERMISSION_MANAGE_ROLES,
        PERMISSION_MANAGE_PERMISSIONS,
        PERMISSION_MANAGE_WORKSPACES,
        PERMISSION_MANAGE_GROUPS,
        PERMISSION_MANAGE_ACTIVITIES,
        PERMISSION_VIEW_DASHBOARD,
    ]),
    (ROLE_SCHOOL_ADMIN, "School Admin", [
        PERMISSION_MANAGE_USERS,
        PERMISSION_MANAGE_WORKSPACES,
        PERMISSION_MANAGE_GROUPS,
        PERMISSION_MANAGE_ACTIVITIES,
        PERMISSION_VIEW_DASHBOARD,
    ]),
    (ROLE_SUPERVISOR, "Supervisor", [
        PERMISSION_MANAGE_GROUPS,
        PERMISSION_MANAGE_ACTIVITIES,
        PERMISSION_VIEW_DASHBOARD,
    ]),
    (ROLE_TEACHER, "Teacher", [
        PERMISSION_MANAGE_ACTIVITIES,
        PERMISSION_VIEW_DASHBOARD,
    ]),
    (ROLE_STUDENT, "Student", [
        PERMISSION_VIEW_DASHBOARD,
    ]),
    (ROLE_PARENT, "Parent", [
        PERMISSION_VIEW_DASHBOARD,
    ]),
]

def bootstrap_system(db: Session) -> dict:
    permission_map = {}
    created_permissions = 0
    for code, name in DEFAULT_PERMISSIONS:
        item = db.query(Permission).filter(Permission.code == code).first()
        if not item:
            item = Permission(code=code, name=name)
            db.add(item)
            created_permissions += 1
        permission_map[code] = item

    role_map = {}
    created_roles = 0
    for code, name, permission_codes in DEFAULT_ROLES:
        item = db.query(Role).filter(Role.code == code).first()
        if not item:
            item = Role(code=code, name=name)
            db.add(item)
            created_roles += 1
        item.permissions = [permission_map[p] for p in permission_codes]
        role_map[code] = item

    admin = db.query(User).filter(User.national_id == settings.bootstrap_admin_national_id).first()
    created_admin_user = False
    if not admin:
        admin = User(
            national_id=settings.bootstrap_admin_national_id,
            full_name=settings.bootstrap_admin_full_name,
            username=settings.bootstrap_admin_username,
            password_hash=hash_password(settings.bootstrap_admin_password),
            roles=[role_map[ROLE_SYSTEM_ADMIN]],
        )
        db.add(admin)
        created_admin_user = True
    else:
        if ROLE_SYSTEM_ADMIN not in {role.code for role in admin.roles}:
            admin.roles.append(role_map[ROLE_SYSTEM_ADMIN])

    db.commit()
    return {
        "created_permissions": created_permissions,
        "created_roles": created_roles,
        "created_admin_user": created_admin_user,
        "admin_national_id": settings.bootstrap_admin_national_id,
        "admin_full_name": settings.bootstrap_admin_full_name,
    }

def bootstrap_status(db: Session) -> dict:
    permissions_count = db.query(Permission).count()
    roles_count = db.query(Role).count()
    users_count = db.query(User).count()
    return {
        "permissions_count": permissions_count,
        "roles_count": roles_count,
        "users_count": users_count,
        "is_ready": permissions_count > 0 and roles_count > 0 and users_count > 0,
    }