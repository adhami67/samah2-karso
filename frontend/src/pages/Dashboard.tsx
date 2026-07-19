import { useEffect, useState } from "react";
import { useAuth } from "@/contexts/AuthContext";
import { api } from "@/lib/api";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { useNavigate } from "react-router-dom";
import {
  LayoutDashboard,
  Clock,
  Send,
  AlertTriangle,
  CheckCircle,
  BarChart3,
  Star,
  LogOut,
  User,
  FileText,
  Zap,
} from "lucide-react";

interface MySummary {
  total_activities: number;
  completed: number;
  in_progress: number;
  submitted: number;
  needs_revision: number;
  approved: number;
  average_score: number | null;
}

export default function Dashboard() {
  const { user, logout } = useAuth();
  const [summary, setSummary] = useState<MySummary | null>(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    api.get<MySummary>("/reports/my-summary")
      .then(setSummary)
      .catch(console.error)
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <div className="p-8 text-center text-slate-500">در حال بارگیری داشبورد...</div>;

  return (
    <div className="min-h-screen bg-gradient-to-br from-orange-50 via-white to-amber-50">
      {/* هدر */}
      <header className="bg-white/80 backdrop-blur shadow-sm border-b border-orange-100 py-4 px-6 flex justify-between items-center">
        <div className="flex items-center gap-2">
          <div className="bg-orange-500 p-2 rounded-lg">
            <LayoutDashboard className="text-white" size={28} />
          </div>
          <h1 className="text-2xl font-bold text-slate-800">کارسو</h1>
          <span className="text-sm text-slate-400 hidden sm:inline">| داشبورد</span>
        </div>
        <div className="flex items-center gap-4">
          <div className="flex items-center gap-2 text-slate-600 text-sm bg-slate-50 py-1 px-3 rounded-full">
            <User size={18} className="text-orange-500" />
            <span>{user?.full_name}</span>
          </div>
          <Button variant="outline" size="sm" onClick={logout} className="flex items-center gap-1">
            <LogOut size={16} /> خروج
          </Button>
        </div>
      </header>

      {/* محتوای اصلی */}
      <main className="p-6 max-w-4xl mx-auto">
        <div className="flex items-center gap-2 mb-6">
          <Zap className="text-orange-500" size={24} />
          <h2 className="text-xl font-semibold text-slate-700">گزارش عملکرد من</h2>
        </div>

        {/* کارت‌های آمار */}
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-8">
          <StatCard
            title="کل فعالیت‌ها"
            value={summary?.total_activities ?? 0}
            icon={<FileText />}
            bgColor="bg-blue-50"
            iconColor="text-blue-500"
          />
          <StatCard
            title="در حال انجام"
            value={summary?.in_progress ?? 0}
            icon={<Clock />}
            bgColor="bg-amber-50"
            iconColor="text-amber-500"
          />
          <StatCard
            title="ارسال‌شده"
            value={summary?.submitted ?? 0}
            icon={<Send />}
            bgColor="bg-indigo-50"
            iconColor="text-indigo-500"
          />
          <StatCard
            title="نیاز به اصلاح"
            value={summary?.needs_revision ?? 0}
            icon={<AlertTriangle />}
            bgColor="bg-rose-50"
            iconColor="text-rose-500"
          />
          <StatCard
            title="تأییدشده"
            value={summary?.approved ?? 0}
            icon={<CheckCircle />}
            bgColor="bg-emerald-50"
            iconColor="text-emerald-500"
          />
          <StatCard
            title="تکمیل‌شده"
            value={summary?.completed ?? 0}
            icon={<BarChart3 />}
            bgColor="bg-green-50"
            iconColor="text-green-500"
          />
          <StatCard
            title="میانگین امتیاز"
            value={summary?.average_score ? `${summary.average_score}%` : "—"}
            icon={<Star />}
            bgColor="bg-purple-50"
            iconColor="text-purple-500"
          />
        </div>

        <div className="flex justify-center">
          <Button onClick={() => navigate("/activities")} className="px-6 py-2 text-base">
            مشاهدهٔ فعالیت‌ها
          </Button>
        </div>
      </main>
    </div>
  );
}

function StatCard({
  title,
  value,
  icon,
  bgColor,
  iconColor,
}: {
  title: string;
  value: string | number;
  icon: React.ReactNode;
  bgColor: string;
  iconColor: string;
}) {
  return (
    <Card className={`${bgColor} border-none shadow-sm hover:shadow-md transition-shadow`}>
      <CardContent className="p-4 flex items-center gap-4">
        <div className={`p-3 rounded-full bg-white ${iconColor}`}>{icon}</div>
        <div>
          <p className="text-sm font-medium text-slate-500">{title}</p>
          <p className="text-2xl font-bold text-slate-800">{value}</p>
        </div>
      </CardContent>
    </Card>
  );
}