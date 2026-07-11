from pydantic_settings import BaseSettings, SettingsConfigDict

class Settings(BaseSettings):
    app_name: str = "Karso"
    app_version: str = "0.3.0"
    database_url: str = "sqlite:///./karso.db"
    secret_key: str = "change-me-in-production"
    access_token_expire_minutes: int = 120

    bootstrap_admin_national_id: str = "1111111111"
    bootstrap_admin_password: str = "Admin@12345"
    bootstrap_admin_full_name: str = "System Admin"

    model_config = SettingsConfigDict(env_file=".env", env_file_encoding="utf-8", extra="ignore")

settings = Settings()
