// frontend/src/components/PriorityList.tsx
import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { AlertTriangle, Clock, MessageSquare, Calendar, Eye } from "lucide-react";
import { useNavigate } from "react-router-dom";

interface Priorities {
  overdue: number;
  due_today: number;
  unread_messages: number;
  classes_today: number;
  needs_review: number;
}

export function PriorityList() {
  const [priorities, setPriorities] = useState<Priorities | null>(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    api.get<Priorities>("/reports/today-priorities")
        .then((data) => setPriorities(data))
        .catch(console.error)
        .finally(() => setLoading(false));
  }, []);

  if (loading) return <div className="animate-pulse h-20 bg-muted/30 rounded-lg" />;
  if (!priorities) return null;

  const items = [
    { label: "معوق", value: priorities.overdue, icon: AlertTriangle, color: "bg-red-100 text-red-700 border-red-300", link: "/activities?status=overdue" },
    { label: "مهلت امروز", value: priorities.due_today, icon: Clock, color: "bg-amber-100 text-amber-700 border-amber-300", link: "/activities?status=due_today" },
    { label: "پیام جدید", value: priorities.unread_messages, icon: MessageSquare, color: "bg-green-100 text-green-700 border-green-300", link: "/messages" },
    { label: "نیاز به بررسی", value: priorities.needs_review, icon: Eye, color: "bg-purple-100 text-purple-700 border-purple-300", link: "/activities?status=needs_review" },
    { label: "کلاس امروز", value: priorities.classes_today, icon: Calendar, color: "bg-blue-100 text-blue-700 border-blue-300", link: "/calendar" },
  ];

  return (
    <div className="grid grid-cols-2 md:grid-cols-5 gap-3">
      {items.filter(item => item.value > 0).map((item) => (
        <Card
          key={item.label}
          className={`cursor-pointer hover:shadow-md transition-all border-l-4 ${item.color}`}
          onClick={() => navigate(item.link)}
        >
          <CardContent className="p-3 flex items-center gap-3">
            <item.icon className="h-5 w-5 flex-shrink-0" />
            <div className="flex-1 min-w-0">
              <p className="text-lg font-bold">{item.value}</p>
              <p className="text-xs text-muted-foreground truncate">{item.label}</p>
            </div>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}