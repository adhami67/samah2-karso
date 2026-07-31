# create_admin.py
from app.db.session import SessionLocal
from app.models.security import User
from app.core.security import get_password_hash

db = SessionLocal()

# اگر کاربر قبلاً وجود ندارد
if not db.query(User).filter(User.national_id == "1111111111").first():
    admin = User(
        id="admin-user-id",  # این خط را اضافه کنید (یا حذف کنید تا خودکار تولید شود)
        username="admin",
        national_id="1111111111",
        full_name="مدیر سیستم",
        password_hash=get_password_hash("Admin@12345"),
        is_active=True
    )
    db.add(admin)
    db.commit()
    print("✅ ادمین با موفقیت ایجاد شد")
else:
    print("⚠️ ادمین قبلاً وجود دارد")

db.close()