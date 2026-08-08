// frontend/src/components/QuickNote.tsx
import { useState, useEffect } from "react";
import { api } from "@/lib/api";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Label } from "@/components/ui/label";
import { PenSquare, Loader2 } from "lucide-react";
import { toast } from "sonner";

interface Student {
  id: string;
  full_name: string;
  national_id: string;
  grade?: string;
  class_name?: string;
}

export function QuickNote() {
  const [students, setStudents] = useState<Student[]>([]);
  const [selectedStudent, setSelectedStudent] = useState<string>("");
  const [content, setContent] = useState("");
  const [category, setCategory] = useState<string>("general");
  const [loading, setLoading] = useState(false);
  const [fetching, setFetching] = useState(true);

  // دریافت لیست دانش‌آموزان
  useEffect(() => {
    api
      .get<Student[]>("/reports/students/list")
      .then(setStudents)
      .catch(console.error)
      .finally(() => setFetching(false));
  }, []);

  const handleSubmit = async () => {
    if (!selectedStudent) {
      toast.error("لطفاً یک دانش‌آموز را انتخاب کنید");
      return;
    }
    if (!content.trim()) {
      toast.error("لطفاً متن یادداشت را وارد کنید");
      return;
    }

    setLoading(true);
    try {
      await api.post("/notes", {
        student_id: selectedStudent,
        content: content.trim(),
        category: category,
        is_private: true,
      });
      toast.success("یادداشت با موفقیت ثبت شد");
      setContent("");
      setSelectedStudent("");
    } catch (error) {
      toast.error("خطا در ثبت یادداشت");
    } finally {
      setLoading(false);
    }
  };

  if (fetching) {
    return (
      <Card className="mt-3">
        <CardContent className="p-4 text-center text-muted-foreground">
          <Loader2 className="h-5 w-5 animate-spin mx-auto" />
          در حال بارگیری...
        </CardContent>
      </Card>
    );
  }

  if (students.length === 0) {
    return null;
  }

  return (
    <Card className="mt-3 border-r-4 border-green-500">
      <CardContent className="p-4">
        <div className="flex items-center gap-2 mb-3">
          <PenSquare className="text-green-500" size={18} />
          <h3 className="text-sm font-semibold text-slate-700">یادداشت سریع</h3>
        </div>

        <div className="space-y-3">
          {/* انتخاب دانش‌آموز */}
          <div>
            <Label className="text-xs text-muted-foreground">دانش‌آموز</Label>
            <Select value={selectedStudent} onValueChange={setSelectedStudent}>
              <SelectTrigger className="h-9 text-sm">
                <SelectValue placeholder="انتخاب دانش‌آموز..." />
              </SelectTrigger>
              <SelectContent>
                {students.map((student) => (
                  <SelectItem key={student.id} value={student.id}>
                    {student.full_name} {student.class_name && `(${student.class_name})`}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          {/* دسته‌بندی */}
          <div>
            <Label className="text-xs text-muted-foreground">دسته‌بندی</Label>
            <Select value={category} onValueChange={setCategory}>
              <SelectTrigger className="h-9 text-sm">
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

          {/* متن یادداشت */}
          <div>
            <Label className="text-xs text-muted-foreground">متن یادداشت</Label>
            <Textarea
              placeholder="یادداشت خود را بنویسید..."
              value={content}
              onChange={(e) => setContent(e.target.value)}
              className="resize-none h-16 text-sm"
            />
          </div>

          {/* دکمه ثبت */}
          <Button
            onClick={handleSubmit}
            disabled={loading || !selectedStudent || !content.trim()}
            className="w-full h-9 text-sm"
            size="sm"
          >
            {loading ? (
              <>
                <Loader2 className="h-4 w-4 animate-spin ml-2" />
                در حال ثبت...
              </>
            ) : (
              "ثبت یادداشت"
            )}
          </Button>
        </div>
      </CardContent>
    </Card>
  );
}