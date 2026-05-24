# Edge Cases and Exception Scenarios — CodeWave

---

## A. User Registration & Authentication Edge Cases

| Edge Case | Description | Expected System Behavior |
|---|---|---|
| Duplicate email registration | A user tries to register with an email already in the system | System should reject the registration and display a clear message: "An account with this email already exists. Please log in or reset your password." |
| OAuth account already exists | User tries to register via Google/GitHub but the email is already linked to an existing account | System should automatically sign the user into their existing account without creating a duplicate. No merge prompt is required. |
| GitHub OAuth user has no public email | User signs in with GitHub but their GitHub account has no public email set | System should prompt the user to manually enter their email after OAuth completes, rather than silently assigning a synthetic placeholder address. |
| Registration with invalid email format | User enters a malformed email (e.g., `user@`, `@domain.com`) | Client-side and server-side validation should reject the input with a descriptive field error before submission |
| Weak password submitted | User submits a password that fails strength requirements | System should block submission and display the password strength indicator with specific requirements not yet met |
| Password confirmation mismatch | User enters two different values in the password fields | System should highlight both fields and show "Passwords do not match" before allowing submission |
| OAuth provider unavailable | Google or GitHub OAuth service is down during login/registration | System should display a user-friendly error: "Third-party login is currently unavailable. Please try email login or try again later." |
| Account created but session fails | Registration succeeds but auto-login fails silently | System should redirect the user to the login page with a success message instead of leaving them on a broken state |
| Invalid characters in name fields | User enters digits, symbols, or special characters in the FirstName or LastName fields | System should reject the input and display a clear validation message. Only letters, spaces, hyphens, and apostrophes should be permitted. |
| User registers but skips onboarding | User navigates away from the welcome/onboarding flow after registration | System should detect incomplete onboarding on next login and redirect back to resume the welcome flow |
| Very long input in form fields | User pastes an extremely long string in name or email fields | System should enforce max-length validation both client-side and server-side to prevent overflow or storage errors |

---

## B. Learning Path & Initial Assessment Edge Cases

| Edge Case | Description | Expected System Behavior |
|---|---|---|
| User skips the initial assessment | User navigates directly to the home page without completing the assessment | System should redirect unevaluated users to the assessment page or assign a default Beginner path with a prompt to complete assessment |
| User submits assessment with no answers selected | User clicks submit without selecting any options | System should block submission and prompt the user to answer all required questions |
| User scores exactly on a threshold boundary | User scores exactly 50% or exactly 80% on the assessment | System should apply thresholds inclusively: ≥80% = Advanced, ≥50% = Intermediate, <50% = Beginner. The result is deterministic — no special handling is needed at boundary values. |
| User wants to retake the assessment | User feels the assigned level is wrong after assessment | System should allow retaking the assessment from settings, replacing the previous result with an audit note |
| Learning path assignment fails silently | Service fails to persist the assigned path after assessment | System should display an error and retain the user's assessment answers, allowing retry without re-answering questions |
| User is assigned a path with no available courses | A learning path has no published courses at the time of assignment | System should display a message: "No courses available for this path yet. Check back soon." rather than showing an empty screen |
| User manually changes their programming path mid-progress | User switches from Python to Java (or another language path) after completing several lessons | System should warn that course recommendations will change. Previously completed lesson and exercise records should be preserved and not reset. The assessed proficiency level remains unchanged. |
| Assessment questions fail to load | Database or service error prevents questions from rendering | System should display a descriptive error page and offer a retry option, not a blank page |

---

## C. Course & Lesson Navigation Edge Cases

| Edge Case | Description | Expected System Behavior |
|---|---|---|
| User tries to skip a locked lesson | User manipulates the URL to access a lesson they have not unlocked | System should check completion of the previous lesson server-side and redirect with an explanation: "Complete the previous lesson to unlock this one." |
| Course deleted while user is enrolled | Admin soft-deletes a course a user is actively taking | System should keep the user's progress record intact but display a notice: "This course is no longer available." |
| User completes last lesson in a course | User marks the final lesson complete | System should automatically calculate 100% course completion, display a congratulations message, and suggest the next recommended course |
| Lesson content is empty | A lesson exists in the database but has no content, video, or code examples | System should display a placeholder: "Content coming soon" rather than a blank or broken layout |
| Lesson order conflict | Two lessons have the same `OrderNumber` within a course | System should handle this gracefully by falling back to creation date ordering and flagging the conflict for admin resolution |
| User loses internet mid-lesson | Connection drops while reading lesson content | System should not lose any lesson completion data already submitted to the server. On reconnect, the user should be able to continue viewing the lesson and resubmit any pending completion actions without starting over. |
| Course has zero lessons | Admin creates a course but adds no lessons | System should not display the course to users in the learning path until at least one lesson exists, or show a "Coming soon" badge |
| Simultaneous lesson completion requests | User double-clicks the "Complete Lesson" button rapidly | System should be idempotent — marking a lesson complete twice should not duplicate records, create errors, or corrupt progress percentage |
| Next lesson navigation at course end | User clicks "Next Lesson" on the final lesson | System should disable or hide the Next button and display a course completion summary instead of throwing a null reference error |
| User accesses lesson from a different learning path | URL is shared to a user not enrolled in that course's path | System should check enrollment server-side and either enroll the user if permitted, or show a "You are not enrolled in this course" message |

---

## D. Coding Exercise & Code Execution Edge Cases

| Edge Case | Description | Expected System Behavior |
|---|---|---|
| Code execution timeout | User submits code that runs an infinite loop or very heavy computation | System should enforce a timeout (e.g., 5 seconds), terminate the Docker container, and return a clear message: "Execution timed out. Check for infinite loops." |
| Docker container unavailable | The code execution service or Docker daemon is down | System should display: "Code execution is temporarily unavailable. Please try again later." and not expose stack traces to the user |
| Empty code submission | User submits an exercise with no code written | System should display a validation message: "Please write some code before submitting." |
| Code compiles but produces wrong output | User's code runs successfully but fails all test cases | System should show each test case result clearly with the expected vs. actual output, without marking the exercise as passed |
| Partial test case pass | User's code passes some but not all test cases | System should display which tests passed and which failed, not mark the exercise as solved, and allow resubmission. |
| Language mismatch | User writes Python syntax in a Java exercise (or vice versa) | System should pass the error from the Docker executor and display compilation or runtime error output in a readable way |
| Exercise has no test cases | An exercise exists but no test cases have been configured | System should inform the user: "This exercise has no automated tests yet" and not allow a false pass |
| Code produces no output | User's code executes without error but prints nothing | System should show the output area as empty and display the test result accurately — not a system error |
| Extremely large output | Code generates thousands of lines of output | System should truncate the displayed output (e.g., first 500 lines) with a note: "Output truncated" to prevent UI overflow |
| Special characters in code | User uses Unicode, emoji, or non-ASCII characters in code or comments | System should handle encoding gracefully and not crash the execution pipeline |
| Exercise re-submission after completion | User resubmits a previously solved exercise | System should allow re-submission, record the new attempt, and keep the latest result without losing the original completion timestamp |
| Code saved locally but session expires | User writes code, session expires, and they return later | System should restore code from local storage (Monaco autosave) and prompt the user to re-authenticate without losing their work |

---

## E. Quiz System Edge Cases

| Edge Case | Description | Expected System Behavior |
|---|---|---|
| Quiz timer expires mid-attempt | User is taking a timed quiz and the countdown reaches zero | System should automatically submit whatever answers were selected at the time and display results, not discard the attempt |
| User refreshes page mid-quiz | User accidentally refreshes the browser during a quiz | System should restore quiz state (current question, selected answers, remaining time) from session storage where possible |
| Quiz submitted with unanswered questions | User skips questions and submits | System should warn the user: "You have X unanswered questions. Are you sure you want to submit?" before final submission |
| All quiz answers are incorrect | User scores 0% on a quiz | System should display results normally, show correct answers with explanations, and allow retake without error |
| Quiz has no questions | A quiz exists in the system but has zero questions configured | System should not display the quiz to users, or show: "This quiz is not yet available." |
| Concurrent quiz submissions | User opens the same quiz in two browser tabs and submits from both | System should record only one attempt per submission, preventing duplicate score entries |
| Quiz questions load partially | A database or network error causes only some questions to load | System should detect incomplete data and either reload or display an error, not allow submission of a partial quiz |
| User passes score threshold exactly | User scores exactly the passing percentage (e.g., 70%) | System should treat this as a pass and display the correct passed status |
| Quiz retake limit | If a retake limit is configured and user exceeds it | System should display: "You have reached the maximum number of attempts for this quiz." and disable the retake button |
| Correct answer not set for a question | Admin creates a question without marking any answer as correct | System should flag this during quiz creation validation and not allow publication of the quiz with invalid questions |

---

## F. Progress Tracking Edge Cases

| Edge Case | Description | Expected System Behavior |
|---|---|---|
| Progress percentage shows over 100% | Due to a calculation error or duplicate completion records | System should cap progress at 100% and log an anomaly for investigation without displaying incorrect values to the user |
| Division by zero in progress calculation | A course has zero total lessons (due to soft delete) | System should handle the zero-denominator case and display 0% or "N/A" rather than throwing an unhandled exception |
| Study time not recorded | Session ends abruptly (browser crash, power loss) before time is logged | System should periodically flush study time to the server (e.g., every 60 seconds) rather than only on explicit actions |
| Dashboard shows stale data | Cached dashboard data does not reflect the latest lesson completion | System should invalidate cache on progress-modifying actions and serve fresh counts on the next dashboard load |
| Completed exercises count is wrong | Exercise marked complete multiple times inflates the count | System should deduplicate completion records and display the accurate count of unique exercises solved |
| Skills extracted from zero completions | User has no completed lessons or exercises, causing skill analysis to fail | System should display an empty state message: "Complete lessons and exercises to see your skill profile." rather than throwing an error |
| User with no quiz attempts | Progress page attempts to display quiz pass rate with no attempts | System should display "No quiz attempts yet" rather than dividing by zero or showing NaN% |
| Learning path progress after path change | User switches paths and progress from the old path appears on the new path dashboard | System should scope all progress metrics to the currently active learning path and clearly separate historical data |

---

## G. Focus Mode & AI Helper Edge Cases

| Edge Case | Description | Expected System Behavior |
|---|---|---|
| AI Helper (Gemini API) is unreachable | The Gemini API key is invalid, rate-limited, or the service is down | System should display: "AI assistant is temporarily unavailable. Please try again later." without exposing API errors or keys |
| AI returns an empty or malformed response | Gemini API returns an empty body or unparseable JSON | System should display a fallback message: "Could not generate a response. Please rephrase your question." |
| User sends an empty message to AI | User clicks Send without typing anything | System should validate the input and show: "Please type a message before sending." |
| Focus Mode loaded with no exercises | Lesson has no associated exercises | System should display the lesson content normally and hide the exercise panel, not show an empty broken panel |
| Focus Mode lesson content is very long | Lesson has an extremely large amount of text/code | System should make the content panel scrollable and not cause the page to freeze or layout to break |
| Simultaneous AI requests | User sends multiple AI messages rapidly before receiving a response | System should queue requests or disable the send button until the previous response is received |
| AI response blocked by content safety filters | Gemini API returns a response blocked by its built-in safety or recitation filters | System should detect the safety block, avoid displaying the raw API response, and show: "This response was blocked by content safety guidelines. Please rephrase your question." |
| Focus Mode navigation with unsaved code | User clicks Next Lesson with unsaved code in the editor | System should prompt: "You have unsaved code. Do you want to leave?" with Save and Discard options before navigating |
| Session expires while in Focus Mode | Authentication cookie expires while the user is actively in Focus Mode | System should detect the expired session on the next API call and redirect to login with a message to preserve their work |

---

## H. CV Generation Edge Cases

| Edge Case | Description | Expected System Behavior |
|---|---|---|
| Beginner user accesses CV page | A Beginner-level user navigates directly to `/Cv` via the URL | System should block access server-side and display a message: "CV generation is unlocked at Intermediate level. Keep learning!" |
| User has no completed courses | An Intermediate/Advanced user has no completed content to populate the CV | System should generate a CV with empty sections and display a prompt: "Complete more courses to enrich your CV." |
| PDF generation fails | The PDF rendering service encounters an error | System should show a descriptive error and offer a retry option, not a blank download or server error |
| CV contains special characters in user data | User's name or skills contain Unicode or special characters | System should encode these correctly in the PDF to prevent garbled text or rendering failures |
| User data changes after CV is generated | User completes more courses after generating a CV | System should generate a fresh CV on each request rather than serving a cached outdated file |
| Profile picture is missing | User has no profile picture set but CV template includes a photo field | System should substitute a default placeholder image gracefully, not break the PDF layout |
| Very large number of skills | User has completed every course and has hundreds of skills | System should paginate or truncate the skills section in the CV to fit within standard page limits, with the most recent/relevant first |

---

## I. Admin Management Edge Cases

| Edge Case | Description | Expected System Behavior |
|---|---|---|
| Admin deletes a course with active enrollments | Admin soft-deletes a course that users are currently taking | System should warn: "X users are enrolled in this course. Deleting will hide it from all users." and require confirmation before proceeding |
| Admin promotes a user to admin role | Admin grants admin privileges to a regular user | System should log the role change with who performed it and when, and require re-login for the promoted user to receive new permissions |
| Admin demotes themselves | The only admin tries to remove their own admin status | System should prevent this action and display: "You cannot remove your own admin privileges. At least one admin must exist." |
| Admin creates a course with duplicate title | A new course has the exact same title as an existing one | System should warn about the potential duplicate and ask for confirmation, but not hard-block (different versions may exist) |
| Admin deletes a user account | Admin deletes a regular user account | System should soft-delete the account to preserve historical activity logs and learning data, not perform a hard delete |
| Admin creates a lesson with no content | Lesson is created with only a title and no description, video, or content | System should allow saving but flag it as incomplete and prevent it from being visible to learners until content is added |
| Admin reorders lessons while users are mid-course | Admin changes lesson order numbers for a course with active learners | System should reorder lessons without resetting any user's completion status or unlocking previously locked content unexpectedly |
| Admin analytics load with no data | A new deployment has no user activity yet | System should display charts with empty states/zero values and a message: "No activity data yet." rather than crashing |
| Pagination request out of range | Admin requests a page number beyond total pages in user/course lists | System should return the last valid page or an empty result, not a 500 error |
| Job offer posted with past deadline | Admin sets an application deadline that has already passed | System should warn: "The deadline is in the past. This offer will appear as expired." and require confirmation |

---

## J. Job Offer & Application Edge Cases

| Edge Case | Description | Expected System Behavior |
|---|---|---|
| No job offers available | The system has no active job offers posted | System should display an empty state message: "No job opportunities are currently available. Check back soon." |
| Offer expires while user is viewing it | Job application deadline passes while a user is on the detail page | System should show the offer as expired on the next load and disable the Apply button, without a broken page |
| User applies for same job twice | User clicks Apply on an offer they already applied to | System should detect the duplicate application and display: "You have already applied for this position." |
| Job offer with no required skills listed | Admin publishes an offer without filling in the skills field | System should still display the offer correctly and show "Skills: Not specified" rather than a blank or broken layout |
| Required skills do not match user's skills | User applies for a role they are underqualified for | System should allow the application (no hard block) but optionally display a soft warning: "Your profile does not yet match all required skills." |

---

## K. Settings & Profile Management Edge Cases

| Edge Case | Description | Expected System Behavior |
|---|---|---|
| Profile picture upload exceeds size limit | User uploads a file larger than 5 MB | System should reject the upload and display: "File size must be under 5 MB. Please choose a smaller image." |
| Profile picture has invalid file type | User uploads a `.exe`, `.pdf`, or other non-image file | System should validate the MIME type server-side and reject the upload, not just rely on client-side file extension checks |
| Password change with wrong current password | User provides an incorrect current password when changing it | System should reject the change and display: "Current password is incorrect." without revealing any account information |
| New password same as current password | User changes password to the same value | System should reject and display: "New password must be different from your current password." |
| User changes email to one already registered | User updates their email to an address belonging to another account | System should reject the change and display: "This email is already associated with another account." |
| Forgot password email not received | User requests a password reset but the email is not delivered | System should display the same "Check your email" message regardless to prevent account enumeration, and ensure the email service is monitored |
| Password reset link has expired | User clicks an old reset link after the token has expired | System should display: "This password reset link has expired. Please request a new one." and not allow the reset |
| User deletes their own account | If account self-deletion is enabled and user confirms | System should soft-delete the account, immediately log out the user, and retain anonymized historical data for analytics integrity |
| OAuth user tries to change password | A user who registered via Google/GitHub (no local password) opens the password change form | System should detect the OAuth-only account and display: "Password management is not available for accounts linked to Google/GitHub." |

---

## L. Security and Audit Edge Cases

| Edge Case | Description | Expected System Behavior |
|---|---|---|
| Unauthorized access to admin routes | A regular user manually navigates to `/Admin` or admin-only endpoints | System should return a 403 Forbidden response or redirect the user to an access-denied page, without exposing any admin data. |
| Unauthorized access to another user's data | User manipulates a URL or API parameter to access another user's profile or progress | System should validate ownership server-side on every request and return a 403 or 404 without exposing the other user's data |
| SQL injection attempt | User enters SQL syntax in a search field or form input | System should use parameterized queries (via Entity Framework Core) throughout, making injection attempts harmless |
| XSS attempt in input fields | User enters `<script>alert('xss')</script>` in a name, comment, or search field | System should HTML-encode all user-generated content before rendering it, preventing script execution |
| Brute force login attempt | A client repeatedly submits login requests with different passwords | System should implement rate limiting on the login endpoint and temporarily block the IP or account after a defined number of failures |
| Session cookie theft scenario | An attacker attempts to use a stolen session cookie | System should use `HttpOnly`, `Secure`, and `SameSite=Strict` cookie flags to mitigate this attack surface |
| CSRF attack on state-changing actions | Attacker tricks an authenticated user's browser into submitting a form | System should include and validate anti-forgery tokens on all POST, PUT, and DELETE actions |
| Direct object reference on exercise submissions | User references an exercise submission ID they do not own via the API | System should verify the submission belongs to the requesting user before returning any data |
| Sensitive data in logs | A logging statement accidentally captures a user's password or token | System should never log authentication credentials, tokens, or sensitive PII — review all log statements at service boundaries |
| Inactivity session timeout | User leaves the browser open and returns after a long period | System should expire the session after the configured inactivity period, redirect to the login page, and display: "Your session has expired. Please log in again." |
| Deactivated account tries to log in | Admin deactivates a user account | System should reject the login attempt and display: "Your account has been deactivated. Please contact support." without exposing why |
| Admin audit log integrity | Admin modifies role assignments, course data, or user records | System should record the actor's ID, action type, target record, previous value, new value, and timestamp in an immutable audit table. |
