const API_BASE = "http://127.0.0.1:8000/api";

async function request<T>(endpoint: string, options?: RequestInit): Promise<T> {
  const token = localStorage.getItem("access_token");
  const headers: Record<string, string> = {
    "Content-Type": "application/json",
    ...(options?.headers as Record<string, string>),
  };
  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  }

  const response = await fetch(`${API_BASE}${endpoint}`, {
    ...options,
    headers,
  });

  if (!response.ok) {
    let errorMessage = "Request failed";
    try {
      const error = await response.json();
      errorMessage = error.detail || errorMessage;
    } catch {
      // اگر پاسخ JSON نبود، پیام پیش‌فرض را نگه دار
    }
    throw new Error(errorMessage);
  }

  // ✅ اگر Status 204 بود، بدنه را پارس نکن
  if (response.status === 204) {
    return {} as T;
  }

  return response.json();
}

export const api = {
  get: <T>(endpoint: string) => request<T>(endpoint),
  post: <T>(endpoint: string, data: unknown) =>
    request<T>(endpoint, {
      method: "POST",
      body: JSON.stringify(data),
    }),
  patch: <T>(endpoint: string, data: unknown) =>
    request<T>(endpoint, {
      method: "PATCH",
      body: JSON.stringify(data),
    }),
  put: <T>(endpoint: string, data: unknown = {}) =>
    request<T>(endpoint, {
      method: "PUT",
      body: JSON.stringify(data),
    }),
  delete: <T>(endpoint: string) =>
    request<T>(endpoint, {
      method: "DELETE",
    }),
};