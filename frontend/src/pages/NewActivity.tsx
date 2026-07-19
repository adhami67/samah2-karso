import { useState, useEffect, FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { ArrowLeft, PlusCircle } from "lucide-react";

interface Workspace {
  id: string;
  name: string;
}

export default function NewActivity() {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [activityType, setActivityType] = useState("task");
  const [dueAt, setDueAt] = useState("");
  const [workspaceId, setWorkspaceId] = useState("");
  const [workspaces, setWorkspaces] = useState<Workspace[]>([]);
  const [loadingWorkspaces, setLoadingWorkspaces] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  // دریافت خودکار حوزه‌های کاربر
  useEffect(() => {
    const fetchWorkspaces = async () => {
      try {
        const data = await api.get<Workspace[]>("/workspaces/");
        setWorkspaces(data);
        if (data.length > 0) {
          setWorkspaceId(data[0].id); // انتخاب اولین حوزه به‌صورت پیش‌فرض
        }
      } catch (err) {
        console.error("خطا در بارگذاری حوزه‌ها", err);
      } finally {
        setLoadingWorkspaces(false);
      }
    };
    fetchWorkspaces();
  }, []);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    if (!workspaceId) {
      setError("لطفاً یک حوزه را انتخاب کنید.");
      return;
    }
    setLoading(true);
    setError("");
    try {
      await api.post("/activities/", {
        workspace_id: workspaceId,
        title,
        description,
        activity_type: activityType,
        due_at: dueAt ? new Date(dueAt).toISOString() : null,
      });
      navigate("/activities");
    } catch (err: any) {
      setError(err.message || "خطا در ایجاد مسئولیت");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-orange-50 via-white to-amber-50">
      <header className="bg-white/80 backdrop-blur shadow-sm border-b border-orange-100 py-4 px-6 flex items-center gap-4">
        <Button variant="ghost" onClick={() => navigate("/activities")}>
          <ArrowLeft size={20} />
        </Button>
        <h1 className="text-xl font-bold text-slate-800">ایجاد مسئولیت جدید</h1>
      </header>

      <main className="p-6 max-w-2xl mx-auto">
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <PlusCircle className="text-orange-500" /> جزئیات مسئولیت
            </CardTitle>
          </CardHeader>
          <CardContent>
            <form onSubmit={handleSubmit} className="space-y-4">
              {/* انتخاب حوزه */}
              <div>
                <label className="block text-sm font-medium mb-1">حوزه</label>
                {loadingWorkspaces ? (
                  <p className="text-sm text-slate-400">در حال بارگیری حوزه‌ها...</p>
                ) : workspaces.length === 0 ? (
                  <p className="text-sm text-red-500">هیچ حوزه‌ای در دسترس نیست. ابتدا یک حوزه بسازید.</p>
                ) : (
                  <select
                    className="w-full border rounded-lg px-3 py-2 bg-white"
                    value={workspaceId}
                    onChange={(e) => setWorkspaceId(e.target.value)}
                    required
                  >
                    {workspaces.map((ws) => (
                      <option key={ws.id} value={ws.id}>{ws.name}</option>
                    ))}
                  </select>
                )}
              </div>

              <div>
                <label className="block text-sm font-medium mb-1">عنوان</label>
                <Input
                  value={title}
                  onChange={(e) => setTitle(e.target.value)}
                  required
                />
              </div>
              <div>
                <label className="block text-sm font-medium mb-1">توضیحات</label>
                <textarea
                  className="w-full border rounded-lg p-2 min-h-[100px]"
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                />
              </div>
              <div className="flex gap-4">
                <div className="flex-1">
                  <label className="block text-sm font-medium mb-1">نوع مسئولیت</label>
                  <select
                    className="w-full border rounded-lg px-3 py-2 bg-white"
                    value={activityType}
                    onChange={(e) => setActivityType(e.target.value)}
                  >
                    <option value="task">تکلیف</option>
                    <option value="project">پروژه</option>
                    <option value="session">جلسه</option>
                    <option value="exam">مطالعه</option>
                    <option value="custom">سایر</option>
                  </select>
                </div>
                <div className="flex-1">
                  <label className="block text-sm font-medium mb-1">مهلت</label>
                  <Input
                    type="datetime-local"
                    value={dueAt}
                    onChange={(e) => setDueAt(e.target.value)}
                  />
                </div>
              </div>
              {error && <p className="text-red-500 text-sm">{error}</p>}
              <Button type="submit" className="w-full" disabled={loading || loadingWorkspaces}>
                {loading ? "در حال ایجاد..." : "ایجاد مسئولیت"}
              </Button>
            </form>
          </CardContent>
        </Card>
      </main>
    </div>
  );
}