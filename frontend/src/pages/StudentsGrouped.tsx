// frontend/src/pages/StudentsGrouped.tsx
import { useEffect, useState, useRef } from "react";
import { useNavigate } from "react-router-dom"; // ✅ اضافه شد
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Loader2, User, Phone, MapPin, Calendar, Users, Pencil, ExternalLink, Edit } from "lucide-react";
import { toast } from "sonner";

interface User {
  id: string;
  full_name: string;
  national_id: string;
  father_name: string | null;
  mother_name: string | null;
  parent_phone: string | null;
  phone: string | null;
  address: string | null;
  birth_date: string | null;
  gender: string | null;
  grade: string | null;
  class_name: string | null;
  is_active: boolean;
  roles: Array<{ code: string; name: string }>;
}

interface Student {
  id: string;
  full_name: string;
  national_id: string;
  father_name: string;
  mother_name: string;
  parent_phone: string;
  phone: string;
  address: string;
  birth_date: string;
  gender: string;
  grade: string;
  class_name: string;
}

interface Group {
  key: string;
  grade: string;
  class: string;
  students: Student[];
}

// استخراج شماره پایه از رشته (مثلاً "سوم" → 3)
const extractGradeNumber = (grade: string): number => {
  const map: Record<string, number> = {
    "اول": 1,
    "دوم": 2,
    "سوم": 3,
    "چهارم": 4,
    "پنجم": 5,
    "ششم": 6,
  };
  return map[grade] || 999;
};

// استخراج نام خانوادگی از نام کامل
const getLastName = (fullName: string): string => {
  const parts = fullName.trim().split(" ");
  return parts.length > 1 ? parts[parts.length - 1] : fullName;
};

export default function StudentsGrouped() {
  const navigate = useNavigate(); // ✅ اضافه شد
  const [groups, setGroups] = useState<Group[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedClassKey, setSelectedClassKey] = useState<string | null>(null);
  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);

  // stateهای دیالوگ ویرایش پایه و کلاس (تک دانش‌آموز)
  const [editDialogOpen, setEditDialogOpen] = useState(false);
  const [editingStudent, setEditingStudent] = useState<Student | null>(null);
  const [editGrade, setEditGrade] = useState("");
  const [editClass, setEditClass] = useState("");

  // ✅ stateهای دیالوگ ویرایش دسته‌جمعی کلاس
  const [batchEditDialogOpen, setBatchEditDialogOpen] = useState(false);
  const [batchEditGroupKey, setBatchEditGroupKey] = useState<string | null>(null);
  const [batchEditGrade, setBatchEditGrade] = useState("");
  const [batchEditClass, setBatchEditClass] = useState("");
  const [batchEditing, setBatchEditing] = useState(false);

  // ref برای مدیریت تایم‌اوت محو شدن
  const hideTimeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  const fetchData = async () => {
    try {
      const users = await api.get<User[]>("/security/users");
      const students = users.filter(
        (user) => user.is_active && user.roles?.some((role) => role.code === "student")
      );

      const grouped = students.reduce<Record<string, Group>>((acc, user) => {
        const grade = user.grade || "بدون پایه";
        const className = user.class_name || "بدون کلاس";
        const key = `${grade}|${className}`;
        if (!acc[key]) {
          acc[key] = {
            key,
            grade,
            class: className,
            students: [],
          };
        }
        acc[key].students.push({
          id: user.id,
          full_name: user.full_name,
          national_id: user.national_id,
          father_name: user.father_name || "",
          mother_name: user.mother_name || "",
          parent_phone: user.parent_phone || "",
          phone: user.phone || "",
          address: user.address || "",
          birth_date: user.birth_date || "",
          gender: user.gender || "",
          grade,
          class_name: className,
        });
        return acc;
      }, {});

      const sortedGroups = Object.values(grouped).sort((a, b) => {
        return extractGradeNumber(a.grade) - extractGradeNumber(b.grade);
      });

      sortedGroups.forEach((group) => {
        group.students.sort((a, b) =>
          getLastName(a.full_name).localeCompare(getLastName(b.full_name))
        );
      });

      setGroups(sortedGroups);
    } catch (err) {
      console.error(err);
      toast.error("خطا در دریافت لیست دانش‌آموزان");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  // ✅ کلیک روی نام دانش‌آموز → رفتن به صفحه پرونده
  const handleStudentClick = (student: Student) => {
    navigate(`/students/${student.id}`);
  };

  // مدیریت نمایش/محو شدن مشخصات با تاخیر
  const handleStudentMouseEnter = (student: Student) => {
    if (hideTimeoutRef.current) {
      clearTimeout(hideTimeoutRef.current);
      hideTimeoutRef.current = null;
    }
    setSelectedStudent(student);
  };

  const handleStudentMouseLeave = () => {
    hideTimeoutRef.current = setTimeout(() => {
      setSelectedStudent(null);
    }, 200);
  };

  const handleDetailsMouseEnter = () => {
    if (hideTimeoutRef.current) {
      clearTimeout(hideTimeoutRef.current);
      hideTimeoutRef.current = null;
    }
  };

  const handleDetailsMouseLeave = () => {
    setSelectedStudent(null);
  };

  const handleClassClick = (key: string) => {
    setSelectedClassKey(key);
    setSelectedStudent(null);
  };

  // باز کردن دیالوگ ویرایش پایه و کلاس (تک دانش‌آموز)
  const openEditGradeClass = (student: Student) => {
    setEditingStudent(student);
    setEditGrade(student.grade || "");
    setEditClass(student.class_name || "");
    setEditDialogOpen(true);
  };

  const handleEditGradeClass = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!editingStudent) return;
    try {
      await api.patch(`/security/users/${editingStudent.id}`, {
        grade: editGrade,
        class_name: editClass,
      });
      toast.success("پایه و کلاس با موفقیت به‌روزرسانی شد");
      setEditDialogOpen(false);
      fetchData();
    } catch (err: any) {
      toast.error(err.message || "خطا در ویرایش");
    }
  };

  // ✅ باز کردن دیالوگ ویرایش دسته‌جمعی کلاس
  const openBatchEdit = (group: Group, e: React.MouseEvent) => {
    e.stopPropagation(); // جلوگیری از انتخاب کلاس
    setBatchEditGroupKey(group.key);
    setBatchEditGrade(group.grade);
    setBatchEditClass(group.class);
    setBatchEditDialogOpen(true);
  };

  // ✅ ذخیره تغییرات دسته‌جمعی
  const handleBatchEdit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!batchEditGroupKey) return;

    const group = groups.find((g) => g.key === batchEditGroupKey);
    if (!group) return;

    setBatchEditing(true);
    let successCount = 0;
    let failCount = 0;

    for (const student of group.students) {
      try {
        await api.patch(`/security/users/${student.id}`, {
          grade: batchEditGrade,
          class_name: batchEditClass,
        });
        successCount++;
      } catch (err) {
        failCount++;
      }
    }

    setBatchEditing(false);
    setBatchEditDialogOpen(false);

    if (failCount === 0) {
      toast.success(`پایه و کلاس ${successCount} دانش‌آموز با موفقیت به‌روزرسانی شد`);
    } else {
      toast.warning(`${successCount} دانش‌آموز به‌روزرسانی شدند و ${failCount} دانش‌آموز با خطا مواجه شدند`);
    }

    fetchData();
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-[80vh]">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
      </div>
    );
  }

  if (groups.length === 0) {
    return (
      <div className="flex items-center justify-center h-[80vh] text-muted-foreground">
        <Users className="h-12 w-12 mr-4 opacity-20" />
        <span>هیچ دانش‌آموزی یافت نشد.</span>
      </div>
    );
  }

  const selectedGroup = groups.find((g) => g.key === selectedClassKey);
  const studentsOfSelectedClass = selectedGroup?.students || [];

  return (
    <div className="flex h-[calc(100vh-6rem)] gap-4 p-4">
      {/* ========== سمت راست: لیست کلاس‌ها یا مشخصات دانش‌آموز ========== */}
      <div
        className="w-1/3 bg-white rounded-lg shadow-md p-4 overflow-y-auto"
        onMouseEnter={handleDetailsMouseEnter}
        onMouseLeave={handleDetailsMouseLeave}
      >
        {selectedStudent ? (
          // نمایش مشخصات کامل دانش‌آموز
          <div className="space-y-4">
            <div className="flex items-center justify-between border-b pb-3">
              <h2 className="text-xl font-bold">{selectedStudent.full_name}</h2>
              <span className="text-sm text-muted-foreground">
                {selectedStudent.national_id}
              </span>
            </div>

            <div className="space-y-2 text-sm">
              <p><span className="font-medium">پدر:</span> {selectedStudent.father_name || "ندارد"}</p>
              <p><span className="font-medium">مادر:</span> {selectedStudent.mother_name || "ندارد"}</p>
              <p><span className="font-medium">شماره والدین:</span> {selectedStudent.parent_phone || "ندارد"}</p>
              <p><span className="font-medium">شماره دانش‌آموز:</span> {selectedStudent.phone || "ندارد"}</p>
              <p><span className="font-medium">آدرس:</span> {selectedStudent.address || "ندارد"}</p>
              <p><span className="font-medium">تاریخ تولد:</span> {selectedStudent.birth_date || "ندارد"}</p>
              <p>
                <span className="font-medium">جنسیت:</span>{" "}
                {selectedStudent.gender === "male" ? "پسر" :
                 selectedStudent.gender === "female" ? "دختر" : "نامشخص"}
              </p>
              <p><span className="font-medium">پایه:</span> {selectedStudent.grade}</p>
              <p><span className="font-medium">کلاس:</span> {selectedStudent.class_name}</p>
            </div>

            {/* ✅ دکمه‌های پایین */}
            <div className="flex flex-col gap-2 mt-4">
              <Button
                variant="outline"
                size="sm"
                className="w-full"
                onClick={() => openEditGradeClass(selectedStudent)}
              >
                <Pencil size={14} className="ml-2" />
                تغییر پایه و کلاس
              </Button>

              {/* ✅ دکمه ورود به پرونده کامل */}
              <Button
                size="sm"
                className="w-full"
                onClick={() => handleStudentClick(selectedStudent)}
              >
                <ExternalLink size={14} className="ml-2" />
                مشاهده پرونده کامل
              </Button>
            </div>
          </div>
        ) : (
          // لیست کلاس‌ها
          <>
            <div className="flex items-center justify-between mb-4">
              <h2 className="text-lg font-semibold">کلاس‌ها</h2>
              <span className="text-xs text-muted-foreground">
                {groups.reduce((acc, g) => acc + g.students.length, 0)} دانش‌آموز
              </span>
            </div>
            <div className="space-y-2">
              {groups.map((group) => (
                <div
                  key={group.key}
                  className={`p-3 rounded-lg cursor-pointer transition-colors group ${
                    selectedClassKey === group.key
                      ? "bg-primary text-primary-foreground"
                      : "bg-gray-50 hover:bg-gray-100"
                  }`}
                  onClick={() => handleClassClick(group.key)}
                >
                  <div className="flex justify-between items-center">
                    <span>
                      {group.grade} - {group.class}
                    </span>
                    <div className="flex items-center gap-2">
                      <span className="text-xs bg-white/20 px-2 py-0.5 rounded-full">
                        {group.students.length}
                      </span>
                      {/* ✅ دکمه ویرایش دسته‌جمعی کلاس */}
                      <Button
                        variant="ghost"
                        size="icon"
                        className={`h-6 w-6 rounded-full opacity-0 group-hover:opacity-100 transition-opacity ${
                          selectedClassKey === group.key
                            ? "hover:bg-white/20 text-primary-foreground"
                            : "hover:bg-gray-200"
                        }`}
                        onClick={(e) => openBatchEdit(group, e)}
                      >
                        <Edit size={12} />
                      </Button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </>
        )}
      </div>

      {/* ========== سمت چپ: لیست شاگردان کلاس انتخاب‌شده ========== */}
      <div className="w-2/3 bg-white rounded-lg shadow-md p-4 overflow-y-auto">
        {selectedClassKey ? (
          <>
            <h3 className="text-lg font-semibold mb-4">
              دانش‌آموزان {selectedGroup?.grade} - {selectedGroup?.class}
            </h3>
            <div className="grid grid-cols-2 gap-3">
              {studentsOfSelectedClass.map((student) => (
                <div
                  key={student.id}
                  className={`p-3 rounded-lg border cursor-pointer transition-all hover:shadow-md ${
                    selectedStudent?.id === student.id
                      ? "border-primary bg-primary/5"
                      : "border-gray-200 hover:border-primary/50"
                  }`}
                  onClick={() => handleStudentClick(student)} // ✅ کلیک → رفتن به پرونده
                  onMouseEnter={() => handleStudentMouseEnter(student)}
                  onMouseLeave={handleStudentMouseLeave}
                >
                  <div className="flex items-center gap-3">
                    <div className="w-10 h-10 rounded-full bg-primary/10 flex items-center justify-center text-primary font-bold">
                      {student.full_name.charAt(0)}
                    </div>
                    <div className="flex-1 min-w-0">
                      <p className="font-medium truncate">{student.full_name}</p>
                      <p className="text-xs text-muted-foreground truncate">
                        {student.national_id}
                      </p>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </>
        ) : (
          <div className="flex items-center justify-center h-full text-muted-foreground">
            <Users className="h-12 w-12 mr-4 opacity-20" />
            <span>برای مشاهده دانش‌آموزان، یک کلاس را انتخاب کنید</span>
          </div>
        )}
      </div>

      {/* ========== دیالوگ ویرایش پایه و کلاس (تک دانش‌آموز) ========== */}
      <Dialog open={editDialogOpen} onOpenChange={setEditDialogOpen}>
        <DialogContent className="max-w-sm">
          <DialogHeader>
            <DialogTitle>تغییر پایه و کلاس</DialogTitle>
          </DialogHeader>
          <form onSubmit={handleEditGradeClass} className="space-y-4">
            <div>
              <label className="text-sm font-medium">پایه</label>
              <select
                className="w-full border rounded-lg px-3 py-2 bg-white mt-1"
                value={editGrade}
                onChange={(e) => setEditGrade(e.target.value)}
                required
              >
                <option value="">انتخاب پایه</option>
                <option value="اول">اول</option>
                <option value="دوم">دوم</option>
                <option value="سوم">سوم</option>
                <option value="چهارم">چهارم</option>
                <option value="پنجم">پنجم</option>
                <option value="ششم">ششم</option>
              </select>
            </div>
            <div>
              <label className="text-sm font-medium">کلاس</label>
              <Input
                placeholder="مثلاً الف یا ۱"
                value={editClass}
                onChange={(e) => setEditClass(e.target.value)}
                required
              />
            </div>
            <div className="flex justify-end gap-2 pt-2">
              <Button
                type="button"
                variant="outline"
                onClick={() => setEditDialogOpen(false)}
              >
                انصراف
              </Button>
              <Button type="submit">ذخیره</Button>
            </div>
          </form>
        </DialogContent>
      </Dialog>

      {/* ========== ✅ دیالوگ ویرایش دسته‌جمعی کلاس ========== */}
      <Dialog open={batchEditDialogOpen} onOpenChange={setBatchEditDialogOpen}>
        <DialogContent className="max-w-sm">
          <DialogHeader>
            <DialogTitle>ویرایش دسته‌جمعی کلاس</DialogTitle>
          </DialogHeader>
          <form onSubmit={handleBatchEdit} className="space-y-4">
            <p className="text-sm text-muted-foreground">
              این تغییرات روی <strong>همه دانش‌آموزان</strong> این کلاس اعمال خواهد شد.
            </p>
            <div>
              <label className="text-sm font-medium">پایه جدید</label>
              <select
                className="w-full border rounded-lg px-3 py-2 bg-white mt-1"
                value={batchEditGrade}
                onChange={(e) => setBatchEditGrade(e.target.value)}
                required
              >
                <option value="">انتخاب پایه</option>
                <option value="اول">اول</option>
                <option value="دوم">دوم</option>
                <option value="سوم">سوم</option>
                <option value="چهارم">چهارم</option>
                <option value="پنجم">پنجم</option>
                <option value="ششم">ششم</option>
              </select>
            </div>
            <div>
              <label className="text-sm font-medium">کلاس جدید</label>
              <Input
                placeholder="مثلاً الف یا ۱"
                value={batchEditClass}
                onChange={(e) => setBatchEditClass(e.target.value)}
                required
              />
            </div>
            <div className="flex justify-end gap-2 pt-2">
              <Button
                type="button"
                variant="outline"
                onClick={() => setBatchEditDialogOpen(false)}
              >
                انصراف
              </Button>
              <Button type="submit" disabled={batchEditing}>
                {batchEditing ? (
                  <>
                    <Loader2 className="h-4 w-4 animate-spin ml-2" />
                    در حال به‌روزرسانی...
                  </>
                ) : (
                  "ذخیره تغییرات"
                )}
              </Button>
            </div>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  );
}