"""
تعریف استثناهای سفارشی پروژه
"""

class NotFoundError(Exception):
    """خطا زمانی که موجودیت در دیتابیس پیدا نشود"""
    def __init__(self, message: str = "Not found"):
        self.message = message
        super().__init__(self.message)

class InvalidStateTransitionError(Exception):
    """خطا زمانی که تغییر وضعیت مجاز نباشد"""
    def __init__(self, message: str = "Invalid state transition"):
        self.message = message
        super().__init__(self.message)

class ConflictError(Exception):
    """خطا زمانی که داده تکراری یا تداخل وجود داشته باشد"""
    def __init__(self, message: str = "Conflict"):
        self.message = message
        super().__init__(self.message)

class ValidationError(Exception):
    """خطا زمانی که داده ورودی معتبر نباشد"""
    def __init__(self, message: str = "Validation error"):
        self.message = message
        super().__init__(self.message)

class PermissionDeniedError(Exception):
    """خطا زمانی که کاربر مجوز انجام عملیات را نداشته باشد"""
    def __init__(self, message: str = "Permission denied"):
        self.message = message
        super().__init__(self.message)