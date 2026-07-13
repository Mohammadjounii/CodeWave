# FYP Vision Document
## CSCI420 – Final Year Project

---

**Project Title:** CodeWave – Interactive Programming Learning Platform

**Group Members:**
- [Student Name 1]
- [Student Name 2]
- [Student Name 3]

**Submission Date:** April 20, 2026

**Supervisor:** CSCI420 Course Supervisor

---

## 1. Project Idea

CodeWave is a web-based Learning Management System (LMS) purpose-built for programming education. The platform allows learners to enroll in structured Python and Java courses, write and execute code directly in the browser, take auto-graded quizzes, and track their learning progress — all in one place.

Beyond learning, CodeWave integrates career-readiness tools: learners can automatically generate a professional PDF CV populated from their completed courses and achievements, and browse a curated job offers board. An AI-powered Focus Mode embeds a Google Gemini chatbot directly into the coding environment, providing contextual assistance without leaving the platform.

The system supports two roles — Learner and Administrator — and is built on a clean four-layer architecture (Domain, Application, Infrastructure, Web) using ASP.NET Core 9.0, MySQL, Entity Framework Core, Docker, and SignalR.

---

## 2. Problem Statement

Learning to program today requires juggling a fragmented set of tools: video tutorials on one platform, a separate code editor, quizzes somewhere else, and no clear connection between effort and career opportunities. This fragmentation creates friction that discourages beginners and reduces learning effectiveness.

Specifically, the problems CodeWave addresses are:

- **No integrated practice environment:** Mainstream platforms (Udemy, Coursera, YouTube) provide content but no hands-on code execution linked to lessons.
- **Lack of personalization:** Learners with different skill levels are given the same content, with no adaptive routing based on existing knowledge.
- **No career bridge:** There is no direct link between what a learner has mastered and how that translates into employability or a professional profile.
- **Passive learning:** Most platforms deliver content passively (watch → read) rather than actively (code → test → feedback).
- **No real-time feedback during practice:** Learners working on coding exercises have no in-context assistance when stuck.

---

## 3. Need and Significance

**Why this matters now:**

- Demand for software developers continues to grow globally, yet entry barriers remain high due to poor learning tooling.
- Self-paced learners drop out at high rates on existing platforms primarily because they lack immediate, interactive feedback loops.
- Educators and bootcamps need affordable, configurable LMS solutions that go beyond slide decks and PDFs.

**Significance of CodeWave:**

- Provides a **single, unified platform** covering content delivery, interactive practice, assessment, and career preparation.
- **Reduces the cost of entry** to quality programming education by eliminating the need to subscribe to multiple services.
- Enables **data-driven learning** through progress dashboards and analytics that identify strengths and weaknesses.
- The **CV auto-generation** feature directly maps learning achievements to professional outcomes, creating a tangible return on investment for learners.
- The Docker-based execution sandbox makes the platform safe and scalable — each code run is isolated, protecting both the learner's work and the server.

---

## 4. Novelty and Innovation

CodeWave differentiates itself from existing platforms through five key innovations:

| # | Innovation | Description |
|---|------------|-------------|
| 1 | **Docker-based secure code sandbox** | Each code submission runs in an isolated Docker container, enabling safe execution of arbitrary Python and Java code without risk to the host system. This is the same approach used by production-grade platforms (e.g., LeetCode, HackerRank). |
| 2 | **AI-powered Focus Mode** | A distraction-free coding environment with a Google Gemini chatbot embedded directly in the editor panel. Learners can ask questions and get contextual hints without switching tabs or leaving their flow state. |
| 3 | **Achievement-driven CV generation** | The platform automatically constructs a professional PDF CV for each learner based on courses completed, skills acquired, and exercises passed — making the learning record immediately portable and shareable with employers. |
| 4 | **Assessment-gated adaptive enrollment** | New users take a skill assessment quiz before being assigned to a learning path. The system routes them to the appropriate starting point (Python beginner, Java intermediate, etc.) rather than forcing all users through the same linear content. |
| 5 | **Real-time progress notifications via SignalR** | Lesson completions, quiz results, and achievement unlocks are pushed to the client in real time, creating an engaging, responsive experience comparable to native mobile applications. |

---

## 5. Key Features

### Learner-Facing Features

- **User registration and authentication** — standard email/password and OAuth login via Google and GitHub
- **Skill assessment quiz** — determines the learner's entry point on enrollment
- **Structured courses and lessons** — organized content modules for Python and Java
- **Monaco-based code editor** — the same editor powering VS Code, embedded in the browser
- **Real-time code execution** — submit Python or Java code and see output instantly, powered by Docker runners
- **Quiz system** — lesson-linked quizzes with automated grading and instant feedback
- **Progress dashboard** — visual tracking of completed lessons, quiz scores, and overall course progress
- **AI Focus Mode** — distraction-free coding environment with integrated Google Gemini chatbot
- **CV generator** — auto-populated PDF CV based on course completions and earned achievements
- **Job offers board** — curated listings that learners can apply to directly from the platform

### Administrator Features

- **Admin dashboard** — overview of platform activity, enrollment metrics, and user engagement
- **Content management** — create, edit, and publish courses, lessons, exercises, and quizzes
- **User management** — view, edit, and manage learner accounts and roles

### Platform Features

- **Role-based access control** — enforced separation between Learner and Admin capabilities
- **Real-time notifications** — SignalR-powered push events for achievements and progress milestones
- **Responsive UI** — Tailwind CSS-based design that works across desktop and mobile browsers
- **Secure architecture** — four-layer clean architecture with repository pattern and unit of work

---

## 6. Project Phases

### Phase 1 – Project Proposal (Current)
- Submit this vision document by **April 20, 2026**
- Confirm group members and project scope with supervisor

### Phase 2 – Engineering (SE Document)
Deliverables:
- **Functional requirements** — drawn from existing SRS and RSD documents
- **Use cases** — formalized from existing use case documentation
- **Edge cases** — identified and documented for all core flows (registration, code execution, quiz submission, CV generation)
- **ER diagram** — finalized entity-relationship diagram (15+ entities including User, Course, Lesson, Exercise, Quiz, Submission, JobOffer, CV)
- **UI interfaces** — wireframes / screenshots of key Razor views
- **APIs and services** — documented REST API endpoints and internal service layer

### Phase 3 – Implementation
Remaining development work:
- Complete quiz system (question bank, grading engine, result history)
- Finalize AI Focus Mode and Gemini chatbot integration
- Finalize CV generation with PDF export
- Build out admin analytics dashboard
- Comprehensive unit and integration testing
- Docker-based deployment pipeline

---

*Document prepared for CSCI420 Final Year Project – Semester submission.*
