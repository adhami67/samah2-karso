from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.schemas.bootstrap import BootstrapStatusRead, BootstrapSetupRead
from app.services.bootstrap import bootstrap_status, bootstrap_system

router = APIRouter()

@router.get("/status", response_model=BootstrapStatusRead)
def status(db: Session = Depends(get_db)):
    return bootstrap_status(db)

@router.post("/setup", response_model=BootstrapSetupRead)
def setup(db: Session = Depends(get_db)):
    return bootstrap_system(db)
