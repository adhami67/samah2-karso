import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { Button } from "@/components/ui/button";
import { Bell } from "lucide-react";
import { Badge } from "@/components/ui/badge";
import { ScrollArea } from "@/components/ui/scroll-area";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";

interface Notification {
  id: string;
  title: string;
  body: string | null;
  link: string | null;
  is_read: boolean;
  created_at: string;
}

export function NotificationBell() {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const [open, setOpen] = useState(false);
  const navigate = useNavigate();

  const fetchNotifications = async () => {
    try {
      const [list, count] = await Promise.all([
        // ✅ بدون اسلش انتهایی
        api.get<Notification[]>("/notifications?limit=20"),
        // ✅ بدون اسلش انتهایی
        api.get<{ count: number }>("/notifications/unread-count"),
      ]);
      setNotifications(list);
      setUnreadCount(count.count);
    } catch (error) {
      console.error("خطا در دریافت اعلان‌ها:", error);
    }
  };

  const markAsRead = async (id: string) => {
    try {
      // ✅ بدون اسلش انتهایی
      await api.patch(`/notifications/${id}/read`, {});
      setNotifications((prev) =>
        prev.map((n) => (n.id === id ? { ...n, is_read: true } : n))
      );
      setUnreadCount((prev) => Math.max(0, prev - 1));
    } catch (error) {
      console.error("خطا در علامت‌گذاری:", error);
    }
  };

  const markAllAsRead = async () => {
    try {
      // ✅ بدون اسلش انتهایی
      await api.post("/notifications/read-all", {});
      setNotifications((prev) => prev.map((n) => ({ ...n, is_read: true })));
      setUnreadCount(0);
      toast.success("همه اعلان‌ها خوانده شدند");
    } catch (error) {
      console.error("خطا در علامت‌گذاری همه:", error);
    }
  };

  const handleNotificationClick = (notification: Notification) => {
    if (!notification.is_read) {
      markAsRead(notification.id);
    }
    if (notification.link) {
      navigate(notification.link);
      setOpen(false);
    }
  };

  useEffect(() => {
    fetchNotifications();
    const interval = setInterval(fetchNotifications, 30000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="relative">
      <Button
        variant="ghost"
        size="icon"
        className="relative"
        onClick={() => setOpen(!open)}
      >
        <Bell className="h-5 w-5" />
        {unreadCount > 0 && (
          <Badge
            variant="destructive"
            className="absolute -top-1 -right-1 h-5 w-5 flex items-center justify-center p-0 text-xs"
          >
            {unreadCount > 9 ? "9+" : unreadCount}
          </Badge>
        )}
      </Button>

      {open && (
        <>
          <div
            className="fixed inset-0 z-40"
            onClick={() => setOpen(false)}
          />
          <div className="absolute left-0 top-12 z-50 w-80 bg-white rounded-lg shadow-lg border">
            <div className="flex items-center justify-between p-3 border-b">
              <span className="font-semibold text-sm">اعلان‌ها</span>
              {unreadCount > 0 && (
                <Button
                  variant="ghost"
                  size="sm"
                  className="text-xs h-8"
                  onClick={markAllAsRead}
                >
                  همه را خوانده‌شده کن
                </Button>
              )}
            </div>
            <ScrollArea className="h-80">
              {notifications.length === 0 ? (
                <div className="flex flex-col items-center justify-center h-full p-4 text-center text-muted-foreground text-sm">
                  <Bell className="h-8 w-8 mb-2 opacity-20" />
                  <p>هیچ اعلانی وجود ندارد</p>
                </div>
              ) : (
                notifications.map((notification) => (
                  <div
                    key={notification.id}
                    className={`p-3 border-b cursor-pointer hover:bg-muted/50 transition-colors ${
                      !notification.is_read ? "bg-primary/5" : ""
                    }`}
                    onClick={() => handleNotificationClick(notification)}
                  >
                    <div className="flex items-start gap-3">
                      <div className="flex-1 min-w-0">
                        <p className="text-sm font-medium">{notification.title}</p>
                        <p className="text-xs text-muted-foreground truncate">
                          {notification.body}
                        </p>
                        <p className="text-xs text-muted-foreground/70 mt-1">
                          {new Date(notification.created_at).toLocaleString("fa-IR")}
                        </p>
                      </div>
                      {!notification.is_read && (
                        <span className="h-2 w-2 rounded-full bg-blue-500 flex-shrink-0 mt-1" />
                      )}
                    </div>
                  </div>
                ))
              )}
            </ScrollArea>
          </div>
        </>
      )}
    </div>
  );
}