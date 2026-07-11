# Karso Bootstrap Step

This package adds the **first real bootstrap layer** for Karso:

- default permission set
- default roles
- first admin user
- bootstrap status endpoint
- bootstrap setup endpoint

## What to do
1. Unzip the project
2. Copy `.env.example` to `.env`
3. Install dependencies
4. Run the app
5. Open `/docs`
6. Call `POST /api/bootstrap/setup` once
7. Login with the created admin user

## Run
```bash
python -m venv .venv
python -m pip install --upgrade pip
python -m pip install -r requirements.txt
python -m uvicorn app.main:app --reload
```

## Default bootstrap admin
If you do not override environment values, the bootstrap endpoint creates:

- National ID: `1111111111`
- Password: `Admin@12345`
- Full name: `System Admin`

Change them in `.env` before setup if needed.
