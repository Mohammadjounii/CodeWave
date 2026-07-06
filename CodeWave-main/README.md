# CodeWave

An interactive programming learning platform built with ASP.NET Core MVC. Learners follow
a single Python, Java, or Web Development path (lessons, coding exercises, quizzes), practice
in a code Playground, build a CV, browse real job listings (via an external job-search API),
and prep for interviews. Includes an admin panel for managing users, courses, and reports.

## Tech stack

- **ASP.NET Core MVC** (.NET 10), Razor views + Tailwind (CDN)
- **Entity Framework Core** on SQL Server
- **Serilog** (console + rolling file logs)
- **AutoMapper**
- **QuestPDF** (CV PDF export)
- **Google** and **GitHub** OAuth sign-in
- **Jint** (in-browser-style JavaScript execution for the Playground/exercises) and process-based
  Python/Java execution for coding exercises

## Project structure

| Project | Purpose |
|---|---|
| `CodeWave.Web` | MVC app — controllers, views, static assets, entry point |
| `CodeWave.Application` | DTOs, service interfaces, application-level services |
| `CodeWave.Infrastructure` | EF Core `DbContext`, migrations, repositories, seed data |
| `CodeWave.Domain` | Entities and enums |
| `CodeWave.Tests` | Unit tests |

## Getting started

**Prerequisites:** .NET 10 SDK, SQL Server (local or remote).

1. Set your connection string in `CodeWave.Web/appsettings.json` (`ConnectionStrings:DefaultConnection`).
2. Apply migrations:
   ```powershell
   dotnet ef database update --project CodeWave.Infrastructure --startup-project CodeWave.Web
   ```
3. Run the app — either `dotnet run --project CodeWave.Web`, or use `.\scripts\run-app.ps1` from
   the project root (see below). It listens on `https://localhost:7000`.

Seed data (courses, lessons, exercises, achievements, etc.) is applied automatically on startup —
see `Program.cs` and `CodeWave.Infrastructure/Data/Seed/`.

## Helper scripts

On Windows, building or restarting the app while a previous instance is still running throws a
"file is locked by CodeWave.Web" error, because the running process holds the output DLLs open.
These scripts handle that:

| Script | What it does |
|---|---|
| `.\scripts\stop-app.ps1` | Stops any running CodeWave instance (matches by port 7000 and by process path) |
| `.\scripts\run-app.ps1` | Stops any running instance, then `dotnet run`s the app |
| `.\scripts\build-safe.ps1` | Stops any running instance, `dotnet clean`s, then `dotnet build`s |

Run them from the project root. If you'd rather do it manually: stop debugging in Visual Studio
(`Shift+F5`) before building, or `Get-Process -Name "CodeWave.Web","dotnet" | Stop-Process -Force`.
