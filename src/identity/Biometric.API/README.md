# BioMet API Setup Guide

## Setting Up the Virtual Environment

### 1. Create and Activate the Virtual Environment
Once the virtual environment is created, activate it before running any commands:

```powershell
.venv\Scripts\Activate.ps1
```
- **Run this command in every new terminal session.**
- Each time you install a new package, ensure the environment is activated.

### 2. Upgrade `pip` (If Needed)
If you encounter errors related to `pip`, update it using:

```powershell
python -m pip install --upgrade pip
```

### 3. Install Dependencies
Install all required packages from `requirements.txt`:

```powershell
pip install -r requirements.txt
```
_(This file functions similarly to `package.json` in Node.js.)_

### 3.1 Run Fast API

```powershell
fastapi dev main.py
```

### 4. Deactivating the Virtual Environment
When you’re done, deactivate the virtual environment:

```powershell
deactivate
```