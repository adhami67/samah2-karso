from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from app.core.config import settings
from app.db.session import Base, engine
from app import models  # noqa: F401
from app.routers import (
    health,
    auth,
    bootstrap,
    security,
    timeline,
    workflow_router,
    work_message_router,
    work_attachment_router,
    report_router,
    notification_router,
    workspaces,
    activities,
    responses,
    bulk_upload,
    student_note_router,
)

Base.metadata.create_all(bind=engine)

app = FastAPI(title=settings.app_name, version=settings.app_version)

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# ========== روت‌های جدید ==========
app.include_router(health.router, tags=["health"])
app.include_router(auth.router, prefix="/api/auth", tags=["auth"])
app.include_router(bootstrap.router, prefix="/api/bootstrap", tags=["bootstrap"])
app.include_router(security.router, prefix="/api/security", tags=["security"])
app.include_router(timeline.router, prefix="/api/timeline", tags=["Timeline"])
app.include_router(workflow_router.router)
app.include_router(work_message_router.router)
app.include_router(work_attachment_router.router)
app.include_router(student_note_router.router)
app.include_router(report_router.router, prefix="/api/reports", tags=["Reports"])

# ✅ اصلاح: پیشوند /api/notifications اضافه شد
app.include_router(notification_router.router, tags=["Notifications"])

app.include_router(bulk_upload.router, prefix="/api/bulk", tags=["Bulk"])
app.include_router(responses.router, prefix="/api/responses", tags=["Responses"])

# ========== روت‌های قدیمی ==========
app.include_router(workspaces.router, prefix="/api/workspaces", tags=["Workspaces"])
app.include_router(activities.router, prefix="/api/activities", tags=["Activities"])

# ❌ خط تکراری زیر حذف شده است:
# app.include_router(notification_router.router)