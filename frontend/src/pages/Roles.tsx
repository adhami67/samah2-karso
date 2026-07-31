import React, { useState, useEffect } from 'react';
import { api } from '@/lib/api';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { Badge } from '@/components/ui/badge';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Checkbox } from '@/components/ui/checkbox';
import { Pencil, Trash2, Plus, Shield, Key, Users, LayoutDashboard, FileText, Settings, BarChart3 } from 'lucide-react';
import { Skeleton } from '@/components/ui/skeleton';
import { toast } from 'sonner';

interface Permission {
  id: string;
  code: string;
  name: string;
  group?: string;
}

interface Role {
  id: string;
  code: string;
  name: string;
  permissions: Permission[];
}

const permissionGroups = [
  { id: 'dashboard', label: 'داشبورد', icon: LayoutDashboard },
  { id: 'activities', label: 'فعالیت‌ها', icon: FileText },
  { id: 'users', label: 'کاربران', icon: Users },
  { id: 'system', label: 'سیستم', icon: Settings },
  { id: 'reports', label: 'گزارش‌ها', icon: BarChart3 },
];

export default function Roles() {
  const [roles, setRoles] = useState<Role[]>([]);
  const [permissions, setPermissions] = useState<Permission[]>([]);
  const [loading, setLoading] = useState(true);
  const [newRole, setNewRole] = useState({ code: '', name: '', permission_ids: [] as string[] });
  const [editingRole, setEditingRole] = useState<Role | null>(null);
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);

  const fetchData = async () => {
    setLoading(true);
    try {
      const [rolesRes, permsRes] = await Promise.all([
        api.get<Role[]>('/security/roles'),
        api.get<Permission[]>('/security/permissions'),
      ]);
      setRoles(rolesRes);
      setPermissions(permsRes);
    } catch (error) {
      toast.error('مشکل در دریافت اطلاعات');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleCreateRole = async () => {
    try {
      await api.post('/security/roles', newRole);
      toast.success('نقش جدید ایجاد شد');
      setIsCreateDialogOpen(false);
      setNewRole({ code: '', name: '', permission_ids: [] });
      fetchData();
    } catch (error) {
      toast.error('مشکل در ایجاد نقش');
    }
  };

  const handleUpdateRole = async () => {
    if (!editingRole) return;
    try {
      await api.patch(`/security/roles/${editingRole.id}`, {
        name: editingRole.name,
        permission_ids: editingRole.permissions.map(p => p.id),
      });
      toast.success('نقش ویرایش شد');
      setIsEditDialogOpen(false);
      setEditingRole(null);
      fetchData();
    } catch (error) {
      toast.error('مشکل در ویرایش نقش');
    }
  };

  const handleDeleteRole = async (roleId: string, roleCode: string) => {
    if (['system_admin', 'school_admin', 'teacher', 'student', 'parent'].includes(roleCode)) {
      toast.error('نقش‌های سیستمی قابل حذف نیستند');
      return;
    }
    if (!confirm('آیا از حذف این نقش اطمینان دارید؟')) return;
    try {
      await api.delete(`/security/roles/${roleId}`);
      toast.success('نقش حذف شد');
      fetchData();
    } catch (error) {
      toast.error('مشکل در حذف نقش');
    }
  };

  const openEditDialog = (role: Role) => {
    setEditingRole(role);
    setIsEditDialogOpen(true);
  };

  const togglePermission = (permId: string) => {
    if (editingRole) {
      const currentPerms = editingRole.permissions.map(p => p.id);
      const newPerms = currentPerms.includes(permId)
        ? currentPerms.filter(id => id !== permId)
        : [...currentPerms, permId];
      const updatedPerms = permissions.filter(p => newPerms.includes(p.id));
      setEditingRole({ ...editingRole, permissions: updatedPerms });
    } else {
      setNewRole(prev => ({
        ...prev,
        permission_ids: prev.permission_ids.includes(permId)
          ? prev.permission_ids.filter(id => id !== permId)
          : [...prev.permission_ids, permId],
      }));
    }
  };

  const getPermissionsByGroup = (perms: Permission[]) => {
    const groups = permissionGroups.map(group => ({
      ...group,
      permissions: perms.filter(p => p.group === group.id),
    }));
    return groups;
  };

  const allPermissions = getPermissionsByGroup(permissions);

  if (loading) {
    return <SkeletonLoader />;
  }

  return (
    <div className="space-y-8 p-6 max-w-7xl mx-auto">
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">مدیریت نقش‌ها</h1>
          <p className="text-sm text-muted-foreground">
            نقش‌ها و سطح دسترسی کاربران را مدیریت کنید
          </p>
        </div>
        <Dialog open={isCreateDialogOpen} onOpenChange={setIsCreateDialogOpen}>
          <DialogTrigger asChild>
            <Button className="gap-2">
              <Plus className="h-4 w-4" />
              نقش جدید
            </Button>
          </DialogTrigger>
          <DialogContent className="sm:max-w-[600px] max-h-[90vh] overflow-y-auto">
            <DialogHeader>
              <DialogTitle>ایجاد نقش جدید</DialogTitle>
              <DialogDescription>
                اطلاعات نقش جدید را وارد کنید. مجوزهای مورد نیاز را انتخاب کنید.
              </DialogDescription>
            </DialogHeader>
            <div className="space-y-4 py-4">
              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="role-code">کد نقش</Label>
                  <Input
                    id="role-code"
                    placeholder="مثلاً: teacher"
                    value={newRole.code}
                    onChange={(e) => setNewRole({ ...newRole, code: e.target.value })}
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="role-name">نام نقش</Label>
                  <Input
                    id="role-name"
                    placeholder="مثلاً: معلم"
                    value={newRole.name}
                    onChange={(e) => setNewRole({ ...newRole, name: e.target.value })}
                  />
                </div>
              </div>
              <div className="space-y-3">
                <Label>مجوزها</Label>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  {allPermissions.map((group) => (
                    <Card key={group.id} className="border border-border/50">
                      <CardHeader className="py-2 px-3">
                        <CardTitle className="text-sm flex items-center gap-2">
                          <group.icon className="h-4 w-4 text-muted-foreground" />
                          {group.label}
                        </CardTitle>
                      </CardHeader>
                      <CardContent className="py-2 px-3 grid grid-cols-1 gap-1">
                        {group.permissions.length > 0 ? (
                          group.permissions.map((perm) => (
                            <div key={perm.id} className="flex items-center space-x-2 rtl:space-x-reverse">
                              <Checkbox
                                id={`new-${perm.id}`}
                                checked={newRole.permission_ids.includes(perm.id)}
                                onCheckedChange={() => togglePermission(perm.id)}
                              />
                              <Label htmlFor={`new-${perm.id}`} className="text-sm cursor-pointer">
                                {perm.name}
                              </Label>
                            </div>
                          ))
                        ) : (
                          <span className="text-xs text-muted-foreground">بدون مجوز</span>
                        )}
                      </CardContent>
                    </Card>
                  ))}
                </div>
              </div>
            </div>
            <DialogFooter>
              <Button variant="outline" onClick={() => setIsCreateDialogOpen(false)}>انصراف</Button>
              <Button onClick={handleCreateRole}>ایجاد نقش</Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>نقش‌های تعریف‌شده</CardTitle>
          <CardDescription>
            در این بخش می‌توانید نقش‌های موجود را مشاهده، ویرایش یا حذف کنید.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="rounded-md border">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead className="w-[100px]">کد</TableHead>
                  <TableHead>نام</TableHead>
                  <TableHead>مجوزها</TableHead>
                  <TableHead className="w-[120px] text-center">عملیات</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {roles.length === 0 ? (
                  <TableRow>
                    <TableCell colSpan={4} className="h-24 text-center">
                      <div className="flex flex-col items-center justify-center text-muted-foreground">
                        <Shield className="h-8 w-8 mb-2 opacity-20" />
                        <p>هیچ نقشی تعریف نشده است</p>
                        <p className="text-sm">برای شروع، یک نقش جدید ایجاد کنید.</p>
                      </div>
                    </TableCell>
                  </TableRow>
                ) : (
                  roles.map((role) => (
                    <TableRow key={role.id}>
                      <TableCell className="font-mono text-xs">{role.code}</TableCell>
                      <TableCell className="font-medium">{role.name}</TableCell>
                      <TableCell>
                        <div className="flex flex-wrap gap-1">
                          {role.permissions.length > 0 ? (
                            role.permissions.slice(0, 3).map((perm) => (
                              <Badge key={perm.id} variant="secondary" className="text-xs">
                                {perm.name}
                              </Badge>
                            ))
                          ) : (
                            <span className="text-xs text-muted-foreground">بدون مجوز</span>
                          )}
                          {role.permissions.length > 3 && (
                            <Badge variant="outline" className="text-xs">
                              +{role.permissions.length - 3} بیشتر
                            </Badge>
                          )}
                        </div>
                      </TableCell>
                      <TableCell>
                        <div className="flex items-center justify-center gap-2">
                          <Button
                            variant="ghost"
                            size="icon"
                            className="h-8 w-8 text-muted-foreground hover:text-primary"
                            onClick={() => openEditDialog(role)}
                          >
                            <Pencil className="h-4 w-4" />
                          </Button>
                          <Button
                            variant="ghost"
                            size="icon"
                            className="h-8 w-8 text-muted-foreground hover:text-destructive"
                            onClick={() => handleDeleteRole(role.id, role.code)}
                          >
                            <Trash2 className="h-4 w-4" />
                          </Button>
                        </div>
                      </TableCell>
                    </TableRow>
                  ))
                )}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>

      <Dialog open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
        <DialogContent className="sm:max-w-[600px] max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>ویرایش نقش</DialogTitle>
            <DialogDescription>
              اطلاعات نقش را تغییر دهید و مجوزهای آن را به‌روزرسانی کنید.
            </DialogDescription>
          </DialogHeader>
          {editingRole && (
            <div className="space-y-4 py-4">
              <div className="space-y-2">
                <Label htmlFor="edit-role-name">نام نقش</Label>
                <Input
                  id="edit-role-name"
                  value={editingRole.name}
                  onChange={(e) => setEditingRole({ ...editingRole, name: e.target.value })}
                />
              </div>
              <div className="space-y-3">
                <Label>مجوزها</Label>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  {allPermissions.map((group) => (
                    <Card key={group.id} className="border border-border/50">
                      <CardHeader className="py-2 px-3">
                        <CardTitle className="text-sm flex items-center gap-2">
                          <group.icon className="h-4 w-4 text-muted-foreground" />
                          {group.label}
                        </CardTitle>
                      </CardHeader>
                      <CardContent className="py-2 px-3 grid grid-cols-1 gap-1">
                        {group.permissions.length > 0 ? (
                          group.permissions.map((perm) => {
                            const isChecked = editingRole.permissions.some(p => p.id === perm.id);
                            return (
                              <div key={perm.id} className="flex items-center space-x-2 rtl:space-x-reverse">
                                <Checkbox
                                  id={`edit-${perm.id}`}
                                  checked={isChecked}
                                  onCheckedChange={() => togglePermission(perm.id)}
                                />
                                <Label htmlFor={`edit-${perm.id}`} className="text-sm cursor-pointer">
                                  {perm.name}
                                </Label>
                              </div>
                            );
                          })
                        ) : (
                          <span className="text-xs text-muted-foreground">بدون مجوز</span>
                        )}
                      </CardContent>
                    </Card>
                  ))}
                </div>
              </div>
            </div>
          )}
          <DialogFooter>
            <Button variant="outline" onClick={() => setIsEditDialogOpen(false)}>انصراف</Button>
            <Button onClick={handleUpdateRole}>ذخیره تغییرات</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}

function SkeletonLoader() {
  return (
    <div className="space-y-6 p-6 max-w-7xl mx-auto">
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <div>
          <Skeleton className="h-8 w-48" />
          <Skeleton className="h-4 w-72 mt-2" />
        </div>
        <Skeleton className="h-10 w-32" />
      </div>
      <Card>
        <CardHeader>
          <Skeleton className="h-6 w-40" />
          <Skeleton className="h-4 w-60" />
        </CardHeader>
        <CardContent>
          <div className="rounded-md border">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead><Skeleton className="h-4 w-16" /></TableHead>
                  <TableHead><Skeleton className="h-4 w-16" /></TableHead>
                  <TableHead><Skeleton className="h-4 w-32" /></TableHead>
                  <TableHead><Skeleton className="h-4 w-16" /></TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {Array.from({ length: 3 }).map((_, i) => (
                  <TableRow key={i}>
                    <TableCell><Skeleton className="h-4 w-16" /></TableCell>
                    <TableCell><Skeleton className="h-4 w-24" /></TableCell>
                    <TableCell>
                      <div className="flex gap-1">
                        <Skeleton className="h-5 w-16" />
                        <Skeleton className="h-5 w-16" />
                      </div>
                    </TableCell>
                    <TableCell>
                      <div className="flex justify-center gap-2">
                        <Skeleton className="h-8 w-8 rounded-full" />
                        <Skeleton className="h-8 w-8 rounded-full" />
                      </div>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}