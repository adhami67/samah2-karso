// frontend/src/pages/Roles.tsx
import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";

interface Permission { id: string; code: string; name: string; }
interface Role { id: string; code: string; name: string; permissions: Permission[]; }

export default function Roles() {
  const [roles, setRoles] = useState<Role[]>([]);
  const [permissions, setPermissions] = useState<Permission[]>([]);
  const [newRole, setNewRole] = useState({ code: "", name: "", permission_ids: [] as string[] });
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
    try {
      const [rolesData, permsData] = await Promise.all([
        api.get<Role[]>("/security/roles"),
        api.get<Permission[]>("/security/permissions"),
      ]);
      setRoles(rolesData);
      setPermissions(permsData);
    } catch (err) { console.error(err); } finally { setLoading(false); }
  };

  const handleCreateRole = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await api.post("/security/roles", newRole);
      setNewRole({ code: "", name: "", permission_ids: [] });
      fetchData();
    } catch (err: any) { alert(err.message); }
  };

  useEffect(() => { fetchData(); }, []);

  if (loading) return <div>در حال بارگیری...</div>;

  return (
    <div className="space-y-6">
      <h1 className="text-2xl font-bold">مدیریت نقش‌ها</h1>
      <Card>
        <CardHeader><CardTitle>ایجاد نقش جدید</CardTitle></CardHeader>
        <CardContent>
          <form onSubmit={handleCreateRole} className="space-y-4">
            <Input placeholder="کد نقش (مثلاً teacher)" value={newRole.code} onChange={(e) => setNewRole({...newRole, code: e.target.value})} required />
            <Input placeholder="نام نقش (مثلاً معلم)" value={newRole.name} onChange={(e) => setNewRole({...newRole, name: e.target.value})} required />
            <div>
              <label className="block text-sm font-medium mb-2">مجوزها</label>
              <div className="flex flex-wrap gap-3">
                {permissions.map(p => (
                  <label key={p.id} className="flex items-center gap-1 text-sm">
                    <input type="checkbox" value={p.id} checked={newRole.permission_ids.includes(p.id)} onChange={(e) => {
                      const ids = e.target.checked ? [...newRole.permission_ids, p.id] : newRole.permission_ids.filter(id => id !== p.id);
                      setNewRole({...newRole, permission_ids: ids});
                    }} />
                    {p.name}
                  </label>
                ))}
              </div>
            </div>
            <Button type="submit">ایجاد نقش</Button>
          </form>
        </CardContent>
      </Card>
      <Card>
        <CardHeader><CardTitle>لیست نقش‌ها</CardTitle></CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow><TableHead>کد</TableHead><TableHead>نام</TableHead><TableHead>مجوزها</TableHead></TableRow>
            </TableHeader>
            <TableBody>
              {roles.map(role => (
                <TableRow key={role.id}>
                  <TableCell>{role.code}</TableCell>
                  <TableCell>{role.name}</TableCell>
                  <TableCell>{role.permissions.map(p => p.name).join("، ")}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}