# create_tables.py
import sys
from pathlib import Path

# افزودن مسیر پروژه به sys.path
sys.path.append(str(Path(__file__).parent))

from app.db.session import engine
from app.models.base import Base

print("در حال ایجاد جدول‌ها...")
Base.metadata.create_all(bind=engine)
print("جدول‌ها با موفقیت ایجاد شدند.")