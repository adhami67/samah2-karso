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
        # ردیف اول توضیحات است، ردیف دوم نام واقعی ستون‌ها
        df = pd.read_excel(BytesIO(contents), header=1)
    except Exception as e:
        raise HTTPException(status_code=400, detail=f"خطا در خواندن فایل: {str(e)}")

    # ستون‌های ضروری برای ایجاد دانش‌آموز
    required_columns = ['first_name', 'last_name', 'username', 'password']
    missing_cols = [col for col in required_columns if col not in df.columns]
    if missing_cols:
        raise HTTPException(status_code=400, detail=f"ستون‌های ضروری وجود ندارند: {', '.join(missing_cols)}")

    # دریافت یا ایجاد نقش دانش‌آموز
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
            first_name = str(row.get('first_name', '')).strip()
            last_name = str(row.get('last_name', '')).strip()
            username = str(row.get('username', '')).strip()        # کد ملی دانش‌آموز
            password = str(row.get('password', '')).strip()

            if not first_name or not last_name or not username or not password:
                failed.append({"row": idx + 3, "error": "اطلاعات ناقص (نام، نام خانوادگی، کد ملی یا رمز عبور)"})
                continue

            # بررسی تکراری بودن کد ملی
            if db.query(User).filter(User.national_id == username).first():
                failed.append({"row": idx + 3, "error": "کد ملی تکراری"})
                continue

            full_name = f"{first_name} {last_name}".strip()

            # پایه و کلاس
            grade = str(row.get('grade', '')).strip() if pd.notna(row.get('grade')) else None
            class_name = str(row.get('class', '')).strip() if pd.notna(row.get('class')) else None

            # شماره همراه پدر (ستون mobile)
            father_phone = str(row.get('mobile', '')).strip() if pd.notna(row.get('mobile')) else None

            # نام پدر
            father_first = str(row.get('father_first_name', '')).strip() if pd.notna(row.get('father_first_name')) else None
            father_last = str(row.get('father_last_name', '')).strip() if pd.notna(row.get('father_last_name')) else None
            father_name = f"{father_first} {father_last}".strip() if father_first or father_last else None

            # نام مادر
            mother_first = str(row.get('mother_first_name', '')).strip() if pd.notna(row.get('mother_first_name')) else None
            mother_last = str(row.get('mother_last_name', '')).strip() if pd.notna(row.get('mother_last_name')) else None
            mother_name = f"{mother_first} {mother_last}".strip() if mother_first or mother_last else None

            # شماره والدین (فعلاً شماره پدر به‌عنوان parent_phone ذخیره می‌شود)
            parent_phone = father_phone

            # شماره خود دانش‌آموز خالی می‌ماند (در صورت نیاز می‌توان از ستون دیگری پر کرد)
            phone = None

            user = User(
                username=username,             # کد ملی
                national_id=username,
                full_name=full_name,
                password_hash=hash_password(password),
                is_active=True,
                grade=grade,
                class_name=class_name,
                father_name=father_name,
                mother_name=mother_name,
                parent_phone=parent_phone,
                phone=phone,
                roles=[student_role]
            )
            db.add(user)
            db.flush()
            success.append({"national_id": username, "full_name": full_name})
        except Exception as e:
            failed.append({"row": idx + 3, "error": str(e)})

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