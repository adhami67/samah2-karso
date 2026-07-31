# seed_data.py
from app.db.session import SessionLocal
from app.models.security import Role, Permission
from app.models.work_type import WorkType
from app.models.work_priority import WorkPriority
from app.core.constants import (
    ROLE_SYSTEM_ADMIN,
    ROLE_SCHOOL_ADMIN,
    ROLE_TEACHER,
    ROLE_STUDENT,
    ROLE_PARENT,
    PERMISSION_MANAGE_SYSTEM,
    PERMISSION_MANAGE_USERS,
    PERMISSION_MANAGE_ROLES,
    PERMISSION_MANAGE_WORKSPACES,
    PERMISSION_MANAGE_ACTIVITIES,
    PERMISSION_VIEW_DASHBOARD,
)

db = SessionLocal()

# ================== ۱. مجوزهای پایه ==================
permissions_data = [
    {"id": "perm_system", "code": PERMISSION_MANAGE_SYSTEM, "name": "مدیریت سیستم"},
    {"id": "perm_users", "code": PERMISSION_MANAGE_USERS, "name": "مدیریت کاربران"},
    {"id": "perm_roles", "code": PERMISSION_MANAGE_ROLES, "name": "مدیریت نقش‌ها"},
    {"id": "perm_workspaces", "code": PERMISSION_MANAGE_WORKSPACES, "name": "مدیریت حوزه‌های کاری"},
    {"id": "perm_activities", "code": PERMISSION_MANAGE_ACTIVITIES, "name": "مدیریت فعالیت‌ها"},
    {"id": "perm_dashboard", "code": PERMISSION_VIEW_DASHBOARD, "name": "مشاهده داشبورد"},
]

# نگاشت کد به شیء Permission (چه جدید چه قدیمی)
permission_map = {}
for p in permissions_data:
    existing = db.query(Permission).filter(Permission.code == p["code"]).first()
    if existing:
        # اگر مجوز وجود دارد، از همان استفاده کن
        permission_map[p["code"]] = existing
    else:
        # در غیر این صورت، با id دلخواه خودمان ایجاد کن
        new_perm = Permission(id=p["id"], code=p["code"], name=p["name"])
        db.add(new_perm)
        permission_map[p["code"]] = new_perm

# ================== ۲. نقش‌های پایه ==================
roles_data = [
    {"code": ROLE_SYSTEM_ADMIN, "name": "مدیر سیستم",
     "permission_codes": [PERMISSION_MANAGE_SYSTEM, PERMISSION_MANAGE_USERS, PERMISSION_MANAGE_ROLES,
                          PERMISSION_MANAGE_WORKSPACES, PERMISSION_MANAGE_ACTIVITIES, PERMISSION_VIEW_DASHBOARD]},
    {"code": ROLE_SCHOOL_ADMIN, "name": "مدیر مدرسه",
     "permission_codes": [PERMISSION_MANAGE_WORKSPACES, PERMISSION_MANAGE_ACTIVITIES, PERMISSION_VIEW_DASHBOARD]},
    {"code": ROLE_TEACHER, "name": "معلم",
     "permission_codes": [PERMISSION_MANAGE_ACTIVITIES, PERMISSION_VIEW_DASHBOARD]},
    {"code": ROLE_STUDENT, "name": "دانش‌آموز",
     "permission_codes": [PERMISSION_VIEW_DASHBOARD]},
    {"code": ROLE_PARENT, "name": "والدین",
     "permission_codes": [PERMISSION_VIEW_DASHBOARD]},
]

for r in roles_data:
    role = db.query(Role).filter(Role.code == r["code"]).first()
    if not role:
        role = Role(code=r["code"], name=r["name"])
        db.add(role)
    # به‌روزرسانی مجوزهای نقش (حتی اگر قبلاً وجود داشت)
    perms = [permission_map[c] for c in r["permission_codes"] if c in permission_map]
    role.permissions = perms

# ================== ۳. انواع کار ==================
if not db.query(WorkType).filter(WorkType.code == "general").first():
    db.add(WorkType(
        id="type1",
        code="general",
        title="تکلیف عمومی",
        description="انجام تکالیف عادی",
        color="#4CAF50",
        icon=b""
    ))

# ================== ۴. اولویت‌ها ==================
if not db.query(WorkPriority).filter(WorkPriority.priority_title == "عادی").first():
    db.add(WorkPriority(
        id="prio1",
        priority_title="عادی",
        priority_number=1,
        priority_color="#4CAF50",
        priority_icon=b"",
        description="اولویت عادی"
    ))

db.commit()
db.close()
print("✅ داده‌های اولیه (نقش‌ها، مجوزها، انواع کار و اولویت‌ها) با موفقیت ایجاد/به‌روز شدند.")