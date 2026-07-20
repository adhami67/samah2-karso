import { useEffect, useState, useCallback } from "react";
import { useAuth } from "@/contexts/AuthContext";
import { api } from "@/lib/api";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { useNavigate, useLocation } from "react-router-dom";
import {
  Clock,
  AlertTriangle,
  CheckCircle,
  Calendar,
} from "lucide-react";
import { motion } from "framer-motion";

// اینترفیس‌ها بدون تغییر
interface Activity {
  id: string;
  title: string;
  status: string;
  due_at: string | null;
  activity_type: string;
}
interface MySummary {
  total_activities: number;
  in_progress: number;
  submitted: number;
  needs_revision: number;
  approved: number;
  completed: number;
  average_score: number | null;
}

const containerVariants = {
  hidden: { opacity: 0 },
  visible: {
    opacity: 1,
    transition: { staggerChildren: 0.1 },
  },
};
const itemVariants = {
  hidden: { opacity: 0, y: 20 },
  visible: { opacity: 1, y: 0 },
};

export default function Dashboard() {
  const { user, logout } = useAuth();
  const [todayActivities, setTodayActivities] = useState<Activity[]>([]);
  const [overdueActivities, setOverdueActivities] = useState<Activity[]>([]);
  const [summary, setSummary] = useState<MySummary | null>(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const location = useLocation();

  const fetchData = useCallback(async () => {
    setLoading(true);
    const cacheBuster = `?_ts=${Date.now()}`;
    try {
      const [today, overdue, sum] = await Promise.all([
        api.get<Activity[]>(`/activities/my/today${cacheBuster}`).catch(() => []),
        api.get<Activity[]>(`/activities/my/overdue${cacheBuster}`).catch(() => []),
        api.get<MySummary>(`/reports/my-summary${cacheBuster}`).catch(() => null),
      ]);
      setTodayActivities(Array.isArray(today) ? today : []);
      setOverdueActivities(Array.isArray(overdue) ? overdue : []);
      setSummary(sum);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, []);

  // راز اصلی: رفرش خودکار هنگام بازگشت به صفحه
  useEffect(() => {
    fetchData();
  }, [fetchData, location.key]);

  if (loading) return <div className="p-8 text-center">در حال بارگیری...</div>;

  return (
    <div className="space-y-6">
      {/* امروز من */}
      <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }}>
        <h2 className="flex items-center gap-2 text-lg font-semibold text-slate-700 mb-3">
          <Calendar className="text-orange-500" size={22} />
          امروز من
        </h2>
        {todayActivities.length === 0 ? (
          <Card className="glass-card">
            <CardContent className="p-4 text-center text-slate-400">
              برای امروز مسئولیتی نداری — آفرین!
            </CardContent>
          </Card>
        ) : (
          <div className="space-y-2">
            {todayActivities.map((act) => (
              <Card
                key={act.id}
                className="glass-card hover:shadow-md cursor-pointer"
                onClick={() => navigate(`/activities/${act.id}`)}
              >
                <CardContent className="p-3 flex items-center justify-between">
                  <span className="font-medium">{act.title}</span>
                  <span className="text-xs text-orange-600 bg-orange-50 px-2 py-1 rounded-full">
                    {act.status === "in_progress" ? "در حال انجام" : act.status}
                  </span>
                </CardContent>
              </Card>
            ))}
          </div>
        )}
      </motion.div>

      {/* معوقه‌ها */}
      {overdueActivities.length > 0 && (
        <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }}>
          <h2 className="flex items-center gap-2 text-lg font-semibold text-rose-600 mb-3">
            <AlertTriangle size={22} />
            عقب‌افتاده
          </h2>
          <div className="space-y-2">
            {overdueActivities.map((act) => (
              <Card
                key={act.id}
                className="border-l-4 border-rose-500 glass-card hover:shadow-md cursor-pointer"
                onClick={() => navigate(`/activities/${act.id}`)}
              >
                <CardContent className="p-3 flex items-center justify-between">
                  <div>
                    <span className="font-medium">{act.title}</span>
                    {act.due_at && (
                      <div className="text-xs text-slate-400 mt-1">
                        مهلت: {new Date(act.due_at).toLocaleDateString("fa-IR")}
                      </div>
                    )}
                  </div>
                  <span className="text-xs text-rose-600 bg-rose-50 px-2 py-1 rounded-full">
                    عقب‌افتاده
                  </span>
                </CardContent>
              </Card>
            ))}
          </div>
        </motion.div>
      )}

      {/* کارت‌های آماری */}
      {summary && (
        <motion.div
          variants={containerVariants}
          initial="hidden"
          animate="visible"
          className="grid grid-cols-2 md:grid-cols-4 gap-4"
        >
          <motion.div variants={itemVariants}>
            <StatCard title="در حال انجام" value={summary.in_progress} icon={<Clock />} color="bg-amber-100 text-amber-700" />
          </motion.div>
          <motion.div variants={itemVariants}>
            <StatCard title="ارسال‌شده" value={summary.submitted} icon={<CheckCircle />} color="bg-indigo-100 text-indigo-700" />
          </motion.div>
          <motion.div variants={itemVariants}>
            <StatCard title="نیاز به اصلاح" value={summary.needs_revision} icon={<AlertTriangle />} color="bg-rose-100 text-rose-700" />
          </motion.div>
          <motion.div variants={itemVariants}>
            <StatCard title="تکمیل‌شده" value={summary.completed} icon={<CheckCircle />} color="bg-green-100 text-green-700" />
          </motion.div>
        </motion.div>
      )}

      <div className="flex justify-center">
        <Button onClick={() => navigate("/activities")} className="px-6">
          همهٔ مسئولیت‌ها
        </Button>
      </div>
    </div>
  );
}

function StatCard({ title, value, icon, color }: { title: string; value: number; icon: React.ReactNode; color: string }) {
  return (
    <Card className="glass-card">
      <CardContent className="p-4 flex items-center gap-3">
        <div className={`p-2 rounded-full ${color}`}>{icon}</div>
        <div>
          <p className="text-sm text-slate-500">{title}</p>
          <p className="text-xl font-bold">{value}</p>
        </div>
      </CardContent>
    </Card>
  );
}