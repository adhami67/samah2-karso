from collections.abc import Generator
from typing import Optional
from fastapi import Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer
from jose import JWTError
from sqlalchemy.orm import Session
from app.db.session import SessionLocal
from app.models.security import User
from app.core.security import decode_token as verify_token

oauth2_scheme = OAuth2PasswordBearer(tokenUrl="api/auth/login")

def get_db() -> Generator:
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

def get_current_user(
    db: Session = Depends(get_db),
    token: str = Depends(oauth2_scheme)
) -> User:
    credentials_exception = HTTPException(
        status_code=status.HTTP_401_UNAUTHORIZED,
        detail="Could not validate credentials",
        headers={"WWW-Authenticate": "Bearer"},
    )
    try:
        payload = verify_token(token)
        user_id: str = payload.get("sub")
        if user_id is None:
            raise credentials_exception
    except (JWTError, ValueError):
        raise credentials_exception

    user = db.get(User, user_id)
    if user is None:
        raise credentials_exception
    return user

async def get_current_user_ws(token: str) -> Optional[User]:
    """
    دریافت کاربر از توکن JWT برای WebSocket
    این تابع به‌صورت async تعریف شده تا در WebSocket قابل استفاده باشد.
    """
    if not token:
        return None
    try:
        payload = verify_token(token)
        user_id: str = payload.get("sub")
        if not user_id:
            return None
        db = SessionLocal()
        user = db.get(User, user_id)
        db.close()
        return user
    except Exception:
        return None