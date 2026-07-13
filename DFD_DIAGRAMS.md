# Data Flow Diagrams (DFD)
## CodeWave Learning Platform

**Version:** 1.0  
**Date:** December 2024

---

## Table of Contents

1. [Overview](#overview)
2. [DFD Notation](#dfd-notation)
3. [Context Diagram (Level 0)](#context-diagram-level-0)
4. [Level 1 DFD](#level-1-dfd)
5. [Level 2 DFDs](#level-2-dfds)
6. [Data Dictionary](#data-dictionary)

---

## Overview

Data Flow Diagrams (DFD) illustrate how data moves through the CodeWave system. They show processes, data stores, external entities, and the data flows between them.

### Purpose
- Visualize data flow through the system
- Identify system boundaries
- Document data transformations
- Support system design and analysis

---

## DFD Notation

### Symbols Used

- **Process**: Rounded rectangle - Represents a function that transforms data
- **External Entity**: Rectangle - Represents external actors or systems
- **Data Store**: Open rectangle (two parallel lines) - Represents data storage
- **Data Flow**: Arrow - Represents data movement with label

### Naming Conventions

- **Processes**: Verb + Noun (e.g., "Authenticate User", "Execute Code")
- **Data Stores**: Noun (e.g., "User Database", "Session Store")
- **External Entities**: Noun (e.g., "Learner", "Administrator")
- **Data Flows**: Noun phrases (e.g., "User Credentials", "Course Data")

---

## Context Diagram (Level 0)

The context diagram shows the system as a single process and its interactions with external entities.

```mermaid
graph LR
    Learner[Learner]
    Admin[Administrator]
    Guest[Guest User]
    Google[Google OAuth]
    GitHub[GitHub OAuth]
    CodeRunner[Code Execution System]
    
    System[CodeWave Learning Platform]
    
    Learner -->|Registration Data<br/>Login Credentials<br/>Course Requests<br/>Code Submissions<br/>Quiz Answers| System
    System -->|Course Content<br/>Progress Data<br/>Quiz Results<br/>Job Offers| Learner
    
    Admin -->|Admin Credentials<br/>Management Requests<br/>Content Updates| System
    System -->|User Data<br/>Course Data<br/>Analytics Reports| Admin
    
    Guest -->|Registration Data<br/>Login Credentials| System
    System -->|Login Page<br/>Registration Form| Guest
    
    Google -->|OAuth Token<br/>User Profile| System
    System -->|OAuth Request| Google
    
    GitHub -->|OAuth Token<br/>User Profile| System
    System -->|OAuth Request| GitHub
    
    CodeRunner -->|Execution Results<br/>Output Data| System
    System -->|Code<br/>Language Type| CodeRunner
    
    style System fill:#9e47eb,stroke:#7c3aed,stroke-width:3px,color:#fff
    style Learner fill:#E6F3FF,stroke:#0066CC
    style Admin fill:#E6F3FF,stroke:#0066CC
    style Guest fill:#E6F3FF,stroke:#0066CC
    style Google fill:#FFE6E6,stroke:#CC0000
    style GitHub fill:#FFE6E6,stroke:#CC0000
    style CodeRunner fill:#FFE6E6,stroke:#CC0000
```

---

## Level 1 DFD

Level 1 DFD decomposes the system into major processes and shows data stores.

```mermaid
graph TB
    subgraph External["External Entities"]
        Learner[Learner]
        Admin[Administrator]
        Guest[Guest User]
        Google[Google OAuth]
        GitHub[GitHub OAuth]
        CodeRunner[Code Execution System]
    end
    
    subgraph System["CodeWave System"]
        P1[1.0<br/>Authenticate User]
        P2[2.0<br/>Manage Assessment]
        P3[3.0<br/>Manage Learning Content]
        P4[4.0<br/>Execute Code]
        P5[5.0<br/>Track Progress]
        P6[6.0<br/>Manage Quizzes]
        P7[7.0<br/>Manage Admin Functions]
        P8[8.0<br/>Generate Reports]
    end
    
    subgraph DataStores["Data Stores"]
        D1[("D1: User Database")]
        D2[("D2: Course Database")]
        D3[("D3: Progress Database")]
        D4[("D4: Quiz Database")]
        D5[("D5: Assessment Database")]
        D6[("D6: Session Store")]
        D7[("D7: Code Cache")]
    end
    
    Learner -->|Login Credentials| P1
    Guest -->|Registration Data| P1
    Google -->|OAuth Token| P1
    GitHub -->|OAuth Token| P1
    P1 -->|User Data| D1
    P1 -->|Session Data| D6
    D1 -->|User Info| P1
    P1 -->|Authenticated User| Learner
    P1 -->|Authenticated Admin| Admin
    
    Learner -->|Assessment Answers| P2
    P2 -->|Assessment Data| D5
    P2 -->|User Assessment| D1
    D5 -->|Questions| P2
    D1 -->|User Profile| P2
    P2 -->|Learning Path Assignment| Learner
    
    Learner -->|Course Request| P3
    Admin -->|Course Management| P3
    P3 -->|Course Data| D2
    D2 -->|Course Content| P3
    P3 -->|Lessons & Exercises| Learner
    P3 -->|Course Updates| Admin
    
    Learner -->|Code| P4
    P4 -->|Code Request| CodeRunner
    CodeRunner -->|Execution Results| P4
    P4 -->|Code Results| D7
    P4 -->|Submission Data| D3
    D7 -->|Cached Results| P4
    P4 -->|Output| Learner
    
    Learner -->|Progress Query| P5
    P5 -->|Progress Data| D3
    D3 -->|Completion Records| P5
    D1 -->|User Data| P5
    D2 -->|Course Data| P5
    P5 -->|Progress Statistics| Learner
    
    Learner -->|Quiz Answers| P6
    Admin -->|Quiz Management| P6
    P6 -->|Quiz Data| D4
    D4 -->|Questions & Answers| P6
    P6 -->|Quiz Results| Learner
    P6 -->|Quiz Updates| Admin
    
    Admin -->|Management Requests| P7
    P7 -->|User Data| D1
    P7 -->|Course Data| D2
    P7 -->|Job Offer Data| D2
    D1 -->|User Information| P7
    D2 -->|Course Information| P7
    P7 -->|Management Results| Admin
    
    Admin -->|Report Request| P8
    P8 -->|Analytics Data| D1
    P8 -->|Analytics Data| D2
    P8 -->|Analytics Data| D3
    P8 -->|Analytics Data| D4
    D1 -->|User Statistics| P8
    D2 -->|Course Statistics| P8
    D3 -->|Progress Statistics| P8
    D4 -->|Quiz Statistics| P8
    P8 -->|Reports| Admin
    
    style P1 fill:#9e47eb,stroke:#7c3aed,color:#fff
    style P2 fill:#9e47eb,stroke:#7c3aed,color:#fff
    style P3 fill:#9e47eb,stroke:#7c3aed,color:#fff
    style P4 fill:#9e47eb,stroke:#7c3aed,color:#fff
    style P5 fill:#9e47eb,stroke:#7c3aed,color:#fff
    style P6 fill:#9e47eb,stroke:#7c3aed,color:#fff
    style P7 fill:#9e47eb,stroke:#7c3aed,color:#fff
    style P8 fill:#9e47eb,stroke:#7c3aed,color:#fff
```

---

## Level 2 DFDs

### DFD 1.0: Authenticate User

```mermaid
graph TB
    subgraph External["External"]
        Guest[Guest User]
        Learner[Learner]
        Admin[Administrator]
        Google[Google OAuth]
        GitHub[GitHub OAuth]
    end
    
    subgraph Process["Process 1.0: Authenticate User"]
        P1_1[1.1<br/>Validate Credentials]
        P1_2[1.2<br/>Create Session]
        P1_3[1.3<br/>Handle OAuth]
        P1_4[1.4<br/>Assign Role]
    end
    
    subgraph Stores["Data Stores"]
        D1[("D1: User Database")]
        D6[("D6: Session Store")]
    end
    
    Guest -->|Email, Password| P1_1
    Learner -->|Email, Password| P1_1
    Admin -->|Email, Password| P1_1
    Google -->|OAuth Token| P1_3
    GitHub -->|OAuth Token| P1_3
    
    P1_1 -->|User Query| D1
    D1 -->|User Data| P1_1
    P1_1 -->|Valid Credentials| P1_2
    P1_1 -->|Invalid| Guest
    
    P1_3 -->|User Query| D1
    D1 -->|User Data| P1_3
    P1_3 -->|New User| D1
    P1_3 -->|Existing User| P1_2
    
    P1_2 -->|Session Data| D6
    P1_2 -->|User ID| P1_4
    D1 -->|User Role| P1_4
    P1_4 -->|Authenticated Session| Learner
    P1_4 -->|Authenticated Session| Admin
```

### DFD 2.0: Manage Assessment

```mermaid
graph TB
    Learner[Learner]
    
    subgraph Process["Process 2.0: Manage Assessment"]
        P2_1[2.1<br/>Display Questions]
        P2_2[2.2<br/>Collect Answers]
        P2_3[2.3<br/>Evaluate Answers]
        P2_4[2.4<br/>Assign Learning Path]
    end
    
    subgraph Stores["Data Stores"]
        D5[("D5: Assessment Database")]
        D1[("D1: User Database")]
    end
    
    Learner -->|Assessment Request| P2_1
    P2_1 -->|Questions Query| D5
    D5 -->|Questions| P2_1
    P2_1 -->|Questions| Learner
    
    Learner -->|Answers| P2_2
    P2_2 -->|Answer Data| P2_3
    P2_3 -->|Score Calculation| P2_4
    P2_4 -->|Learning Path| D1
    P2_4 -->|User Profile Update| D1
    D1 -->|Updated Profile| Learner
```

### DFD 3.0: Manage Learning Content

```mermaid
graph TB
    Learner[Learner]
    Admin[Administrator]
    
    subgraph Process["Process 3.0: Manage Learning Content"]
        P3_1[3.1<br/>Retrieve Courses]
        P3_2[3.2<br/>Display Lessons]
        P3_3[3.3<br/>Track Completion]
        P3_4[3.4<br/>Manage Course Content]
    end
    
    subgraph Stores["Data Stores"]
        D2[("D2: Course Database")]
        D3[("D3: Progress Database")]
        D1[("D1: User Database")]
    end
    
    Learner -->|Course Request| P3_1
    Admin -->|Content Management| P3_4
    
    P3_1 -->|Course Query| D2
    D2 -->|Course Data| P3_1
    D1 -->|User Learning Path| P3_1
    P3_1 -->|Filtered Courses| Learner
    
    Learner -->|Lesson Request| P3_2
    P3_2 -->|Lesson Query| D2
    D2 -->|Lesson Content| P3_2
    P3_2 -->|Lessons| Learner
    
    Learner -->|Completion Status| P3_3
    P3_3 -->|Completion Record| D3
    P3_3 -->|Progress Update| D2
    D3 -->|Progress Data| Learner
    
    Admin -->|Course CRUD| P3_4
    P3_4 -->|Course Updates| D2
    D2 -->|Course Data| P3_4
    P3_4 -->|Confirmation| Admin
```

### DFD 4.0: Execute Code

```mermaid
graph TB
    Learner[Learner]
    CodeRunner[Code Execution System]
    
    subgraph Process["Process 4.0: Execute Code"]
        P4_1[4.1<br/>Receive Code]
        P4_2[4.2<br/>Detect Language]
        P4_3[4.3<br/>Send to Runner]
        P4_4[4.4<br/>Process Results]
        P4_5[4.5<br/>Validate Exercise]
    end
    
    subgraph Stores["Data Stores"]
        D2[("D2: Course Database")]
        D3[("D3: Progress Database")]
        D7[("D7: Code Cache")]
    end
    
    Learner -->|Code, Exercise ID| P4_1
    P4_1 -->|Code Data| P4_2
    P4_2 -->|Course Query| D2
    D2 -->|Language Info| P4_2
    P4_2 -->|Language| P4_3
    
    P4_3 -->|Code, Language| CodeRunner
    CodeRunner -->|Output, Errors| P4_3
    P4_3 -->|Results| P4_4
    
    P4_4 -->|Results| D7
    P4_4 -->|Output| Learner
    
    P4_1 -->|Exercise ID| P4_5
    P4_5 -->|Test Cases Query| D2
    D2 -->|Test Cases| P4_5
    P4_4 -->|Test Results| P4_5
    P4_5 -->|Validation Result| P4_4
    P4_5 -->|Submission Record| D3
    P4_4 -->|Test Results| Learner
```

### DFD 5.0: Track Progress

```mermaid
graph TB
    Learner[Learner]
    
    subgraph Process["Process 5.0: Track Progress"]
        P5_1[5.1<br/>Calculate Statistics]
        P5_2[5.2<br/>Extract Skills]
        P5_3[5.3<br/>Identify Weaknesses]
        P5_4[5.4<br/>Generate Dashboard]
    end
    
    subgraph Stores["Data Stores"]
        D1[("D1: User Database")]
        D2[("D2: Course Database")]
        D3[("D3: Progress Database")]
        D4[("D4: Quiz Database")]
    end
    
    Learner -->|Progress Request| P5_4
    P5_4 -->|Data Queries| P5_1
    P5_4 -->|Data Queries| P5_2
    P5_4 -->|Data Queries| P5_3
    
    P5_1 -->|Completion Query| D3
    P5_1 -->|User Query| D1
    P5_1 -->|Course Query| D2
    D3 -->|Completion Data| P5_1
    D1 -->|User Data| P5_1
    D2 -->|Course Data| P5_1
    P5_1 -->|Statistics| P5_4
    
    P5_2 -->|Lesson Query| D2
    P5_2 -->|Completion Query| D3
    D2 -->|Lesson Content| P5_2
    D3 -->|Completed Lessons| P5_2
    P5_2 -->|Skills List| P5_4
    
    P5_3 -->|Submission Query| D3
    P5_3 -->|Quiz Query| D4
    D3 -->|Failed Exercises| P5_3
    D4 -->|Failed Quizzes| P5_3
    P5_3 -->|Weaknesses List| P5_4
    
    P5_4 -->|Dashboard Data| Learner
```

### DFD 6.0: Manage Quizzes

```mermaid
graph TB
    Learner[Learner]
    Admin[Administrator]
    
    subgraph Process["Process 6.0: Manage Quizzes"]
        P6_1[6.1<br/>Display Available Quizzes]
        P6_2[6.2<br/>Present Questions]
        P6_3[6.3<br/>Calculate Score]
        P6_4[6.4<br/>Manage Quiz Content]
    end
    
    subgraph Stores["Data Stores"]
        D4[("D4: Quiz Database")]
        D3[("D3: Progress Database")]
        D2[("D2: Course Database")]
        D1[("D1: User Database")]
    end
    
    Learner -->|Quiz Request| P6_1
    P6_1 -->|Completed Lessons Query| D3
    P6_1 -->|User Learning Path| D1
    P6_1 -->|Quiz Query| D4
    D3 -->|Completed Lessons| P6_1
    D1 -->|Learning Path| P6_1
    D4 -->|Available Quizzes| P6_1
    P6_1 -->|Quiz List| Learner
    
    Learner -->|Start Quiz| P6_2
    P6_2 -->|Questions Query| D4
    D4 -->|Questions| P6_2
    P6_2 -->|Questions| Learner
    
    Learner -->|Answers| P6_3
    P6_3 -->|Answer Validation| D4
    D4 -->|Correct Answers| P6_3
    P6_3 -->|Score Calculation| D3
    P6_3 -->|Attempt Record| D4
    P6_3 -->|Results| Learner
    
    Admin -->|Quiz Management| P6_4
    P6_4 -->|Quiz CRUD| D4
    D4 -->|Quiz Data| P6_4
    P6_4 -->|Confirmation| Admin
```

### DFD 7.0: Manage Admin Functions

```mermaid
graph TB
    Admin[Administrator]
    
    subgraph Process["Process 7.0: Manage Admin Functions"]
        P7_1[7.1<br/>Manage Users]
        P7_2[7.2<br/>Manage Courses]
        P7_3[7.3<br/>Manage Job Offers]
        P7_4[7.4<br/>View User Details]
    end
    
    subgraph Stores["Data Stores"]
        D1[("D1: User Database")]
        D2[("D2: Course Database")]
        D3[("D3: Progress Database")]
    end
    
    Admin -->|User Management| P7_1
    P7_1 -->|User CRUD| D1
    D1 -->|User Data| P7_1
    P7_1 -->|Results| Admin
    
    Admin -->|Course Management| P7_2
    P7_2 -->|Course CRUD| D2
    D2 -->|Course Data| P7_2
    P7_2 -->|Results| Admin
    
    Admin -->|Job Offer Management| P7_3
    P7_3 -->|Job CRUD| D2
    D2 -->|Job Data| P7_3
    P7_3 -->|Results| Admin
    
    Admin -->|User Details Request| P7_4
    P7_4 -->|User Query| D1
    P7_4 -->|Progress Query| D3
    D1 -->|User Info| P7_4
    D3 -->|Progress Info| P7_4
    P7_4 -->|User Details| Admin
```

### DFD 8.0: Generate Reports

```mermaid
graph TB
    Admin[Administrator]
    
    subgraph Process["Process 8.0: Generate Reports"]
        P8_1[8.1<br/>Collect Statistics]
        P8_2[8.2<br/>Calculate Metrics]
        P8_3[8.3<br/>Format Report]
    end
    
    subgraph Stores["Data Stores"]
        D1[("D1: User Database")]
        D2[("D2: Course Database")]
        D3[("D3: Progress Database")]
        D4[("D4: Quiz Database")]
    end
    
    Admin -->|Report Request| P8_1
    P8_1 -->|User Statistics Query| D1
    P8_1 -->|Course Statistics Query| D2
    P8_1 -->|Progress Statistics Query| D3
    P8_1 -->|Quiz Statistics Query| D4
    
    D1 -->|User Data| P8_1
    D2 -->|Course Data| P8_1
    D3 -->|Progress Data| P8_1
    D4 -->|Quiz Data| P8_1
    
    P8_1 -->|Raw Data| P8_2
    P8_2 -->|Calculated Metrics| P8_3
    P8_3 -->|Formatted Report| Admin
```

---

## Data Dictionary

### Data Stores

#### D1: User Database
- **Description**: Stores all user account information and profiles
- **Contents**:
  - User credentials (email, password hash)
  - User profile (firstName, lastName, level, learningPath)
  - User preferences and onboarding data
  - Admin status flag
- **Access**: Read/Write by authentication, assessment, progress, admin processes

#### D2: Course Database
- **Description**: Stores learning content including courses, lessons, exercises, and job offers
- **Contents**:
  - Courses (title, description, difficulty, learningPath, programmingLanguage)
  - Lessons (title, content, orderNumber, courseId)
  - CodingExercises (title, description, starterCode, expectedOutput)
  - ExerciseTestCases (input, expectedOutput, orderNumber)
  - JobOffers (jobTitle, company, description, requiredSkills, deadline)
- **Access**: Read/Write by learning content, code execution, admin processes

#### D3: Progress Database
- **Description**: Tracks user progress and submissions
- **Contents**:
  - LessonCompletions (userId, lessonId, completionDate, timeSpentMinutes)
  - ExerciseSubmissions (userId, exerciseId, submittedCode, output, isCorrect)
  - UserCourses (userId, courseId, progressPercent, completionDate)
- **Access**: Read/Write by progress tracking, learning content, code execution processes

#### D4: Quiz Database
- **Description**: Stores quiz content and user attempts
- **Contents**:
  - Quizzes (title, description, timeLimitMinutes, passingScore, courseId)
  - QuizQuestions (text, difficulty, orderNumber, quizId)
  - QuizAnswerOptions (text, isCorrect, quizQuestionId)
  - UserQuizAttempts (userId, quizId, score, isPassed, timeSpentMinutes)
  - UserQuizAnswers (userQuizAttemptId, quizQuestionId, selectedAnswerOptionId, isCorrect)
- **Access**: Read/Write by quiz management, progress tracking processes

#### D5: Assessment Database
- **Description**: Stores initial assessment questions and user responses
- **Contents**:
  - Assessments (title)
  - Questions (text, difficulty, assessmentId)
  - AnswerOptions (text, isCorrect, questionId)
  - UserAssessments (userId, assessmentId, score, resultLevel, assignedLearningPath)
  - UserAnswers (userAssessmentId, questionId, answerOptionId, isCorrect)
- **Access**: Read/Write by assessment management process

#### D6: Session Store
- **Description**: Temporary storage for user sessions
- **Contents**:
  - Session ID
  - User ID
  - Session expiration time
  - Authentication tokens
- **Access**: Read/Write by authentication process

#### D7: Code Cache
- **Description**: Temporary storage for code execution results
- **Contents**:
  - Code hash
  - Execution results
  - Output data
  - Cache expiration time
- **Access**: Read/Write by code execution process

### Data Flows

#### Authentication Flows
- **Login Credentials**: Email, Password → Process 1.0
- **Registration Data**: Email, Password, FirstName, LastName → Process 1.0
- **OAuth Token**: Authentication token from Google/GitHub → Process 1.0
- **User Profile**: User information from OAuth provider → Process 1.0
- **Authenticated Session**: Session ID, User ID, Role → External Entities

#### Learning Content Flows
- **Course Request**: Learning path filter → Process 3.0
- **Course Data**: Course list with details → Learner
- **Lesson Request**: Lesson ID → Process 3.0
- **Lesson Content**: Lesson text, code examples, exercises → Learner
- **Completion Status**: Lesson ID, completion flag → Process 3.0

#### Code Execution Flows
- **Code**: User-written code, Exercise ID (optional) → Process 4.0
- **Language Type**: Python or Java → Code Execution System
- **Execution Results**: Output, errors, execution time → Process 4.0
- **Test Results**: Pass/fail status, test case details → Learner

#### Progress Flows
- **Progress Query**: User ID → Process 5.0
- **Progress Statistics**: Completed lessons, exercises, quizzes, study time → Learner
- **Skills List**: Extracted skills from completed content → Learner
- **Weaknesses List**: Topics with low performance → Learner

#### Quiz Flows
- **Quiz Request**: User ID, completed lessons → Process 6.0
- **Available Quizzes**: Quiz list filtered by progress → Learner
- **Quiz Answers**: Selected answer options → Process 6.0
- **Quiz Results**: Score, pass/fail, detailed feedback → Learner

#### Admin Flows
- **Management Requests**: CRUD operations → Process 7.0
- **User Data**: User list, user details → Administrator
- **Course Data**: Course list, course details → Administrator
- **Analytics Data**: Statistics, metrics → Process 8.0
- **Reports**: Formatted analytics report → Administrator

---

## Process Descriptions

### 1.0 Authenticate User
- **Input**: Login credentials, OAuth tokens
- **Output**: Authenticated session, user role
- **Function**: Validates user credentials, creates session, assigns role-based access

### 2.0 Manage Assessment
- **Input**: Assessment answers
- **Output**: Learning path assignment, user profile update
- **Function**: Evaluates answers, calculates score, assigns learning path

### 3.0 Manage Learning Content
- **Input**: Course requests, content management requests
- **Output**: Course content, lesson materials, completion confirmations
- **Function**: Retrieves and displays learning content, tracks completions

### 4.0 Execute Code
- **Input**: User code, exercise ID (optional)
- **Output**: Execution results, test validation results
- **Function**: Executes code in isolated environment, validates against test cases

### 5.0 Track Progress
- **Input**: Progress queries
- **Output**: Progress statistics, skills, weaknesses
- **Function**: Calculates and aggregates user progress metrics

### 6.0 Manage Quizzes
- **Input**: Quiz requests, quiz answers, quiz management
- **Output**: Available quizzes, quiz questions, quiz results
- **Function**: Displays quizzes, collects answers, calculates scores

### 7.0 Manage Admin Functions
- **Input**: Admin management requests
- **Output**: Management results, user/course data
- **Function**: Performs CRUD operations on users, courses, job offers

### 8.0 Generate Reports
- **Input**: Report requests
- **Output**: Analytics reports
- **Function**: Collects statistics, calculates metrics, formats reports

---

## Data Flow Summary

### Input Flows (External → System)
- User registration data
- Login credentials
- OAuth tokens
- Course requests
- Code submissions
- Quiz answers
- Assessment answers
- Admin management requests

### Output Flows (System → External)
- Authenticated sessions
- Course content
- Lesson materials
- Code execution results
- Progress statistics
- Quiz results
- Assessment results
- Management confirmations
- Analytics reports

### Internal Flows (Process → Data Store)
- User data storage/retrieval
- Course data storage/retrieval
- Progress data storage/retrieval
- Quiz data storage/retrieval
- Assessment data storage/retrieval
- Session data storage/retrieval
- Code cache storage/retrieval

---

**End of Document**

