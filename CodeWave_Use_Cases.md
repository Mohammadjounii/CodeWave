# CodeWave – Use Cases Document
**Version:** 1.0
**Project:** CodeWave – Interactive Programming Learning Management System
**Platform:** ASP.NET Core MVC Web Application
**Date:** May 2026

---

UC-01: Register New User
Primary Actor
New User (Learner)
Goal
Create a new account on the CodeWave platform.
Preconditions
The user has a valid email address not already registered in the system.
Main Flow
1. The user opens the registration page.
2. The system displays the registration form.
3. The user enters their first name, last name, email address, and password.
4. The user submits the form.
5. The system validates that all required fields are filled and the email is not already in use.
6. The system creates a new user account.
7. The system assigns a unique GUID-based identifier to the account.
8. The system redirects the user to the onboarding wizard.
Alternative Flows
A1: Register Using Google or GitHub
1. The user clicks the Google or GitHub sign-in button.
2. The system redirects the user to the selected OAuth2 provider.
3. The user authenticates with the provider.
4. The provider returns the user's identity to the system.
5. The system creates the account using the returned email and name.
6. The system redirects the user to the onboarding wizard.
Exception Flows
E1: Email Already Registered
1. The system detects that the entered email is already linked to an existing account.
2. The system displays an error message informing the user.
3. The user is prompted to log in or use a different email address.
E2: Password Does Not Meet Requirements
1. The system rejects the password if it does not meet the minimum security requirements.
2. The system displays the password requirements to the user.
3. The user enters a valid password and resubmits.

---

UC-02: Log In to the Platform
Primary Actor
Registered User or Administrator
Goal
Authenticate and access the platform.
Preconditions
The user has a registered account.
Main Flow
1. The user opens the login page.
2. The system displays the login form.
3. The user enters their email address and password.
4. The user submits the form.
5. The system validates the credentials.
6. If the user is an administrator, the system redirects them to the admin dashboard.
7. If the user is a regular learner, the system checks whether onboarding and assessment are complete.
8. The system redirects the user to the appropriate page based on their status.
Alternative Flows
A1: Log In Using Google or GitHub
1. The user clicks the Google or GitHub sign-in button.
2. The system redirects the user to the selected provider.
3. The user authenticates successfully.
4. The system logs the user in and redirects them based on their role and onboarding status.
A2: User Has Not Completed Onboarding
1. The system detects that onboarding is incomplete.
2. The system redirects the user to the onboarding wizard before granting access to learning content.
A3: User Has Not Completed Assessment
1. The system detects that the assessment has not been taken.
2. The system redirects the user to the skill assessment page.
Exception Flows
E1: Invalid Credentials
1. The system cannot match the entered email and password to any account.
2. The system displays an error message.
3. The user may retry or use the forgot password option.
E2: Account Locked
1. The system detects that the account has been locked due to repeated failed login attempts.
2. The system displays a lockout message.
3. The user must wait for the lockout period to expire or contact support.

---

UC-03: Complete Onboarding Wizard
Primary Actor
New User (Learner)
Goal
Provide personal learning preferences to personalize the platform experience.
Preconditions
The user has a registered account and has not yet completed onboarding.
Main Flow
1. The system redirects the user to the first step of the onboarding wizard.
2. The user selects their personal learning interests from a list.
3. The user submits and proceeds to the next step.
4. The user enters their career goal.
5. The user submits and proceeds to the next step.
6. The user selects a self-reported current skill level (Beginner, Intermediate, or Advanced).
7. The user submits and proceeds to the next step.
8. The user enters their personal learning motivation.
9. The user submits and proceeds to the next step.
10. The user enters the number of weekly hours they can dedicate to learning.
11. The user submits and proceeds to the next step.
12. The user selects a preferred programming language or track (Python, Java, or Web Development).
13. The user submits the final step.
14. The system saves all onboarding data to the user's profile.
15. The system redirects the user to the skill assessment.
Alternative Flows
A1: User Navigates Back to a Previous Step
1. The user clicks the back button during onboarding.
2. The system returns to the previous step with the previously entered data preserved.
3. The user updates their answer and proceeds forward again.
Exception Flows
E1: User Closes the Wizard Before Completing It
1. The user closes the browser or navigates away before completing all steps.
2. The system does not save incomplete onboarding data.
3. The next time the user logs in, the system redirects them back to the incomplete step.

---

UC-04: Take Skill Assessment
Primary Actor
New User (Learner)
Goal
Complete the skill assessment to receive an assigned proficiency level and recommended learning path.
Preconditions
The user has completed the onboarding wizard.
Main Flow
1. The system redirects the user to the skill assessment page.
2. The system displays the first question with multiple-choice answer options.
3. The user selects an answer.
4. The user clicks Next to proceed to the following question.
5. The system saves the selected answer and displays the next question.
6. Steps 3 through 5 repeat for all questions in the assessment.
7. The user submits the assessment after answering the final question.
8. The system scores the assessment based on the number of correct answers.
9. The system determines the user's proficiency level (Beginner, Intermediate, or Advanced).
10. The system assigns a recommended learning path based on the result.
11. The system saves the assessed level and learning path to the user's profile.
12. The system redirects the user to their learning dashboard.
Alternative Flows
A1: User Navigates Back to a Previous Question
1. The user clicks the Previous button on any question.
2. The system displays the previous question with the previously selected answer preserved.
3. The user may change their answer and navigate forward again.
Exception Flows
E1: Assessment Cannot Be Submitted
1. The system encounters an error when saving the assessment result.
2. The system displays an error message.
3. The user is prompted to retry submission.
4. The system logs the error for administrator review.

---

UC-05: Start and Navigate a Course
Primary Actor
Learner
Goal
Access and navigate through the lessons of a course within the assigned learning path.
Preconditions
The user is logged in and has completed onboarding and assessment.
Main Flow
1. The user navigates to their learning path overview page.
2. The system displays all courses within the assigned learning path with progress percentages.
3. The user selects a course.
4. The system displays the course page with a lesson sidebar listing all lessons.
5. Completed lessons are visually marked in the sidebar.
6. The user selects a lesson from the sidebar.
7. The system loads and displays the lesson content, including HTML text, video if available, and image if available.
8. The user reads or watches the lesson content.
Alternative Flows
A1: User Activates Focus Mode
1. The user clicks the Focus Mode button on a lesson.
2. The system hides the sidebar and navigation and displays the lesson in full-screen mode.
3. The user completes the lesson in a distraction-free environment.
A2: User Switches Learning Path
1. The user navigates to settings and selects a different learning path.
2. The system updates the active learning path on the user's profile.
3. The system preserves any progress the user had on the new path from prior activity.
Exception Flows
E1: Course or Lesson Fails to Load
1. The system fails to retrieve the lesson content.
2. The system displays an error message.
3. The user may reload the page or return to the course overview.

---

UC-06: Complete a Lesson
Primary Actor
Learner
Goal
Mark a lesson as completed and record study time.
Preconditions
The user is viewing a lesson within a course.
Main Flow
1. The user reads or watches the lesson content.
2. The system tracks time spent on the lesson based on active session duration.
3. The user clicks the Mark as Complete button.
4. The system records the lesson completion with the current date and elapsed time in minutes.
5. The system updates the course progress percentage.
6. The system visually marks the lesson as completed in the sidebar.
7. If all lessons in the course are now complete, the system automatically marks the course as 100% completed.
8. The system recommends the next lesson if one is available.
Alternative Flows
A1: Lesson Already Marked as Complete
1. The user revisits a lesson they previously completed.
2. The system displays the lesson content normally.
3. The completion status remains unchanged.
Exception Flows
E1: Completion Cannot Be Saved
1. The system fails to save the completion record.
2. The system displays an error message.
3. The lesson progress is not updated until the issue is resolved.

---

UC-07: Submit a Coding Exercise
Primary Actor
Learner
Goal
Write and submit code for an exercise and receive test case feedback.
Preconditions
The user is viewing a lesson that has one or more coding exercises.
Main Flow
1. The system displays the coding exercise with the title, description, and starter code loaded into the code editor.
2. The user reads the exercise description.
3. The user writes or modifies the code in the editor.
4. The user clicks the Submit button.
5. The system sends the code to the backend code runner.
6. The code runner executes the code against all defined test cases in order.
7. The system compares the actual output to the expected output for each test case after trimming whitespace.
8. The system returns a pass or fail result for each test case and an overall correctness status.
9. The system displays the results to the user.
10. The system saves the submission with the code, output, correctness status, and submission date.
Alternative Flows
A1: User Revisits a Previously Submitted Exercise
1. The system loads the user's last submitted code into the editor automatically.
2. The user may modify and resubmit.
Exception Flows
E1: Compilation Error
1. The code runner fails to compile the submitted code.
2. The system displays the compilation error message to the user.
3. The submission is saved with an incorrect status.
4. The user corrects the code and resubmits.
E2: Code Execution Timeout
1. The code runner exceeds the maximum allowed execution time.
2. The system terminates the process.
3. The system notifies the user that their submission timed out.
4. The user optimizes the code and resubmits.
E3: Runtime Error
1. The code compiles but throws a runtime exception during execution.
2. The system displays the runtime error to the user.
3. The submission is saved with an incorrect status.

---

UC-08: Take a Quiz
Primary Actor
Learner
Goal
Attempt a quiz for a course and receive a score and pass or fail result.
Preconditions
The user is logged in.
The required lessons for the quiz course have been completed.
Main Flow
1. The user navigates to the quiz listing page.
2. The system displays all available quizzes with attempt history if applicable.
3. The user selects a quiz and clicks Start.
4. The system creates a quiz attempt record with the current timestamp.
5. The system displays the quiz interface with a countdown timer.
6. Answer options for each question are shuffled on display.
7. The user selects an answer for each question.
8. The user submits the quiz before the timer expires.
9. The system evaluates each answer using unique option identifiers.
10. The system calculates the score as a percentage of correct answers.
11. The system compares the score to the passing threshold (default 70%).
12. The system records the attempt as passed or failed with the time spent.
13. The system redirects the user to the results page.
Alternative Flows
A1: Timer Expires Before Submission
1. The countdown timer reaches zero.
2. The system automatically submits all answers selected up to that point.
3. Unanswered questions are treated as incorrect.
4. The system proceeds to scoring and results display.
A2: User Retakes a Previously Attempted Quiz
1. The user selects a quiz they have already attempted.
2. The system creates a new attempt record.
3. The system shuffles the answer options again.
4. All previous attempt history is preserved alongside the new attempt.
Exception Flows
E1: Quiz Submission Fails
1. The system encounters an error when saving the attempt.
2. The system displays an error message.
3. The quiz attempt is not recorded.
4. The user is prompted to retry.

---

UC-09: Build and Download CV
Primary Actor
Learner (Intermediate or Advanced level)
Goal
Create a professional CV using the CV builder and download it as a PDF.
Preconditions
The user is logged in.
The user's assessed skill level is Intermediate or Advanced.
Main Flow
1. The user navigates to the CV Builder section.
2. The system displays the CV form with all input sections.
3. The user fills in personal details, education, work experience, programming languages, spoken languages, certifications, and projects.
4. The user selects a CV template from the available options.
5. The user uploads a CV profile picture if desired.
6. The user clicks Save.
7. The system saves all CV data to the user's profile.
8. The user clicks Generate PDF.
9. The system generates a formatted PDF using the saved data and the selected template.
10. The system makes the PDF available for download.
11. The user downloads the PDF.
Alternative Flows
A1: User Uses Auto-Fill
1. The user clicks the Auto-Fill button.
2. The system populates the CV form fields using data from the user's profile including name, email, completed courses, programming languages, and saved projects.
3. The user reviews and edits the auto-filled data before saving.
A2: User Uploads an Existing CV File
1. The user uploads an existing PDF or DOC CV file (maximum 10 MB).
2. The system stores the uploaded file.
3. The system reminds the user to manually enter their skills in the form fields so that job skill matching can function correctly.
Exception Flows
E1: PDF Generation Fails
1. The system fails to generate the PDF.
2. The system displays an error message.
3. The user may retry after verifying all required fields are filled.
E2: Beginner User Attempts to Access CV Builder
1. The system detects that the user's assessed level is Beginner.
2. The system blocks access and displays an access-denied message.
3. The user is redirected to their learning dashboard.

---

UC-10: Apply for a Job Offer
Primary Actor
Learner (Intermediate or Advanced level)
Goal
Submit a job application with a cover letter for a listed job offer.
Preconditions
The user is logged in.
The user's assessed skill level is Intermediate or Advanced.
Main Flow
1. The user navigates to the Job Board page.
2. The system displays all active job offers with deadlines on or after the current date.
3. The system displays a skill match percentage for each offer based on the user's CV skills.
4. The user browses or searches for a suitable job offer.
5. The user selects a job offer to view its full details.
6. The user clicks Apply.
7. The system displays the application form with a cover letter field.
8. The user writes and submits the cover letter.
9. The system records the application date and calculates the match percentage at time of submission.
10. The system sets the initial application status to Pending.
11. The system confirms the application and updates the apply button to show the current status.
Alternative Flows
A1: User Has Already Applied for This Job
1. The system detects an existing application for the same job offer.
2. The system replaces the Apply button with the current application status indicator.
3. The user cannot submit a second application to the same offer.
A2: User Searches for a Specific Job
1. The user enters a keyword in the search field.
2. The system filters the job offer list to show only offers whose title, company, or description match the keyword.
3. The user selects from the filtered results.
A3: User Withdraws an Application
1. The user navigates to the Applied Jobs view.
2. The user selects a Pending application and clicks Withdraw.
3. The system updates the application status to Withdrawn.
4. The application no longer appears in the user's active applications list.
Exception Flows
E1: Job Offer Deadline Has Passed
1. The system does not display expired offers in the listing.
2. If the user accesses an expired offer directly, the system blocks the application.
3. The system informs the user that the offer has closed.
E2: Beginner User Attempts to Access Job Board
1. The system detects that the user's assessed level is Beginner.
2. The system blocks access and displays an access-denied notice.
3. The user is redirected to their learning dashboard.

---

UC-11: Reset Forgotten Password
Primary Actor
Registered User
Goal
Regain access to the account by resetting a forgotten password.
Preconditions
The user has a registered account.
Main Flow
1. The user clicks the Forgot Password link on the login page.
2. The system displays the forgot password form.
3. The user enters their registered email address and submits the form.
4. The system generates a secure password reset token.
5. The system sends a reset link containing the token to the user's email.
6. The user clicks the reset link in their email.
7. The system validates the token and displays the reset password form.
8. The user enters and confirms a new password.
9. The user submits the form.
10. The system updates the account password.
11. The system redirects the user to the login page with a success message.
Alternative Flows
A1: User Remembers Password Before Completing Reset
1. The user navigates away from the reset page without submitting.
2. The token remains valid until it expires.
3. The user may log in normally or restart the reset process.
Exception Flows
E1: Email Address Not Found
1. The user enters an email address that is not registered in the system.
2. The system displays a neutral message instructing the user to check their inbox without revealing whether the address exists.
E2: Reset Token Has Expired
1. The user clicks a reset link that has already expired.
2. The system invalidates the token and displays an expiry message.
3. The user must request a new reset link.

---

UC-12: Manage Courses (Admin)
Primary Actor
Administrator
Goal
Create, edit, and soft-delete courses and their lessons through the admin panel.
Preconditions
The administrator is logged in and has admin access.
Main Flow
1. The administrator navigates to the Course Management panel.
2. The system displays a searchable and paginated list of all courses.
3. The administrator selects Create New Course.
4. The system displays the course editor form.
5. The administrator enters the course title, description, difficulty level, and programming language.
6. The administrator adds one or more lessons, providing a title, HTML content, optional video URL, optional image URL, and order number for each.
7. The administrator saves the course.
8. The system validates and saves the course and all associated lessons.
9. The system confirms the save with a success message.
Alternative Flows
A1: Administrator Edits an Existing Course
1. The administrator selects an existing course from the list.
2. The system loads the course details and all associated lessons into the editor.
3. The administrator modifies any field or lesson and saves.
4. The system updates the existing records.
A2: Administrator Soft-Deletes a Course
1. The administrator clicks Delete on an existing course.
2. The system flags the course as deleted without removing it from the database.
3. The course no longer appears in any user-facing learning path pages.
A3: Administrator Soft-Deletes a Lesson
1. The administrator clicks Delete next to a specific lesson in the course editor.
2. The system flags the lesson as deleted.
3. The lesson no longer appears to learners but is retained in the database.
Exception Flows
E1: Course Save Fails
1. The system encounters an error when saving the course or lesson data.
2. The system displays an error message.
3. The administrator retries or corrects the invalid data.

---

UC-13: Manage Users (Admin)
Primary Actor
Administrator
Goal
View, edit, toggle admin status, and delete user accounts through the admin panel.
Preconditions
The administrator is logged in and has admin access.
Main Flow
1. The administrator navigates to the User Management panel.
2. The system displays a searchable and paginated list of all registered users.
3. The administrator searches for a user by name or email.
4. The system filters and displays matching users.
5. The administrator selects a user to view their profile details and learning statistics.
6. The system displays the user's completed lessons, submitted exercises, and quiz attempts.
7. The administrator may toggle the user's admin status or delete the account.
8. The system saves the changes and confirms with a success message.
Alternative Flows
A1: Administrator Creates a New User
1. The administrator selects Create New User.
2. The system displays a user creation form.
3. The administrator enters the user's details and an initial password.
4. The system creates the account and saves it.
A2: Administrator Toggles Admin Status
1. The administrator clicks the Toggle Admin button on a user's profile.
2. The system enables or disables admin access for that user.
3. The system saves the change immediately.
Exception Flows
E1: User Deletion Fails
1. The system encounters an error when deleting the user account.
2. The system displays an error message.
3. The account is not deleted and no data is lost.
E2: Unauthorized Access Attempt
1. A non-admin user attempts to access the admin panel directly via URL.
2. The system detects that the user does not have the admin role.
3. The system redirects the user to the access-denied page.

---

UC-14: Manage Job Offers (Admin)
Primary Actor
Administrator
Goal
Create, edit, and soft-delete job offers and review submitted applications.
Preconditions
The administrator is logged in and has admin access.
Main Flow
1. The administrator navigates to the Job Offer Management panel.
2. The system displays all job offers with stats including total active offers, expired offers, and total applications received.
3. The administrator selects Create New Job Offer.
4. The system displays the job offer form.
5. The administrator enters the job title, company name, description, required skills, and application deadline.
6. The administrator saves the job offer.
7. The system saves the record and confirms with a success message.
8. The offer appears in the user-facing Job Board.
Alternative Flows
A1: Administrator Edits an Existing Job Offer
1. The administrator selects an existing offer from the list.
2. The system loads the offer details into the editor.
3. The administrator updates any field and saves.
4. The system updates the record.
A2: Administrator Soft-Deletes a Job Offer
1. The administrator clicks Delete on an existing offer.
2. The system flags the offer as deleted without removing it from the database.
3. The offer no longer appears on the user-facing Job Board.
A3: Administrator Reviews Applications for a Job Offer
1. The administrator selects a job offer and views its submitted applications.
2. The system displays applicant names, cover letters, skill match percentages, application dates, and current statuses.
Exception Flows
E1: Job Offer Save Fails
1. The system encounters an error when saving the job offer.
2. The system displays an error message.
3. The administrator retries or corrects any invalid fields.

---

UC-15: View Platform Reports (Admin)
Primary Actor
Administrator
Goal
Review platform-wide analytics and performance reports.
Preconditions
The administrator is logged in and has admin access.
Main Flow
1. The administrator navigates to the admin dashboard.
2. The system displays key metrics including total registered users, total courses, total job offers, and skill level distribution.
3. The system displays a user engagement graph.
4. The administrator filters the engagement graph by time range (last 7, 14, or 30 days).
5. The system updates the graph based on the selected filter.
6. The administrator navigates to the Reports section.
7. The system generates and displays reports including user breakdown by skill level and learning path, course completion rates, exercise submission and correctness rates, and job application volume and status distribution.
Alternative Flows
A1: Administrator Filters Reports by Time Range
1. The administrator selects a specific date range for the report.
2. The system recalculates and displays the filtered data.
Exception Flows
E1: Report Data Fails to Load
1. The system encounters an error when generating the report data.
2. The system displays an error message.
3. The administrator may refresh the page or try again later.

---

*End of Use Cases Document*
