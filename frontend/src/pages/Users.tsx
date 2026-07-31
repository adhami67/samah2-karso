// frontend/src/pages/Users.tsx
import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { useAuth } from "@/contexts/AuthContext";

interface User {
  id: string;
  national_id: string;
  full_name: string;
  username: string;
  is_active: boolean;
  roles?: { id: string; code: string; name: string }[];
  grade?: string;
  class_name?: string;
  parent_phone?: string;
}

export default function Users() {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [newUser, setNewUser] = useState({
    national_id: "",
    full_name: "",
    password: "",
    role: "student",
  });
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [uploading, setUploading] = useState(false);
  const [uploadResult, setUploadResult] = useState<any>(null);

  const fetchUsers = async () => {
    try {
      const data = await api.get<User[]>("/security/users");
      setUsers(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleCreateUser = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      // نقش‌های سیستم باید دقیقاً همان idهایی باشند که در دیتابیس وجود دارند
      const roleMap: Record<string, string> = {
        student: "role_student",
        teacher: "role_teacher",
        parent: "role_parent",
        admin: "role_admin",
      };
      const roleId = roleMap[newUser.role] || "role_student";
      await api.post("/security/users", {
        national_id: newUser.national_id,
        full_name: newUser.full_name,
        password: newUser.password,
        role_ids: [roleId],
      });
      setNewUser({ national_id: "", full_name: "", password: "", role: "student" });
      fetchUsers();
    } catch (err: any) {
      alert(err.message);
    }
  };

  const handleBulkUpload = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedFile) return;
    setUploading(true);
    const formData = new FormData();
    formData.append("file", selectedFile);
    try {
      // مسیر API اصلاح می‌شود؛ در ادامهٔ main.py توضیح داده می‌شود
      const result = await api.post("/security/users/bulk", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });
      setUploadResult(result);
      fetchUsers();
    } catch (err: any) {
      alert(err.message);
    } finally {
      setUploading(false);
      setSelectedFile(null);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  if (loading) return <div className="p-8 text-center">در حال بارگیری...</div>;

  return (
    <div className="space-y-6">
      <h1 className="text-2xl font-bold">مدیریت کاربران</h1>

      {/* فرم ایجاد کاربر */}
      <Card>
        <CardHeader className="pb-2">
          <CardTitle className="text-base">ایجاد کاربر جدید</CardTitle>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleCreateUser} className="grid grid-cols-1 md:grid-cols-4 gap-3">
            <Input
              placeholder="کد ملی"
              value={newUser.national_id}
              onChange={(e) => setNewUser({ ...newUser, national_id: e.target.value })}
              required
              className="h-9"
            />
            <Input
              placeholder="نام کامل"
              value={newUser.full_name}
              onChange={(e) => setNewUser({ ...newUser, full_name: e.target.value })}
              required
              className="h-9"
            />
            <Input
              type="password"
              placeholder="رمز عبور"
              value={newUser.password}
              onChange={(e) => setNewUser({ ...newUser, password: e.target.value })}
              required
              className="h-9"
            />
            <div className="flex gap-2">
              <select
                className="border rounded-lg px-3 py-1.5 text-sm bg-white flex-1 h-9"
                value={newUser.role}
                onChange={(e) => setNewUser({ ...newUser, role: e.target.value })}
              >
                <option value="student">دانش‌آموز</option>
                <option value="teacher">معلم</option>
                <option value="parent">والدین</option>
                <option value="admin">مدیر</option>
              </select>
              <Button type="submit" className="h-9 px-4 text-sm">ایجاد</Button>
            </div>
          </form>
        </CardContent>
      </Card>

      {/* بخش آپلود انبوه */}
      <Card>
        <CardHeader className="pb-2">
          <CardTitle className="text-base">آپلود انبوه کاربران</CardTitle>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleBulkUpload} className="flex items-center gap-3 flex-wrap">
            <Input
              type="file"
              accept=".xlsx,.xls"
              onChange={(e) => setSelectedFile(e.target.files?.[0] || null)}
              required
              className="h-9 flex-1 min-w-[200px]"
            />
            <Button type="submit" disabled={!selectedFile || uploading} className="h-9">
              {uploading ? "در حال آپلود..." : "آپلود و ایجاد"}
            </Button>
          </form>
          {uploadResult && (
            <div className="mt-3 text-sm">
              <span className="text-green-600">موفق: {uploadResult.success?.length || 0}</span>
              <span className="text-red-600 mr-3">ناموفق: {uploadResult.failed?.length || 0}</span>
            </div>
          )}
        </CardContent>
      </Card>

      {/* لیست کاربران */}
      <Card>
        <CardHeader className="pb-2">
          <CardTitle className="text-base">لیست کاربران</CardTitle>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead className="w-[50px]">#</TableHead>
                <TableHead>نام کامل</TableHead>
                <TableHead>نقش‌ها</TableHead>
                <TableHead>کد ملی</TableHead>
                <TableHead>وضعیت</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {users.map((user, index) => (
                <TableRow key={user.id}>
                  <TableCell>{index + 1}</TableCell>
                  <TableCell className="font-medium">{user.full_name}</TableCell>
                  <TableCell>
                    {user.roles?.map((r) => r.name).join("، ") || "بدون نقش"}
                  </TableCell>
                  <TableCell dir="ltr" className="text-left">{user.national_id}</TableCell>
                  <TableCell>
                    <span className={`px-2 py-0.5 rounded-full text-xs ${user.is_active ? "bg-green-100 text-green-700" : "bg-red-100 text-red-700"}`}>
                      {user.is_active ? "فعال" : "غیرفعال"}
                    </span>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}