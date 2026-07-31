// frontend/src/contexts/AuthContext.tsx
import { createContext, useContext, useState, useEffect, ReactNode } from "react";
import { api } from "@/lib/api";

interface User {
  id: string;
  national_id: string;
  full_name: string;
  username?: string;
  roles: string[]; // ✅ نقش‌های کاربر
}

interface AuthContextType {
  user: User | null;
  token: string | null;
  login: (national_id: string, password: string) => Promise<void>;
  logout: () => void;
  isLoading: boolean;
  hasRole: (role: string) => boolean;
  hasAnyRole: (roles: string[]) => boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(localStorage.getItem("access_token"));
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (token) {
      api
        .get<{
          id: string;
          national_id: string;
          full_name: string;
          username?: string;
          roles?: string[];
        }>("/auth/me")
        .then((data) => {
          setUser({
            id: data.id,
            national_id: data.national_id,
            full_name: data.full_name,
            username: data.username,
            roles: data.roles || [],
          });
        })
        .catch(() => {
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
    } finally {
      setIsLoading(false);
    }
  };

  const logout = () => {
    localStorage.removeItem("access_token");
    setToken(null);
    setUser(null);
  };

  const hasRole = (role: string): boolean => {
    return user?.roles?.includes(role) ?? false;
  };

  const hasAnyRole = (roles: string[]): boolean => {
    return roles.some((role) => user?.roles?.includes(role));
  };

  return (
    <AuthContext.Provider
      value={{ user, token, login, logout, isLoading, hasRole, hasAnyRole }}
    >
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