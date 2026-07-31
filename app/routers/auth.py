# app/routers/auth.py
from fastapi import APIRouter, Depends, HTTPException, status
from fastapi.security import OAuth2PasswordRequestForm
from sqlalchemy.orm import Session
from app.db.deps import get_db
from app.models.security import User
from app.schemas.auth import LoginRequest, Token
from app.core.security import verify_password, create_access_token
from app.utils.deps import get_current_user

router = APIRouter()

# لاگین با JSON (برای استفاده در فرانت‌اند)
@router.post("/login", response_model=Token)
def login(payload: LoginRequest, db: Session = Depends(get_db)):
    user = db.query(User).filter(User.national_id == payload.national_id).first()
    if not user or not verify_password(payload.password, user.password_hash):
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="کد ملی یا رمز عبور اشتباه است"
        )
    token = create_access_token(data={"sub": user.id})
    return Token(access_token=token)

# لاگین با فرم OAuth2 (برای Swagger UI)
@router.post("/token", response_model=Token)
def login_for_access_token(
    form_data: OAuth2PasswordRequestForm = Depends(),
    db: Session = Depends(get_db)
):
    """
    دریافت توکن با استفاده از فرم OAuth2 (برای Swagger UI)
    در اینجا `username` معادل `national_id` است.
    """
    user = db.query(User).filter(User.national_id == form_data.username).first()
    if not user or not verify_password(form_data.password, user.password_hash):
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="کد ملی یا رمز عبور اشتباه است"
        )
    token = create_access_token(data={"sub": user.id})
    return Token(access_token=token)

@router.get("/me")
def me(user: User = Depends(get_current_user)):
    return {
        "id": user.id,
        "national_id": user.national_id,
        "full_name": user.full_name,
        "username": user.username,
        "roles": [role.code for role in user.roles],
    }