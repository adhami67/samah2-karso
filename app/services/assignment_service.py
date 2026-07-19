from sqlalchemy.orm import Session

from app.models.assignment import Assignment
from app.models.timeline_event import TimelineEvent


class AssignmentService:

    def __init__(self, db: Session):
        self.db = db

    def create_assignment(self, assignment: Assignment):

        self.db.add(assignment)
        self.db.flush()

        event = TimelineEvent(
            activity_id=assignment.activity_id,
            event_type="assignment_created",
            note="Activity assigned",
        )

        self.db.add(event)

        self.db.commit()

        self.db.refresh(assignment)

        return assignment