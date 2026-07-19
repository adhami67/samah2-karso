import { useEffect, useState, FormEvent } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import {
  ArrowLeft,
  Calendar,
  Clock,
  UserPlus,
  Send,
  CheckCircle,
  AlertTriangle,
  FileText,
  ClipboardList,
} from "lucide-react";

interface Activity {
  id: string;
  title: string;
  description: string;
  status: string;
  activity_type: string;
  due_at: string | null;
  workspace_id: string;
}

interface Assignment {
  id: string;
  assignee_id: string;
  assignee_type: string;
  role: string;
  created_at: string;
}

interface Response {
  id: string;
  assignment_id: string;
  body: string;
  status: string;
  created_at: string;
}

interface Evaluation {
  id: string;
  response_id: string;
  score: number | null;
  note: string;
  status: string;
}

interface TimelineEvent {
  id: string;
  event_type: string;
  note: string;
  created_at: string;
}

const statusMap: Record<string, { label: string; color: string }> = {
  draft: { label: "پیش‌نویس", color: "bg-gray-100 text-gray-700" },
  published: { label: "منتشرشده", color: "bg-blue-100 text-blue-700" },
  in_progress: { label: "در حال انجام", color: "bg-amber-100 text-amber-700" },
  submitted: { label: "ارسال‌شده", color: "bg-indigo-100 text-indigo-700" },
  needs_revision: { label: "نیاز به اصلاح", color: "bg-rose-100 text-rose-700" },
  approved: { label: "تأییدشده", color: "bg-emerald-100 text-emerald-700" },
  completed: { label: "تکمیل‌شده", color: "bg-green-100 text-green-700" },
  archived: { label: "بایگانی", color: "bg-slate-100 text-slate-500" },
};

export default function ActivityDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [activity, setActivity] = useState<Activity | null>(null);
  const [assignments, setAssignments] = useState<Assignment[]>([]);
  const [responses, setResponses] = useState<Response[]>([]);
  const [evaluation, setEvaluation] = useState<Evaluation | null>(null);
  const [timeline, setTimeline] = useState<TimelineEvent[]>([]);

  // فرم‌ها
  const [assigneeId, setAssigneeId] = useState("");
  const [assignRole, setAssignRole] = useState("executor");
  const [responseBody, setResponseBody] = useState("");
  const [evalScore, setEvalScore] = useState("");
  const [evalNote, setEvalNote] = useState("");
  const [evalStatus, setEvalStatus] = useState("approved");

  const [loading, setLoading] = useState(true);

  const fetchActivity = async () => {
    try {
      const data = await api.get<Activity>(`/activities/${id}`);
      setActivity(data);
      // بعد از گرفتن فعالیت، اطلاعات جانبی را بگیر
      const [assigns, resps, evalData, events] = await Promise.all([
        api.get<Assignment[]>(`/assignments/?activity_id=${id}`).catch(() => []),
        api.get<Response[]>(`/responses/?activity_id=${id}`).catch(() => []),
        api.get<Evaluation | null>(`/evaluations/?response_id=latest`).catch(() => null), // نیاز به endpoint مناسب
        api.get<TimelineEvent[]>(`/timeline/?activity_id=${id}`).catch(() => []),
      ]);
      setAssignments(Array.isArray(assigns) ? assigns : []);
      setResponses(Array.isArray(resps) ? resps : []);
      setEvaluation(evalData);
      setTimeline(Array.isArray(events) ? events : []);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (id) fetchActivity();
  }, [id]);

  const handleAssign = async (e: FormEvent) => {
    e.preventDefault();
    try {
      await api.post(`/activities/${id}/assign`, {
        assignee_id: assigneeId,
        role: assignRole,
      });
      setAssigneeId("");
      fetchActivity();
    } catch (err: any) {
      alert(err.message);
    }
  };

  const handleResponse = async (e: FormEvent) => {
    e.preventDefault();
    // نیاز به assignment_id داریم، می‌توان از اولین assignment استفاده کرد
    if (assignments.length === 0) {
      alert("ابتدا فعالیت را به یک کاربر واگذار کنید.");
      return;
    }
    const assignmentId = assignments[0].id;
    try {
      await api.post("/responses/", {
        assignment_id: assignmentId,
        body: responseBody,
      });
      setResponseBody("");
      fetchActivity();
    } catch (err: any) {
      alert(err.message);
    }
  };

  const handleEvaluation = async (e: FormEvent) => {
    e.preventDefault();
    const latestResponse = responses[responses.length - 1];
    if (!latestResponse) {
      alert("ابتدا باید پاسخی ثبت شده باشد.");
      return;
    }
    try {
      await api.post("/evaluations/", {
        response_id: latestResponse.id,
        score: evalScore ? Number(evalScore) : null,
        note: evalNote,
        status: evalStatus,
      });
      setEvalScore("");
      setEvalNote("");
      fetchActivity();
    } catch (err: any) {
      alert(err.message);
    }
  };

  const handleStatusChange = async (newStatus: string) => {
    try {
      await api.patch(`/activities/${id}`, { status: newStatus });
      fetchActivity();
    } catch (err: any) {
      alert(err.message);
    }
  };

  if (loading) return <div className="p-8 text-center">در حال بارگیری...</div>;
  if (!activity) return <div className="p-8 text-center text-red-500">فعالیت یافت نشد.</div>;

  const st = statusMap[activity.status] || { label: activity.status, color: "bg-gray-100" };

  return (
    <div className="min-h-screen bg-gradient-to-br from-orange-50 via-white to-amber-50">
      {/* هدر */}
      <header className="bg-white/80 backdrop-blur shadow-sm border-b border-orange-100 py-4 px-6 flex items-center gap-4">
        <Button variant="ghost" onClick={() => navigate("/activities")}>
          <ArrowLeft size={20} />
        </Button>
        <h1 className="text-xl font-bold text-slate-800">{activity.title}</h1>
        <span className={`px-2 py-1 rounded-full text-xs ${st.color}`}>{st.label}</span>
      </header>

      <main className="p-6 max-w-2xl mx-auto">
        {/* اطلاعات اصلی */}
        <Card className="mb-6">
          <CardContent className="p-4 space-y-2">
            {activity.description && (
              <p className="text-slate-600">{activity.description}</p>
            )}
            <div className="flex flex-wrap gap-4 text-sm">
              <div className="flex items-center gap-1">
                <ClipboardList size={16} className="text-slate-400" />
                <span>نوع: {activity.activity_type}</span>
              </div>
              {activity.due_at && (
                <div className="flex items-center gap-1">
                  <Calendar size={16} className="text-slate-400" />
                  <span>مهلت: {new Date(activity.due_at).toLocaleDateString("fa-IR")}</span>
                </div>
              )}
            </div>
            {/* دکمه‌های تغییر وضعیت */}
            <div className="flex flex-wrap gap-2 mt-3">
              {activity.status === "draft" && (
                <Button size="sm" onClick={() => handleStatusChange("published")}>انتشار</Button>
              )}
              {activity.status === "published" && (
                <Button size="sm" onClick={() => handleStatusChange("in_progress")}>شروع</Button>
              )}
              {activity.status === "submitted" && (
                <Button size="sm" variant="outline" onClick={() => handleStatusChange("completed")}>تکمیل</Button>
              )}
              {(activity.status === "completed" || activity.status === "approved") && (
                <Button size="sm" variant="outline" onClick={() => handleStatusChange("archived")}>بایگانی</Button>
              )}
            </div>
          </CardContent>
        </Card>

        {/* تب‌ها */}
        <Tabs defaultValue="assign">
          <TabsList className="grid w-full grid-cols-4">
            <TabsTrigger value="assign">واگذاری</TabsTrigger>
            <TabsTrigger value="response">پاسخ</TabsTrigger>
            <TabsTrigger value="evaluation">ارزیابی</TabsTrigger>
            <TabsTrigger value="timeline">تاریخچه</TabsTrigger>
          </TabsList>

          {/* تب واگذاری */}
          <TabsContent value="assign">
            <Card>
              <CardHeader><CardTitle className="text-lg">واگذاری جدید</CardTitle></CardHeader>
              <CardContent>
                <form onSubmit={handleAssign} className="space-y-3">
                  <Input
                    placeholder="شناسه کاربر (مثلاً 93002685...)"
                    value={assigneeId}
                    onChange={(e) => setAssigneeId(e.target.value)}
                    required
                  />
                  <select
                    className="w-full border rounded-lg px-3 py-2"
                    value={assignRole}
                    onChange={(e) => setAssignRole(e.target.value)}
                  >
                    <option value="executor">مجری</option>
                    <option value="reviewer">بازبین</option>
                  </select>
                  <Button type="submit" className="w-full">واگذار کن</Button>
                </form>
                <div className="mt-4">
                  <h3 className="font-medium mb-2">واگذاری‌های فعلی</h3>
                  {assignments.length === 0 ? (
                    <p className="text-sm text-slate-400">هنوز واگذاری‌ای ثبت نشده.</p>
                  ) : (
                    <ul className="space-y-2">
                      {assignments.map((a) => (
                        <li key={a.id} className="text-sm border-b pb-1">
                          کاربر {a.assignee_id} ({a.role})
                        </li>
                      ))}
                    </ul>
                  )}
                </div>
              </CardContent>
            </Card>
          </TabsContent>

          {/* تب پاسخ */}
          <TabsContent value="response">
            <Card>
              <CardHeader><CardTitle className="text-lg">ثبت پاسخ</CardTitle></CardHeader>
              <CardContent>
                <form onSubmit={handleResponse} className="space-y-3">
                  <Textarea
                    placeholder="پاسخ خود را بنویسید..."
                    value={responseBody}
                    onChange={(e) => setResponseBody(e.target.value)}
                    required
                  />
                  <Button type="submit" className="w-full">ارسال پاسخ</Button>
                </form>
                <div className="mt-4">
                  <h3 className="font-medium mb-2">پاسخ‌های قبلی</h3>
                  {responses.length === 0 ? (
                    <p className="text-sm text-slate-400">پاسخی ثبت نشده.</p>
                  ) : (
                    <ul className="space-y-2">
                      {responses.map((r) => (
                        <li key={r.id} className="text-sm border-b pb-1">
                          {r.body} <span className="text-slate-400">({new Date(r.created_at).toLocaleTimeString("fa-IR")})</span>
                        </li>
                      ))}
                    </ul>
                  )}
                </div>
              </CardContent>
            </Card>
          </TabsContent>

          {/* تب ارزیابی */}
          <TabsContent value="evaluation">
            <Card>
              <CardHeader><CardTitle className="text-lg">ارزیابی</CardTitle></CardHeader>
              <CardContent>
                {evaluation ? (
                  <div className="text-sm space-y-1">
                    <p>امتیاز: {evaluation.score ?? "—"}</p>
                    <p>بازخورد: {evaluation.note || "—"}</p>
                    <p>وضعیت: {evaluation.status}</p>
                  </div>
                ) : (
                  <form onSubmit={handleEvaluation} className="space-y-3">
                    <Input
                      type="number"
                      placeholder="امتیاز (۰ تا ۱۰۰)"
                      value={evalScore}
                      onChange={(e) => setEvalScore(e.target.value)}
                    />
                    <Textarea
                      placeholder="بازخورد..."
                      value={evalNote}
                      onChange={(e) => setEvalNote(e.target.value)}
                    />
                    <select
                      className="w-full border rounded-lg px-3 py-2"
                      value={evalStatus}
                      onChange={(e) => setEvalStatus(e.target.value)}
                    >
                      <option value="approved">تأیید</option>
                      <option value="needs_revision">نیاز به اصلاح</option>
                    </select>
                    <Button type="submit" className="w-full">ثبت ارزیابی</Button>
                  </form>
                )}
              </CardContent>
            </Card>
          </TabsContent>

          {/* تب تایم‌لاین */}
          <TabsContent value="timeline">
            <Card>
              <CardHeader><CardTitle className="text-lg">تاریخچه رویدادها</CardTitle></CardHeader>
              <CardContent>
                {timeline.length === 0 ? (
                  <p className="text-sm text-slate-400">رویدادی ثبت نشده.</p>
                ) : (
                  <ul className="space-y-2">
                    {timeline.map((e) => (
                      <li key={e.id} className="flex gap-2 text-sm">
                        <Clock size={16} className="text-slate-400 mt-0.5" />
                        <div>
                          <span className="font-medium">{e.event_type}</span> — {e.note}
                          <br />
                          <span className="text-xs text-slate-400">{new Date(e.created_at).toLocaleString("fa-IR")}</span>
                        </div>
                      </li>
                    ))}
                  </ul>
                )}
              </CardContent>
            </Card>
          </TabsContent>
        </Tabs>
      </main>
    </div>
  );
}