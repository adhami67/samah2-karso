// frontend/src/pages/StudentsGrouped.tsx
import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from "@/components/ui/accordion";

interface Student { id: string; full_name: string; national_id: string; father_name: string; mother_name: string; parent_phone: string; phone: string; address: string; birth_date: string; gender: string; }
interface Group { grade: string; class: string; students: Student[]; }

export default function StudentsGrouped() {
  const [groups, setGroups] = useState<Group[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get("/security/students/grouped").then(setGroups).catch(console.error).finally(() => setLoading(false));
  }, []);

  if (loading) return <div>در حال بارگیری...</div>;

  return (
    <div className="space-y-6">
      <h1 className="text-2xl font-bold">دانش‌آموزان</h1>
      {groups.length === 0 ? <p>هیچ دانش‌آموزی یافت نشد.</p> : (
        groups.map((group, idx) => (
          <Card key={idx}>
            <CardHeader><CardTitle>{group.grade || "بدون پایه"} - {group.class || "بدون کلاس"}</CardTitle></CardHeader>
            <CardContent>
              <Accordion type="single" collapsible className="space-y-2">
                {group.students.map(student => (
                  <AccordionItem value={student.id} key={student.id}>
                    <AccordionTrigger className="text-right">{student.full_name}</AccordionTrigger>
                    <AccordionContent className="text-right space-y-1 text-sm">
                      <p>کد ملی: {student.national_id}</p>
                      <p>پدر: {student.father_name || "ندارد"}</p>
                      <p>مادر: {student.mother_name || "ندارد"}</p>
                      <p>شماره تماس والدین: {student.parent_phone || "ندارد"}</p>
                      <p>شماره تماس دانش‌آموز: {student.phone || "ندارد"}</p>
                      <p>آدرس: {student.address || "ندارد"}</p>
                      <p>تاریخ تولد: {student.birth_date || "ندارد"}</p>
                      <p>جنسیت: {student.gender === "male" ? "پسر" : student.gender === "female" ? "دختر" : "نامشخص"}</p>
                    </AccordionContent>
                  </AccordionItem>
                ))}
              </Accordion>
            </CardContent>
          </Card>
        ))
      )}
    </div>
  );
}