// frontend/src/components/WeeklySchedule.tsx
import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Calendar } from "lucide-react";

interface ClassSchedule {
  id: string;
  name: string;
  teacher: string;
  time: string;
  day: string;
}

export function WeeklySchedule() {
  const [classes, setClasses] = useState<ClassSchedule[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get<ClassSchedule[]>("/schedule/week")
      .then(setClasses)
      .catch(console.error)
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <div className="animate-pulse h-32 bg-muted/30 rounded-lg" />;
  if (classes.length === 0) return null;

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center gap-2 text-base">
          <Calendar className="h-4 w-4" />
          برنامه هفتگی کلاس‌ها
        </CardTitle>
      </CardHeader>
      <CardContent>
        <div className="space-y-2">
          {classes.slice(0, 5).map((cls) => (
            <div key={cls.id} className="flex items-center justify-between p-2 rounded-lg bg-muted/30">
              <div>
                <p className="font-medium text-sm">{cls.name}</p>
                <p className="text-xs text-muted-foreground">{cls.teacher}</p>
              </div>
              <div className="text-sm text-muted-foreground">
                {cls.day} • {cls.time}
              </div>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
}