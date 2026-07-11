from app.db.session import Base, engine, SessionLocal
from app.services.bootstrap import bootstrap_system

def main():
    Base.metadata.create_all(bind=engine)
    db = SessionLocal()
    try:
        result = bootstrap_system(db)
        print(result)
    finally:
        db.close()

if __name__ == "__main__":
    main()
