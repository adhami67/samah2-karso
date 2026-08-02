import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog";
import { Label } from "@/components/ui/label";
import { Pencil, Trash2, Plus } from "lucide-react";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "sonner";

interface Workspace {
  id: string;
  name: string;
  description: string | null;
  status: string;
  owner_user_id: string;
}

export default function Workspaces() {
  const { user } = useAuth();
  const [workspaces, setWorkspaces] = useState<Workspace[]>([]);
  const [loading, setLoading] = useState(true);
  const [newWorkspace, setNewWorkspace] = useState({ name: "", description: "" });
  const [editingWorkspace, setEditingWorkspace] = useState<Workspace | null>(null);
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);

  const fetchWorkspaces = async () => {
    try {
      const data = await api.get<Workspace[]>("/workspaces");
      setWorkspaces(data);
    } catch (error) {
      toast.error("مشکل در دریافت حوزه‌ها");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchWorkspaces();
  }, []);

  const handleCreate = async () => {
    try {
      await api.post("/workspaces", {
        name: newWorkspace.name,
        description: newWorkspace.description || null,
        type: "public",                         // مقدار پیش‌فرض
        owner_id: user?.id,                     // حتماً شناسه کاربر فعلی را بفرست
      });
      toast.success("حوزه جدید ایجاد شد");
      setIsCreateDialogOpen(false);
      setNewWorkspace({ name: "", description: "" });
      fetchWorkspaces();
    } catch (error: any) {
      toast.error(error.message || "مشکل در ایجاد حوزه");
    }
  };

  const handleUpdate = async () => {
    if (!editingWorkspace) return;
    try {
      await api.patch(`/workspaces/${editingWorkspace.id}`, {
        name: editingWorkspace.name,
        description: editingWorkspace.description,
      });
      toast.success("حوزه ویرایش شد");
      setIsEditDialogOpen(false);
      setEditingWorkspace(null);
      fetchWorkspaces();
    } catch (error: any) {
      toast.error(error.message || "مشکل در ویرایش حوزه");
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("آیا از حذف این حوزه اطمینان دارید؟")) return;
    try {
      await api.delete(`/workspaces/${id}`);
      toast.success("حوزه حذف شد");
      fetchWorkspaces();
    } catch (error: any) {
      toast.error(error.message || "مشکل در حذف حوزه");
    }
  };

  if (loading) return <div className="p-8 text-center">در حال بارگیری...</div>;

  return (
    <div className="space-y-6 p-6 max-w-6xl mx-auto">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">مدیریت حوزه‌های کاری</h1>
        <Button onClick={() => setIsCreateDialogOpen(true)} className="gap-2">
          <Plus className="h-4 w-4" /> حوزه جدید
        </Button>
      </div>

      {/* دیالوگ ایجاد */}
      <Dialog open={isCreateDialogOpen} onOpenChange={setIsCreateDialogOpen}>
        <DialogContent>
          <DialogHeader><DialogTitle>ایجاد حوزه جدید</DialogTitle></DialogHeader>
          <div className="space-y-4 py-4">
            <div className="space-y-2">
              <Label htmlFor="name">نام حوزه</Label>
              <Input
                id="name"
                value={newWorkspace.name}
                onChange={(e) => setNewWorkspace({ ...newWorkspace, name: e.target.value })}
                placeholder="مثلاً: دبستان"
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="desc">توضیحات</Label>
              <Input
                id="desc"
                value={newWorkspace.description}
                onChange={(e) => setNewWorkspace({ ...newWorkspace, description: e.target.value })}
                placeholder="توضیح اختیاری"
              />
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setIsCreateDialogOpen(false)}>انصراف</Button>
            <Button onClick={handleCreate}>ایجاد</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* دیالوگ ویرایش */}
      <Dialog open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
        <DialogContent>
          <DialogHeader><DialogTitle>ویرایش حوزه</DialogTitle></DialogHeader>
          {editingWorkspace && (
            <div className="space-y-4 py-4">
              <div className="space-y-2">
                <Label htmlFor="edit-name">نام حوزه</Label>
                <Input
                  id="edit-name"
                  value={editingWorkspace.name}
                  onChange={(e) => setEditingWorkspace({ ...editingWorkspace, name: e.target.value })}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="edit-desc">توضیحات</Label>
                <Input
                  id="edit-desc"
                  value={editingWorkspace.description || ""}
                  onChange={(e) => setEditingWorkspace({ ...editingWorkspace, description: e.target.value })}
                />
              </div>
            </div>
          )}
          <DialogFooter>
            <Button variant="outline" onClick={() => { setIsEditDialogOpen(false); setEditingWorkspace(null); }}>انصراف</Button>
            <Button onClick={handleUpdate}>ذخیره</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* جدول حوزه‌ها */}
      <Card>
        <CardHeader><CardTitle>لیست حوزه‌ها</CardTitle></CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>نام</TableHead>
                <TableHead>توضیحات</TableHead>
                <TableHead>وضعیت</TableHead>
                <TableHead className="text-center">عملیات</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {workspaces.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={4} className="text-center text-muted-foreground">
                    هیچ حوزه‌ای تعریف نشده است.
                  </TableCell>
                </TableRow>
              ) : (
                workspaces.map((ws) => (
                  <TableRow key={ws.id}>
                    <TableCell className="font-medium">{ws.name}</TableCell>
                    <TableCell>{ws.description || "-"}</TableCell>
                    <TableCell>
                      <span className={`px-2 py-0.5 rounded-full text-xs ${ws.status === "active" ? "bg-green-100 text-green-700" : "bg-gray-100 text-gray-500"}`}>
                        {ws.status === "active" ? "فعال" : "غیرفعال"}
                      </span>
                    </TableCell>
                    <TableCell>
                      <div className="flex justify-center gap-2">
                        <Button
                          variant="ghost"
                          size="icon"
                          onClick={() => { setEditingWorkspace(ws); setIsEditDialogOpen(true); }}
                        >
                          <Pencil className="h-4 w-4" />
                        </Button>
                        <Button
                          variant="ghost"
                          size="icon"
                          className="text-red-500"
                          onClick={() => handleDelete(ws.id)}
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
        </CardContent>
      </Card>
    </div>
  );
}