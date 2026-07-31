from datetime import datetime, timedelta, timezone
from typing import Optional

import bcrypt
from jose import JWTError, jwt

from app.core.config import settings


def hash_password(password: str) -> str:
    """هش کردن رمز عبور با bcrypt"""
    salt = bcrypt.gensalt()
    hashed = bcrypt.hashpw(password.encode("utf-8"), salt)
    return hashed.decode("utf-8")


def verify_password(plain_password: str, hashed_password: str) -> bool:
    """تأیید رمز عبور با bcrypt"""
    return bcrypt.checkpw(
        plain_password.encode("utf-8"),
        hashed_password.encode("utf-8")
    )


def create_access_token(subject: str, expires_delta: Optional[timedelta] = None) -> str:
    """ساخت JWT توکن با دریافت subject"""
    to_encode = {"sub": subject}
    if expires_delta:
        expire = datetime.now(timezone.utc) + expires_delta
    else:
        # ✅ اصلاح: استفاده از نام صحیح
        expire = datetime.now(timezone.utc) + timedelta(minutes=settings.jwt_access_token_expire_minutes)
    to_encode.update({"exp": expire})
    encoded_jwt = jwt.encode(to_encode, settings.secret_key, algorithm="HS256")
    return encoded_jwt


def decode_token(token: str) -> dict:
    """رمزگشایی JWT و برگرداندن payload"""
    try:
        payload = jwt.decode(token, settings.secret_key, algorithms=["HS256"])
        return payload
    except JWTError:
        raise ValueError("Invalid token")