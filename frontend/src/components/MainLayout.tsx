import { Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "@/contexts/AuthContext";
import { Button } from "@/components/ui/button";
import { LogOut } from "lucide-react";
import { motion } from "framer-motion";
import { Sidebar } from "./Sidebar";
import { NotificationBell } from "./NotificationBell";

export default function MainLayout() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  return (
    <div className="min-h-screen flex bg-slate-50/50 relative">
      <Sidebar />

      <div className="flex-1 flex flex-col min-h-screen">
        <motion.header
          initial={{ opacity: 0, y: -10 }}
          animate={{ opacity: 1, y: 0 }}
          className="sticky top-0 z-20 bg-white/70 backdrop-blur-sm border-b border-slate-200/30 px-6 py-3 flex items-center justify-between"
        >
          <div className="flex items-center gap-2">
            <span className="text-sm text-slate-500 hidden sm:inline">
              خوش آمدید،
            </span>
            <span className="text-sm font-medium text-slate-700">
              {user?.full_name || "کاربر"}
            </span>
          </div>

          <div className="flex items-center gap-2">
            <NotificationBell />
            <Button
              variant="ghost"
              size="sm"
              onClick={logout}
              className="text-slate-500 hover:text-red-500 gap-1"
            >
              <LogOut size={18} />
              <span className="hidden sm:inline text-sm">خروج</span>
            </Button>
          </div>
        </motion.header>

        <motion.main
          initial={{ opacity: 0, y: 10 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.2 }}
          className="flex-1 p-6"
        >
          <Outlet />
        </motion.main>
      </div>
    </div>
  );
}