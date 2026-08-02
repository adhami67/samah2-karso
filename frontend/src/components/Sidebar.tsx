// frontend/src/components/Sidebar.tsx
import { useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "@/contexts/AuthContext";
import {
  LayoutDashboard,
  CheckSquare,
  Users,
  School,
  Award,
  Calendar,
  MessageSquare,
  BarChart3,
  FolderOpen,
  Settings,
  Shield,
  GraduationCap,
  Layers,
} from "lucide-react";
import { cn } from "@/lib/utils";

interface NavItem {
  label: string;
  icon: React.ReactNode;
  path: string;
  roles?: string[]; // نقش‌هایی که به این آیتم دسترسی دارند
}

const allNavItems: NavItem[] = [
  { label: "داشبورد", icon: <LayoutDashboard size={20} />, path: "/dashboard" },
  { label: "حوزه‌ها", icon: <Layers size={20} />, path: "/workspaces", roles: ["system_admin", "school_admin"] },
  { label: "فعالیت‌ها", icon: <CheckSquare size={20} />, path: "/activities" },
  { label: "اشخاص", icon: <Users size={20} />, path: "/people", roles: ["system_admin", "school_admin"] },
  { label: "آموزش", icon: <School size={20} />, path: "/education", roles: ["teacher", "school_admin"] },
  { label: "ارزشیابی", icon: <Award size={20} />, path: "/evaluations", roles: ["teacher", "school_admin"] },
  { label: "تقویم", icon: <Calendar size={20} />, path: "/calendar" },
  { label: "پیام‌ها", icon: <MessageSquare size={20} />, path: "/messages" },
  { label: "گزارش‌ها", icon: <BarChart3 size={20} />, path: "/reports", roles: ["system_admin", "school_admin", "assistant"] },
  { label: "مستندات", icon: <FolderOpen size={20} />, path: "/documents" },
  { label: "تنظیمات", icon: <Settings size={20} />, path: "/settings", roles: ["system_admin", "school_admin"] },
  { label: "مدیریت نقش‌ها", icon: <Shield size={20} />, path: "/roles", roles: ["system_admin"] },
  { label: "دانش‌آموزان", icon: <GraduationCap size={20} />, path: "/students", roles: ["system_admin", "school_admin"] },
];

export function Sidebar() {
  const navigate = useNavigate();
  const location = useLocation();
  const { user, hasAnyRole } = useAuth();

  // فیلتر آیتم‌ها بر اساس نقش کاربر
  const navItems = allNavItems.filter((item) => {
    if (!item.roles) return true;
    return hasAnyRole(item.roles);
  });

  return (
    <aside className="hidden md:flex flex-col w-64 h-screen sticky top-0 glass-card rounded-none rounded-r-2xl border-r border-slate-200/30 p-4">
      {/* لوگو */}
      <div
        className="flex items-center gap-2 cursor-pointer mb-6 px-2"
        onClick={() => navigate("/dashboard")}
      >
        <div className="bg-orange-500 p-2 rounded-xl shadow-lg shadow-orange-500/30">
          <CheckSquare className="text-white" size={22} />
        </div>
        <span className="text-xl font-bold bg-gradient-to-l from-orange-600 to-amber-500 bg-clip-text text-transparent">
          کارسو
        </span>
      </div>

      {/* منو */}
      <nav className="flex-1 space-y-1">
        {navItems.map((item) => (
          <button
            key={item.path}
            onClick={() => navigate(item.path)}
            className={cn(
              "w-full flex items-center gap-3 px-3 py-2.5 rounded-xl text-sm font-medium transition-all duration-200",
              location.pathname === item.path
                ? "bg-orange-50 text-orange-600 shadow-sm"
                : "text-slate-500 hover:bg-slate-50 hover:text-slate-700"
            )}
          >
            <span className="flex-shrink-0">{item.icon}</span>
            <span className="truncate text-right">{item.label}</span>
          </button>
        ))}
      </nav>

      {/* اطلاعات کاربر */}
      <div className="pt-4 border-t border-slate-200/50 px-2">
        <div className="text-sm font-medium text-slate-700 truncate">
          {user?.full_name || "کاربر"}
        </div>
        <div className="text-xs text-slate-400 truncate">
          {user?.roles?.join("، ") || "بدون نقش"}
        </div>
      </div>
    </aside>
  );
}