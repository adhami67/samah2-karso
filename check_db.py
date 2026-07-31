# check_db.py
import sqlite3

conn = sqlite3.connect("karso.db")
cursor = conn.cursor()

# بررسی جدول works
cursor.execute("SELECT id, subject FROM works;")
works = cursor.fetchall()

if works:
    print("✅ کار(ها) در دیتابیس وجود دارند:")
    for w in works:
        print(f"   - ID: {w[0]}, Subject: {w[1]}")
else:
    print("❌ هیچ کاری در دیتابیس پیدا نشد.")

# بررسی جدول work_groups
cursor.execute("SELECT id, name FROM work_groups;")
groups = cursor.fetchall()

if groups:
    print("✅ حوزه(ها) در دیتابیس وجود دارند:")
    for g in groups:
        print(f"   - ID: {g[0]}, Name: {g[1]}")
else:
    print("❌ هیچ حوزه‌ای در دیتابیس پیدا نشد.")

conn.close()