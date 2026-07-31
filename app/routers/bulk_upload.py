from fastapi import APIRouter, Depends, HTTPException, File, UploadFile, status
from sqlalchemy.orm import Session
import pandas as pd
from io import BytesIO
from app.db.deps import get_db
from app.models.security import User, Role
from app.utils.security import hash_password
from app.utils.deps import require_role

router = APIRouter()

@router.post("/users", response_model=dict, status_code=status.HTTP_201_CREATED)
def bulk_upload_users(
    file: UploadFile = File(...),
    db: Session = Depends(get_db),
    current_user: User = Depends(require_role("system_admin"))
):
    if not file.filename.endswith(('.xlsx', '.xls')):
        raise HTTPException(status_code=400, detail="فقط فایل‌های Excel پذیرفته می‌شوند")

    try:
        contents = file.file.read()
        df = pd.read_excel(BytesIO(contents))
    except Exception as e:
        raise HTTPException(status_code=400, detail=f"خطا در خواندن فایل: {str(e)}")

    required_columns = ['national_id', 'full_name', 'password']
    missing_cols = [col for col in required_columns if col not in df.columns]
    if missing_cols:
        raise HTTPException(status_code=400, detail=f"ستون‌های ضروری وجود ندارند: {', '.join(missing_cols)}")

    student_role = db.query(Role).filter(Role.code == "student").first()
    if not student_role:
        student_role = Role(code="student", name="دانش‌آموز")
        db.add(student_role)
        db.flush()

    success = []
    failed = []
    total = len(df)

    for idx, row in df.iterrows():
        try:
            national_id = str(row.get('national_id', '')).strip()
            full_name = str(row.get('full_name', '')).strip()
            password = str(row.get('password', '')).strip()

            if not national_id or not full_name or not password:
                failed.append({"row": idx + 2, "error": "اطلاعات ناقص"})
                continue

            if db.query(User).filter(User.national_id == national_id).first():
                failed.append({"row": idx + 2, "error": "کد ملی تکراری"})
                continue

            grade = str(row['grade']).strip() if pd.notna(row.get('grade')) else None
            class_name = str(row['class']).strip() if pd.notna(row.get('class')) else None
            father_name = str(row.get('father_name', '')).strip() if pd.notna(row.get('father_name')) else None
            mother_name = str(row.get('mother_name', '')).strip() if pd.notna(row.get('mother_name')) else None
            phone = str(row.get('phone', '')).strip() if pd.notna(row.get('phone')) else None
            parent_phone = str(row.get('parent_phone', '')).strip() if pd.notna(row.get('parent_phone')) else None
            address = str(row.get('address', '')).strip() if pd.notna(row.get('address')) else None
            birth_date = str(row.get('birth_date', '')).strip() if pd.notna(row.get('birth_date')) else None
            gender = str(row.get('gender', '')).strip() if pd.notna(row.get('gender')) else None

            user = User(
                username=national_id,
                national_id=national_id,
                full_name=full_name,
                password_hash=hash_password(password),
                is_active=True,
                grade=grade,
                class_name=class_name,
                father_name=father_name,
                mother_name=mother_name,
                parent_phone=parent_phone,
                phone=phone,
                address=address,
                birth_date=birth_date,
                gender=gender,
                roles=[student_role]
            )
            db.add(user)
            db.flush()
            success.append({"national_id": national_id, "full_name": full_name})
        except Exception as e:
            failed.append({"row": idx + 2, "error": str(e)})

    db.commit()
    return {"total": total, "success": success, "failed": failed}


@router.get("/students/grouped")
def get_students_grouped(
    db: Session = Depends(get_db),
    current_user: User = Depends(require_role("system_admin"))
):
    student_role = db.query(Role).filter(Role.code == "student").first()
    if not student_role:
        return []
    students = db.query(User).filter(User.roles.any(Role.id == student_role.id)).all()

    grouped = {}
    for student in students:
        key = f"{student.grade}_{student.class_name}" if student.grade and student.class_name else "بدون کلاس"
        if key not in grouped:
            grouped[key] = {
                "grade": student.grade,
                "class": student.class_name,
                "students": []
            }
        grouped[key]["students"].append({
            "id": student.id,
            "full_name": student.full_name,
            "national_id": student.national_id,
            "father_name": student.father_name,
            "mother_name": student.mother_name,
            "parent_phone": student.parent_phone,
            "phone": student.phone,
            "address": student.address,
            "birth_date": student.birth_date,
            "gender": student.gender,
        })
    return list(grouped.values())