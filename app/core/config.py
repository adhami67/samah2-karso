from pydantic_settings import BaseSettings, SettingsConfigDict

class Settings(BaseSettings):
    # تنظیمات عمومی
    app_name: str = "Karso"
    app_version: str = "0.3.0"
    database_url: str = "sqlite:///./karso.db"
    
    # امنیت
    secret_key: str = "change-me-in-production"  # برای سازگاری با کدهای قبلی
    jwt_secret_key: str = "change-me-in-production-jwt"  # کلید مخصوص JWT
    jwt_algorithm: str = "HS256"  # الگوریتم JWT
    jwt_access_token_expire_minutes: int = 120  # مدت اعتبار توکن بر حسب دقیقه
    
    # تنظیمات سیستمی
    upload_dir: str = "./uploads"  # مسیر ذخیره فایل‌ها
    
    # تنظیمات Bootstrap (برای ایجاد کاربر ادمین اولیه)
    bootstrap_admin_national_id: str = "1111111111"
    bootstrap_admin_username: str = "admin"
    bootstrap_admin_password: str = "Admin@12345"
    bootstrap_admin_full_name: str = "System Admin"

    # پیکربندی محیطی
    model_config = SettingsConfigDict(
        env_file=".env",
        env_file_encoding="utf-8",
        extra="ignore",
        case_sensitive=False,
    )

    # برای سازگاری با کدهای قبلی که از secret_key استفاده می‌کردند
    @property
    def JWT_SECRET_KEY(self) -> str:
        """ارائه کلید JWT با نامی که در کدهای قبلی استفاده شده"""
        return self.jwt_secret_key

    @property
    def JWT_ALGORITHM(self) -> str:
        """ارائه الگوریتم JWT با نامی که در کدهای قبلی استفاده شده"""
        return self.jwt_algorithm

    @property
    def JWT_ACCESS_TOKEN_EXPIRE_MINUTES(self) -> int:
        """ارائه زمان انقضای توکن با نامی که در کدهای قبلی استفاده شده"""
        return self.jwt_access_token_expire_minutes


settings = Settings()