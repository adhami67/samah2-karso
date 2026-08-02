import { useEffect, useState, FormEvent } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { api } from "@/lib/api";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Badge } from "@/components/ui/badge";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import {
  Calendar,
  Clock,
  ClipboardList,
  AlertTriangle,
  ArrowRightLeft,
  Archive,
  Timer,
} from "lucide-react";
import { UserSearch } from "@/components/UserSearch";
import { toast } from "sonner";

interface Activity {
  id: string;
  subject: string;
  description: string;
  status: string;
  activity_type: string;
  due_at: string | null;
  work_group_id: string;
}

interface Assignment {
  id: string;
  assignee_id: string;
  assignee_name?: string;
  assignee_type: string;
  role: string;
  status?: string;
  reply_deadline_time?: string;
  receiver_user?: { full_name?: string };
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
  activity_id: string;
  event_type: string;
  note: string;
  created_at: string;
}

const statusMap: Record<string, { label: string; cssClass: string }> = {
  draft: { label: "پیش‌نویس", cssClass: "status-badge--info" },
  published: { label: "منتشرشده", cssClass: "status-badge--info" },
  in_progress: { label: "در حال انجام", cssClass: "status-badge--warning" },
  submitted: { label: "ارسال‌شده", cssClass: "status-badge--info" },
  needs_revision: { label: "نیاز به اصلاح", cssClass: "status-badge--needs_revision" },
  approved: { label: "تأییدشده", cssClass: "status-badge--success" },
  completed: { label: "تکمیل‌شده", cssClass: "status-badge--success" },
  archived: { label: "بایگانی", cssClass: "status-badge--inactive" },
};

export default function ActivityDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [activity, setActivity] = useState<Activity | null>(null);
  const [assignments, setAssignments] = useState<Assignment[]>([]);
  const [responses, setResponses] = useState<Response[]>([]);
  const [evaluation, setEvaluation] = useState<Evaluation | null>(null);
  const [timeline, setTimeline] = useState<TimelineEvent[]>([]);

  const [selectedAssignee, setSelectedAssignee] = useState<{ id: string; user: any } | null>(null);
  const [responseBody, setResponseBody] = useState("");
  const [evalScore, setEvalScore] = useState("");
  const [evalNote, setEvalNote] = useState("");
  const [evalStatus, setEvalStatus] = useState("approved");

  const [newDueDate, setNewDueDate] = useState("");
  const [showExtend, setShowExtend] = useState(false);

  const [loading, setLoading] = useState(true);

  const fetchActivity = async () => {
    try {
      const data = await api.get<Activity>(`/activities/${id}`);
      setActivity(data);

      const [assigns, resps] = await Promise.all([
        api.get<Assignment[]>(`/assignments?activity_id=${id}`).catch(() => []),
        api.get<Response[]>(`/responses?activity_id=${id}`).catch(() => []),
      ]);
      setAssignments(Array.isArray(assigns) ? assigns : []);
      const responseList = Array.isArray(resps) ? resps : [];
      setResponses(responseList);

      if (responseList.length > 0) {
        const lastResp = responseList[responseList.length - 1];
        try {
          const evalList = await api.get<Evaluation[]>(`/evaluations?response_id=${lastResp.id}`);
          setEvaluation(evalList.length > 0 ? evalList[0] : null);
        } catch {
          setEvaluation(null);
        }
      } else {
        setEvaluation(null);
      }

      try {
        const events = await api.get<TimelineEvent[]>(`/timeline?activity_id=${id}`);
        setTimeline(Array.isArray(events) ? events : []);
      } catch {
        setTimeline([]);
      }
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
    if (!selectedAssignee) {
      toast.error("لطفاً یک کاربر را انتخاب کنید");
      return;
    }
    try {
      await api.post(`/activities/${id}/assign`, {
        receiver_user_ids: [selectedAssignee.id],
      });
      toast.success(`کار به ${selectedAssignee.user.full_name} ارجاع شد`);
      setSelectedAssignee(null);
      fetchActivity();
    } catch (err: any) {
      toast.error(err.message);
    }
  };

  const handleResponse = async (e: FormEvent) => {
    e.preventDefault();
    if (assignments.length === 0) {
      toast.error("ابتدا فعالیت را به یک کاربر واگذار کنید.");
      return;
    }
    const assignmentId = assignments[0].id;
    try {
      await api.post("/responses", {
        assignment_id: assignmentId,
        body: responseBody,
      });
      setResponseBody("");
      toast.success("پاسخ با موفقیت ثبت شد.");
      fetchActivity();
    } catch (err: any) {
      toast.error(err.message);
    }
  };

  const handleEvaluation = async (e: FormEvent) => {
    e.preventDefault();
    const latestResponse = responses[responses.length - 1];
    if (!latestResponse) {
      toast.error("ابتدا باید پاسخی ثبت شده باشد.");
      return;
    }
    try {
      await api.post("/evaluations", {
        response_id: latestResponse.id,
        score: evalScore ? Number(evalScore) : null,
        note: evalNote,
        status: evalStatus,
      });
      setEvalScore("");
      setEvalNote("");
      toast.success("ارزیابی ثبت شد.");
      fetchActivity();
    } catch (err: any) {
      toast.error(err.message);
    }
  };

  const handleStatusChange = async (newStatus: string) => {
    try {
      await api.patch(`/activities/${id}`, { status: newStatus });
      toast.success("وضعیت با موفقیت تغییر کرد.");
      fetchActivity();
    } catch (err: any) {
      toast.error(err.message);
    }
  };

  const handleExtendDeadline = async () => {
    if (!newDueDate) return;
    try {
      await api.patch(`/activities/${id}`, { due_at: new Date(newDueDate).toISOString() });
      setShowExtend(false);
      setNewDueDate("");
      toast.success("مهلت با موفقیت تمدید شد.");
      fetchActivity();
    } catch (err: any) {
      toast.error(err.message);
    }
  };

  const handleArchive = async () => {
    try {
      await api.patch(`/activities/${id}`, { status: "archived" });
      toast.success("فعالیت بایگانی شد.");
      fetchActivity();
    } catch (err: any) {
      toast.error(err.message);
    }
  };

  const isOverdue = (): boolean => {
    if (!activity || !activity.due_at) return false;
    const due = new Date(activity.due_at);
    const now = new Date();
    return due < now && ["published", "in_progress", "submitted", "needs_revision"].includes(activity.status);
  };

  if (loading) return <div className="p-8 text-center">در حال بارگیری...</div>;
  if (!activity) return <div className="p-8 text-center text-red-500">فعالیت یافت نشد.</div>;

  const st = statusMap[activity.status] || { label: activity.status, cssClass: "status-badge--info" };

  return (
    <div className="space-y-6">
      {/* هشدار معوقه */}
      {isOverdue() && (
        <Card className="border-2 border-rose-300 bg-rose-50">
          <CardContent className="p-4 space-y-3">
            <div className="flex items-center gap-2 text-rose-700 font-semibold">
              <AlertTriangle size={20} />
              <span>این فعالیت از موعد مقرر گذشته است. اقدامی انتخاب کنید:</span>
            </div>
            <div className="flex flex-wrap gap-2">
              <Button variant="outline" size="sm" onClick={() => setShowExtend(!showExtend)}>
                <Timer size={16} className="ml-1" /> تمدید مهلت
              </Button>
              <Button variant="outline" size="sm" onClick={() => navigate(`/activities/${id}?tab=assign`)}>
                <ArrowRightLeft size={16} className="ml-1" /> واگذاری دوباره
              </Button>
              <Button variant="outline" size="sm" onClick={handleArchive}>
                <Archive size={16} className="ml-1" /> بایگانی (لغو)
              </Button>
            </div>
            {showExtend && (
              <div className="flex items-center gap-2">
                <Input type="datetime-local" value={newDueDate} onChange={(e) => setNewDueDate(e.target.value)} className="w-auto" />
                <Button size="sm" onClick={handleExtendDeadline}>ثبت تاریخ جدید</Button>
                <Button size="sm" variant="ghost" onClick={() => setShowExtend(false)}>انصراف</Button>
              </div>
            )}
          </CardContent>
        </Card>
      )}

      {/* اطلاعات اصلی */}
      <Card className="glass-card">
        <CardContent className="p-4 space-y-3">
          <div className="flex items-center justify-between">
            <h2 className="text-lg font-bold text-slate-800">{activity.subject}</h2>
            <span className={`status-badge ${st.cssClass}`}>{st.label}</span>
          </div>
          {activity.description && <p className="text-slate-600 text-sm">{activity.description}</p>}
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
          <div className="flex flex-wrap gap-2 pt-2">
            {activity.status === "draft" && <Button size="sm" onClick={() => handleStatusChange("published")}>انتشار</Button>}
            {activity.status === "published" && <Button size="sm" onClick={() => handleStatusChange("in_progress")}>شروع</Button>}
            {activity.status === "in_progress" && <Button size="sm" onClick={() => handleStatusChange("submitted")}>ارسال برای بررسی</Button>}
            {activity.status === "submitted" && (
              <>
                <Button variant="outline" size="sm" onClick={() => handleStatusChange("needs_revision")}>نیاز به اصلاح</Button>
                <Button size="sm" onClick={() => handleStatusChange("approved")}>تأیید</Button>
              </>
            )}
            {activity.status === "needs_revision" && <Button size="sm" onClick={() => handleStatusChange("in_progress")}>بازگشت به انجام</Button>}
            {activity.status === "approved" && <Button size="sm" onClick={() => handleStatusChange("completed")}>تکمیل</Button>}
            {(activity.status === "completed" || activity.status === "approved") && <Button size="sm" variant="outline" onClick={handleArchive}>بایگانی</Button>}
          </div>
        </CardContent>
      </Card>

      {/* تب‌ها */}
      <Tabs defaultValue="assign">
        <TabsList className="grid w-full grid-cols-4 mb-4">
          <TabsTrigger value="assign">واگذاری</TabsTrigger>
          <TabsTrigger value="response">پاسخ</TabsTrigger>
          <TabsTrigger value="evaluation">ارزیابی</TabsTrigger>
          <TabsTrigger value="timeline">تاریخچه</TabsTrigger>
        </TabsList>

        {/* تب واگذاری */}
        <TabsContent value="assign">
          <Card>
            <CardHeader>
              <CardTitle className="text-lg">ارجاع به کاربر دیگر</CardTitle>
              <CardDescription>
                کاربری را که می‌خواهید این فعالیت را به او ارجاع دهید، جستجو و انتخاب کنید.
              </CardDescription>
            </CardHeader>
            <CardContent>
              <form onSubmit={handleAssign} className="space-y-4">
                <UserSearch
                  onChange={(userId, user) => setSelectedAssignee({ id: userId, user })}
                  placeholder="نام یا کد ملی کاربر را وارد کنید..."
                  excludeIds={assignments.map(a => a.assignee_id)}
                />
                <Button type="submit" className="w-full" disabled={!selectedAssignee}>
                  ارجاع به کاربر انتخاب‌شده
                </Button>
              </form>

              <div className="mt-6">
                <h4 className="text-sm font-medium mb-3">ارجاع‌های فعلی</h4>
                {assignments.length === 0 ? (
                  <p className="text-sm text-muted-foreground">هیچ ارجاعی ثبت نشده است</p>
                ) : (
                  <div className="space-y-2">
                    {assignments.map((a) => (
                      <div key={a.id} className="flex items-center justify-between p-3 bg-muted/30 rounded-lg">
                        <div className="flex items-center gap-3">
                          <Avatar className="h-8 w-8">
                            <AvatarFallback className="bg-primary/10 text-primary text-xs">
                              {a.assignee_name?.split(" ").map(n => n[0]).join("").substring(0, 2) || "??"}
                            </AvatarFallback>
                          </Avatar>
                          <div>
                            <p className="text-sm font-medium">{a.assignee_name || "کاربر ناشناس"}</p>
                            <p className="text-xs text-muted-foreground">
                              وضعیت: {a.status === "assigned" ? "ارجاع‌شده" : a.status === "in_progress" ? "در حال انجام" : a.status === "done" ? "انجام‌شده" : a.status || "نامشخص"}
                            </p>
                          </div>
                        </div>
                        {a.reply_deadline_time && (
                          <Badge variant="outline" className="text-xs">
                            مهلت: {new Date(a.reply_deadline_time).toLocaleDateString("fa-IR")}
                          </Badge>
                        )}
                      </div>
                    ))}
                  </div>
                )}
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* تب پاسخ */}
        <TabsContent value="response">
          <Card className="glass-card">
            <CardHeader><CardTitle className="text-lg">ثبت پاسخ</CardTitle></CardHeader>
            <CardContent>
              <form onSubmit={handleResponse} className="space-y-3">
                <Textarea placeholder="پاسخ خود را بنویسید..." value={responseBody} onChange={(e) => setResponseBody(e.target.value)} required />
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
          <Card className="glass-card">
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
                  <Input type="number" placeholder="امتیاز (۰ تا ۱۰۰)" value={evalScore} onChange={(e) => setEvalScore(e.target.value)} />
                  <Textarea placeholder="بازخورد..." value={evalNote} onChange={(e) => setEvalNote(e.target.value)} />
                  <select className="w-full border rounded-lg px-3 py-2 bg-white" value={evalStatus} onChange={(e) => setEvalStatus(e.target.value)}>
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
          <Card className="glass-card">
            <CardHeader><CardTitle className="text-lg">تاریخچه رویدادها</CardTitle></CardHeader>
            <CardContent>
              {timeline.length === 0 ? (
                <p className="text-sm text-slate-400">رویدادی ثبت نشده.</p>
              ) : (
                <ul className="space-y-3">
                  {timeline.map((e) => (
                    <li key={e.id} className="flex gap-2 text-sm">
                      <Clock size={16} className="text-slate-400 mt-0.5 flex-shrink-0" />
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
    </div>
  );
}