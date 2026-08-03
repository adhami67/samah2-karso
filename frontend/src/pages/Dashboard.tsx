// frontend/src/pages/Dashboard.tsx
import { useEffect, useState, useCallback } from "react";
import { useAuth } from "@/contexts/AuthContext";
import { api } from "@/lib/api";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { useNavigate, useLocation } from "react-router-dom";
import { Clock, AlertTriangle, CheckCircle, Calendar, Users, BookOpen } from "lucide-react";
import { motion } from "framer-motion";
import { PriorityList } from "@/components/PriorityList";
import { toast } from "sonner";

// اینترفیس‌ها
interface Activity {
  id: string;
  subject: string;
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

// اینترفیس موقت برای کلاس‌ها (تا زمانی که API کامل شود)
interface ClassSchedule {
  id: string;
  name: string;
  teacher: string;
  time: string;
  day: string;
  grade: string;
}

const containerVariants = {
  hidden: { opacity: 0 },
  visible: { opacity: 1, transition: { staggerChildren: 0.1 } },
};

const itemVariants = {
  hidden: { opacity: 0, y: 20 },
  visible: { opacity: 1, y: 0 },
};

export default function Dashboard() {
  const { user } = useAuth();
  const [todayActivities, setTodayActivities] = useState<Activity[]>([]);
  const [overdueActivities, setOverdueActivities] = useState<Activity[]>([]);
  const [summary, setSummary] = useState<MySummary | null>(null);
  const [classesToday, setClassesToday] = useState<ClassSchedule[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const location = useLocation();

  // دریافت همزمان اطلاعات
  const fetchData = useCallback(async () => {
    setLoading(true);
    try {
      const [today, overdue, sum, classes] = await Promise.all([
        api.get<Activity[]>("/activities/my/today").catch(() => []),
        api.get<Activity[]>("/activities/my/overdue").catch(() => []),
        api.get<MySummary>("/reports/my/stats").catch(() => ({
          total_activities: 0,
          in_progress: 0,
          submitted: 0,
          needs_revision: 0,
          approved: 0,
          completed: 0,
          average_score: null,
        })),
        // داده‌های موقت برای کلاس‌ها - بعداً با API واقعی جایگزین می‌شود
        Promise.resolve([
          { id: "1", name: "ریاضی", teacher: "خانم مرادی", time: "08:00 - 09:30", day: "امروز", grade: "هفتم" },
          { id: "2", name: "فیزیک", teacher: "آقای رنجبر", time: "10:00 - 11:30", day: "امروز", grade: "دهم" },
          { id: "3", name: "شیمی", teacher: "خانم علیپور", time: "13:00 - 14:30", day: "امروز", grade: "یازدهم" },
        ]).catch(() => []),
      ]);
      setTodayActivities(Array.isArray(today) ? today : []);
      setOverdueActivities(Array.isArray(overdue) ? overdue : []);
      setSummary(sum as MySummary);
      setClassesToday(Array.isArray(classes) ? classes : []);
    } catch (err) {
      console.error(err);
      toast.error("خطا در دریافت اطلاعات داشبورد");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchData();
  }, [fetchData, location.key]);

  if (loading) {
    return (
      <div className="space-y-6">
        <div className="grid grid-cols-2 md:grid-cols-4 gap-3 animate-pulse">
          {Array.from({ length: 4 }).map((_, i) => (
            <div key={i} className="h-20 bg-muted/30 rounded-lg" />
          ))}
        </div>
        <div className="h-40 bg-muted/30 rounded-lg animate-pulse" />
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div className="h-64 bg-muted/30 rounded-lg animate-pulse" />
          <div className="h-64 bg-muted/30 rounded-lg animate-pulse" />
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6 max-w-7xl mx-auto">
      {/* ===== بخش اولویت‌های امروز ===== */}
      <motion.div initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }}>
        <div className="flex items-center justify-between mb-2">
          <h2 className="flex items-center gap-2 text-lg font-semibold text-slate-700">
            <AlertTriangle className="text-orange-500" size={20} />
            اولویت‌های امروز
          </h2>
          <span className="text-xs text-muted-foreground">
            {new Date().toLocaleDateString("fa-IR", { weekday: "long", day: "numeric", month: "long" })}
          </span>
        </div>
        <PriorityList />
      </motion.div>

      {/* ===== دو ستون: برنامه هفتگی + خلاصه وضعیت ===== */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* برنامه هفتگی (۲/۳ عرض) */}
        <motion.div
          initial={{ opacity: 0, x: -20 }}
          animate={{ opacity: 1, x: 0 }}
          className="lg:col-span-2"
        >
          <Card className="glass-card h-full">
            <CardContent className="p-4">
              <div className="flex items-center justify-between mb-3">
                <h3 className="flex items-center gap-2 text-base font-semibold text-slate-700">
                  <Calendar className="text-blue-500" size={18} />
                  برنامه امروز
                </h3>
                <Button variant="ghost" size="sm" onClick={() => navigate("/calendar")}>
                  مشاهده همه
                </Button>
              </div>
              {classesToday.length === 0 ? (
                <div className="text-center text-muted-foreground py-6 text-sm">
                  <BookOpen className="h-8 w-8 mx-auto mb-2 opacity-20" />
                  امروز کلاسی ندارید
                </div>
              ) : (
                <div className="space-y-2">
                  {classesToday.map((cls) => (
                    <div
                      key={cls.id}
                      className="flex items-center justify-between p-3 rounded-lg bg-muted/30 hover:bg-muted/50 transition-colors cursor-pointer"
                      onClick={() => navigate(`/classes/${cls.id}`)}
                    >
                      <div className="flex items-center gap-3">
                        <div className="bg-blue-100 p-2 rounded-lg">
                          <BookOpen className="h-4 w-4 text-blue-600" />
                        </div>
                        <div>
                          <p className="font-medium text-sm">{cls.name}</p>
                          <p className="text-xs text-muted-foreground">
                            {cls.grade} • {cls.teacher}
                          </p>
                        </div>
                      </div>
                      <div className="text-sm font-medium text-blue-600">
                        {cls.time}
                      </div>
                    </div>
                  ))}
                </div>
              )}
            </CardContent>
          </Card>
        </motion.div>

        {/* خلاصه وضعیت (۱/۳ عرض) */}
        <motion.div
          initial={{ opacity: 0, x: 20 }}
          animate={{ opacity: 1, x: 0 }}
        >
          <Card className="glass-card h-full">
            <CardContent className="p-4">
              <h3 className="flex items-center gap-2 text-base font-semibold text-slate-700 mb-3">
                <Users className="text-purple-500" size={18} />
                خلاصه وضعیت
              </h3>
              <div className="space-y-3">
                <div className="flex justify-between items-center p-2 rounded-lg bg-amber-50">
                  <span className="text-sm text-amber-700">در حال انجام</span>
                  <span className="font-bold text-amber-700">{summary?.in_progress || 0}</span>
                </div>
                <div className="flex justify-between items-center p-2 rounded-lg bg-indigo-50">
                  <span className="text-sm text-indigo-700">ارسال‌شده</span>
                  <span className="font-bold text-indigo-700">{summary?.submitted || 0}</span>
                </div>
                <div className="flex justify-between items-center p-2 rounded-lg bg-rose-50">
                  <span className="text-sm text-rose-700">نیاز به اصلاح</span>
                  <span className="font-bold text-rose-700">{summary?.needs_revision || 0}</span>
                </div>
                <div className="flex justify-between items-center p-2 rounded-lg bg-green-50">
                  <span className="text-sm text-green-700">تکمیل‌شده</span>
                  <span className="font-bold text-green-700">{summary?.completed || 0}</span>
                </div>
                {summary?.average_score !== null && summary?.average_score !== undefined && (
                  <div className="flex justify-between items-center p-2 rounded-lg bg-slate-50">
                    <span className="text-sm text-slate-700">میانگین امتیاز</span>
                    <span className="font-bold text-slate-700">{summary.average_score.toFixed(1)}</span>
                  </div>
                )}
              </div>
            </CardContent>
          </Card>
        </motion.div>
      </div>

      {/* ===== کارهای امروز ===== */}
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.2 }}
      >
        <div className="flex items-center justify-between mb-3">
          <h2 className="flex items-center gap-2 text-lg font-semibold text-slate-700">
            <Clock className="text-orange-500" size={20} />
            کارهای امروز
          </h2>
          <Button variant="ghost" size="sm" onClick={() => navigate("/activities")}>
            مشاهده همه
          </Button>
        </div>
        {todayActivities.length === 0 ? (
          <Card className="glass-card">
            <CardContent className="p-6 text-center text-muted-foreground">
              <CheckCircle className="h-8 w-8 mx-auto mb-2 text-green-500" />
              برای امروز مسئولیتی نداری — آفرین! 🎉
            </CardContent>
          </Card>
        ) : (
          <div className="space-y-2">
            {todayActivities.map((act) => (
              <Card
                key={act.id}
                className="glass-card hover:shadow-md cursor-pointer transition-all"
                onClick={() => navigate(`/activities/${act.id}`)}
              >
                <CardContent className="p-3 flex items-center justify-between">
                  <span className="font-medium">{act.subject}</span>
                  <span className="text-xs px-2 py-1 rounded-full bg-orange-100 text-orange-700">
                    {act.status === "in_progress" ? "در حال انجام" : act.status}
                  </span>
                </CardContent>
              </Card>
            ))}
          </div>
        )}
      </motion.div>

      {/* کارهای معوق */}
      {overdueActivities.length > 0 && (
        <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }}>
          <h2 className="flex items-center gap-2 text-lg font-semibold text-rose-600 mb-3">
            <AlertTriangle size={20} />
            عقب‌افتاده
          </h2>
          <div className="space-y-2">
            {overdueActivities.map((act) => (
              <Card
                key={act.id}
                className="border-r-4 border-rose-500 glass-card hover:shadow-md cursor-pointer"
                onClick={() => navigate(`/activities/${act.id}`)}
              >
                <CardContent className="p-3 flex items-center justify-between">
                  <div>
                    <span className="font-medium">{act.subject}</span>
                    {act.due_at && (
                      <div className="text-xs text-muted-foreground mt-1">
                        مهلت: {new Date(act.due_at).toLocaleDateString("fa-IR")}
                      </div>
                    )}
                  </div>
                  <span className="text-xs px-2 py-1 rounded-full bg-rose-100 text-rose-700">
                    عقب‌افتاده
                  </span>
                </CardContent>
              </Card>
            ))}
          </div>
        </motion.div>
      )}

      {/* دکمه همه مسئولیت‌ها */}
      <div className="flex justify-center pt-4">
        <Button onClick={() => navigate("/activities")} className="px-8 gap-2">
          <AlertTriangle className="h-4 w-4" />
          همهٔ مسئولیت‌ها
        </Button>
      </div>
    </div>
  );
}