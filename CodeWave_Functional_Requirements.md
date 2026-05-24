# CodeWave – Functional Requirements Document
**Version:** 1.1  
**Project:** CodeWave – Interactive Programming Learning Management System  
**Platform:** ASP.NET Core MVC Web Application  
**Date:** May 2026

---

## Functional Requirements

---

### A. User and Role Management

**FR-01: User Registration**  
The system shall allow new users to register by providing a first name, last name, email address, and password.

**FR-02: Social Authentication**  
The system shall allow users to register and log in using third-party OAuth2 providers, specifically Google and GitHub.

**FR-03: User Login**  
The system shall allow registered users to log in using their email address and password, and redirect administrators to the admin dashboard upon successful authentication.

**FR-04: Role-Based Access Control**  
The system shall restrict access to features based on the user's assigned role and skill level.  
Examples:
- **Regular users** can access learning content, quizzes, projects, and settings.
- **Beginner-level users** are restricted from accessing the CV Builder and Job Board sections.
- **Administrators** can access the admin panel to manage users, courses, and job offers.

**FR-05: User Management (Admin)**  
The administrator shall be able to create, update, delete, and toggle the admin status of any user account through the admin panel.

**FR-06: Password Reset**  
The system shall allow users to request a password reset via their registered email address and reset their password using a secure token-based link.

**FR-07: Password Change**  
The system shall allow authenticated users to change their current password by verifying their old password and entering a new one through the settings page.

**FR-08: Profile Picture Upload**  
The system shall allow users to upload a profile picture (JPG, PNG, or GIF format, maximum 5 MB), replacing any previously uploaded image.

**FR-76: Account Lockout**  
The system shall temporarily lock a user account after a defined number of consecutive failed login attempts in order to prevent brute-force attacks. The lockout duration and attempt threshold shall be configurable.

**FR-77: Email Verification**  
The system shall send a confirmation link to the user's registered email address upon account creation. Full access to learning content shall require the email address to be verified.

---

### B. Onboarding and Initial Skill Assessment

**FR-09: Onboarding Wizard**  
The system shall guide newly registered users through a sequential multi-step onboarding wizard before granting access to the main platform. The steps shall be:
1. Select personal learning interests.
2. Enter a career goal.
3. Select a self-reported current skill level (Beginner, Intermediate, or Advanced). This selection is used as an initial preference only; the final assigned skill level shall be determined by the assessment result as described in FR-12.
4. Enter personal learning motivation.
5. Enter the number of weekly hours available for learning.
6. Select a preferred programming language or track (Python, Java, or Web Development).

**FR-10: Skill Assessment**  
The system shall present each new user with a skill assessment consisting of multiple-choice questions spanning Beginner, Intermediate, and Advanced difficulty levels, covering topics such as HTML, Python, Java, OOP, SQL, algorithms, and REST APIs.

**FR-11: Question-by-Question Navigation**  
The system shall display one assessment question at a time and allow the user to navigate forward and backward between questions, preserving answers already selected.

**FR-12: Assessment Scoring and Level Assignment**  
The system shall automatically score the assessment upon submission and determine the user's proficiency level (Beginner, Intermediate, or Advanced). The assessed skill level shall override the self-reported level entered during onboarding and shall be the authoritative level used for access control and content personalization throughout the platform.

**FR-13: Learning Path Assignment**  
The system shall recommend a learning path based on the assessment result while allowing the user to confirm or change the recommended path. The confirmed learning path and assessed skill level shall be stored on the user's profile and used to personalize course recommendations throughout the platform.

---

### C. Learning Path and Course Management

**FR-14: Learning Path Overview**  
The system shall display an overview page for each available learning path (Python, Java, Web Development) showing the enrolled courses and overall progress percentage.

**FR-15: Course Enrollment and Progress Display**  
The system shall display all courses within a learning path, indicating for each course the title, description, difficulty level, number of lessons, and the user's current progress percentage.

**FR-16: Course Lesson Sidebar**  
The system shall display a navigable sidebar listing all lessons within a course, visually indicating which lessons the user has completed and which remain pending.

**FR-17: Learning Path Switching**  
The system shall allow users to switch their active learning path (e.g., from Python to Java) from the settings page. Upon switching, the user's progress on the new path is preserved from any prior enrollment.

**FR-18: Admin Course Management**  
The administrator shall be able to create, edit, and soft-delete courses. Each course shall include a title, description, difficulty level, programming language, and associated lessons.

**FR-19: Admin Lesson Management**  
The administrator shall be able to add, edit, and soft-delete lessons within a course. Each lesson shall include a title, HTML content, an optional video URL, an optional image URL, and an order number.

---

### D. Lesson and Coding Exercise Management

**FR-20: Lesson Content Viewing**  
The system shall display lesson content including formatted HTML text, an embedded video player (if a video URL is provided), and an associated image (if provided), within the course layout.

**FR-21: Focus Mode**  
The system shall provide a distraction-free full-screen focus mode for any lesson, displaying only the lesson content and its associated coding exercise without the course sidebar or navigation.

**FR-22: Lesson Completion Marking**  
The system shall allow users to mark a lesson as completed. The system shall track time spent on the lesson automatically based on active session duration and store the elapsed time in minutes at the moment the lesson is marked complete. Once all lessons within a course are marked complete, the system shall automatically mark the course as 100% completed.

**FR-23: Coding Exercise Display**  
The system shall display one or more coding exercises associated with each lesson, including the exercise title, description, and starter code pre-loaded into an interactive code editor.

**FR-24: Code Submission and Execution**  
The system shall allow users to write and submit code directly in the browser. The submitted code shall be compiled and executed against a set of predefined test cases specific to the exercise.

**FR-25: Test Case Validation**  
The system shall evaluate the submitted code against all test cases defined for the exercise, in order, comparing the actual program output with the expected output after trimming extra leading and trailing whitespace. The system shall report a pass or fail result per test case and an overall correctness status. The system shall separately display compilation errors, runtime errors, timeout errors, and failed test case outputs so that users can identify the exact point of failure.

**FR-26: Exercise Submission History**  
The system shall save each code submission for a user and exercise, recording the submitted code, output, correctness status, and submission date. The system shall retrieve and display the user's latest submission when revisiting an exercise.

**FR-27: Java Code Execution**  
The system shall support compiling and executing Java code submissions using an isolated backend code runner service with a configurable timeout to prevent infinite loops or resource exhaustion.

**FR-78: Python Code Execution**  
The system shall support executing Python code submissions using an isolated backend code runner service. The runner shall capture standard output and standard error, enforce a configurable execution timeout, and return the results to the user.

**FR-79: Web Development Exercise Support**  
The system shall support Web Development track exercises by providing an in-browser preview or validation environment for HTML, CSS, and JavaScript code. The system shall render the submitted code in an isolated preview panel so that users can observe the visual output directly.

---

### E. Quiz System

**FR-28: Quiz Availability**  
The system shall display quizzes for a course only after the user has completed the required lessons for that course. Quizzes belonging to courses whose lessons have not yet been completed shall remain locked and inaccessible until the required lessons are marked complete.

**FR-29: Timed Quiz Interface**  
The system shall present quizzes with a countdown timer based on the quiz's configured time limit in minutes. The quiz shall auto-submit when the timer reaches zero.

**FR-30: Randomized Answer Options**  
The system shall shuffle the answer options for each quiz question on every attempt to prevent pattern-based guessing. The system shall store each user's submitted answer using the unique identifier of the selected answer option, not by its display position, to ensure result correctness is independent of the shuffled order.

**FR-31: Quiz Submission and Scoring**  
Upon quiz submission, the system shall evaluate each answer, calculate the score as a percentage of correct answers, compare the score against the quiz's passing threshold (default 70%), and record the attempt as passed or failed.

**FR-32: Quiz Results Display**  
The system shall display a detailed results page after quiz submission showing the overall score, pass/fail status, time spent, and a question-by-question breakdown indicating whether each answer was correct, which option the user selected, and what the correct answer was.

**FR-33: Quiz Attempt History**  
The system shall store all quiz attempts for a user, including the score, pass/fail status, start time, completion time, and time spent in minutes.

**FR-34: Quiz Performance Overview**  
The system shall provide users with an overall quiz performance summary showing total quizzes taken, total quizzes passed, and average score across all attempts.

---

### F. Progress Tracking and Analytics

**FR-35: User Dashboard**  
The system shall display a personalized home dashboard for each authenticated user showing:
- Number of courses in progress and completed.
- Overall learning progress percentage.
- Quizzes taken and passed.
- Next recommended lesson to continue.
- Total study time recorded.
- Identified skill strengths and weaknesses.

**FR-36: Lesson Progress Tracking**  
The system shall track the number of lessons completed versus the total number of lessons available within each learning path, and display this as a progress percentage.

**FR-37: Exercise Progress Tracking**  
The system shall track the number of coding exercises solved correctly versus the total number of exercises available within each learning path.

**FR-38: Study Time Tracking**  
The system shall record time spent on each lesson and aggregate total study time per topic and per learning path, displaying this data to the user on the dashboard and progress pages.

**FR-39: Skill Mastery Tracking**  
The system shall calculate a mastery level (0–100) for each topic within a learning path. The mastery score shall be calculated using: 50% weight on the lesson completion rate for the topic, and 50% weight on the exercise correctness rate for the topic. A topic shall be considered mastered when its mastery level reaches or exceeds 80. The system shall visually display each topic's mastery level as a skill proficiency indicator on the user's dashboard.

**FR-40: Weakness Identification**  
The system shall identify topics where the user has low mastery or has made two or more failed exercise attempts, and display them as recommended areas for improvement along with the names of the relevant lessons to revisit.

**FR-41: Course Progress Percentage**  
The system shall calculate and update the user's progress percentage for each enrolled course based on the ratio of completed lessons to total lessons, and display this on course and learning path overview pages.

---

### G. Project Portfolio Management

**FR-42: Project Creation**  
The system shall allow authenticated users to create personal project entries by providing a project title, description, completion date, and a summary of technologies or results achieved.

**FR-43: Project Editing**  
The system shall allow users to edit any of their existing project entries at any time.

**FR-44: Project Deletion**  
The system shall allow users to soft-delete any of their existing project entries. Deleted projects shall no longer appear on the user's portfolio.

**FR-45: Project Listing**  
The system shall display a list of all active projects belonging to the authenticated user, showing the title, description, completion date, and technologies used.

**FR-46: Projects in CV**  
The system shall include the user's saved projects as a section within the CV Builder view, allowing them to be reflected in the generated CV.

---

### H. CV Builder and Career Tools

**FR-47: CV Form Builder**  
The system shall provide a structured CV builder form allowing users to enter and save the following information:
- Personal details: full name, age, location, email, phone number.
- Online profiles: LinkedIn URL, GitHub URL.
- Education: institution, degree, and details.
- Professional summary.
- Work experience (free-text or structured entry).
- Programming languages known.
- Spoken languages.
- Certifications.
- Projects.
- CV profile picture.

**FR-48: CV Auto-Fill**  
The system shall allow users to automatically populate CV fields using data already stored in their profile, including their name, email, completed courses, programming languages, and saved projects, reducing manual data entry.

**FR-49: CV PDF Generation**  
The system shall generate a downloadable PDF version of the user's CV using the data stored in the CV builder, formatted according to the user's selected template.

**FR-50: CV File Upload**  
The system shall allow users to upload an existing CV file (PDF or DOC format, maximum 10 MB) as an alternative to building one from scratch. If the user uploads an external CV file without entering skills in the CV builder form, the system shall require the user to manually enter their programming languages and skills so that job skill matching can be calculated correctly.

**FR-51: CV Template Selection**  
The system shall offer multiple CV layout templates for the user to choose from when generating the PDF, and apply the selected template to the generated document.

**FR-52: CV View with Completed Courses**  
The system shall display a read-only CV preview page that includes the user's personal information, professional summary, skills, projects, and a list of all courses the user has completed on the platform.

**FR-53: CV Section Access Restriction**  
The system shall restrict access to the CV Builder section for users whose assessed skill level is Beginner. These users shall be redirected or shown an access-denied message until their level is upgraded.

---

### I. Job Board and Applications

**FR-54: Job Offer Listing**  
The system shall display a list of active job offers, showing for each position the job title, company name, description, required skills, posting date, and application deadline. Only offers with a deadline on or after the current date shall be shown.

**FR-55: Job Offer Search**  
The system shall allow users to search job offers by keyword, filtering results to show only offers whose title, company, or description match the search term.

**FR-56: Job Skill Matching**  
The system shall calculate and display a skill match percentage for each job offer based on the overlap between the programming languages and skills entered by the user in the CV builder form and the required skills listed in the job offer. Matching shall be case-insensitive. If the user has not entered any skills in the CV builder, the match percentage shall be displayed as 0%.

**FR-57: Job Application Submission**  
The system shall allow users to apply for a job offer by submitting a cover letter. The system shall record the application date, calculate the match percentage at time of submission, and set the initial application status to Pending.

**FR-58: Duplicate Application Prevention**  
The system shall prevent a user from submitting more than one application to the same job offer. If the user has already applied, the apply option shall be replaced with an indicator showing the current application status.

**FR-59: Application Status Tracking**  
The system shall track the status of each job application through the following states: Pending, Reviewed, Accepted, Rejected, and Withdrawn.

**FR-60: Applied Jobs View**  
The system shall display a list of all job offers the user has applied to, showing the job title, company, application date, match percentage, and current application status.

**FR-61: Application Withdrawal**  
The system shall allow users to withdraw a job application that is still in Pending status. Upon withdrawal, the application status shall be updated to Withdrawn. Withdrawn applications shall no longer appear in the user's active applications list.

**FR-62: Job Board Access Restriction**  
The system shall restrict access to the Job Board section for users whose assessed skill level is Beginner. These users shall be redirected or shown an access-denied notice until their level is upgraded.

**FR-63: Admin Job Offer Management**  
The administrator shall be able to create, edit, and soft-delete job offers. Each offer shall include a job title, company name, description, required skills, posting date, and application deadline.

---

### J. Administration and Reporting

**FR-64: Admin Dashboard**  
The system shall provide an administrator-only dashboard displaying key platform metrics, including total registered users, total courses, total job offers, skill level distribution among users, and a user engagement graph filterable by the last 7, 14, or 30 days.

**FR-65: User Management Panel**  
The administrator shall be able to search and paginate through all registered users, view each user's profile details, completion statistics (lessons, exercises, quizzes), and toggle their administrator status or delete their account.

**FR-66: Course Management Panel**  
The administrator shall be able to search and paginate through all courses, create new courses with full lesson sets, edit existing courses and their lessons (including content, order, video, and images), and soft-delete courses.

**FR-67: Job Offer Management Panel**  
The administrator shall be able to view all job offers along with stats such as total active offers, expired offers, and total applications received. The administrator shall be able to create, edit, and soft-delete job offers from this panel.

**FR-80: Admin Job Application View**  
The administrator shall be able to view all applications submitted for each job offer, including the applicant's name, cover letter, skill match percentage, application date, and current application status.

**FR-68: Platform Reports**  
The system shall generate administrative reports including:
- Total and active users broken down by skill level and learning path.
- Course completion rates and most popular courses.
- Exercise submission rates and correctness percentages.
- Job application volume and status distribution.

**FR-69: Admin-Only Access Control**  
The system shall enforce that the admin panel and all its sub-pages are accessible only to users with the administrator role. Any unauthorized access attempt shall be redirected to an access-denied page.

---

### K. System Constraints and Cross-Cutting Requirements

**FR-70: Soft Deletion**  
The system shall implement soft deletion for all major entities (courses, lessons, exercises, quizzes, job offers, projects) so that deleted records are flagged as deleted but retained in the database and excluded from all user-facing queries. Job applications are managed through a status lifecycle (Pending, Reviewed, Accepted, Rejected, Withdrawn) rather than soft deletion.

**FR-71: Unique User Identification**  
The system shall assign a unique GUID-based identifier to every registered user at the time of account creation. This identifier shall be used as the primary key across all related records.

**FR-72: Onboarding Enforcement**  
The system shall detect whether a newly registered or OAuth-authenticated user has completed the onboarding wizard, and redirect them to complete it before allowing access to learning content.

**FR-73: Assessment Enforcement**  
The system shall detect whether an onboarded user has completed the skill assessment, and redirect them to take the assessment before allowing access to courses and learning paths.

**FR-74: File Upload Validation**  
The system shall validate all file uploads for type (allowed formats) and size (maximum limits) before saving them to the server. Rejected uploads shall display an appropriate error message to the user.

**FR-75: Code Execution Timeout**  
The system shall enforce a maximum execution time for all submitted code in the code runner. Code that exceeds the timeout shall be terminated and the user shall be notified that their submission timed out.

---

*End of Functional Requirements Document*
