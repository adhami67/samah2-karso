from sqlalchemy.orm import Session
from app.models.activity import Activity
from app.models.assignment import Assignment
from app.models.timeline_event import TimelineEvent
from app.models.workspace import Workspace
from app.schemas.activity import ActivityCreate
from typing import Optional


class ActivityService:
    def __init__(self, db: Session):
        self.db = db

    def create_activity(self, data: ActivityCreate, current_user_id: str) -> Activity:
        workspace = self.db.get(Workspace, data.workspace_id)
        if workspace is None:
            raise ValueError("Workspace not found")

        activity = Activity(
            workspace_id=data.workspace_id,
            created_by_user_id=current_user_id,
            title=data.title,
            description=data.description,
            activity_type=data.activity_type,
            due_at=data.due_at,
            # status default is draft from model
        )

        self.db.add(activity)
        self.db.flush()

        event = TimelineEvent(
            activity_id=activity.id,
            event_type="activity_created",
            note="Activity created",
        )
        self.db.add(event)

        self.db.commit()
        self.db.refresh(activity)
        return activity

    def assign_to_user(
        self,
        activity: Activity,
        assignee_id: str,
        role: str = "executor",
    ) -> Assignment:
        assignment = Assignment(
            activity_id=activity.id,
            assignee_id=assignee_id,
            assignee_type="user",
            role=role,
        )
        self.db.add(assignment)
        self.db.flush()

        event = TimelineEvent(
            activity_id=activity.id,
            event_type="assignment_created",
            note=f"Assigned to user {assignee_id}",
        )
        self.db.add(event)

        self.db.commit()
        self.db.refresh(assignment)
        return assignment