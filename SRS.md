# Software Requirements Specification (SRS)
## CodeWave Learning Platform

**Version:** 1.0  
**Date:** December 2024  
**Document Status:** Final

---

## Table of Contents

1. [Introduction](#1-introduction)
2. [Overall Description](#2-overall-description)
3. [System Features](#3-system-features)
4. [External Interface Requirements](#4-external-interface-requirements)
5. [System Requirements](#5-system-requirements)
6. [Non-Functional Requirements](#6-non-functional-requirements)
7. [Appendices](#7-appendices)

---

## 1. Introduction

### 1.1 Purpose
This Software Requirements Specification (SRS) document provides a comprehensive description of the CodeWave Learning Platform. It details the functional and non-functional requirements, system architecture, and design constraints for the platform.

### 1.2 Scope
CodeWave is an interactive online learning platform designed to teach programming languages (Python and Java) through structured courses, hands-on coding exercises, quizzes, and progress tracking. The platform serves both learners and administrators, providing a complete learning management system with real-time code execution capabilities.

### 1.3 Definitions, Acronyms, and Abbreviations
- **SRS**: Software Requirements Specification
- **LMS**: Learning Management System
- **IDE**: Integrated Development Environment
- **API**: Application Programming Interface
- **CRUD**: Create, Read, Update, Delete
- **OAuth**: Open Authorization
- **EF Core**: Entity Framework Core
- **MVC**: Model-View-Controller

### 1.4 References
- ASP.NET Core 9.0 Documentation
- Entity Framework Core Documentation
- MySQL 8.0 Documentation
- Identity Framework Documentation

### 1.5 Overview
This document is organized into sections covering system overview, functional requirements, interface requirements, and system constraints. Each section provides detailed specifications for implementation.

---

## 2. Overall Description

### 2.1 Product Perspective
CodeWave is a standalone web application built using ASP.NET Core MVC architecture. It integrates with:
- **Database**: MySQL 8.0 (with SQL Server support)
- **Authentication**: ASP.NET Core Identity with OAuth providers (Google, GitHub)
- **Code Execution**: Docker-based code runners for Java and Python
- **Frontend**: Razor Views with Tailwind CSS

### 2.2 Product Functions
The platform provides the following major functions:

1. **User Management**
   - User registration and authentication
   - Profile management
   - Role-based access control (Admin/Regular User)

2. **Learning Management**
   - Course browsing and enrollment
   - Interactive lessons with content
   - Coding exercises with test cases
   - Real-time code execution
   - Progress tracking

3. **Assessment System**
   - Initial skill assessment
   - Learning path assignment (Python/Java)
   - Quizzes based on completed lessons
   - Performance tracking

4. **Admin Dashboard**
   - User management
   - Course management
   - Job offer management
   - Analytics and reports

5. **Career Tools**
   - Job offer listings
   - Auto-generated CV based on achievements

### 2.3 User Classes and Characteristics

#### 2.3.1 Learners
- **Characteristics**: Students, professionals, or hobbyists learning programming
- **Technical Level**: Beginner to intermediate
- **Goals**: Learn Python or Java through structured courses
- **Access**: Standard user account with learning features

#### 2.3.2 Administrators
- **Characteristics**: Platform managers and content creators
- **Technical Level**: Advanced
- **Goals**: Manage users, courses, content, and platform analytics
- **Access**: Admin account with full system access

### 2.4 Operating Environment
- **Server**: .NET 9.0 runtime
- **Database**: MySQL 8.0 (primary) or SQL Server
- **Web Server**: Kestrel (development), IIS/Apache (production)
- **Browser Support**: Modern browsers (Chrome, Firefox, Safari, Edge)
- **Operating System**: Cross-platform (Windows, macOS, Linux)

### 2.5 Design and Implementation Constraints
- **Architecture**: Clean Architecture with separation of concerns
- **Framework**: ASP.NET Core MVC 9.0
- **Database**: Entity Framework Core with MySQL provider
- **Authentication**: ASP.NET Core Identity
- **Code Execution**: Docker containers for security isolation
- **Frontend**: Server-side rendering with Razor Views

### 2.6 Assumptions and Dependencies
- Users have internet connectivity
- Docker is available for code execution (production)
- MySQL server is accessible
- OAuth providers (Google, GitHub) are configured
- Modern web browser with JavaScript enabled

---

## 3. System Features

### 3.1 User Authentication and Authorization

#### 3.1.1 User Registration
**Priority**: High  
**Description**: Users can create accounts through multiple methods.

**Functional Requirements**:
- FR-1.1: System shall allow users to register with email and password
- FR-1.2: System shall support OAuth registration via Google
- FR-1.3: System shall support OAuth registration via GitHub
- FR-1.4: System shall validate email format and password strength
- FR-1.5: System shall redirect new users to onboarding flow
- FR-1.6: System shall store user profile information (FirstName, LastName, Email)

**Input**: Email, Password, First Name, Last Name (optional)  
**Output**: User account created, redirect to onboarding

#### 3.1.2 User Login
**Priority**: High  
**Description**: Users can authenticate using multiple methods.

**Functional Requirements**:
- FR-2.1: System shall allow email/password login
- FR-2.2: System shall support Google OAuth login
- FR-2.3: System shall support GitHub OAuth login
- FR-2.4: System shall redirect admins to `/Admin` dashboard after login
- FR-2.5: System shall redirect regular users to `/Home` dashboard after login
- FR-2.6: System shall maintain session state after login
- FR-2.7: System shall handle invalid credentials with error messages

**Input**: Email/Username, Password (or OAuth token)  
**Output**: Authenticated session, redirect to appropriate dashboard

#### 3.1.3 Role-Based Access Control
**Priority**: High  
**Description**: System enforces different access levels for users and admins.

**Functional Requirements**:
- FR-3.1: System shall support two user roles: Admin and Regular User
- FR-3.2: System shall restrict admin pages to users with `IsAdmin = true`
- FR-3.3: System shall use policy-based authorization (`AdminOnly` policy)
- FR-3.4: System shall add `IsAdmin` claim to user principal for admins

**Input**: User identity  
**Output**: Access granted or denied based on role

### 3.2 Learning Path Assessment

#### 3.2.1 Initial Assessment
**Priority**: High  
**Description**: New users complete an assessment to determine their learning path.

**Functional Requirements**:
- FR-4.1: System shall present assessment questions to new users
- FR-4.2: System shall collect user answers to assessment questions
- FR-4.3: System shall evaluate answers to determine skill level
- FR-4.4: System shall assign learning path (Python or Java) based on assessment
- FR-4.5: System shall set user's `Level` and `LearningPath` properties
- FR-4.6: System shall redirect users to their assigned learning path dashboard

**Input**: Assessment answers  
**Output**: Learning path assignment, user profile updated

### 3.3 Course Management

#### 3.3.1 Course Browsing
**Priority**: High  
**Description**: Users can view available courses.

**Functional Requirements**:
- FR-5.1: System shall display courses filtered by user's learning path
- FR-5.2: System shall show course title, description, difficulty level
- FR-5.3: System shall display course progress for enrolled users
- FR-5.4: System shall allow users to navigate to course details

**Input**: User's learning path  
**Output**: List of relevant courses

#### 3.3.2 Course Enrollment
**Priority**: High  
**Description**: Users can enroll in courses.

**Functional Requirements**:
- FR-6.1: System shall automatically enroll users in courses matching their learning path
- FR-6.2: System shall create `UserCourse` entry upon enrollment
- FR-6.3: System shall track enrollment date

**Input**: Course selection  
**Output**: Enrollment confirmation

#### 3.3.3 Course Content
**Priority**: High  
**Description**: Users can access course lessons and exercises.

**Functional Requirements**:
- FR-7.1: System shall display course lessons in order
- FR-7.2: System shall show lesson content with code examples
- FR-7.3: System shall provide coding exercises for each lesson
- FR-7.4: System shall track lesson completion
- FR-7.5: System shall unlock next lesson upon completion

**Input**: Course ID, Lesson ID  
**Output**: Lesson content, exercises

### 3.4 Interactive Code Editor

#### 3.4.1 Code Editor Interface
**Priority**: High  
**Description**: Users can write and edit code in an IDE-like environment.

**Functional Requirements**:
- FR-8.1: System shall provide Monaco Editor for code editing
- FR-8.2: System shall support syntax highlighting for Python and Java
- FR-8.3: System shall provide code snippets/suggestions
- FR-8.4: System shall auto-save user code to local storage
- FR-8.5: System shall allow code execution without exercise selection (experiment mode)
- FR-8.6: System shall display code editor in Focus Mode lesson view

**Input**: User code input  
**Output**: Code editor interface with syntax highlighting

#### 3.4.2 Code Execution
**Priority**: High  
**Description**: Users can execute code and see results.

**Functional Requirements**:
- FR-9.1: System shall execute Python code using Python 3 interpreter
- FR-9.2: System shall execute Java code using Java compiler and runtime
- FR-9.3: System shall detect programming language from course context
- FR-9.4: System shall run code in isolated Docker containers
- FR-9.5: System shall display execution output in console
- FR-9.6: System shall handle compilation and runtime errors
- FR-9.7: System shall support code execution with or without exercise context

**Input**: Code, Language, Exercise ID (optional)  
**Output**: Execution results, output, errors

#### 3.4.3 Exercise Submission
**Priority**: High  
**Description**: Users can submit code for exercises and receive feedback.

**Functional Requirements**:
- FR-10.1: System shall validate code against multiple test cases
- FR-10.2: System shall compare output with expected results
- FR-10.3: System shall provide detailed test case results
- FR-10.4: System shall mark exercise as completed when all tests pass
- FR-10.5: System shall save exercise submission with timestamp
- FR-10.6: System shall unlock next lesson upon exercise completion

**Input**: Code, Exercise ID  
**Output**: Test results, completion status

### 3.5 Progress Tracking

#### 3.5.1 User Dashboard
**Priority**: High  
**Description**: Users can view their learning progress.

**Functional Requirements**:
- FR-11.1: System shall display completed lessons count
- FR-11.2: System shall display completed exercises count
- FR-11.3: System shall display quiz attempts and scores
- FR-11.4: System shall calculate and display total study time
- FR-11.5: System shall show dynamically acquired skills
- FR-11.6: System shall identify user weaknesses
- FR-11.7: System shall display learning path progress percentage

**Input**: User ID  
**Output**: Progress statistics, skills, weaknesses

#### 3.5.2 Lesson Completion Tracking
**Priority**: High  
**Description**: System tracks which lessons users have completed.

**Functional Requirements**:
- FR-12.1: System shall record lesson completion with timestamp
- FR-12.2: System shall prevent duplicate completion entries
- FR-12.3: System shall calculate course progress based on completions

**Input**: Lesson ID, User ID  
**Output**: Completion record saved

### 3.6 Quiz System

#### 3.6.1 Quiz Display
**Priority**: Medium  
**Description**: Users can view available quizzes.

**Functional Requirements**:
- FR-13.1: System shall display quizzes based on user's completed lessons
- FR-13.2: System shall filter quizzes by learning path
- FR-13.3: System shall show quiz title, description, question count
- FR-13.4: System shall display quiz icon on home page

**Input**: User ID, Completed lessons  
**Output**: List of available quizzes

#### 3.6.2 Quiz Taking
**Priority**: Medium  
**Description**: Users can take quizzes and answer questions.

**Functional Requirements**:
- FR-14.1: System shall present quiz questions one at a time or all at once
- FR-14.2: System shall provide multiple choice answer options
- FR-14.3: System shall allow users to select answers
- FR-14.4: System shall track time spent on quiz
- FR-14.5: System shall validate that quiz has at least one question

**Input**: Quiz ID, Selected answers  
**Output**: Quiz interface with questions

#### 3.6.3 Quiz Results
**Priority**: Medium  
**Description**: Users can view quiz results and performance.

**Functional Requirements**:
- FR-15.1: System shall calculate quiz score (percentage)
- FR-15.2: System shall determine pass/fail status (threshold configurable)
- FR-15.3: System shall save quiz attempt with score and timestamp
- FR-15.4: System shall display correct/incorrect answers
- FR-15.5: System shall track user performance over time

**Input**: Quiz answers, Quiz ID  
**Output**: Score, pass/fail status, detailed results

### 3.7 Focus Mode

#### 3.7.1 Focus Mode Lesson View
**Priority**: Medium  
**Description**: Users can view lessons in a distraction-free environment.

**Functional Requirements**:
- FR-16.1: System shall display lesson content in full-screen focus mode
- FR-16.2: System shall provide code editor in focus mode
- FR-16.3: System shall allow exercise selection within lesson
- FR-16.4: System shall provide navigation to previous/next lesson
- FR-16.5: System shall make page scrollable for long content
- FR-16.6: System shall integrate AI Helper chat

**Input**: Lesson ID  
**Output**: Focus mode lesson interface

#### 3.7.2 AI Helper Chat
**Priority**: Low  
**Description**: Users can get help from an AI assistant.

**Functional Requirements**:
- FR-17.1: System shall provide chat interface in focus mode
- FR-17.2: System shall respond to user questions with rule-based answers
- FR-17.3: System shall provide quick action buttons
- FR-17.4: System shall maintain chat history during session

**Input**: User message  
**Output**: AI response

### 3.8 Admin Dashboard

#### 3.8.1 Admin Dashboard Overview
**Priority**: High  
**Description**: Admins can view platform statistics.

**Functional Requirements**:
- FR-18.1: System shall display total users count
- FR-18.2: System shall display total courses count
- FR-18.3: System shall display total job offers count
- FR-18.4: System shall show recent activity feed
- FR-18.5: System shall provide navigation to management pages

**Input**: None  
**Output**: Dashboard with KPIs and statistics

#### 3.8.2 User Management
**Priority**: High  
**Description**: Admins can manage user accounts.

**Functional Requirements**:
- FR-19.1: System shall list all users with pagination
- FR-19.2: System shall allow search by email, name
- FR-19.3: System shall allow creating new users
- FR-19.4: System shall allow editing user details
- FR-19.5: System shall allow toggling admin status
- FR-19.6: System shall allow deleting users
- FR-19.7: System shall display user details (progress, courses, quizzes)
- FR-19.8: System shall show user statistics (lessons, exercises, quizzes completed)

**Input**: User data, search terms  
**Output**: User list, user details, CRUD operations

#### 3.8.3 Course Management
**Priority**: High  
**Description**: Admins can manage courses.

**Functional Requirements**:
- FR-20.1: System shall list all courses with pagination
- FR-20.2: System shall allow creating new courses
- FR-20.3: System shall allow editing course details (title, description, difficulty)
- FR-20.4: System shall allow deleting courses (soft delete)
- FR-20.5: System shall filter courses by learning path

**Input**: Course data  
**Output**: Course list, CRUD operations

#### 3.8.4 Job Offer Management
**Priority**: Medium  
**Description**: Admins can manage job offers.

**Functional Requirements**:
- FR-21.1: System shall list all job offers with pagination
- FR-21.2: System shall allow creating new job offers
- FR-21.3: System shall allow editing job offer details
- FR-21.4: System shall allow deleting job offers (soft delete)
- FR-21.5: System shall track posted date and deadline

**Input**: Job offer data  
**Output**: Job offer list, CRUD operations

#### 3.8.5 Reports and Analytics
**Priority**: Medium  
**Description**: Admins can view platform analytics.

**Functional Requirements**:
- FR-22.1: System shall display user progress statistics
- FR-22.2: System shall show course enrollment distribution
- FR-22.3: System shall display top performing users
- FR-22.4: System shall show platform usage statistics
- FR-22.5: System shall provide export functionality (future)

**Input**: None  
**Output**: Analytics dashboard, charts, statistics

### 3.9 Job Offers

#### 3.9.1 Job Offer Display
**Priority**: Medium  
**Description**: Users can view available job opportunities.

**Functional Requirements**:
- FR-23.1: System shall display job offers on home page
- FR-23.2: System shall show job title, company, description, required skills
- FR-23.3: System shall filter job offers by relevance
- FR-23.4: System shall display application deadline

**Input**: None  
**Output**: List of job offers

### 3.10 CV Generation

#### 3.10.1 Auto-Generated CV
**Priority**: Low  
**Description**: System generates CV based on user achievements.

**Functional Requirements**:
- FR-24.1: System shall collect user's completed courses
- FR-24.2: System shall include user's skills and achievements
- FR-24.3: System shall generate downloadable CV document
- FR-24.4: System shall format CV professionally

**Input**: User achievements  
**Output**: CV document

---

## 4. External Interface Requirements

### 4.1 User Interfaces

#### 4.1.1 Web Interface
- **Technology**: ASP.NET Core Razor Views with Tailwind CSS
- **Responsive Design**: Mobile-first approach, supports desktop and mobile devices
- **Browser Support**: Chrome, Firefox, Safari, Edge (latest versions)
- **Accessibility**: WCAG 2.1 Level AA compliance (target)

#### 4.1.2 Key UI Components
- **Dashboard**: User progress, statistics, quick actions
- **Course View**: Lesson list, content display, code editor
- **Focus Mode**: Full-screen lesson view with code editor
- **Admin Panel**: Management interfaces for users, courses, jobs
- **Quiz Interface**: Question display, answer selection, results

### 4.2 Hardware Interfaces
- **Server**: Standard web server hardware
- **Client**: Any device with web browser and internet connection
- **Code Execution**: Docker-enabled server for code runners

### 4.3 Software Interfaces

#### 4.3.1 Database
- **Primary**: MySQL 8.0
- **Alternative**: SQL Server (configurable)
- **ORM**: Entity Framework Core 9.0
- **Provider**: Pomelo.EntityFrameworkCore.MySql

#### 4.3.2 Authentication Providers
- **Google OAuth 2.0**: User authentication via Google accounts
- **GitHub OAuth**: User authentication via GitHub accounts
- **Identity Framework**: Email/password authentication

#### 4.3.3 Code Execution
- **Docker**: Containerized code execution for security
- **Java**: OpenJDK for Java code compilation and execution
- **Python**: Python 3 interpreter for Python code execution

### 4.4 Communication Interfaces
- **HTTP/HTTPS**: Standard web protocols
- **RESTful API**: For code execution endpoints
- **WebSockets**: (Future enhancement for real-time features)

---

## 5. System Requirements

### 5.1 Functional Requirements Summary

| ID | Requirement | Priority | Status |
|----|-------------|----------|--------|
| FR-1.1 to FR-1.6 | User Registration | High | Implemented |
| FR-2.1 to FR-2.7 | User Login | High | Implemented |
| FR-3.1 to FR-3.4 | Role-Based Access | High | Implemented |
| FR-4.1 to FR-4.6 | Initial Assessment | High | Implemented |
| FR-5.1 to FR-5.4 | Course Browsing | High | Implemented |
| FR-6.1 to FR-6.3 | Course Enrollment | High | Implemented |
| FR-7.1 to FR-7.5 | Course Content | High | Implemented |
| FR-8.1 to FR-8.6 | Code Editor Interface | High | Implemented |
| FR-9.1 to FR-9.7 | Code Execution | High | Implemented |
| FR-10.1 to FR-10.6 | Exercise Submission | High | Implemented |
| FR-11.1 to FR-11.7 | User Dashboard | High | Implemented |
| FR-12.1 to FR-12.3 | Lesson Completion | High | Implemented |
| FR-13.1 to FR-13.4 | Quiz Display | Medium | Implemented |
| FR-14.1 to FR-14.5 | Quiz Taking | Medium | Implemented |
| FR-15.1 to FR-15.5 | Quiz Results | Medium | Implemented |
| FR-16.1 to FR-16.6 | Focus Mode | Medium | Implemented |
| FR-17.1 to FR-17.4 | AI Helper Chat | Low | Implemented |
| FR-18.1 to FR-18.5 | Admin Dashboard | High | Implemented |
| FR-19.1 to FR-19.8 | User Management | High | Implemented |
| FR-20.1 to FR-20.5 | Course Management | High | Implemented |
| FR-21.1 to FR-21.5 | Job Offer Management | Medium | Implemented |
| FR-22.1 to FR-22.5 | Reports and Analytics | Medium | Implemented |
| FR-23.1 to FR-23.4 | Job Offer Display | Medium | Implemented |
| FR-24.1 to FR-24.4 | CV Generation | Low | Planned |

### 5.2 Database Schema

#### 5.2.1 Core Entities
- **ApplicationUser**: User accounts, authentication, profile
- **Course**: Learning courses with metadata
- **Lesson**: Individual lessons within courses
- **CodingExercise**: Programming exercises with test cases
- **ExerciseTestCase**: Test cases for exercises
- **ExerciseSubmission**: User code submissions
- **LessonCompletion**: Lesson completion tracking
- **Quiz**: Quiz definitions
- **QuizQuestion**: Questions within quizzes
- **AnswerOption**: Answer options for questions
- **UserQuizAttempt**: User quiz attempts and scores
- **UserQuizAnswer**: Individual answers to quiz questions
- **Assessment**: Initial skill assessment
- **UserAssessment**: User assessment responses
- **JobOffer**: Job opportunity listings
- **UserCourse**: Course enrollment tracking
- **CV**: Generated CV documents

### 5.3 API Endpoints

#### 5.3.1 Code Execution API
- `POST /api/code/run`: Execute code (with or without exercise)
- `POST /api/code/submit`: Submit code for exercise validation

#### 5.3.2 Progress API
- `GET /api/progress/user`: Get user progress statistics
- `GET /api/progress/skills`: Get user acquired skills
- `GET /api/progress/weaknesses`: Get user weaknesses

#### 5.3.3 Quiz API
- `GET /api/quiz/available`: Get available quizzes for user
- `POST /api/quiz/submit`: Submit quiz answers

---

## 6. Non-Functional Requirements

### 6.1 Performance Requirements
- **Page Load Time**: < 2 seconds for standard pages
- **Code Execution**: < 5 seconds for typical code snippets
- **Database Queries**: < 500ms for standard queries
- **Concurrent Users**: Support at least 100 concurrent users
- **Response Time**: API endpoints respond within 1 second

### 6.2 Security Requirements
- **Authentication**: Secure password hashing (Identity framework)
- **Authorization**: Role-based access control with policies
- **Code Execution**: Isolated Docker containers to prevent security breaches
- **SQL Injection**: Prevented via Entity Framework parameterized queries
- **XSS Protection**: Input sanitization and output encoding
- **CSRF Protection**: Anti-forgery tokens on forms
- **HTTPS**: All communications encrypted in production
- **Session Management**: Secure session handling with timeout

### 6.3 Reliability Requirements
- **Uptime**: 99.5% availability target
- **Error Handling**: Graceful error messages, no system crashes
- **Data Backup**: Daily database backups
- **Recovery**: System recovery within 4 hours of failure

### 6.4 Usability Requirements
- **User Interface**: Intuitive, modern design with Tailwind CSS
- **Responsive Design**: Works on desktop, tablet, and mobile devices
- **Accessibility**: Keyboard navigation, screen reader support (target)
- **Help System**: Tooltips, help text, AI Helper chat
- **Error Messages**: Clear, actionable error messages

### 6.5 Scalability Requirements
- **Horizontal Scaling**: Architecture supports load balancing
- **Database**: Supports read replicas for scaling
- **Code Execution**: Docker containers can scale independently
- **Caching**: Implement caching for frequently accessed data (future)

### 6.6 Maintainability Requirements
- **Code Organization**: Clean Architecture with separation of concerns
- **Documentation**: Code comments, API documentation
- **Testing**: Unit tests, integration tests (target)
- **Version Control**: Git-based version control
- **Logging**: Comprehensive logging for debugging and monitoring

### 6.7 Portability Requirements
- **Cross-Platform**: Runs on Windows, macOS, Linux
- **Database Agnostic**: Supports MySQL and SQL Server
- **Browser Compatibility**: Works on major modern browsers

---

## 7. Appendices

### 7.1 Glossary

- **Learning Path**: A structured sequence of courses (e.g., Python Path, Java Path)
- **Focus Mode**: A distraction-free lesson viewing mode
- **Exercise**: A coding challenge with test cases
- **Test Case**: Input/output pair for validating code
- **Soft Delete**: Marking records as deleted without removing from database
- **Monaco Editor**: VS Code's code editor component
- **Docker Container**: Isolated environment for code execution

### 7.2 Acronyms

- **API**: Application Programming Interface
- **CRUD**: Create, Read, Update, Delete
- **EF Core**: Entity Framework Core
- **HTTP**: Hypertext Transfer Protocol
- **HTTPS**: HTTP Secure
- **IDE**: Integrated Development Environment
- **LMS**: Learning Management System
- **MVC**: Model-View-Controller
- **OAuth**: Open Authorization
- **ORM**: Object-Relational Mapping
- **REST**: Representational State Transfer
- **SRS**: Software Requirements Specification
- **UI**: User Interface
- **UX**: User Experience
- **XSS**: Cross-Site Scripting
- **CSRF**: Cross-Site Request Forgery

### 7.3 Change Log

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | December 2024 | Development Team | Initial SRS document |

---

## Document Approval

**Prepared by**: CodeWave Development Team  
**Reviewed by**: [To be filled]  
**Approved by**: [To be filled]  
**Date**: December 2024

---

**End of Document**

