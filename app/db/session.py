from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker, declarative_base
from sqlalchemy.pool import NullPool
from app.core.config import settings

# اضافه کردن timeout برای جلوگیری از قفل شدن دیتابیس SQLite
database_url = settings.database_url
if "sqlite" in database_url and "?" not in database_url:
    database_url += "?timeout=10"

engine = create_engine(
    database_url,
    connect_args={"check_same_thread": False},
    poolclass=NullPool,           # برای رفع مشکل قفل شدن
)

SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# تعریف Base برای ساخت مدل‌ها
Base = declarative_base()

def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()