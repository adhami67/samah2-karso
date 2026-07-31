// frontend/src/pages/Users.tsx
import React, { useState, useEffect, useCallback } from 'react';
import { api } from '@/lib/api';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { Badge } from '@/components/ui/badge';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Checkbox } from '@/components/ui/checkbox';
import { Pencil, Trash2, Plus, Upload, FileDown, User, Users as UsersIcon, Search, Filter, ChevronLeft, ChevronRight, MoreHorizontal } from 'lucide-react';
import { Skeleton } from '@/components/ui/skeleton';
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from '@/components/ui/dropdown-menu';
import { toast } from 'sonner';
import { useAuth } from '@/contexts/AuthContext';

interface User {
  id: string;
  national_id: string;
  full_name: string;
  username: string;
  is_active: boolean;
  roles: { id: string; code: string; name: string }[];
  grade?: string;
  class_name?: string;
  parent_phone?: string;
  last_login?: string;
}

const roleOptions = [
  { value: 'student', label: 'دانش‌آموز' },
  { value: 'teacher', label: 'معلم' },
  { value: 'parent', label: 'والدین' },
  { value: 'admin', label: 'مدیر' },
];

export default function Users() {
  const { token } = useAuth();
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [roleFilter, setRoleFilter] = useState<string>('all');
  const [newUser, setNewUser] = useState({
    national_id: '',
    full_name: '',
    password: '',
    role: 'student',
  });
  const [editingUser, setEditingUser] = useState<User | null>(null);
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [uploading, setUploading] = useState(false);
  const [uploadResult, setUploadResult] = useState<any>(null);
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);

  const fetchUsers = useCallback(async () => {
    setLoading(true);
    try {
      const data = await api.get<User[]>('/security/users');
      setUsers(data);
    } catch (error) {
      toast.error('مشکل در دریافت کاربران');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

  const handleCreateUser = async () => {
    try {
      const roleMap: Record<string, string> = {
        student: 'role_student',
        teacher: 'role_teacher',
        parent: 'role_parent',
        admin: 'role_admin',
      };
      const roleId = roleMap[newUser.role] || 'role_student';
      await api.post('/security/users', {
        national_id: newUser.national_id,
        full_name: newUser.full_name,
        password: newUser.password,
        role_ids: [roleId],
      });
      toast.success('کاربر جدید ایجاد شد');
      setIsCreateDialogOpen(false);
      setNewUser({ national_id: '', full_name: '', password: '', role: 'student' });
      fetchUsers();
    } catch (error) {
      toast.error('مشکل در ایجاد کاربر');
    }
  };

  const handleUpdateUser = async () => {
    if (!editingUser) return;
    try {
      await api.patch(`/security/users/${editingUser.id}`, {
        full_name: editingUser.full_name,
        is_active: editingUser.is_active,
        grade: editingUser.grade,
        class_name: editingUser.class_name,
        parent_phone: editingUser.parent_phone,
      });
      toast.success('اطلاعات کاربر به‌روزرسانی شد');
      setIsEditDialogOpen(false);
      setEditingUser(null);
      fetchUsers();
    } catch (error) {
      toast.error('مشکل در ویرایش کاربر');
    }
  };

  const handleDeleteUser = async (userId: string) => {
    if (!confirm('آیا از حذف این کاربر اطمینان دارید؟')) return;
    try {
      await api.delete(`/security/users/${userId}`);
      toast.success('کاربر حذف شد');
      fetchUsers();
    } catch (error) {
      toast.error('مشکل در حذف کاربر');
    }
  };

  const handleBulkUpload = async () => {
    if (!selectedFile || !token) return;
    setUploading(true);
    const formData = new FormData();
    formData.append('file', selectedFile);
    try {
        const response = await fetch('http://127.0.0.1:8000/api/bulk/users', {
          method: 'POST',
        headers: {
          Authorization: `Bearer ${token}`,
        },
        body: formData,
      });
      if (!response.ok) {
        const errorData = await response.json().catch(() => ({ detail: 'خطا در آپلود' }));
        throw new Error(errorData.detail || 'خطا در آپلود فایل');
      }
      const result = await response.json();
      setUploadResult(result);
      toast.success(`${result.success?.length || 0} کاربر با موفقیت ایجاد شد.`);
      fetchUsers();
    } catch (err: any) {
      toast.error(err.message || 'مشکل در آپلود فایل');
    } finally {
      setUploading(false);
      setSelectedFile(null);
    }
  };

  const openEditDialog = (user: User) => {
    setEditingUser(user);
    setIsEditDialogOpen(true);
  };

  const filteredUsers = users.filter(user => {
    const matchesSearch = user.full_name.includes(searchTerm) || user.national_id.includes(searchTerm);
    const matchesRole = roleFilter === 'all' || user.roles.some(r => r.code === roleFilter);
    return matchesSearch && matchesRole;
  });

  if (loading) {
    return <SkeletonLoader />;
  }

  return (
    <div className="space-y-8 p-6 max-w-7xl mx-auto">
      {/* هدر صفحه */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">مدیریت کاربران</h1>
          <p className="text-sm text-muted-foreground">
            کاربران سیستم را مدیریت کنید و دسترسی‌های آنها را تنظیم نمایید
          </p>
        </div>
        <Dialog open={isCreateDialogOpen} onOpenChange={setIsCreateDialogOpen}>
          <DialogTrigger asChild>
            <Button className="gap-2">
              <Plus className="h-4 w-4" />
              کاربر جدید
            </Button>
          </DialogTrigger>
          <DialogContent className="sm:max-w-[500px]">
            <DialogHeader>
              <DialogTitle>ایجاد کاربر جدید</DialogTitle>
              <DialogDescription>
                اطلاعات کاربر جدید را وارد کنید. نقش مشخص‌کننده دسترسی‌های اوست.
              </DialogDescription>
            </DialogHeader>
            <div className="space-y-4 py-4">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="user-national-id">کد ملی</Label>
                  <Input
                    id="user-national-id"
                    placeholder="کد ملی"
                    value={newUser.national_id}
                    onChange={(e) => setNewUser({ ...newUser, national_id: e.target.value })}
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="user-full-name">نام کامل</Label>
                  <Input
                    id="user-full-name"
                    placeholder="نام و نام خانوادگی"
                    value={newUser.full_name}
                    onChange={(e) => setNewUser({ ...newUser, full_name: e.target.value })}
                  />
                </div>
              </div>
              <div className="space-y-2">
                <Label htmlFor="user-password">رمز عبور</Label>
                <Input
                  id="user-password"
                  type="password"
                  placeholder="رمز عبور"
                  value={newUser.password}
                  onChange={(e) => setNewUser({ ...newUser, password: e.target.value })}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="user-role">نقش</Label>
                <Select
                  value={newUser.role}
                  onValueChange={(value) => setNewUser({ ...newUser, role: value })}
                >
                  <SelectTrigger id="user-role">
                    <SelectValue placeholder="انتخاب نقش" />
                  </SelectTrigger>
                  <SelectContent>
                    {roleOptions.map((role) => (
                      <SelectItem key={role.value} value={role.value}>
                        {role.label}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>
            </div>
            <DialogFooter>
              <Button variant="outline" onClick={() => setIsCreateDialogOpen(false)}>انصراف</Button>
              <Button onClick={handleCreateUser}>ایجاد کاربر</Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>

      {/* کارت آپلود انبوه */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Upload className="h-5 w-5 text-muted-foreground" />
            آپلود انبوه کاربران
          </CardTitle>
          <CardDescription>
            با آپلود یک فایل اکسل، می‌توانید چندین کاربر را به‌طور همزمان ایجاد کنید.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="flex flex-col sm:flex-row items-start sm:items-center gap-4">
            <div className="flex-1 w-full">
              <Label htmlFor="file-upload" className="cursor-pointer">
                <div className="flex items-center justify-center w-full h-24 border-2 border-dashed rounded-lg border-muted-foreground/20 hover:border-primary/50 transition-colors">
                  <div className="flex flex-col items-center gap-1">
                    <FileDown className="h-8 w-8 text-muted-foreground" />
                    <span className="text-sm text-muted-foreground">
                      {selectedFile ? selectedFile.name : 'فایل اکسل خود را انتخاب کنید'}
                    </span>
                    <span className="text-xs text-muted-foreground/70">.xlsx یا .xls</span>
                  </div>
                </div>
                <Input
                  id="file-upload"
                  type="file"
                  accept=".xlsx,.xls"
                  className="hidden"
                  onChange={(e) => setSelectedFile(e.target.files?.[0] || null)}
                />
              </Label>
            </div>
            <div className="flex gap-2">
              <Button variant="outline" className="gap-2">
                <FileDown className="h-4 w-4" />
                دانلود نمونه
              </Button>
              <Button onClick={handleBulkUpload} disabled={!selectedFile || uploading}>
                {uploading ? 'در حال آپلود...' : 'آپلود و ایجاد'}
              </Button>
            </div>
          </div>
          {uploadResult && (
            <div className="mt-4 text-sm">
              <span className="text-green-600">موفق: {uploadResult.success?.length || 0}</span>
              <span className="text-red-600 mr-3">ناموفق: {uploadResult.failed?.length || 0}</span>
            </div>
          )}
        </CardContent>
      </Card>

      {/* جستجو و فیلتر */}
      <div className="flex flex-col sm:flex-row gap-4">
        <div className="flex-1 relative">
          <Search className="absolute right-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input
            placeholder="جستجو در نام یا کد ملی..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="pr-9"
          />
        </div>
        <Select value={roleFilter} onValueChange={setRoleFilter}>
          <SelectTrigger className="w-[180px]">
            <div className="flex items-center gap-2">
              <Filter className="h-4 w-4" />
              <SelectValue placeholder="فیلتر نقش" />
            </div>
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="all">همه نقش‌ها</SelectItem>
            {roleOptions.map((role) => (
              <SelectItem key={role.value} value={role.value}>
                {role.label}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>

      {/* جدول کاربران */}
      <Card>
        <CardHeader>
          <CardTitle>لیست کاربران</CardTitle>
          <CardDescription>
            {filteredUsers.length} کاربر یافت شد
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="rounded-md border overflow-x-auto">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead className="w-[50px]">#</TableHead>
                  <TableHead>نام کامل</TableHead>
                  <TableHead>نقش</TableHead>
                  <TableHead>کد ملی</TableHead>
                  <TableHead>وضعیت</TableHead>
                  <TableHead className="w-[120px] text-center">عملیات</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredUsers.length === 0 ? (
                  <TableRow>
                    <TableCell colSpan={6} className="h-24 text-center">
                      <div className="flex flex-col items-center justify-center text-muted-foreground">
                        <UsersIcon className="h-8 w-8 mb-2 opacity-20" />
                        <p>هیچ کاربری یافت نشد</p>
                        <p className="text-sm">با جستجو یا ایجاد کاربر جدید شروع کنید.</p>
                      </div>
                    </TableCell>
                  </TableRow>
                ) : (
                  filteredUsers.map((user, index) => (
                    <TableRow key={user.id}>
                      <TableCell>{index + 1}</TableCell>
                      <TableCell>
                        <div className="flex items-center gap-3">
                          <Avatar className="h-8 w-8">
                            <AvatarFallback className="bg-primary/10 text-primary text-xs">
                              {user.full_name.split(' ').map(n => n[0]).join('').substring(0, 2)}
                            </AvatarFallback>
                          </Avatar>
                          <span className="font-medium">{user.full_name}</span>
                        </div>
                      </TableCell>
                      <TableCell>
                        <div className="flex flex-wrap gap-1">
                          {user.roles.map((role) => (
                            <Badge key={role.id} variant="outline" className="text-xs">
                              {role.name}
                            </Badge>
                          ))}
                          {user.roles.length === 0 && (
                            <span className="text-xs text-muted-foreground">بدون نقش</span>
                          )}
                        </div>
                      </TableCell>
                      <TableCell dir="ltr" className="text-left font-mono text-sm">
                        {user.national_id}
                      </TableCell>
                      <TableCell>
                        <Badge
                          variant={user.is_active ? 'default' : 'secondary'}
                          className="text-xs"
                        >
                          {user.is_active ? 'فعال' : 'غیرفعال'}
                        </Badge>
                      </TableCell>
                      <TableCell>
                        <DropdownMenu>
                          <DropdownMenuTrigger asChild>
                            <Button variant="ghost" size="icon" className="h-8 w-8">
                              <span className="sr-only">باز کردن منو</span>
                              <MoreHorizontal className="h-4 w-4" />
                            </Button>
                          </DropdownMenuTrigger>
                          <DropdownMenuContent align="end">
                            <DropdownMenuItem onClick={() => openEditDialog(user)}>
                              <Pencil className="ml-2 h-4 w-4" />
                              ویرایش
                            </DropdownMenuItem>
                            <DropdownMenuItem
                              className="text-destructive"
                              onClick={() => handleDeleteUser(user.id)}
                            >
                              <Trash2 className="ml-2 h-4 w-4" />
                              حذف
                            </DropdownMenuItem>
                          </DropdownMenuContent>
                        </DropdownMenu>
                      </TableCell>
                    </TableRow>
                  ))
                )}
              </TableBody>
            </Table>
          </div>
        </CardContent>
        <CardFooter className="border-t px-6 py-4">
          <div className="flex items-center justify-between w-full">
            <p className="text-sm text-muted-foreground">
              نمایش {filteredUsers.length} کاربر
            </p>
            <div className="flex items-center gap-2">
              <Button variant="outline" size="sm" disabled>
                <ChevronRight className="h-4 w-4" />
              </Button>
              <Button variant="outline" size="sm" disabled>
                <ChevronLeft className="h-4 w-4" />
              </Button>
            </div>
          </div>
        </CardFooter>
      </Card>

      {/* دیالوگ ویرایش کاربر */}
      <Dialog open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
        <DialogContent className="sm:max-w-[500px]">
          <DialogHeader>
            <DialogTitle>ویرایش کاربر</DialogTitle>
            <DialogDescription>
              اطلاعات کاربر را به‌روزرسانی کنید.
            </DialogDescription>
          </DialogHeader>
          {editingUser && (
            <div className="space-y-4 py-4">
              <div className="space-y-2">
                <Label htmlFor="edit-user-full-name">نام کامل</Label>
                <Input
                  id="edit-user-full-name"
                  value={editingUser.full_name}
                  onChange={(e) => setEditingUser({ ...editingUser, full_name: e.target.value })}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="edit-user-grade">پایه (برای دانش‌آموز)</Label>
                <Input
                  id="edit-user-grade"
                  placeholder="مثلاً: اول"
                  value={editingUser.grade || ''}
                  onChange={(e) => setEditingUser({ ...editingUser, grade: e.target.value })}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="edit-user-class">کلاس (برای دانش‌آموز)</Label>
                <Input
                  id="edit-user-class"
                  placeholder="مثلاً: الف"
                  value={editingUser.class_name || ''}
                  onChange={(e) => setEditingUser({ ...editingUser, class_name: e.target.value })}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="edit-user-parent-phone">شماره والدین (برای دانش‌آموز)</Label>
                <Input
                  id="edit-user-parent-phone"
                  placeholder="شماره تماس والدین"
                  value={editingUser.parent_phone || ''}
                  onChange={(e) => setEditingUser({ ...editingUser, parent_phone: e.target.value })}
                />
              </div>
              <div className="flex items-center space-x-2 rtl:space-x-reverse">
                <Checkbox
                  id="edit-user-active"
                  checked={editingUser.is_active}
                  onCheckedChange={(checked: boolean) => setEditingUser({ ...editingUser, is_active: checked })}
                />
                <Label htmlFor="edit-user-active">فعال</Label>
              </div>
            </div>
          )}
          <DialogFooter>
            <Button variant="outline" onClick={() => setIsEditDialogOpen(false)}>انصراف</Button>
            <Button onClick={handleUpdateUser}>ذخیره تغییرات</Button>
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
          <div className="flex flex-col sm:flex-row gap-4">
            <Skeleton className="h-10 flex-1" />
            <Skeleton className="h-10 w-[180px]" />
          </div>
          <div className="rounded-md border mt-4">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead><Skeleton className="h-4 w-8" /></TableHead>
                  <TableHead><Skeleton className="h-4 w-24" /></TableHead>
                  <TableHead><Skeleton className="h-4 w-24" /></TableHead>
                  <TableHead><Skeleton className="h-4 w-24" /></TableHead>
                  <TableHead><Skeleton className="h-4 w-16" /></TableHead>
                  <TableHead><Skeleton className="h-4 w-16" /></TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {Array.from({ length: 5 }).map((_, i) => (
                  <TableRow key={i}>
                    <TableCell><Skeleton className="h-4 w-8" /></TableCell>
                    <TableCell>
                      <div className="flex items-center gap-3">
                        <Skeleton className="h-8 w-8 rounded-full" />
                        <Skeleton className="h-4 w-32" />
                      </div>
                    </TableCell>
                    <TableCell><Skeleton className="h-5 w-16" /></TableCell>
                    <TableCell><Skeleton className="h-4 w-24" /></TableCell>
                    <TableCell><Skeleton className="h-5 w-12" /></TableCell>
                    <TableCell><Skeleton className="h-8 w-8 rounded-full" /></TableCell>
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