import { useEffect, useState } from "react";
import { useAuth } from "@/contexts/AuthContext";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { useNavigate } from "react-router-dom";

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

  if (loading) return <div className="p-8 text-center">در حال بارگیری...</div>;

  return (
    <div className="min-h-screen bg-slate-50">
      <header className="bg-white shadow-sm py-4 px-6 flex justify-between items-center">
        <h1 className="text-xl font-bold">کارسو | داشبورد</h1>
        <div className="flex items-center gap-4">
          <span className="text-sm text-slate-600">
            {user?.full_name} (کد ملی: {user?.national_id})
          </span>
          <Button variant="outline" onClick={logout}>خروج</Button>
        </div>
      </header>

      <main className="p-6 max-w-4xl mx-auto">
        <h2 className="text-2xl font-semibold mb-6">گزارش عملکرد من</h2>
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-8">
          <StatCard title="کل فعالیت‌ها" value={summary?.total_activities ?? 0} />
          <StatCard title="در حال انجام" value={summary?.in_progress ?? 0} />
          <StatCard title="ارسال‌شده" value={summary?.submitted ?? 0} />
          <StatCard title="نیاز به اصلاح" value={summary?.needs_revision ?? 0} />
          <StatCard title="تأییدشده" value={summary?.approved ?? 0} />
          <StatCard title="تکمیل‌شده" value={summary?.completed ?? 0} />
          <StatCard
            title="میانگین امتیاز"
            value={summary?.average_score ? `${summary.average_score}%` : "—"}
          />
        </div>

        <Button onClick={() => navigate("/activities")} className="mt-4">
          مشاهدهٔ فعالیت‌ها
        </Button>
      </main>
    </div>
  );
}

function StatCard({ title, value }: { title: string; value: string | number }) {
  return (
    <Card>
      <CardHeader className="pb-2">
        <CardTitle className="text-sm font-medium text-slate-500">{title}</CardTitle>
      </CardHeader>
      <CardContent>
        <p className="text-2xl font-bold">{value}</p>
      </CardContent>
    </Card>
  );
}