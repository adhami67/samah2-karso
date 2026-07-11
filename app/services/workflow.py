ALLOWED_ACTIVITY_TRANSITIONS = {
    "draft": {"published", "archived"},
    "published": {"in_progress", "archived"},
    "in_progress": {"submitted", "needs_revision", "archived"},
    "submitted": {"needs_revision", "approved", "completed", "archived"},
    "needs_revision": {"in_progress", "archived"},
    "approved": {"completed", "archived"},
    "completed": {"archived"},
    "archived": set(),
}

def can_transition_activity(current_status: str, target_status: str) -> bool:
    return target_status in ALLOWED_ACTIVITY_TRANSITIONS.get(current_status, set())
