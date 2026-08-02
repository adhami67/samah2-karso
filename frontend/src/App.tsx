// frontend/src/App.tsx
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider, useAuth } from "@/contexts/AuthContext";
import MainLayout from "@/components/MainLayout";
import Login from "@/pages/Login";
import Dashboard from "@/pages/Dashboard";
import Activities from "@/pages/Activities";
import NewActivity from "@/pages/NewActivity";
import ActivityDetail from "@/pages/ActivityDetail";
import Users from "@/pages/Users";  // اضافه کنید
import Roles from "@/pages/Roles";
import StudentsGrouped from "@/pages/StudentsGrouped";
import { Toaster } from "sonner"; // اضافه کنید
import Workspaces from "@/pages/Workspaces";
import Reports from "@/pages/Reports";




function ProtectedRoute({ children }: { children: React.ReactNode }) {
  const { token } = useAuth();
  if (!token) return <Navigate to="/login" replace />;
  return <>{children}</>;
}

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Toaster position="top-center" richColors />   {/* ← این خط را اضافه کنید */}
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route
            element={
              <ProtectedRoute>
                <MainLayout />
              </ProtectedRoute>
            }
          >
            <Route path="/dashboard" element={<Dashboard />} />
            <Route path="/activities" element={<Activities />} />
            <Route path="/activities/new" element={<NewActivity />} />
            <Route path="/activities/:id" element={<ActivityDetail />} />
            <Route path="/people" element={<Users />} />
            <Route path="/roles" element={<Roles />} />
            <Route path="/students" element={<StudentsGrouped />} />
            <Route path="/workspaces" element={<Workspaces />} />
            <Route path="/reports" element={<Reports />} />

            {/* مسیرهای جدید برای آینده */}
            <Route path="/people" element={<div>صفحه اشخاص</div>} />
            <Route path="/calendar" element={<div>صفحه تقویم</div>} />
            <Route path="/messages" element={<div>صفحه پیام‌ها</div>} />
          </Route>
          <Route path="*" element={<Navigate to="/dashboard" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;