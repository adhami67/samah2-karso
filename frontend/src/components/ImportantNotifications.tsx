import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Bell } from "lucide-react";
import { useNavigate } from "react-router-dom";

interface Notification {
  id: string;
  title: string;
  body: string | null;
  link: string | null;
  is_read: boolean;
  created_at: string;
}

export function ImportantNotifications() {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    // ✅ مسیر بدون اسلش انتهایی (فقط کوئری پارامتر)
    api
      .get<Notification[]>("/notifications?limit=3&unread_only=true")
      .then(setNotifications)
      .catch(console.error)
      .finally(() => setLoading(false));
  }, []);

  if (loading) return null;
  if (notifications.length === 0) return null;

  return (
    <Card className="glass-card border-r-4 border-blue-500">
      <CardContent className="p-4">
        <div className="flex items-center justify-between mb-2">
          <h3 className="flex items-center gap-2 text-base font-semibold text-slate-700">
            <Bell className="text-blue-500" size={18} />
            اعلان‌های مهم
          </h3>
          <Button variant="ghost" size="sm" onClick={() => navigate("/notifications")}>
            مشاهده همه
          </Button>
        </div>
        <div className="space-y-2">
          {notifications.map((notif) => (
            <div
              key={notif.id}
              className="p-2 rounded-lg bg-blue-50 hover:bg-blue-100 cursor-pointer transition-colors"
              onClick={() => {
                if (notif.link) navigate(notif.link);
              }}
            >
              <p className="text-sm font-medium">{notif.title}</p>
              {notif.body && (
                <p className="text-xs text-muted-foreground truncate">{notif.body}</p>
              )}
              <p className="text-xs text-muted-foreground/70 mt-1">
                {new Date(notif.created_at).toLocaleString("fa-IR")}
              </p>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
}