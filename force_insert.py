import sqlite3

conn = sqlite3.connect("karso.db")
cursor = conn.cursor()

# ۱. پیدا کردن کاربر ادمین
cursor.execute("SELECT id FROM users WHERE national_id = '1111111111'")
admin = cursor.fetchone()
if not admin:
    print("❌ کاربر ادمین پیدا نشد! اول لاگین کن.")
    exit()
admin_id = admin[0]
print(f"✅ admin_id: {admin_id}")

# ۲. ایجاد یک WorkType پیش‌فرض (اگر وجود نداشته باشد)
cursor.execute("SELECT id FROM work_types LIMIT 1")
wt = cursor.fetchone()
if not wt:
    cursor.execute("""
    INSERT INTO work_types (id, code, title, color, icon, description, is_deleted, created_at, updated_at)
    VALUES (?, ?, ?, ?, ?, ?, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
    """, ("123e4567-e89b-12d3-a456-426614174050", "general", "عمومی", "#000000", "icon", "نوع پیش‌فرض"))
    wt_id = "123e4567-e89b-12d3-a456-426614174050"
else:
    wt_id = wt[0]
print(f"✅ work_type_id: {wt_id}")

# ۳. ایجاد یک WorkPriority پیش‌فرض (اگر وجود نداشته باشد)
cursor.execute("SELECT id FROM work_priorities LIMIT 1")
wp = cursor.fetchone()
if not wp:
    cursor.execute("""
    INSERT INTO work_priorities (id, priority_number, priority_title, priority_color, priority_icon, description, is_deleted, created_at, updated_at)
    VALUES (?, ?, ?, ?, ?, ?, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
    """, ("123e4567-e89b-12d3-a456-426614174060", 1, "عادی", "#000000", "icon", "اولویت پیش‌فرض"))
    wp_id = "123e4567-e89b-12d3-a456-426614174060"
else:
    wp_id = wp[0]
print(f"✅ work_priority_id: {wp_id}")

# ۴. ایجاد حوزه
group_id = "123e4567-e89b-12d3-a456-426614174099"
cursor.execute("""
INSERT OR REPLACE INTO work_groups (
    id, name, description, status, owner_user_id, is_deleted, created_at, updated_at
) VALUES (?, ?, ?, ?, ?, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
""", (group_id, "حوزه تست", "این حوزه برای تست است", "active", admin_id))

# ۵. ایجاد کار با شناسه‌های معتبر
work_id = "123e4567-e89b-12d3-a456-426614174100"
cursor.execute("""
INSERT OR REPLACE INTO works (
    id, work_group_id, work_type_id, work_priority_id,
    sender_user_id, created_by_user_id,
    subject, description, status, due_at, is_deleted,
    created_at, updated_at
) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, datetime('now', '+2 days'), 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
""", (work_id, group_id, wt_id, wp_id, admin_id, admin_id,
      "تست ارجاع", "این یک کار تستی است", "published"))

conn.commit()

# ۶. بررسی نهایی
cursor.execute("SELECT id, subject FROM works WHERE id = ?", (work_id,))
result = cursor.fetchone()
if result:
    print(f"✅ کار با موفقیت ذخیره شد: {result[0]} - {result[1]}")
else:
    print("❌ متأسفانه کار ذخیره نشد!")

conn.close()