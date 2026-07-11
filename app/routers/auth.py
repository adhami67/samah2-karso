from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.security import User
from app.schemas.auth import LoginRequest, Token
from app.utils.security import verify_password, create_access_token
from app.utils.deps import get_current_user

router = APIRouter()

@router.post("/login", response_model=Token)
def login(payload: LoginRequest, db: Session = Depends(get_db)):
    user = db.query(User).filter(User.national_id == payload.national_id).first()
    if not user or not verify_password(payload.password, user.password_hash):
        raise HTTPException(status_code=401, detail="Incorrect national id or password")
    token = create_access_token(subject=user.id)
    return Token(access_token=token)

@router.get("/me")
def me(user: User = Depends(get_current_user)):
    return {
        "id": user.id,
        "national_id": user.national_id,
        "full_name": user.full_name,
        "roles": [role.code for role in user.roles],
    }
