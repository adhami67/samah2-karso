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
import { motion } from "framer-motion";

interface MySummary {
  total_activities: number;
  completed: number;
  in_progress: number;
  submitted: number;
  needs_revision: number;
  approved: number;
  average_score: number | null;
}

const containerVariants = {
  hidden: { opacity: 0 },
  visible: {
    opacity: 1,
    transition: {
      staggerChildren: 0.1,
    },
  },
};

const itemVariants = {
  hidden: { opacity: 0, y: 20 },
  visible: { opacity: 1, y: 0 },
};

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

      {/* محتوای اصلی */}
      <main className="p-6 max-w-4xl mx-auto">
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          className="flex items-center gap-2 mb-6"
        >
          <Zap className="text-orange-500" size={24} />
          <h2 className="text-xl font-semibold text-slate-700">گزارش عملکرد من</h2>
        </motion.div>

        {/* کارت‌های آمار */}
        <motion.div
          variants={containerVariants}
          initial="hidden"
          animate="visible"
          className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-8"
        >
          <motion.div variants={itemVariants}>
            <StatCard title="کل فعالیت‌ها" value={summary?.total_activities ?? 0} icon={<FileText />} bgColor="bg-blue-50" iconColor="text-blue-500" />
          </motion.div>
          <motion.div variants={itemVariants}>
            <StatCard title="در حال انجام" value={summary?.in_progress ?? 0} icon={<Clock />} bgColor="bg-amber-50" iconColor="text-amber-500" />
          </motion.div>
          <motion.div variants={itemVariants}>
            <StatCard title="ارسال‌شده" value={summary?.submitted ?? 0} icon={<Send />} bgColor="bg-indigo-50" iconColor="text-indigo-500" />
          </motion.div>
          <motion.div variants={itemVariants}>
            <StatCard title="نیاز به اصلاح" value={summary?.needs_revision ?? 0} icon={<AlertTriangle />} bgColor="bg-rose-50" iconColor="text-rose-500" />
          </motion.div>
          <motion.div variants={itemVariants}>
            <StatCard title="تأییدشده" value={summary?.approved ?? 0} icon={<CheckCircle />} bgColor="bg-emerald-50" iconColor="text-emerald-500" />
          </motion.div>
          <motion.div variants={itemVariants}>
            <StatCard title="تکمیل‌شده" value={summary?.completed ?? 0} icon={<BarChart3 />} bgColor="bg-green-50" iconColor="text-green-500" />
          </motion.div>
          <motion.div variants={itemVariants}>
            <StatCard title="میانگین امتیاز" value={summary?.average_score ? `${summary.average_score}%` : "—"} icon={<Star />} bgColor="bg-purple-50" iconColor="text-purple-500" />
          </motion.div>
        </motion.div>

        <motion.div initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.8 }} className="flex justify-center">
          <Button onClick={() => navigate("/activities")} className="px-6 py-2 text-base">
            مشاهدهٔ فعالیت‌ها
          </Button>
        </motion.div>
      </main>
    </div>
  );
}

function StatCard({ title, value, icon, bgColor, iconColor }: { title: string; value: string | number; icon: React.ReactNode; bgColor: string; iconColor: string }) {
  return (
    <Card className={`${bgColor} border-none shadow-sm hover:shadow-md transition-shadow hover:scale-105 duration-200`}>
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