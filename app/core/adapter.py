# app/core/adapter.py

# این فایل به‌عنوان پل ارتباطی بین فایل‌های قدیمی و مدل‌های جدید عمل می‌کند.
# با این کار، فایل‌های قدیمی بدون تغییرات عمده به مدل‌های جدید متصل می‌شوند.

from app.models.work import Work
from app.models.work_group import WorkGroup
from app.models.work_group_member import WorkGroupMember
from app.models.work_receiver import WorkReceiver
from app.models.work_message import WorkMessage
from app.models.work_attachment import WorkAttachment
from app.models.work_priority import WorkPriority
from app.models.work_type import WorkType
from app.models.work_permission import WorkPermission
from app.models.timeline_event import TimelineEvent
from app.models.notification import Notification
from app.models.evaluation import Evaluation
from app.models.response import Response

# ========== نام‌های جدید را با نام‌های قدیمی alias می‌کنیم ==========
Workspace = WorkGroup
Activity = Work
Assignment = WorkReceiver
Group = WorkGroupMember

# Response و Notification قبلاً وجود دارند و نیازی به alias ندارند،
# اما اگر در جایی از نام قدیمی استفاده شده باشد، می‌توانیم alias کنیم.
# مثلاً اگر فایل قدیمی از Notification استفاده می‌کند، همین نام کافی است.