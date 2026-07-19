import { useState, FormEvent } from "react";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useNavigate } from "react-router-dom";
import { ArrowLeft, PlusCircle } from "lucide-react";

export default function NewActivity() {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [activityType, setActivityType] = useState("task");
  const [dueAt, setDueAt] = useState("");
  const [workspaceId, setWorkspaceId] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
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
      setError(err.message || "خطا در ایجاد فعالیت");
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
        <h1 className="text-xl font-bold text-slate-800">ایجاد فعالیت جدید</h1>
      </header>

      <main className="p-6 max-w-2xl mx-auto">
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <PlusCircle className="text-orange-500" /> جزئیات فعالیت
            </CardTitle>
          </CardHeader>
          <CardContent>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium mb-1">شناسه حوزه</label>
                <Input
                  value={workspaceId}
                  onChange={(e) => setWorkspaceId(e.target.value)}
                  placeholder="مثلاً: 7f58a7ba-..."
                  required
                />
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
                  <label className="block text-sm font-medium mb-1">نوع فعالیت</label>
                  <select
                    className="w-full border rounded-lg px-3 py-2"
                    value={activityType}
                    onChange={(e) => setActivityType(e.target.value)}
                  >
                    <option value="task">تکلیف</option>
                    <option value="project">پروژه</option>
                    <option value="session">جلسه</option>
                    <option value="exam">امتحان</option>
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
              <Button type="submit" className="w-full" disabled={loading}>
                {loading ? "در حال ایجاد..." : "ایجاد فعالیت"}
              </Button>
            </form>
          </CardContent>
        </Card>
      </main>
    </div>
  );
}