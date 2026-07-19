import { Outlet, useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "@/contexts/AuthContext";
import { Button } from "@/components/ui/button";
import ParticlesBackground from "@/components/ParticlesBackground";
import {
  LayoutDashboard,
  FileText,
  LogOut,
  User,
} from "lucide-react";
import { motion } from "framer-motion";

export default function MainLayout() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const navItems = [
    {
      label: "داشبورد",
      icon: <LayoutDashboard size={20} />,
      path: "/dashboard",
    },
    {
      label: "فعالیت‌ها",
      icon: <FileText size={20} />,
      path: "/activities",
    },
  ];

  return (
    <div className="min-h-screen flex flex-col relative">
      {/* ذرات پس‌زمینه */}
      <ParticlesBackground />

      {/* هدر شیشه‌ای */}
      <motion.header
        initial={{ opacity: 0, y: -20 }}
        animate={{ opacity: 1, y: 0 }}
        className="glass-card sticky top-3 z-50 mx-3 mt-3 px-4 py-3 flex items-center justify-between"
      >
        <div className="flex items-center gap-2 cursor-pointer" onClick={() => navigate("/dashboard")}>
          <div className="bg-orange-500 p-2 rounded-xl shadow-lg shadow-orange-500/30">
            <LayoutDashboard className="text-white" size={22} />
          </div>
          <span className="text-xl font-bold bg-gradient-to-l from-orange-600 to-amber-500 bg-clip-text text-transparent">
            کارسو
          </span>
        </div>
        <div className="flex items-center gap-3">
          <div className="hidden sm:flex items-center gap-1 text-slate-600 text-sm bg-white/50 py-1 px-3 rounded-full">
            <User size={16} className="text-orange-500" />
            <span>{user?.full_name}</span>
          </div>
          <Button variant="ghost" size="sm" onClick={logout} className="text-slate-500">
            <LogOut size={18} />
          </Button>
        </div>
      </motion.header>

      {/* محتوای اصلی */}
      <motion.main
        key={location.pathname}
        initial={{ opacity: 0, y: 15 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.2 }}
        className="flex-1 p-3 relative z-10"
      >
        <Outlet />
      </motion.main>

      {/* منوی پایین (موبایل) */}
      <nav className="sticky bottom-3 mx-3 mb-3 glass-card py-2 px-4 flex justify-around md:hidden z-10">
        {navItems.map((item) => (
          <button
            key={item.path}
            onClick={() => navigate(item.path)}
            className={`flex flex-col items-center gap-0.5 p-2 rounded-xl transition-colors ${
              location.pathname === item.path
                ? "bg-orange-100 text-orange-600"
                : "text-slate-400 hover:text-slate-600"
            }`}
          >
            {item.icon}
            <span className="text-[10px] font-medium">{item.label}</span>
          </button>
        ))}
      </nav>
    </div>
  );
}