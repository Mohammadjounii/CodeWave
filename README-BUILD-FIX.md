# How to Prevent "File is Locked" Errors

## The Problem
When you try to build the project while the application is still running, you get errors like:
```
Could not copy "...dll" to "...dll". Exceeded retry count of 10. Failed. 
The file is locked by: "CodeWave.Web (PID)"
```

## The Solution

### Option 1: Use the Helper Scripts (Recommended)

**To stop the app before building:**
```powershell
.\stop-app.ps1
```

**To build safely (stops processes, cleans, then builds):**
```powershell
.\build-safe.ps1
```

**To run the app (automatically stops old processes first):**
```powershell
.\run-app.ps1
```

### Option 2: Manual Steps

1. **Stop the application in Visual Studio:**
   - Press `Shift + F5` or click the Stop button in the toolbar

2. **Or use PowerShell:**
   ```powershell
   Get-Process -Name "CodeWave.Web","dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force
   ```

3. **Wait 2-3 seconds**, then build/run

### Option 3: Always Stop Before Building

**In Visual Studio:**
- Always press `Shift + F5` to stop before building
- Or close the browser window and wait a few seconds

**Best Practice:**
- Stop the application (`Shift + F5`) before any build operation
- Use the provided scripts for automated process management

## Quick Reference

| Action | Command |
|--------|---------|
| Stop app | `.\stop-app.ps1` |
| Build safely | `.\build-safe.ps1` |
| Run app | `.\run-app.ps1` |
| Manual stop | `Get-Process -Name "CodeWave.Web","dotnet" \| Stop-Process -Force` |


