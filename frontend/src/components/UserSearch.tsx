// frontend/src/components/UserSearch.tsx
import { useState, useEffect, useRef } from "react";
import { api } from "@/lib/api";
import { Input } from "@/components/ui/input";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Check, Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";

interface User {
  id: string;
  full_name: string;
  national_id: string;
  roles: { name: string }[];
}

interface UserSearchProps {
  value?: string;
  onChange: (userId: string, user: User) => void;
  placeholder?: string;
  excludeIds?: string[];
  roleFilter?: string[];
  className?: string;
}

export function UserSearch({
  value,
  onChange,
  placeholder = "جستجوی کاربر...",
  excludeIds = [],
  roleFilter = [],
  className,
}: UserSearchProps) {
  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState("");
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(false);
  const [selectedUser, setSelectedUser] = useState<User | null>(null);
  const wrapperRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);

  // بارگذاری اولیه کاربران
  useEffect(() => {
    const fetchUsers = async () => {
      setLoading(true);
      try {
        const data = await api.get<User[]>("/security/users");
        setUsers(data);
      } catch (error) {
        console.error("خطا در دریافت کاربران:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchUsers();
  }, []);

  // فیلتر کردن کاربران بر اساس جستجو و نقش
  const filteredUsers = users.filter((user) => {
    const matchesSearch =
      user.full_name.includes(search) ||
      user.national_id.includes(search);
    const matchesRole =
      roleFilter.length === 0 ||
      user.roles.some((r) => roleFilter.includes(r.name));
    const notExcluded = !excludeIds.includes(user.id);
    return matchesSearch && matchesRole && notExcluded;
  });

  // انتخاب کاربر
  const handleSelect = (user: User) => {
    setSelectedUser(user);
    setSearch(user.full_name);
    onChange(user.id, user);
    setOpen(false);
  };

  // کلیک خارج از کامپوننت
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (wrapperRef.current && !wrapperRef.current.contains(event.target as Node)) {
        setOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  return (
    <div ref={wrapperRef} className={cn("relative", className)}>
      <Input
        ref={inputRef}
        value={search}
        onChange={(e) => {
          setSearch(e.target.value);
          setOpen(true);
          if (e.target.value === "") {
            setSelectedUser(null);
            onChange("", null as any);
          }
        }}
        onFocus={() => setOpen(true)}
        placeholder={placeholder}
        className="pr-9"
      />
      {loading && (
        <Loader2 className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 animate-spin text-muted-foreground" />
      )}

      {/* لیست کشویی */}
      {open && search && (
        <div className="absolute z-50 w-full mt-1 bg-white rounded-lg border shadow-lg max-h-60 overflow-y-auto">
          {filteredUsers.length === 0 ? (
            <div className="p-3 text-sm text-muted-foreground text-center">
              کاربری یافت نشد
            </div>
          ) : (
            filteredUsers.map((user) => (
              <button
                key={user.id}
                className="w-full flex items-center gap-3 px-3 py-2 hover:bg-accent text-right transition-colors"
                onClick={() => handleSelect(user)}
              >
                <Avatar className="h-8 w-8 flex-shrink-0">
                  <AvatarFallback className="bg-primary/10 text-primary text-xs">
                    {user.full_name.split(" ").map((n) => n[0]).join("").substring(0, 2)}
                  </AvatarFallback>
                </Avatar>
                <div className="flex-1 min-w-0">
                  <div className="text-sm font-medium truncate">{user.full_name}</div>
                  <div className="text-xs text-muted-foreground truncate">
                    {user.national_id} • {user.roles.map((r) => r.name).join("، ")}
                  </div>
                </div>
                {selectedUser?.id === user.id && (
                  <Check className="h-4 w-4 text-primary flex-shrink-0" />
                )}
              </button>
            ))
          )}
        </div>
      )}
    </div>
  );
}