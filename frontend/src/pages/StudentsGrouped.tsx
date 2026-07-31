// frontend/src/pages/StudentsGrouped.tsx
import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Pencil, Trash2 } from "lucide-react";

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
}

interface Group {
  grade: string;
  class: string;
  students: Student[];
}

export default function StudentsGrouped() {
  const [groups, setGroups] = useState<Group[]>([]);
  const [loading, setLoading] = useState(true);

  // stateهای ویرایش
  const [editDialogOpen, setEditDialogOpen] = useState(false);
  const [editingStudent, setEditingStudent] = useState<Student | null>(null);
  const [editForm, setEditForm] = useState({
    full_name: "",
    father_name: "",
    mother_name: "",
    parent_phone: "",
    phone: "",
    address: "",
    birth_date: "",
    gender: "",
  });

  const fetchData = async () => {
    try {
      const data = await api.get<Group[]>("/bulk/students/grouped");
      setGroups(data || []);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const openEditDialog = (student: Student) => {
    setEditingStudent(student);
    setEditForm({
      full_name: student.full_name || "",
      father_name: student.father_name || "",
      mother_name: student.mother_name || "",
      parent_phone: student.parent_phone || "",
      phone: student.phone || "",
      address: student.address || "",
      birth_date: student.birth_date || "",
      gender: student.gender || "",
    });
    setEditDialogOpen(true);
  };

  const handleEditStudent = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!editingStudent) return;
    try {
      await api.patch(`/security/users/${editingStudent.id}`, editForm);
      setEditDialogOpen(false);
      fetchData();
    } catch (err: any) {
      alert(err.message);
    }
  };

  const handleDeleteStudent = async (userId: string) => {
    if (!confirm("آیا از حذف این دانش‌آموز اطمینان دارید؟")) return;
    try {
      await api.delete(`/security/users/${userId}`);
      fetchData();
    } catch (err: any) {
      alert(err.message);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  if (loading) return <div className="p-8 text-center">در حال بارگیری...</div>;

  return (
    <div className="space-y-6">
      <h1 className="text-2xl font-bold">دانش‌آموزان</h1>

      {groups.length === 0 ? (
        <p className="text-center text-slate-400">هیچ دانش‌آموزی یافت نشد.</p>
      ) : (
        groups.map((group, idx) => (
          <Card key={idx}>
            <CardHeader>
              <CardTitle>
                {group.grade || "بدون پایه"} - {group.class || "بدون کلاس"}
              </CardTitle>
            </CardHeader>
            <CardContent>
              <Accordion type="single" collapsible className="space-y-2">
                {group.students.map((student) => (
                  <AccordionItem value={student.id} key={student.id}>
                    <AccordionTrigger className="text-right">
                      {student.full_name}
                    </AccordionTrigger>
                    <AccordionContent className="text-right space-y-2 text-sm">
                      <div className="space-y-1">
                        <p>کد ملی: {student.national_id}</p>
                        <p>پدر: {student.father_name || "ندارد"}</p>
                        <p>مادر: {student.mother_name || "ندارد"}</p>
                        <p>شماره والدین: {student.parent_phone || "ندارد"}</p>
                        <p>شماره دانش‌آموز: {student.phone || "ندارد"}</p>
                        <p>آدرس: {student.address || "ندارد"}</p>
                        <p>تاریخ تولد: {student.birth_date || "ندارد"}</p>
                        <p>
                          جنسیت:{" "}
                          {student.gender === "male"
                            ? "پسر"
                            : student.gender === "female"
                            ? "دختر"
                            : "نامشخص"}
                        </p>
                      </div>

                      {/* دکمه‌های ویرایش و حذف */}
                      <div className="flex gap-2 pt-2 border-t">
                        <Button
                          variant="outline"
                          size="sm"
                          onClick={() => openEditDialog(student)}
                        >
                          <Pencil size={14} className="ml-1" />
                          ویرایش
                        </Button>
                        <Button
                          variant="outline"
                          size="sm"
                          className="text-red-500 border-red-200 hover:bg-red-50"
                          onClick={() => handleDeleteStudent(student.id)}
                        >
                          <Trash2 size={14} className="ml-1" />
                          حذف
                        </Button>
                      </div>
                    </AccordionContent>
                  </AccordionItem>
                ))}
              </Accordion>
            </CardContent>
          </Card>
        ))
      )}

      {/* دیالوگ ویرایش دانش‌آموز */}
      <Dialog open={editDialogOpen} onOpenChange={setEditDialogOpen}>
        <DialogContent className="max-w-lg">
          <DialogHeader>
            <DialogTitle>ویرایش دانش‌آموز</DialogTitle>
          </DialogHeader>
          <form onSubmit={handleEditStudent} className="space-y-4">
            <Input
              placeholder="نام کامل"
              value={editForm.full_name}
              onChange={(e) =>
                setEditForm({ ...editForm, full_name: e.target.value })
              }
              required
            />
            <Input
              placeholder="نام پدر"
              value={editForm.father_name}
              onChange={(e) =>
                setEditForm({ ...editForm, father_name: e.target.value })
              }
            />
            <Input
              placeholder="نام مادر"
              value={editForm.mother_name}
              onChange={(e) =>
                setEditForm({ ...editForm, mother_name: e.target.value })
              }
            />
            <Input
              placeholder="شماره والدین"
              value={editForm.parent_phone}
              onChange={(e) =>
                setEditForm({ ...editForm, parent_phone: e.target.value })
              }
            />
            <Input
              placeholder="شماره دانش‌آموز"
              value={editForm.phone}
              onChange={(e) =>
                setEditForm({ ...editForm, phone: e.target.value })
              }
            />
            <Input
              placeholder="آدرس"
              value={editForm.address}
              onChange={(e) =>
                setEditForm({ ...editForm, address: e.target.value })
              }
            />
            <Input
              placeholder="تاریخ تولد"
              value={editForm.birth_date}
              onChange={(e) =>
                setEditForm({ ...editForm, birth_date: e.target.value })
              }
            />
            <select
              className="w-full border rounded-lg px-3 py-2 bg-white"
              value={editForm.gender}
              onChange={(e) =>
                setEditForm({ ...editForm, gender: e.target.value })
              }
            >
              <option value="">نامشخص</option>
              <option value="male">پسر</option>
              <option value="female">دختر</option>
            </select>

            <div className="flex justify-end gap-2 pt-2">
              <Button
                type="button"
                variant="outline"
                onClick={() => setEditDialogOpen(false)}
              >
                انصراف
              </Button>
              <Button type="submit">ذخیره تغییرات</Button>
            </div>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  );
}