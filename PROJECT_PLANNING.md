# CodeWave Learning Platform - Project Planning and Scheduling

## Overview
This document contains the project planning and scheduling analysis for the CodeWave Learning Platform, including task breakdown, PERT analysis, and Gantt chart visualization.

---

## Step 1: Task Extraction from Functional Requirements

Based on the Software Requirements Specification (SRS), tasks have been extracted from the following functional requirement areas:

### Functional Requirement Categories:
1. **User Authentication and Authorization** (FR-1.1 to FR-3.4)
2. **Learning Path Assessment** (FR-4.1 to FR-4.6)
3. **Course Management** (FR-5.1 to FR-7.5)
4. **Interactive Code Editor** (FR-8.1 to FR-10.6)
5. **Progress Tracking** (FR-11.1 to FR-12.3)
6. **Quiz System** (FR-13.1 to FR-15.5)
7. **Focus Mode** (FR-16.1 to FR-17.4)
8. **Admin Dashboard** (FR-18.1 to FR-22.5)
9. **Job Offers** (FR-23.1 to FR-23.4)
10. **CV Generation** (FR-24.1 to FR-24.4)

---

## Step 2: Task Scheduling Table

| Task ID | Task Name | Predecessor Tasks | Duration (Days) | ES | EF | LS | LF | Critical Path | Slack (Days) |
|---------|-----------|-------------------|-----------------|----|----|----|----|---------------|--------------|
| T1 | Project Setup & Infrastructure | - | 5 | 0 | 5 | 0 | 5 | Yes | 0 |
| T2 | Database Schema Design | T1 | 4 | 5 | 9 | 5 | 9 | Yes | 0 |
| T3 | Domain Entities Implementation | T2 | 6 | 9 | 15 | 9 | 15 | Yes | 0 |
| T4 | Database Context & Migrations | T3 | 3 | 15 | 18 | 15 | 18 | Yes | 0 |
| T5 | User Registration (Email/Password) | T4 | 3 | 18 | 21 | 18 | 21 | Yes | 0 |
| T6 | OAuth Integration (Google/GitHub) | T5 | 5 | 21 | 26 | 21 | 26 | Yes | 0 |
| T7 | User Login & Session Management | T5 | 4 | 21 | 25 | 27 | 31 | No | 6 |
| T8 | Role-Based Access Control | T6 | 3 | 26 | 29 | 26 | 29 | Yes | 0 |
| T9 | Assessment System - Database Models | T4 | 2 | 18 | 20 | 23 | 25 | No | 5 |
| T10 | Assessment Questions Implementation | T9 | 3 | 20 | 23 | 25 | 28 | No | 5 |
| T11 | Assessment Evaluation Logic | T10 | 4 | 23 | 27 | 28 | 32 | No | 5 |
| T12 | Learning Path Assignment | T11 | 2 | 27 | 29 | 32 | 34 | No | 5 |
| T13 | Course Management - Repository Layer | T4 | 3 | 18 | 21 | 24 | 27 | No | 6 |
| T14 | Course Browsing & Display | T13 | 3 | 21 | 24 | 27 | 30 | No | 6 |
| T15 | Course Enrollment Logic | T14 | 2 | 24 | 26 | 30 | 32 | No | 6 |
| T16 | Lesson Management - Repository Layer | T4 | 3 | 18 | 21 | 26 | 29 | No | 8 |
| T17 | Lesson Content Display | T16 | 3 | 21 | 24 | 29 | 32 | No | 8 |
| T18 | Exercise Management - Repository Layer | T4 | 3 | 18 | 21 | 28 | 31 | No | 10 |
| T19 | Code Editor Interface (Monaco Editor) | T8 | 4 | 29 | 33 | 29 | 33 | Yes | 0 |
| T20 | Code Execution Service - Docker Setup | T19 | 5 | 33 | 38 | 33 | 38 | Yes | 0 |
| T21 | Python Code Execution | T20 | 3 | 38 | 41 | 38 | 41 | Yes | 0 |
| T22 | Java Code Execution | T20 | 4 | 38 | 42 | 38 | 42 | Yes | 0 |
| T23 | Exercise Submission & Validation | T21, T22 | 5 | 42 | 47 | 42 | 47 | Yes | 0 |
| T24 | Test Case Validation System | T23 | 3 | 47 | 50 | 47 | 50 | Yes | 0 |
| T25 | Progress Tracking - Service Layer | T24 | 4 | 50 | 54 | 50 | 54 | Yes | 0 |
| T26 | Lesson Completion Tracking | T17 | 2 | 24 | 26 | 32 | 34 | No | 8 |
| T27 | User Dashboard - Progress Display | T25 | 4 | 54 | 58 | 54 | 58 | Yes | 0 |
| T28 | Skills & Weaknesses Analysis | T25 | 3 | 50 | 53 | 55 | 58 | No | 5 |
| T29 | Quiz System - Database Models | T4 | 2 | 18 | 20 | 38 | 40 | No | 20 |
| T30 | Quiz Management - Repository Layer | T29 | 3 | 20 | 23 | 40 | 43 | No | 20 |
| T31 | Quiz Display & Filtering | T30 | 3 | 23 | 26 | 43 | 46 | No | 20 |
| T32 | Quiz Taking Interface | T31 | 4 | 26 | 30 | 46 | 50 | No | 20 |
| T33 | Quiz Results & Scoring | T32 | 3 | 30 | 33 | 50 | 53 | No | 20 |
| T34 | Focus Mode - UI Implementation | T19 | 3 | 33 | 36 | 35 | 38 | No | 2 |
| T35 | Focus Mode - Lesson View | T34 | 3 | 36 | 39 | 38 | 41 | No | 2 |
| T36 | AI Helper Chat - Basic Implementation | T35 | 4 | 39 | 43 | 41 | 45 | No | 2 |
| T37 | Admin Dashboard - Overview | T8 | 3 | 29 | 32 | 30 | 33 | No | 1 |
| T38 | User Management - CRUD Operations | T37 | 5 | 32 | 37 | 33 | 38 | No | 1 |
| T39 | Course Management - Admin Panel | T37 | 4 | 32 | 36 | 34 | 38 | No | 2 |
| T40 | Job Offer Management | T37 | 3 | 32 | 35 | 35 | 38 | No | 3 |
| T41 | Reports & Analytics | T38, T39 | 4 | 37 | 41 | 38 | 42 | No | 1 |
| T42 | Job Offers - Public Display | T40 | 2 | 35 | 37 | 38 | 40 | No | 3 |
| T43 | CV Generation System | T27 | 4 | 58 | 62 | 58 | 62 | Yes | 0 |
| T44 | Testing & Quality Assurance | T43 | 8 | 62 | 70 | 62 | 70 | Yes | 0 |
| T45 | Deployment & Configuration | T44 | 3 | 70 | 73 | 70 | 73 | Yes | 0 |

---

## Step 3: PERT Analysis Calculations

### Critical Path Analysis:
The critical path is the longest path through the project network and determines the minimum project duration.

**Critical Path:** T1 → T2 → T3 → T4 → T5 → T6 → T8 → T19 → T20 → T21/T22 → T23 → T24 → T25 → T27 → T43 → T44 → T45

**Total Project Duration:** 73 days

### Early Start (ES) and Early Finish (EF) Calculation:
- ES = Maximum EF of all predecessor tasks
- EF = ES + Duration

### Late Start (LS) and Late Finish (LF) Calculation:
- LF = Minimum LS of all successor tasks
- LS = LF - Duration

### Slack (Margin) Calculation:
- Slack = LS - ES = LF - EF

---

## Step 4: Task Dependencies Graph

### Dependency Summary:
- **Foundation Tasks:** T1 (Project Setup)
- **Database Tasks:** T2 → T3 → T4
- **Authentication Tasks:** T4 → T5 → T6, T7 (parallel to T6)
- **Authorization:** T6 → T8
- **Assessment System:** T4 → T9 → T10 → T11 → T12
- **Course Management:** T4 → T13 → T14 → T15
- **Lesson Management:** T4 → T16 → T17 → T26
- **Exercise Management:** T4 → T18
- **Code Editor:** T8 → T19
- **Code Execution:** T19 → T20 → T21, T22 (parallel) → T23 → T24
- **Progress Tracking:** T24 → T25 → T27, T28 (parallel)
- **Quiz System:** T4 → T29 → T30 → T31 → T32 → T33
- **Focus Mode:** T19 → T34 → T35 → T36
- **Admin Features:** T8 → T37 → T38, T39, T40 (parallel) → T41
- **Job Offers:** T37 → T40 → T42
- **CV Generation:** T27 → T43
- **Final Tasks:** T43 → T44 → T45

---

## Key Metrics

- **Total Tasks:** 45
- **Critical Path Duration:** 73 days
- **Tasks on Critical Path:** 17
- **Total Slack in Project:** Tasks with slack can be delayed without affecting project completion
- **Parallel Work Opportunities:** Many tasks can be worked on in parallel after T4, T8, T19, T20, T37

---

## Resource Allocation Recommendations

1. **Phase 1 (Days 0-18):** Infrastructure & Database Setup
   - Focus on T1-T4
   - Critical path activities

2. **Phase 2 (Days 18-29):** Core Authentication & Authorization
   - Focus on T5-T8 (critical path)
   - Can parallel work on T9-T18 (non-critical)

3. **Phase 3 (Days 29-47):** Code Editor & Execution Engine
   - Focus on T19-T24 (critical path)
   - Can parallel work on T29-T33, T34-T36, T37-T42 (non-critical)

4. **Phase 4 (Days 47-62):** Progress Tracking & Advanced Features
   - Focus on T25, T27, T43 (critical path)
   - Complete T28, T36, T41, T42 (non-critical)

5. **Phase 5 (Days 62-73):** Testing & Deployment
   - Focus on T44-T45 (critical path)

---

## Risk Factors

1. **Code Execution Service (T20-T23):** High complexity, Docker dependencies
2. **Assessment Logic (T11):** Requires careful algorithm design
3. **Progress Tracking (T25):** Complex calculations for skills/weaknesses
4. **Integration Testing (T44):** All components must work together

---

## Notes

- Duration estimates are based on typical development timelines for a team of 3-5 developers
- Actual durations may vary based on team size, experience, and technical challenges
- Some tasks may require additional buffer time for unexpected issues
- The critical path should be monitored closely to ensure on-time project completion
- Tasks with slack can be scheduled flexibly to optimize resource utilization

