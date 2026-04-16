# Gantt Chart - CodeWave Learning Platform Project Schedule

## Visual Timeline Representation

This document provides a text-based Gantt chart visualization of the project schedule. For interactive diagrams, see the PlantUML files.

---

## Legend
- **Critical Path Tasks** are marked with `[CRITICAL]`
- Duration shown in parentheses (days)
- Arrows (→) indicate dependencies

---

## Project Timeline (73 Days Total)

### Days 0-18: Foundation & Infrastructure Phase
```
Day:  0    5    10   15   18
      |----|----|----|----|
T1    [========][CRITICAL]
T2          [========][CRITICAL]
T3                [==========][CRITICAL]
T4                         [====][CRITICAL]
```

### Days 18-26: Authentication & Core Features
```
Day:  18   21   24   26
      |----|----|----|
T5    [====][CRITICAL]
T6        [========][CRITICAL]
T7        [====]
T8            [====][CRITICAL]
T9    [==]
T10       [====]
T11          [====]
T12             [==]
T13   [====]
T14       [====]
T15          [==]
T16   [====]
T17       [====]
T18   [====]
T29   [==]
T30       [====]
```

### Days 26-38: Code Editor & Execution Setup
```
Day:  26   29   33   36   38
      |----|----|----|----|
T8    [====][CRITICAL]
T19       [========][CRITICAL]
T20            [==========][CRITICAL]
T34            [====]
T37       [====]
```

### Days 38-50: Code Execution & Validation
```
Day:  38   42   46   50
      |----|----|----|
T20   [==========][CRITICAL]
T21   [====][CRITICAL]
T22   [========][CRITICAL]
T23       [==========][CRITICAL]
T24               [====][CRITICAL]
T35       [====]
```

### Days 50-62: Progress Tracking & Advanced Features
```
Day:  50   54   58   62
      |----|----|----|
T24   [====][CRITICAL]
T25   [========][CRITICAL]
T27       [========][CRITICAL]
T28   [====]
T36           [========]
T43                   [========][CRITICAL]
```

### Days 62-73: Testing & Deployment
```
Day:  62   70   73
      |----|----|
T43   [========][CRITICAL]
T44   [================][CRITICAL]
T45                       [====][CRITICAL]
```

---

## Complete Timeline View

| Task ID | Task Name | Week 1 | Week 2 | Week 3 | Week 4 | Week 5 | Week 6 | Week 7 | Week 8 | Week 9 | Week 10 | Week 11 |
|---------|-----------|:------:|:------:|:------:|:------:|:------:|:------:|:------:|:------:|:------:|:-------:|:-------:|
| **T1** | Project Setup | █████ | | | | | | | | | | |
| **T2** | Database Schema | ░░░██ | | | | | | | | | | |
| **T3** | Domain Entities | | ░░░██████ | | | | | | | | | |
| **T4** | DB Context | | | ░░░███ | | | | | | | | |
| **T5** | User Registration | | | | ███ | | | | | | | |
| **T6** | OAuth Integration | | | | ░░░█████ | | | | | | |
| **T7** | User Login | | | | ░░░████ | | | | | | |
| **T8** | RBAC | | | | | ███ | | | | | | |
| **T9** | Assessment Models | | | | ██ | | | | | | |
| **T10** | Assessment Questions | | | | ░░███ | | | | | |
| **T11** | Assessment Logic | | | | | ████ | | | | |
| **T12** | Learning Path | | | | | | ██ | | | |
| **T13** | Course Repository | | | | ███ | | | | | | |
| **T14** | Course Browsing | | | | ░░░███ | | | | | |
| **T15** | Course Enrollment | | | | | ██ | | | | |
| **T16** | Lesson Repository | | | | ███ | | | | | | |
| **T17** | Lesson Display | | | | ░░░███ | | | | | |
| **T18** | Exercise Repository | | | | ███ | | | | | | |
| **T19** | Code Editor | | | | | ░░░████ | | | |
| **T20** | Docker Setup | | | | | | ░░░░░█████ | | |
| **T21** | Python Execution | | | | | | | ███ | | |
| **T22** | Java Execution | | | | | | | ░░████ | |
| **T23** | Exercise Submission | | | | | | | | ░░░░░█████ | |
| **T24** | Test Validation | | | | | | | | | ░░░███ |
| **T25** | Progress Service | | | | | | | | | | ░░░░░████ |
| **T26** | Lesson Completion | | | | | ██ | | | | |
| **T27** | User Dashboard | | | | | | | | | | | ░░░░░░░░░████ |
| **T28** | Skills Analysis | | | | | | | | | | ░░░███ |
| **T29** | Quiz Models | | | | ██ | | | | | | |
| **T30** | Quiz Repository | | | | ░░███ | | | | | |
| **T31** | Quiz Display | | | | | ███ | | | | |
| **T32** | Quiz Taking | | | | | | ████ | | |
| **T33** | Quiz Results | | | | | | | ███ | |
| **T34** | Focus Mode UI | | | | | | ░░░███ | | |
| **T35** | Focus Mode Lesson | | | | | | | ░░░███ |
| **T36** | AI Helper Chat | | | | | | | | ░░░░░████ |
| **T37** | Admin Dashboard | | | | | ███ | | | |
| **T38** | User Management | | | | | | ░░░░░█████ | |
| **T39** | Course Admin | | | | | | ░░░░░████ | |
| **T40** | Job Management | | | | | | ░░░███ | |
| **T41** | Reports & Analytics | | | | | | | ░░░░░░░░░████ |
| **T42** | Job Display | | | | | | | ░░░██ |
| **T43** | CV Generation | | | | | | | | | | | ░░░░░░░░░░░░░░░░░░░░░░░░░████ |
| **T44** | Testing & QA | | | | | | | | | | | | ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░████████ |
| **T45** | Deployment | | | | | | | | | | | | | | | | | | | ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░███ |

**Legend:**
- `█` = Critical Path Task
- `░` = Non-Critical Task with Slack
- Each `█` or `░` represents 1 day

---

## Milestones

| Milestone | Day | Description |
|-----------|-----|-------------|
| M1 | 5 | Project Setup Complete |
| M2 | 18 | Database Infrastructure Ready |
| M3 | 29 | Authentication & Authorization Complete |
| M4 | 38 | Code Execution Engine Ready |
| M5 | 50 | Core Learning Features Complete |
| M6 | 62 | All Features Implemented |
| M7 | 70 | Testing Complete |
| M8 | 73 | Project Deployed |

---

## Parallel Work Streams

### Stream 1: Critical Path (Must be completed on schedule)
- T1 → T2 → T3 → T4 → T5 → T6 → T8 → T19 → T20 → T21/T22 → T23 → T24 → T25 → T27 → T43 → T44 → T45

### Stream 2: Assessment System (Can run in parallel)
- T9 → T10 → T11 → T12 (5 days slack)

### Stream 3: Course Management (Can run in parallel)
- T13 → T14 → T15 (6 days slack)

### Stream 4: Lesson Management (Can run in parallel)
- T16 → T17 → T26 (8 days slack)

### Stream 5: Quiz System (Can run in parallel)
- T29 → T30 → T31 → T32 → T33 (20 days slack)

### Stream 6: Focus Mode (Can run in parallel)
- T34 → T35 → T36 (2 days slack)

### Stream 7: Admin Features (Can run in parallel)
- T37 → T38/T39/T40 → T41/T42 (1-3 days slack)

---

## Resource Allocation by Phase

- **Weeks 1-2:** Infrastructure & Database (High focus on critical path)
- **Weeks 3-4:** Authentication & Core Features (Mixed critical/non-critical)
- **Weeks 5-7:** Code Editor & Execution (Critical path focus)
- **Weeks 8-9:** Progress Tracking & Features (Critical + parallel work)
- **Weeks 10-11:** Testing & Deployment (Critical path focus)

