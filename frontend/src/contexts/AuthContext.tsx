import { createContext, useContext, useState, useEffect, ReactNode } from "react";
import { api } from "@/lib/api";

interface User {
  id: string;
  national_id: string;
  full_name: string;
  email?: string;
}

interface AuthContextType {
  user: User | null;
  token: string | null;
  login: (national_id: string, password: string) => Promise<void>;
  logout: () => void;
  isLoading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(localStorage.getItem("access_token"));
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (token) {
      // دریافت اطلاعات کاربر با توکن موجود
      api.get<{ id: string; national_id: string; full_name: string; email?: string }>("/auth/me")
        .then((userData) => setUser(userData))
        .catch(() => {
          // توکن نامعتبر
          localStorage.removeItem("access_token");
          setToken(null);
        });
    }
  }, [token]);

  const login = async (national_id: string, password: string) => {
    setIsLoading(true);
    try {
      const data = await api.post<{ access_token: string }>("/auth/login", {
        national_id,
        password,
      });
      localStorage.setItem("access_token", data.access_token);
      setToken(data.access_token);
      // بعد از تنظیم توکن، useEffect اطلاعات کاربر را می‌گیرد
    } finally {
      setIsLoading(false);
    }
  };

  const logout = () => {
    localStorage.removeItem("access_token");
    setToken(null);
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, token, login, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
}