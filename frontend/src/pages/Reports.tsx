// frontend/src/pages/Reports.tsx
import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { BarChart3, Users, FolderOpen, Clock, TrendingUp } from "lucide-react";
import { useAuth } from "@/contexts/AuthContext";

interface SystemStats {
  total_users: number;
  total_workspaces: number;
  total_activities: number;
  total_overdue: number;
}

export default function Reports() {
  const { user } = useAuth();
  const [stats, setStats] = useState<SystemStats | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const data = await api.get<SystemStats>("/reports/system/stats");
        setStats(data);
      } catch (error) {
        console.error("خطا در دریافت آمار سیستم:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchStats();
  }, []);

  if (loading) {
    return (
      <div className="space-y-6 p-6 max-w-6xl mx-auto">
        <Skeleton className="h-8 w-48" />
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          {Array.from({ length: 4 }).map((_, i) => (
            <Skeleton key={i} className="h-28 rounded-xl" />
          ))}
        </div>
      </div>
    );
  }

  const statItems = [
    { label: "کاربران کل", value: stats?.total_users || 0, icon: Users, color: "bg-blue-100 text-blue-700" },
    { label: "حوزه‌های کاری", value: stats?.total_workspaces || 0, icon: FolderOpen, color: "bg-purple-100 text-purple-700" },
    { label: "فعالیت‌های کل", value: stats?.total_activities || 0, icon: BarChart3, color: "bg-green-100 text-green-700" },
    { label: "فعالیت‌های معوق", value: stats?.total_overdue || 0, icon: Clock, color: "bg-red-100 text-red-700" },
  ];

  return (
    <div className="space-y-6 p-6 max-w-6xl mx-auto">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold">گزارش‌های سیستم</h1>
        <Button variant="outline" className="gap-2">
          <TrendingUp className="h-4 w-4" />
          دانلود گزارش کامل
        </Button>
      </div>

      {/* کارت‌های آماری */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        {statItems.map((item, idx) => (
          <Card key={idx} className="hover:shadow-md transition-shadow">
            <CardContent className="p-6">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-muted-foreground">{item.label}</p>
                  <p className="text-2xl font-bold mt-1">{item.value}</p>
                </div>
                <div className={`p-3 rounded-full ${item.color}`}>
                  <item.icon className="h-5 w-5" />
                </div>
              </div>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* سایر گزارشات (می‌توان بعداً توسعه داد) */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <Card>
          <CardHeader>
            <CardTitle className="text-base">گزارش عملکرد کاربران</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-sm text-muted-foreground">به زودی...</p>
          </CardContent>
        </Card>
        <Card>
          <CardHeader>
            <CardTitle className="text-base">گزارش فعالیت‌ها بر اساس حوزه</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-sm text-muted-foreground">به زودی...</p>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}