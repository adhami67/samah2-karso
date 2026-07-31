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
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Pencil, Trash2 } from "lucide-react";

interface Permission {
  id: string;
  code: string;
  name: string;
}

interface Role {
  id: string;
  code: string;
  name: string;
  permissions: Permission[];
}

export default function Roles() {
  const [roles, setRoles] = useState<Role[]>([]);
  const [permissions, setPermissions] = useState<Permission[]>([]);
  const [newRole, setNewRole] = useState({
    code: "",
    name: "",
    permission_ids: [] as string[],
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // stateهای ویرایش
  const [editDialogOpen, setEditDialogOpen] = useState(false);
  const [editingRole, setEditingRole] = useState<Role | null>(null);
  const [editForm, setEditForm] = useState({
    name: "",
    permission_ids: [] as string[],
  });

  const fetchData = async () => {
    try {
      const [rolesData, permsData] = await Promise.all([
        api.get<Role[]>("/security/roles"),
        api.get<Permission[]>("/security/permissions"),
      ]);
      setRoles(rolesData || []);
      setPermissions(permsData || []);
      setError(null);
    } catch (err: any) {
      console.error(err);
      setError(err.message || "خطا در دریافت داده‌ها");
    } finally {
      setLoading(false);
    }
  };

  const handleCreateRole = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await api.post("/security/roles", newRole);
      setNewRole({ code: "", name: "", permission_ids: [] });
      fetchData();
    } catch (err: any) {
      alert(err.message);
    }
  };

  const openEditDialog = (role: Role) => {
    setEditingRole(role);
    setEditForm({
      name: role.name,
      permission_ids: role.permissions.map((p) => p.id),
    });
    setEditDialogOpen(true);
  };

  const handleEditRole = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!editingRole) return;
    try {
      await api.patch(`/security/roles/${editingRole.id}`, editForm);
      setEditDialogOpen(false);
      fetchData();
    } catch (err: any) {
      alert(err.message);
    }
  };

  const handleDeleteRole = async (roleId: string) => {
    if (!confirm("آیا از حذف این نقش اطمینان دارید؟")) return;
    try {
      await api.delete(`/security/roles/${roleId}`);
      fetchData();
    } catch (err: any) {
      alert(err.message);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  if (loading) return <div className="p-8 text-center">در حال بارگیری...</div>;
  if (error)
    return <div className="p-8 text-center text-red-500">خطا: {error}</div>;

  return (
    <div className="space-y-6">
      <h1 className="text-2xl font-bold">مدیریت نقش‌ها</h1>

      {/* فرم ایجاد نقش جدید */}
      <Card>
        <CardHeader>
          <CardTitle>ایجاد نقش جدید</CardTitle>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleCreateRole} className="space-y-4">
            <Input
              placeholder="کد نقش (مثلاً teacher)"
              value={newRole.code}
              onChange={(e) => setNewRole({ ...newRole, code: e.target.value })}
              required
            />
            <Input
              placeholder="نام نقش (مثلاً معلم)"
              value={newRole.name}
              onChange={(e) => setNewRole({ ...newRole, name: e.target.value })}
              required
            />
            <div>
              <label className="block text-sm font-medium mb-2">مجوزها</label>
              <div className="flex flex-wrap gap-3">
                {permissions.map((p) => (
                  <label
                    key={p.id}
                    className="flex items-center gap-1 text-sm"
                  >
                    <input
                      type="checkbox"
                      value={p.id}
                      checked={newRole.permission_ids.includes(p.id)}
                      onChange={(e) => {
                        const ids = e.target.checked
                          ? [...newRole.permission_ids, p.id]
                          : newRole.permission_ids.filter((id) => id !== p.id);
                        setNewRole({ ...newRole, permission_ids: ids });
                      }}
                    />
                    {p.name}
                  </label>
                ))}
              </div>
            </div>
            <Button type="submit">ایجاد نقش</Button>
          </form>
        </CardContent>
      </Card>

      {/* لیست نقش‌ها */}
      <Card>
        <CardHeader>
          <CardTitle>لیست نقش‌ها</CardTitle>
        </CardHeader>
        <CardContent>
          {roles.length === 0 ? (
            <p className="text-center text-slate-400">هیچ نقشی یافت نشد.</p>
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>کد</TableHead>
                  <TableHead>نام</TableHead>
                  <TableHead>مجوزها</TableHead>
                  <TableHead className="w-[100px]">عملیات</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {roles.map((role) => (
                  <TableRow key={role.id}>
                    <TableCell>{role.code}</TableCell>
                    <TableCell>{role.name}</TableCell>
                    <TableCell>
                      {role.permissions?.map((p) => p.name).join("، ") ||
                        "بدون مجوز"}
                    </TableCell>
                    <TableCell>
                      <div className="flex gap-1">
                        <Button
                          variant="ghost"
                          size="sm"
                          onClick={() => openEditDialog(role)}
                        >
                          <Pencil size={16} />
                        </Button>
                        <Button
                          variant="ghost"
                          size="sm"
                          className="text-red-500"
                          onClick={() => handleDeleteRole(role.id)}
                        >
                          <Trash2 size={16} />
                        </Button>
                      </div>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          )}
        </CardContent>
      </Card>

      {/* دیالوگ ویرایش */}
      <Dialog open={editDialogOpen} onOpenChange={setEditDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>ویرایش نقش</DialogTitle>
          </DialogHeader>
          <form onSubmit={handleEditRole} className="space-y-4">
            <Input
              placeholder="نام نقش"
              value={editForm.name}
              onChange={(e) =>
                setEditForm({ ...editForm, name: e.target.value })
              }
              required
            />
            <div>
              <label className="block text-sm font-medium mb-2">مجوزها</label>
              <div className="flex flex-wrap gap-3">
                {permissions.map((p) => (
                  <label
                    key={p.id}
                    className="flex items-center gap-1 text-sm"
                  >
                    <input
                      type="checkbox"
                      checked={editForm.permission_ids.includes(p.id)}
                      onChange={(e) => {
                        const newIds = e.target.checked
                          ? [...editForm.permission_ids, p.id]
                          : editForm.permission_ids.filter(
                              (id) => id !== p.id
                            );
                        setEditForm({ ...editForm, permission_ids: newIds });
                      }}
                      value={p.id}
                    />
                    {p.name}
                  </label>
                ))}
              </div>
            </div>
            <div className="flex justify-end gap-2">
              <Button type="button" variant="outline" onClick={() => setEditDialogOpen(false)}>
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