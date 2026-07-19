from fastapi import FastAPI
from app.core.config import settings
from app.db.session import Base, engine
from app import models  # noqa: F401
from app.routers import health, auth, bootstrap, security, workspaces, groups, activities, assignments, responses, evaluations, reports
from fastapi.middleware.cors import CORSMiddleware

Base.metadata.create_all(bind=engine)

app = FastAPI(title=settings.app_name, version=settings.app_version)

app.include_router(health.router, tags=["health"])
app.include_router(auth.router, prefix="/api/auth", tags=["auth"])
app.include_router(bootstrap.router, prefix="/api/bootstrap", tags=["bootstrap"])
app.include_router(security.router, prefix="/api/security", tags=["security"])
app.include_router(workspaces.router, prefix="/api/workspaces", tags=["workspaces"])
app.include_router(groups.router, prefix="/api/groups", tags=["groups"])
app.include_router(activities.router, prefix="/api/activities", tags=["activities"])
app.include_router(assignments.router, prefix="/api/assignments", tags=["assignments"])
app.include_router(responses.router, prefix="/api/responses", tags=["responses"])
app.include_router(evaluations.router, prefix="/api/evaluations", tags=["evaluations"])
app.include_router(reports.router, prefix="/api/reports", tags=["Reports"])
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5173"],   # آدرس فرانت‌اند
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

