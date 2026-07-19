import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useNavigate } from "react-router-dom";
import {
  Search,
  Filter,
  Calendar,
  Clock,
  CheckCircle,
  AlertTriangle,
  Send,
  FileText,
} from "lucide-react";

interface Activity {
  id: string;
  title: string;
  description: string;
  status: string;
  activity_type: string;
  due_at: string | null;
  workspace_id: string;
  created_by_user_id: string;
}

const statusMap: Record<string, { label: string; color: string; icon: JSX.Element }> = {
  draft: { label: "پیش‌نویس", color: "bg-gray-100 text-gray-700", icon: <FileText size={16} /> },
  published: { label: "منتشرشده", color: "bg-blue-100 text-blue-700", icon: <Send size={16} /> },
  in_progress: { label: "در حال انجام", color: "bg-amber-100 text-amber-700", icon: <Clock size={16} /> },
  submitted: { label: "ارسال‌شده", color: "bg-indigo-100 text-indigo-700", icon: <Send size={16} /> },
  needs_revision: { label: "نیاز به اصلاح", color: "bg-rose-100 text-rose-700", icon: <AlertTriangle size={16} /> },
  approved: { label: "تأییدشده", color: "bg-emerald-100 text-emerald-700", icon: <CheckCircle size={16} /> },
  completed: { label: "تکمیل‌شده", color: "bg-green-100 text-green-700", icon: <CheckCircle size={16} /> },
  archived: { label: "بایگانی", color: "bg-slate-100 text-slate-500", icon: <FileText size={16} /> },
};

export default function Activities() {
  const [activities, setActivities] = useState<Activity[]>([]);
  const [search, setSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState("");
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  const fetchActivities = async () => {
    setLoading(true);
    try {
      const params = new URLSearchParams();
      if (search) params.append("search", search);
      if (statusFilter) params.append("status", statusFilter);
      const data = await api.get<Activity[]>(`/activities/?${params.toString()}`);
      setActivities(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchActivities();
  }, [statusFilter]); // می‌توانی جستجو را با دکمه یا debounce مدیریت کنی

  const handleSearch = () => {
    fetchActivities();
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-orange-50 via-white to-amber-50">
      <main className="p-6 max-w-4xl mx-auto">
        {/* نوار جستجو و فیلتر */}
        <div className="flex flex-col sm:flex-row gap-3 mb-6">
          <div className="relative flex-1">
            <Search className="absolute right-3 top-2.5 text-slate-400" size={20} />
            <Input
              placeholder="جستجو در عنوان..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="pr-10"
            />
          </div>
          <select
            className="border rounded-lg px-3 py-2 bg-white"
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
          >
            <option value="">همهٔ وضعیت‌ها</option>
            <option value="draft">پیش‌نویس</option>
            <option value="published">منتشرشده</option>
            <option value="in_progress">در حال انجام</option>
            <option value="submitted">ارسال‌شده</option>
            <option value="needs_revision">نیاز به اصلاح</option>
            <option value="approved">تأییدشده</option>
            <option value="completed">تکمیل‌شده</option>
            <option value="archived">بایگانی</option>
          </select>
          <Button onClick={handleSearch} className="flex items-center gap-1">
            <Filter size={18} /> اعمال
          </Button>
          <Button onClick={() => navigate("/activities/new")} className="flex items-center gap-1">
            + فعالیت جدید
          </Button>
        </div>

        {/* لیست فعالیت‌ها */}
        {loading ? (
          <p className="text-center text-slate-500">در حال بارگیری...</p>
        ) : activities.length === 0 ? (
          <Card>
            <CardContent className="p-6 text-center text-slate-400">
              هیچ فعالیتی یافت نشد.
            </CardContent>
          </Card>
        ) : (
          <div className="space-y-3">
            {activities.map((act) => {
              const st = statusMap[act.status] || { label: act.status, color: "bg-gray-100", icon: null };
              return (
                <Card
                  key={act.id}
                  className="hover:shadow-md transition-shadow cursor-pointer"
                  onClick={() => navigate(`/activities/${act.id}`)}
                >
                  <CardContent className="p-4 flex items-center justify-between">
                    <div className="flex items-center gap-4">
                      <div className={`p-2 rounded-full ${st.color}`}>
                        {st.icon}
                      </div>
                      <div>
                        <h3 className="font-semibold text-slate-800">{act.title}</h3>
                        {act.due_at && (
                          <div className="flex items-center gap-1 text-xs text-slate-500 mt-1">
                            <Calendar size={14} />
                            {new Date(act.due_at).toLocaleDateString("fa-IR")}
                          </div>
                        )}
                      </div>
                    </div>
                    <span className={`text-xs px-2 py-1 rounded-full ${st.color}`}>
                      {st.label}
                    </span>
                  </CardContent>
                </Card>
              );
            })}
          </div>
        )}
      </main>
    </div>
  );
}