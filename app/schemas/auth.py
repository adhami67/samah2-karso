from app.schemas.base import ORMModel

class Token(ORMModel):
    access_token: str
    token_type: str = "bearer"

class LoginRequest(ORMModel):
    national_id: str
    password: str
