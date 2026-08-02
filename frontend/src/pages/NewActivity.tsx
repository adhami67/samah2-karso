// frontend/src/pages/NewActivity.tsx
import { useState, useEffect, FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { ArrowLeft, PlusCircle } from "lucide-react";
import { toast } from "sonner";

interface Workspace {
  id: string;
  name: string;
}

export default function NewActivity() {
  const [subject, setSubject] = useState("");
  const [description, setDescription] = useState("");
  const [activityType, setActivityType] = useState("task");
  const [dueAt, setDueAt] = useState("");
  const [workspaceId, setWorkspaceId] = useState("");
  const [workspaces, setWorkspaces] = useState<Workspace[]>([]);
  const [loadingWorkspaces, setLoadingWorkspaces] = useState(true);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  // دریافت خودکار حوزه‌ها
  useEffect(() => {
    const fetchWorkspaces = async () => {
      try {
        const data = await api.get<Workspace[]>("/workspaces");
        setWorkspaces(data);
        if (data.length > 0) {
          setWorkspaceId(data[0].id);
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
      toast.error("لطفاً یک حوزه را انتخاب کنید.");
      return;
    }
    setLoading(true);
    try {
      await api.post("/activities", {
        work_group_id: workspaceId,
        work_type_id: "type1",
        work_priority_id: "prio1",
        subject: subject,
        description: description,
        activity_type: activityType,
        due_at: dueAt ? new Date(dueAt).toISOString() : null,
      });
      toast.success("مسئولیت با موفقیت ایجاد شد");
      navigate("/activities");
    } catch (err: any) {
      toast.error(err.message || "خطا در ایجاد مسئولیت");
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
                  <p className="text-sm text-red-500">
                    هیچ حوزه‌ای در دسترس نیست. ابتدا یک حوزه بسازید.
                  </p>
                ) : (
                  <Select
                    value={workspaceId}
                    onValueChange={(value) => setWorkspaceId(value)}
                  >
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder="انتخاب حوزه" />
                    </SelectTrigger>
                    <SelectContent>
                      {workspaces.map((ws) => (
                        <SelectItem key={ws.id} value={ws.id}>
                          {ws.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                )}
              </div>

              {/* عنوان */}
              <div>
                <label className="block text-sm font-medium mb-1">عنوان</label>
                <Input
                  value={subject}
                  onChange={(e) => setSubject(e.target.value)}
                  required
                />
              </div>

              {/* توضیحات */}
              <div>
                <label className="block text-sm font-medium mb-1">توضیحات</label>
                <textarea
                  className="w-full border rounded-lg p-2 min-h-[100px]"
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                />
              </div>

              {/* نوع و مهلت */}
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

              {/* دکمهٔ ارسال */}
              <Button
                type="submit"
                className="w-full"
                disabled={loading || loadingWorkspaces}
              >
                {loading ? "در حال ایجاد..." : "ایجاد مسئولیت"}
              </Button>
            </form>
          </CardContent>
        </Card>
      </main>
    </div>
  );
}