from fastapi import WebSocket, WebSocketDisconnect
from typing import Dict, List
import json

class ConnectionManager:
    def __init__(self):
        # نگهداری لیست WebSocket‌ها برای هر کاربر
        self.active_connections: Dict[str, List[WebSocket]] = {}

    async def connect(self, user_id: str, websocket: WebSocket):
        """پذیرش اتصال جدید"""
        await websocket.accept()
        if user_id not in self.active_connections:
            self.active_connections[user_id] = []
        self.active_connections[user_id].append(websocket)

    def disconnect(self, user_id: str, websocket: WebSocket):
        """قطع اتصال"""
        if user_id in self.active_connections:
            self.active_connections[user_id].remove(websocket)
            if not self.active_connections[user_id]:
                del self.active_connections[user_id]

    async def send_personal_message(self, user_id: str, message: dict):
        """ارسال پیام به یک کاربر خاص"""
        if user_id in self.active_connections:
            for connection in self.active_connections[user_id]:
                try:
                    await connection.send_json(message)
                except:
                    # در صورت خطا، اتصال را حذف می‌کنیم
                    self.disconnect(user_id, connection)

    async def broadcast(self, message: dict):
        """ارسال پیام به همه کاربران متصل"""
        for user_id in self.active_connections:
            await self.send_personal_message(user_id, message)

# نمونه‌ای از manager برای استفاده در سراسر برنامه
manager = ConnectionManager()