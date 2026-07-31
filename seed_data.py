# seed_data.py
from app.db.session import SessionLocal
from app.models.work_type import WorkType
from app.models.work_priority import WorkPriority

db = SessionLocal()

# ------------------ ایجاد نوع کار ------------------
if not db.query(WorkType).filter(WorkType.id == "type1").first():
    work_type = WorkType(
        id="type1",
        code="general",
        title="تکلیف عمومی",
        description="انجام تکالیف عادی",
        color="#4CAF50",          # رنگ پیش‌فرض
        icon=b""                  # آیکون باینری خالی (اگر ستون از نوع BLOB است)
    )
    db.add(work_type)
    print("✅ نوع کار اضافه شد")

# ------------------ ایجاد اولویت ------------------
if not db.query(WorkPriority).filter(WorkPriority.id == "prio1").first():
    work_priority = WorkPriority(
        id="prio1",
        priority_title="عادی",
        priority_number=1,
        priority_color="#4CAF50",
        priority_icon=b"",        # مقدار باینری خالی
        description="اولویت عادی"
    )
    db.add(work_priority)
    print("✅ اولویت اضافه شد")

db.commit()
db.close()
print("🎉 داده‌های پیش‌فرض با موفقیت درج شدند.")