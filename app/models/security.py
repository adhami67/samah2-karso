from sqlalchemy import ForeignKey, String, Table, Column
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.models.base import TimestampedUUIDModel
from app.db.session import Base

# ---------- جداول واسط ----------
user_roles = Table(
    "user_roles",
    Base.metadata,
    Column("user_id", ForeignKey("users.id"), primary_key=True),
    Column("role_id", ForeignKey("roles.id"), primary_key=True),
)

role_permissions = Table(
    "role_permissions",
    Base.metadata,
    Column("role_id", ForeignKey("roles.id"), primary_key=True),
    Column("permission_id", ForeignKey("permissions.id"), primary_key=True),
)

# ---------- Permission ----------
class Permission(TimestampedUUIDModel):
    __tablename__ = "permissions"
    code: Mapped[str] = mapped_column(String(120), unique=True, nullable=False, index=True)
    name: Mapped[str] = mapped_column(String(200), nullable=False)

# ---------- Role ----------
class Role(TimestampedUUIDModel):
    __tablename__ = "roles"
    code: Mapped[str] = mapped_column(String(120), unique=True, nullable=False, index=True)
    name: Mapped[str] = mapped_column(String(200), nullable=False)

    permissions = relationship("Permission", secondary=role_permissions, backref="roles")

# ---------- User (ادغام‌شده) ----------
class User(TimestampedUUIDModel):
    __tablename__ = "users"
    
    username: Mapped[str] = mapped_column(String(100), unique=True, nullable=False, index=True)
    national_id: Mapped[str] = mapped_column(String(20), unique=True, nullable=False, index=True)
    full_name: Mapped[str] = mapped_column(String(200), nullable=False)
    password_hash: Mapped[str] = mapped_column(String(255), nullable=False)
    is_active: Mapped[bool] = mapped_column(default=True, nullable=False)

    # فیلدهای اضافی برای دانش‌آموزان (اختیاری)
    grade: Mapped[str | None] = mapped_column(String(50), nullable=True)
    class_name: Mapped[str | None] = mapped_column(String(50), nullable=True)
    father_name: Mapped[str | None] = mapped_column(String(200), nullable=True)
    mother_name: Mapped[str | None] = mapped_column(String(200), nullable=True)
    parent_phone: Mapped[str | None] = mapped_column(String(20), nullable=True)
    phone: Mapped[str | None] = mapped_column(String(20), nullable=True)
    address: Mapped[str | None] = mapped_column(String(500), nullable=True)
    birth_date: Mapped[str | None] = mapped_column(String(20), nullable=True)
    gender: Mapped[str | None] = mapped_column(String(10), nullable=True)
    father_last_name: Mapped[str | None] = mapped_column(String(200), nullable=True)
    mother_last_name: Mapped[str | None] = mapped_column(String(200), nullable=True)
    major: Mapped[str | None] = mapped_column(String(100), nullable=True)
    academic_year: Mapped[str | None] = mapped_column(String(20), nullable=True)
    roles = relationship("Role", secondary=user_roles, backref="users")
    