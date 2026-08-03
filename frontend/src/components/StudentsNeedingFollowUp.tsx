// frontend/src/components/StudentsNeedingFollowUp.tsx
import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Users, AlertCircle, ChevronRight } from "lucide-react";
import { useNavigate } from "react-router-dom";

interface Student {
  id: string;
  full_name: string;
  national_id: string;
  overdue_count: number;
  status: string;
  priority: "high" | "medium" | "low";
}

const priorityColors = {
  high: "border-red-500 bg-red-50",
  medium: "border-yellow-500 bg-yellow-50",
  low: "border-blue-500 bg-blue-50",
};

export function StudentsNeedingFollowUp() {
  const [students, setStudents] = useState<Student[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    api
      .get<Student[]>("/reports/students/need-follow-up")
      .then(setStudents)
      .catch(console.error)
      .finally(() => setLoading(false));
  }, []);

  if (loading) {
    return <div className="animate-pulse h-24 bg-muted/30 rounded-lg" />;
  }

  if (students.length === 0) {
    return (
      <Card className="mt-3 overflow-hidden border-r-4 border-purple-500">
        <CardContent className="p-3">
          <div className="flex items-center justify-between mb-2">
            <h3 className="flex items-center gap-2 text-sm font-semibold text-slate-700">
              <Users className="text-purple-500" size={18} />
              شاگردان نیازمند پیگیری
            </h3>
            <Button
              variant="ghost"
              size="sm"
              className="text-xs h-7"
              onClick={() => navigate("/students")}
            >
              مشاهده همه
            </Button>
          </div>
          <div className="text-center text-muted-foreground text-sm py-4">
            <AlertCircle className="h-6 w-6 mx-auto mb-2 opacity-30" />
            همه دانش‌آموزان در وضعیت عادی هستند
            <br />
            <span className="text-xs">هیچ فعالیت معوقی وجود ندارد</span>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card className="mt-3 overflow-hidden border-r-4 border-purple-500">
      <CardContent className="p-3">
        <div className="flex items-center justify-between mb-2">
          <h3 className="flex items-center gap-2 text-sm font-semibold text-slate-700">
            <Users className="text-purple-500" size={18} />
            شاگردان نیازمند پیگیری
            <span className="text-xs bg-purple-100 text-purple-700 px-2 py-0.5 rounded-full">
              {students.length}
            </span>
          </h3>
          <Button
            variant="ghost"
            size="sm"
            className="text-xs h-7"
            onClick={() => navigate("/students")}
          >
            مشاهده همه
          </Button>
        </div>
        <div className="space-y-2">
          {students.slice(0, 5).map((student) => (
            <div
              key={student.id}
              className={`flex items-center justify-between p-2 rounded-lg cursor-pointer hover:shadow-sm transition-all border-r-4 ${priorityColors[student.priority]}`}
              onClick={() => navigate(`/students/${student.id}`)}
            >
              <div className="flex items-center gap-3">
                <div className="w-8 h-8 rounded-full bg-purple-100 flex items-center justify-center text-purple-700 text-xs font-bold">
                  {student.full_name.charAt(0)}
                </div>
                <div>
                  <p className="text-sm font-medium">{student.full_name}</p>
                  <p className="text-xs text-muted-foreground">
                    {student.overdue_count} فعالیت معوق
                  </p>
                </div>
              </div>
              <div className="flex items-center gap-2">
                <span className={`text-xs px-2 py-0.5 rounded-full ${
                  student.priority === "high" ? "bg-red-100 text-red-700" :
                  student.priority === "medium" ? "bg-yellow-100 text-yellow-700" :
                  "bg-blue-100 text-blue-700"
                }`}>
                  {student.priority === "high" ? "فوری" :
                   student.priority === "medium" ? "متوسط" : "عادی"}
                </span>
                <ChevronRight className="h-4 w-4 text-muted-foreground" />
              </div>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
}