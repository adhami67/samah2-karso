// frontend/src/pages/StudentProfile.tsx
import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Label } from "@/components/ui/label";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger, DialogFooter } from "@/components/ui/dialog";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Skeleton } from "@/components/ui/skeleton";
import { toast } from "sonner";
import { Pencil, Trash2, Plus, Phone, Mail, MapPin, Calendar, User, BookOpen, FileText, GraduationCap, Home, Users, ChevronLeft } from "lucide-react";

interface StudentProfile {
  id: string;
  national_id: string;
  full_name: string;
  father_name?: string;
  mother_name?: string;
  parent_phone?: string;
  phone?: string;
  address?: string;
  birth_date?: string;
  gender?: string;
  grade?: string;
  class_name?: string;
  is_active: boolean;
  // فیلدهای جدید
  father_last_name?: string;
  mother_last_name?: string;
  major?: string;
  academic_year?: string;
}

interface Note {
  id: string;
  content: string;
  category: string;
  is_private: boolean;
  created_at: string;
  teacher_id: string;
  teacher?: { full_name: string };
}

export default function StudentProfile() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [student, setStudent] = useState<StudentProfile | null>(null);
  const [notes, setNotes] = useState<Note[]>([]);
  const [loading, setLoading] = useState(true);
  const [editing, setEditing] = useState(false);
  const [editData, setEditData] = useState<Partial<StudentProfile>>({});
  const [newNote, setNewNote] = useState({ content: "", category: "general" });
  const [isNoteDialogOpen, setIsNoteDialogOpen] = useState(false);

  const fetchData = async () => {
    try {
      const [studentData, notesData] = await Promise.all([
        api.get<StudentProfile>(`/security/users/${id}`),
        api.get<Note[]>(`/notes/student/${id}?include_private=true`),
      ]);
      setStudent(studentData);
      setNotes(notesData);
    } catch (error: any) {
      // اگر خطای ۴۰۱ (Unauthorized) رخ داد، کاربر را به صفحه لاگین ببر
      if (error.message?.includes("401") || error.status === 401) {
        toast.error("نشست شما منقضی شده است. لطفاً دوباره وارد شوید.");
        navigate("/login");
        return;
      }
      toast.error("خطا در دریافت اطلاعات");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (id) fetchData();
  }, [id]);

  const handleUpdate = async () => {
    try {
      await api.patch(`/security/users/${id}`, editData);
      toast.success("اطلاعات به‌روزرسانی شد");
      setEditing(false);
      fetchData();
    } catch (error: any) {
      if (error.message?.includes("401") || error.status === 401) {
        toast.error("نشست شما منقضی شده است. لطفاً دوباره وارد شوید.");
        navigate("/login");
        return;
      }
      toast.error("خطا در ویرایش");
    }
  };

  const handleAddNote = async () => {
    if (!newNote.content.trim()) {
      toast.error("لطفاً متن یادداشت را وارد کنید");
      return;
    }
    try {
      await api.post("/notes", {
        student_id: id,
        content: newNote.content,
        category: newNote.category,
        is_private: true,
      });
      toast.success("یادداشت ثبت شد");
      setNewNote({ content: "", category: "general" });
      setIsNoteDialogOpen(false);
      fetchData();
    } catch (error: any) {
      if (error.message?.includes("401") || error.status === 401) {
        toast.error("نشست شما منقضی شده است. لطفاً دوباره وارد شوید.");
        navigate("/login");
        return;
      }
      toast.error("خطا در ثبت یادداشت");
    }
  };

  const handleDeleteNote = async (noteId: string) => {
    if (!confirm("آیا از حذف این یادداشت اطمینان دارید؟")) return;
    try {
      await api.delete(`/notes/${noteId}`);
      toast.success("یادداشت حذف شد");
      fetchData();
    } catch (error: any) {
      if (error.message?.includes("401") || error.status === 401) {
        toast.error("نشست شما منقضی شده است. لطفاً دوباره وارد شوید.");
        navigate("/login");
        return;
      }
      toast.error("خطا در حذف یادداشت");
    }
  };

  if (loading) {
    return (
      <div className="space-y-4 p-6 max-w-4xl mx-auto">
        <Skeleton className="h-8 w-48" />
        <Skeleton className="h-64 rounded-lg" />
      </div>
    );
  }

  if (!student) {
    return <div className="p-6 text-center">دانش‌آموز یافت نشد</div>;
  }

  const categoryLabels: Record<string, string> = {
    general: "عمومی",
    behavior: "رفتاری",
    educational: "آموزشی",
    follow_up: "پیگیری",
  };

  return (
    <div className="space-y-6 p-6 max-w-4xl mx-auto">
      {/* هدر */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold">{student.full_name}</h1>
          <p className="text-sm text-muted-foreground">کد ملی: {student.national_id}</p>
        </div>
        <div className="flex gap-2">
          <Button variant="outline" size="sm" onClick={() => navigate("/students")}>
            <ChevronLeft className="h-4 w-4 ml-1" />
            بازگشت
          </Button>
          <Button size="sm" onClick={() => { setEditData(student); setEditing(true); }}>
            <Pencil className="h-4 w-4 ml-2" />
            ویرایش
          </Button>
        </div>
      </div>

      {/* اطلاعات فردی */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <Card>
          <CardHeader><CardTitle className="text-base flex items-center gap-2"><User className="h-4 w-4" />اطلاعات فردی</CardTitle></CardHeader>
          <CardContent className="space-y-2 text-sm">
            <p><span className="text-muted-foreground">نام کامل:</span> {student.full_name}</p>
            <p><span className="text-muted-foreground">کد ملی:</span> {student.national_id}</p>
            <p><span className="text-muted-foreground">تاریخ تولد:</span> {student.birth_date || "—"}</p>
            <p><span className="text-muted-foreground">جنسیت:</span> {student.gender === "male" ? "پسر" : student.gender === "female" ? "دختر" : "—"}</p>
            <p><span className="text-muted-foreground">تلفن:</span> {student.phone || "—"}</p>
            <p><span className="text-muted-foreground">آدرس:</span> {student.address || "—"}</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader><CardTitle className="text-base flex items-center gap-2"><Users className="h-4 w-4" />اطلاعات خانواده</CardTitle></CardHeader>
          <CardContent className="space-y-2 text-sm">
            <p><span className="text-muted-foreground">نام پدر:</span> {student.father_name || "—"}</p>
            <p><span className="text-muted-foreground">نام خانوادگی پدر:</span> {student.father_last_name || "—"}</p>
            <p><span className="text-muted-foreground">نام مادر:</span> {student.mother_name || "—"}</p>
            <p><span className="text-muted-foreground">نام خانوادگی مادر:</span> {student.mother_last_name || "—"}</p>
            <p><span className="text-muted-foreground">شماره والدین:</span> {student.parent_phone || "—"}</p>
          </CardContent>
        </Card>
      </div>

      {/* اطلاعات تحصیلی */}
      <Card>
        <CardHeader><CardTitle className="text-base flex items-center gap-2"><GraduationCap className="h-4 w-4" />اطلاعات تحصیلی</CardTitle></CardHeader>
        <CardContent className="grid grid-cols-2 md:grid-cols-4 gap-4 text-sm">
          <div><span className="text-muted-foreground">پایه:</span> {student.grade || "—"}</div>
          <div><span className="text-muted-foreground">کلاس:</span> {student.class_name || "—"}</div>
          <div><span className="text-muted-foreground">مقطع:</span> {student.major || "—"}</div>
          <div><span className="text-muted-foreground">سال تحصیلی:</span> {student.academic_year || "—"}</div>
          <div className="col-span-4"><span className="text-muted-foreground">وضعیت:</span> {student.is_active ? "فعال" : "غیرفعال"}</div>
        </CardContent>
      </Card>

      {/* تب‌ها */}
      <Tabs defaultValue="notes">
        <TabsList className="grid w-full grid-cols-3">
          <TabsTrigger value="notes">یادداشت‌ها</TabsTrigger>
          <TabsTrigger value="activities">فعالیت‌ها</TabsTrigger>
          <TabsTrigger value="attendance">حضور و غیاب</TabsTrigger>
        </TabsList>

        <TabsContent value="notes" className="mt-4">
          <Card>
            <CardHeader className="flex flex-row items-center justify-between">
              <CardTitle className="text-base">یادداشت‌های مربی</CardTitle>
              <Dialog open={isNoteDialogOpen} onOpenChange={setIsNoteDialogOpen}>
                <DialogTrigger asChild>
                  <Button size="sm" className="gap-1">
                    <Plus className="h-4 w-4" />
                    یادداشت جدید
                  </Button>
                </DialogTrigger>
                <DialogContent>
                  <DialogHeader><DialogTitle>ثبت یادداشت جدید</DialogTitle></DialogHeader>
                  <div className="space-y-4 py-4">
                    <div>
                      <Label>دسته‌بندی</Label>
                      <Select value={newNote.category} onValueChange={(v) => setNewNote({...newNote, category: v})}>
                        <SelectTrigger>
                          <SelectValue />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="general">عمومی</SelectItem>
                          <SelectItem value="behavior">رفتاری</SelectItem>
                          <SelectItem value="educational">آموزشی</SelectItem>
                          <SelectItem value="follow_up">پیگیری</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>
                    <div>
                      <Label>متن یادداشت</Label>
                      <Textarea
                        value={newNote.content}
                        onChange={(e) => setNewNote({...newNote, content: e.target.value})}
                        placeholder="متن یادداشت..."
                        className="min-h-[100px]"
                      />
                    </div>
                  </div>
                  <DialogFooter>
                    <Button variant="outline" onClick={() => setIsNoteDialogOpen(false)}>انصراف</Button>
                    <Button onClick={handleAddNote}>ثبت</Button>
                  </DialogFooter>
                </DialogContent>
              </Dialog>
            </CardHeader>
            <CardContent>
              {notes.length === 0 ? (
                <p className="text-sm text-muted-foreground text-center py-4">هیچ یادداشتی ثبت نشده است</p>
              ) : (
                <div className="space-y-3">
                  {notes.map((note) => (
                    <div key={note.id} className="p-3 rounded-lg border bg-muted/20">
                      <div className="flex items-start justify-between">
                        <div>
                          <span className="text-xs font-medium px-2 py-0.5 rounded-full bg-primary/10 text-primary">
                            {categoryLabels[note.category] || note.category}
                          </span>
                          <span className="text-xs text-muted-foreground mr-2">
                            {new Date(note.created_at).toLocaleDateString("fa-IR")}
                          </span>
                        </div>
                        <Button
                          variant="ghost"
                          size="sm"
                          className="h-6 w-6 text-red-500"
                          onClick={() => handleDeleteNote(note.id)}
                        >
                          <Trash2 className="h-3 w-3" />
                        </Button>
                      </div>
                      <p className="text-sm mt-1">{note.content}</p>
                    </div>
                  ))}
                </div>
              )}
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="activities" className="mt-4">
          <Card>
            <CardContent className="p-6 text-center text-muted-foreground">
              <FileText className="h-8 w-8 mx-auto mb-2 opacity-20" />
              فعالیت‌ها به‌زودی اضافه می‌شوند
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="attendance" className="mt-4">
          <Card>
            <CardContent className="p-6 text-center text-muted-foreground">
              <Calendar className="h-8 w-8 mx-auto mb-2 opacity-20" />
              حضور و غیاب به‌زودی اضافه می‌شود
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>

      {/* دیالوگ ویرایش کامل */}
      <Dialog open={editing} onOpenChange={setEditing}>
        <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
          <DialogHeader><DialogTitle>ویرایش اطلاعات دانش‌آموز</DialogTitle></DialogHeader>
          <div className="space-y-4 py-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <Label>نام کامل</Label>
                <Input value={editData.full_name || ""} onChange={(e) => setEditData({...editData, full_name: e.target.value})} />
              </div>
              <div>
                <Label>کد ملی</Label>
                <Input value={editData.national_id || ""} onChange={(e) => setEditData({...editData, national_id: e.target.value})} disabled />
              </div>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <Label>نام پدر</Label>
                <Input value={editData.father_name || ""} onChange={(e) => setEditData({...editData, father_name: e.target.value})} />
              </div>
              <div>
                <Label>نام خانوادگی پدر</Label>
                <Input value={editData.father_last_name || ""} onChange={(e) => setEditData({...editData, father_last_name: e.target.value})} />
              </div>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <Label>نام مادر</Label>
                <Input value={editData.mother_name || ""} onChange={(e) => setEditData({...editData, mother_name: e.target.value})} />
              </div>
              <div>
                <Label>نام خانوادگی مادر</Label>
                <Input value={editData.mother_last_name || ""} onChange={(e) => setEditData({...editData, mother_last_name: e.target.value})} />
              </div>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <Label>شماره والدین</Label>
                <Input value={editData.parent_phone || ""} onChange={(e) => setEditData({...editData, parent_phone: e.target.value})} />
              </div>
              <div>
                <Label>شماره تماس دانش‌آموز</Label>
                <Input value={editData.phone || ""} onChange={(e) => setEditData({...editData, phone: e.target.value})} />
              </div>
            </div>
            <div>
              <Label>آدرس</Label>
              <Input value={editData.address || ""} onChange={(e) => setEditData({...editData, address: e.target.value})} />
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <Label>پایه</Label>
                <Input value={editData.grade || ""} onChange={(e) => setEditData({...editData, grade: e.target.value})} />
              </div>
              <div>
                <Label>کلاس</Label>
                <Input value={editData.class_name || ""} onChange={(e) => setEditData({...editData, class_name: e.target.value})} />
              </div>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <Label>مقطع</Label>
                <Input value={editData.major || ""} onChange={(e) => setEditData({...editData, major: e.target.value})} />
              </div>
              <div>
                <Label>سال تحصیلی</Label>
                <Input value={editData.academic_year || ""} onChange={(e) => setEditData({...editData, academic_year: e.target.value})} />
              </div>
            </div>
            <div>
              <Label>تاریخ تولد</Label>
              <Input value={editData.birth_date || ""} onChange={(e) => setEditData({...editData, birth_date: e.target.value})} />
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setEditing(false)}>انصراف</Button>
            <Button onClick={handleUpdate}>ذخیره تغییرات</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}