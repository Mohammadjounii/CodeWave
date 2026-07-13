# CodeWave — ER Diagram Tables

This document presents the ER diagram as database tables, showing:
- Table name
- Attributes
- Primary keys
- Foreign keys
- Short description of each attribute
- Relationships at the end

---

## 1. ApplicationUser Table

Stores all registered user accounts on the CodeWave platform, including learners and administrators. Extends ASP.NET Core Identity's built-in user entity. Each user has a proficiency level and a programming learning path assigned after the initial assessment.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each user account. Generated as a GUID. Inherited from ASP.NET Core Identity. |
| UserName | | Login username. Typically the same value as the email address. Inherited from Identity. |
| Email | | User's email address used for login, notifications, and account recovery. Inherited from Identity. |
| PasswordHash | | Encrypted hash of the user's password. Null for users registered via OAuth. Inherited from Identity. |
| FirstName | | User's first name. |
| LastName | | User's last name. |
| Description | | Optional personal bio or profile description written by the user. |
| Level | | User's assessed proficiency level assigned after the initial assessment. Values: Beginner, Intermediate, or Advanced. |
| LearningPath | | The programming path the user is following. Values: Python, Java, or Web Development. |
| Interests | | Programming interests selected by the user during the onboarding flow. |
| Goal | | Learning goal selected during onboarding, such as career change, skill improvement, or academic study. |
| SkillLevel | | Self-reported skill level provided by the user during onboarding. |
| Motivation | | User's motivation for learning, selected during the onboarding flow. |
| WeeklyHours | | Number of hours per week the user plans to dedicate to learning. |
| PreferredLanguage | | Programming language preferred by the user, selected during onboarding. Values: Python, Java, or Web Development. |
| ProfilePictureUrl | | File path or URL to the user's uploaded profile picture. |
| IsAdmin | | Indicates whether the user has administrator privileges in the system. |

---

## 2. Assessment Table

Stores the initial skill assessments offered to new users upon registration. Each assessment contains a set of questions that determine the user's proficiency level and assign the appropriate learning path.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each assessment. |
| Title | | Title of the assessment shown to the user. |
| CreatedAt | | Date and time when the assessment was created. |
| IsDeleted | | Indicates whether the assessment has been soft-deleted and is no longer active. |

---

## 3. Question Table

Stores individual questions that belong to an assessment. Each question is linked to one assessment and has multiple answer options associated with it.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each question. |
| AssessmentId | FK | References the assessment this question belongs to. |
| Text | | The full question text displayed to the user during the assessment. |
| Difficulty | | Difficulty level of the question, such as Easy, Medium, or Hard. |
| CreatedAt | | Date and time when the question was created. |
| IsDeleted | | Indicates whether the question has been soft-deleted. |

---

## 4. AnswerOption Table

Stores the possible answer choices for each assessment question. One question can have multiple answer options. Each option is marked as either correct or incorrect. Only one option per question should be marked correct.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each answer option. |
| QuestionId | FK | References the question this answer option belongs to. |
| Text | | The answer option text displayed to the user. |
| IsCorrect | | Indicates whether this option is the correct answer for the question. |
| CreatedAt | | Date and time when the answer option was created. |
| IsDeleted | | Indicates whether the answer option has been soft-deleted. |

---

## 5. UserAssessment Table

Records the result of a user completing an assessment. Stores the percentage score achieved, the proficiency level assigned, and the learning path recommended by the system. One user can take the assessment multiple times, and each attempt is stored as a separate record.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each assessment result record. |
| UserId | FK | References the user who took the assessment. |
| AssessmentId | FK | References the assessment that was taken. |
| Score | | Percentage score achieved by the user, calculated as correct answers divided by total questions multiplied by 100. |
| ResultLevel | | Proficiency level assigned based on the score. Values: Beginner if below 50%, Intermediate if 50% or above, Advanced if 80% or above. |
| AssignedLearningPath | | Programming path recommended to the user based on the assessment result. Values: Python, Java, or Web Development. |
| DateTaken | | Date and time when the user completed the assessment. |
| CreatedAt | | Date and time when the result record was created. |
| IsDeleted | | Indicates whether the record has been soft-deleted. |

---

## 6. UserAnswer Table

Records the individual answer selected by a user for each question within a specific assessment attempt. Links three entities together: the assessment attempt, the question, and the selected answer option. One record is created per question per attempt.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each user answer record. |
| UserAssessmentId | FK | References the assessment attempt this answer is part of. |
| QuestionId | FK | References the question that was answered. |
| AnswerOptionId | FK | References the answer option selected by the user. |
| IsCorrect | | Indicates whether the selected answer option was correct. |

---

## 7. Course Table

Stores the programming courses available on the CodeWave platform. Each course belongs to a specific learning path and targets a particular programming language and difficulty level. Courses contain ordered lessons.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each course. |
| Title | | Title of the course. |
| Description | | Description of what the course covers and what the learner will achieve. |
| DifficultyLevel | | Target difficulty level of the course. Values: Beginner, Intermediate, or Advanced. |
| LearningPath | | The learning path this course belongs to. Values: Python, Java, or Web Development. |
| ProgrammingLanguage | | Programming language of the course, stored as an integer enum. 0 = Python, 1 = Java. |
| CreatedAt | | Date and time when the course was created. |
| IsDeleted | | Indicates whether the course has been soft-deleted and hidden from all users. |

---

## 8. Lesson Table

Stores individual lessons that make up a course. Lessons are presented to users in a defined sequential order. Each lesson may include text content, an optional video URL, an optional image, and associated coding exercises.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each lesson. |
| CourseId | FK | References the course this lesson belongs to. |
| Title | | Title of the lesson. |
| Content | | Main educational content of the lesson, stored as rich text or HTML. |
| VideoUrl | | Optional URL to a supplementary video resource for the lesson. |
| ImageUrl | | Optional URL to an image or diagram associated with the lesson. |
| OrderNumber | | Integer that controls the sequential display order of the lesson within its course. |
| CreatedAt | | Date and time when the lesson was created. |
| IsDeleted | | Indicates whether the lesson has been soft-deleted. |

---

## 9. CodingExercise Table

Stores coding exercises attached to lessons. Each exercise presents a problem to the user with a description, pre-written starter code, and a fallback expected output used when no test cases are defined.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each coding exercise. |
| LessonId | FK | References the lesson this exercise belongs to. |
| Title | | Title of the coding exercise. |
| Description | | Problem statement and instructions displayed to the user. |
| StarterCode | | Pre-written code template provided to the user as a starting point in the editor. |
| ExpectedOutput | | The expected output used as a fallback comparison when no test cases are configured for this exercise. |
| CreatedAt | | Date and time when the exercise was created. |
| IsDeleted | | Indicates whether the exercise has been soft-deleted. |

---

## 10. ExerciseTestCase Table

Stores individual test cases used to automatically validate a user's submitted code. One exercise can have multiple test cases. Each test case defines an input and the expected output that the user's code must produce for that input.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each test case. |
| ExerciseId | FK | References the coding exercise this test case belongs to. |
| Input | | Input value passed to the user's code during execution. May be empty for exercises that require no input. |
| ExpectedOutput | | The exact output that the user's code must produce when given the specified input. |
| Description | | Short description explaining what this test case is validating. |
| OrderNumber | | Integer that controls the order in which test cases are executed and displayed. |
| CreatedAt | | Date and time when the test case was created. |
| IsDeleted | | Indicates whether the test case has been soft-deleted. |

---

## 11. LessonCompletion Table

Tracks whether a specific user has completed a specific lesson. One record exists per user per lesson. Also stores the total time the user spent on that lesson. Completion of a lesson unlocks the next lesson in the sequence.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each lesson completion record. |
| UserId | FK | References the user whose completion is recorded. |
| LessonId | FK | References the lesson that was completed. |
| IsCompleted | | Indicates whether the lesson has been successfully marked as complete by the user. |
| CompletionDate | | Date and time when the user completed the lesson. Nullable if the lesson has not yet been completed. |
| TimeSpentMinutes | | Total time in minutes that the user spent on this lesson across all visits. |
| CreatedAt | | Date and time when the completion record was first created. |
| IsDeleted | | Indicates whether the record has been soft-deleted. |

---

## 12. UserCourse Table

Tracks a user's enrollment in a course. Acts as the enrollment record linking a user to a course. Stores the current overall progress percentage and the completion date if the course has been finished.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each user-course enrollment record. |
| UserId | FK | References the user who is enrolled. |
| CourseId | FK | References the course the user is enrolled in. |
| ProgressPercent | | Current overall progress of the user in this course, expressed as a percentage from 0 to 100. |
| CompletionDate | | Date and time when the user completed all lessons in the course. Nullable if not yet completed. |
| CreatedAt | | Date and time when the enrollment record was created. |
| IsDeleted | | Indicates whether the enrollment record has been soft-deleted. |

---

## 13. ExerciseSubmission Table

Records each code submission made by a user for a coding exercise. Multiple submissions per user per exercise are permitted. Stores the submitted code, the execution output, and whether the submission passed all test cases.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each exercise submission. |
| UserId | FK | References the user who submitted the code. |
| ExerciseId | FK | References the coding exercise the submission is for. |
| SubmittedCode | | The full source code submitted by the user. |
| Output | | The actual output produced by executing the submitted code in the Docker container. |
| IsCorrect | | Indicates whether the submission passed all test cases or matched the expected output. |
| SubmissionDate | | Date and time when the submission was made. |
| CreatedAt | | Date and time when the submission record was created. |
| IsDeleted | | Indicates whether the submission record has been soft-deleted. |

---

## 14. Quiz Table

Stores quizzes associated with courses. Each quiz has a configurable time limit and a minimum passing score threshold. Quizzes are used to assess user understanding after completing course content.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each quiz. |
| CourseId | FK | References the course this quiz belongs to. |
| Title | | Title of the quiz. |
| Description | | Description of what the quiz covers. |
| TimeLimitMinutes | | Maximum time allowed to complete the quiz, in minutes. |
| PassingScore | | Minimum percentage score required to pass the quiz. Default value is 70. |
| CreatedAt | | Date and time when the quiz was created. |
| IsDeleted | | Indicates whether the quiz has been soft-deleted. |

---

## 15. QuizQuestion Table

Stores individual questions within a quiz. Questions are displayed to the user in a defined sequential order. Each question has a difficulty level and multiple answer options.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each quiz question. |
| QuizId | FK | References the quiz this question belongs to. |
| Text | | The full question text displayed to the user during the quiz. |
| Difficulty | | Difficulty level of the question. Values: Easy, Medium, or Hard. |
| OrderNumber | | Integer that controls the sequential display order of the question within its quiz. |
| CreatedAt | | Date and time when the question was created. |
| IsDeleted | | Indicates whether the question has been soft-deleted. |

---

## 16. QuizAnswerOption Table

Stores the possible answer choices for each quiz question. One question can have multiple options. Exactly one option per question should be marked as correct. These options are displayed to the user during a quiz attempt.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each quiz answer option. |
| QuizQuestionId | FK | References the quiz question this option belongs to. |
| Text | | The answer option text displayed to the user. |
| IsCorrect | | Indicates whether this option is the correct answer for the question. |
| CreatedAt | | Date and time when the answer option was created. |
| IsDeleted | | Indicates whether the answer option has been soft-deleted. |

---

## 17. UserQuizAttempt Table

Records each attempt made by a user on a quiz. Stores the score achieved, whether the user passed, and timing information. Multiple attempts per user per quiz are allowed. Each attempt links to individual answer records.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each quiz attempt. |
| UserId | FK | References the user who made this attempt. |
| QuizId | FK | References the quiz being attempted. |
| Score | | Percentage score achieved in this attempt, calculated from correct answers. |
| IsPassed | | Indicates whether the score met or exceeded the quiz's passing score threshold. |
| StartedAt | | Date and time when the user began the quiz. |
| CompletedAt | | Date and time when the quiz was submitted. Nullable if the attempt is still in progress. |
| TimeSpentMinutes | | Total time spent on this attempt, in minutes. |
| CreatedAt | | Date and time when the attempt record was created. |
| IsDeleted | | Indicates whether the attempt record has been soft-deleted. |

---

## 18. UserQuizAnswer Table

Records the individual answer selected by a user for each question within a specific quiz attempt. Links three entities: the quiz attempt, the question, and the selected answer option. One record is created per question per attempt. The selected answer is nullable if the user skipped the question.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each user quiz answer record. |
| UserQuizAttemptId | FK | References the quiz attempt this answer is part of. |
| QuizQuestionId | FK | References the quiz question that was answered. |
| SelectedAnswerOptionId | FK, Nullable | References the answer option selected by the user. Nullable if the user did not answer the question. |
| IsCorrect | | Indicates whether the selected answer option was the correct one. |
| CreatedAt | | Date and time when the answer record was created. |
| IsDeleted | | Indicates whether the record has been soft-deleted. |

---

## 19. Project Table

Stores coding projects linked to a user's profile. Projects serve as portfolio items that are displayed on the user's profile page and can be included in the auto-generated CV.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each project record. |
| UserId | FK | References the user who owns this project. |
| Title | | Title of the project. |
| Description | | Description of the project, what it does, and the technologies used. |
| CompletionDate | | Date when the project was completed. |
| Result | | Outcome or result of the project, such as a grade, score, or completion status. |
| CreatedAt | | Date and time when the project record was created. |
| IsDeleted | | Indicates whether the project record has been soft-deleted. |

---

## 20. CV Table

Stores the CV (curriculum vitae) profile for each user. Contains personal information, education, skills, work experience, certifications, and paths to generated or uploaded CV files. Access to this table is restricted to Intermediate and Advanced level users.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each CV record. |
| UserId | FK | References the user who owns this CV. |
| FullName | | User's full name as it appears on the CV. |
| Age | | User's age. Optional. |
| Location | | User's city or country of residence shown on the CV. |
| Email | | Contact email address shown on the CV. May differ from the login email. |
| Phone | | Contact phone number shown on the CV. |
| LinkedInUrl | | URL to the user's LinkedIn profile. |
| GitHubUrl | | URL to the user's GitHub profile. |
| CVPictureUrl | | File path or URL to the profile picture used specifically in the CV layout. |
| UploadedCVFilePath | | File system path to a CV file manually uploaded by the user, such as a PDF or DOCX. |
| UpgradedCVFilePath | | File system path to an AI-enhanced version of the user's uploaded CV. |
| GeneratedPDFPath | | File system path to the professional PDF CV generated by the system. |
| Template | | CV design template selected by the user. Values: modern, classic, creative, minimalist, or executive. |
| Education | | Name of the educational institution attended by the user. |
| EducationDetails | | Academic details such as degree, field of study, and graduation year. |
| Skills | | General skills listed on the CV. |
| ProgrammingLanguages | | Programming languages listed on the CV, such as Python, Java, HTML, or CSS. |
| SpokenLanguages | | Spoken languages listed on the CV, such as English or Arabic. |
| Summary | | Short professional summary or objective statement placed at the top of the CV. |
| Experience | | Work experience entries stored as formatted text or JSON. |
| Certifications | | Certifications and academic achievements earned on the platform or externally. |
| Projects | | Projects section of the CV, populated from the user's project portfolio. |
| LastUpdated | | Date and time when the CV record was last modified. |
| CreatedAt | | Date and time when the CV record was first created. |

---

## 21. JobOffer Table

Stores job offers posted by administrators for platform users to browse and apply to. Each offer includes a skills requirement field used by the system to calculate how well a user's profile matches the position.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each job offer. |
| JobTitle | | Title of the job position being offered. |
| Company | | Name of the company or organisation posting the offer. |
| Description | | Full description of the role, responsibilities, and requirements. |
| RequiredSkills | | Skills required for the position. Used by the system to calculate the skill-match percentage against the user's profile. |
| PostedDate | | Date and time when the job offer was published on the platform. |
| Deadline | | Application deadline. Applications are blocked after this date. |
| CreatedAt | | Date and time when the job offer record was created. |
| IsDeleted | | Indicates whether the job offer has been soft-deleted and hidden from users. |

---

## 22. JobApplication Table

Stores applications submitted by users for job offers. Tracks the application status, the user's optional cover letter, and a calculated percentage showing how well the user's skills match the job's requirements.

| Attribute | Key | Description |
|---|---|---|
| Id | PK | Unique identifier for each job application. |
| UserId | FK | References the user who submitted the application. |
| JobOfferId | FK | References the job offer the user applied to. |
| AppliedDate | | Date and time when the application was submitted. |
| Status | | Current status of the application. Values: Pending, Reviewed, Accepted, or Rejected. |
| CoverLetter | | Optional cover letter text written by the user when applying. |
| MatchPercentage | | Calculated percentage indicating how well the user's skills match the job offer's required skills. |

---

## Relationships

### 1. One-to-Many Relationships

---

**ApplicationUser → UserAssessment**
One user can take the assessment multiple times, producing a separate result record for each attempt.
```
ApplicationUser 1 ─── many UserAssessment
```
Example: A user retakes the assessment after completing more courses, generating a new result record each time.

---

**Assessment → Question**
One assessment contains many questions.
```
Assessment 1 ─── many Question
```
Example: The initial skill assessment contains 10 questions covering Python, Java, and Web concepts.

---

**Assessment → UserAssessment**
One assessment can be taken by many users, with each attempt stored as a separate record.
```
Assessment 1 ─── many UserAssessment
```
Example: Every new user takes the same assessment, and each result is stored individually.

---

**Question → AnswerOption**
One question has many answer options.
```
Question 1 ─── many AnswerOption
```
Example: A question about Python data types may have four answer options, one of which is correct.

---

**UserAssessment → UserAnswer**
One assessment attempt records one answer per question, so one attempt links to many user answer records.
```
UserAssessment 1 ─── many UserAnswer
```
Example: An assessment with 10 questions produces 10 UserAnswer records per attempt.

---

**ApplicationUser → UserCourse**
One user can be enrolled in many courses.
```
ApplicationUser 1 ─── many UserCourse
```
Example: A user enrolled in the Python path may be enrolled in three courses simultaneously.

---

**Course → UserCourse**
One course can have many users enrolled in it.
```
Course 1 ─── many UserCourse
```
Example: The Python Basics course may have hundreds of enrolled users.

---

**Course → Lesson**
One course contains many lessons delivered in sequential order.
```
Course 1 ─── many Lesson
```
Example: The Java OOP course contains 8 lessons covering classes, inheritance, and polymorphism.

---

**Course → Quiz**
One course can have many quizzes associated with it.
```
Course 1 ─── many Quiz
```
Example: A Web Development course may have a quiz at the end of each major topic.

---

**Lesson → CodingExercise**
One lesson can have many coding exercises for the user to complete.
```
Lesson 1 ─── many CodingExercise
```
Example: A lesson on Python lists may include three exercises of increasing difficulty.

---

**Lesson → LessonCompletion**
One lesson can have many completion records, one per user who completed it.
```
Lesson 1 ─── many LessonCompletion
```
Example: If 200 users complete the same lesson, 200 LessonCompletion records are created.

---

**ApplicationUser → LessonCompletion**
One user can have many lesson completion records, one for each lesson they completed.
```
ApplicationUser 1 ─── many LessonCompletion
```
Example: A user who completed 15 lessons has 15 LessonCompletion records.

---

**CodingExercise → ExerciseTestCase**
One coding exercise can have many test cases used to validate submitted code.
```
CodingExercise 1 ─── many ExerciseTestCase
```
Example: An exercise asking the user to reverse a string may have five test cases with different inputs.

---

**CodingExercise → ExerciseSubmission**
One exercise can receive many submissions from many users over time.
```
CodingExercise 1 ─── many ExerciseSubmission
```
Example: A popular exercise may have thousands of submissions from different users across different attempts.

---

**ApplicationUser → ExerciseSubmission**
One user can make many code submissions across different exercises.
```
ApplicationUser 1 ─── many ExerciseSubmission
```
Example: A user who attempted 20 exercises, some multiple times, may have 35 total submission records.

---

**Quiz → QuizQuestion**
One quiz contains many questions.
```
Quiz 1 ─── many QuizQuestion
```
Example: A Python Basics quiz contains 15 questions of varying difficulty.

---

**QuizQuestion → QuizAnswerOption**
One quiz question has many answer options.
```
QuizQuestion 1 ─── many QuizAnswerOption
```
Example: A multiple-choice question has four answer options, one of which is marked correct.

---

**Quiz → UserQuizAttempt**
One quiz can have many attempt records from many users.
```
Quiz 1 ─── many UserQuizAttempt
```
Example: A quiz available to all users in the Python path accumulates hundreds of attempt records over time.

---

**ApplicationUser → UserQuizAttempt**
One user can make many quiz attempts across different quizzes.
```
ApplicationUser 1 ─── many UserQuizAttempt
```
Example: A user who took three quizzes, retaking one of them, has four UserQuizAttempt records.

---

**UserQuizAttempt → UserQuizAnswer**
One quiz attempt records one answer per question, so one attempt links to many answer records.
```
UserQuizAttempt 1 ─── many UserQuizAnswer
```
Example: A quiz with 15 questions produces 15 UserQuizAnswer records per attempt.

---

**ApplicationUser → Project**
One user can have many projects in their portfolio.
```
ApplicationUser 1 ─── many Project
```
Example: A user may have built three projects throughout their learning journey.

---

**JobOffer → JobApplication**
One job offer can receive many applications from different users.
```
JobOffer 1 ─── many JobApplication
```
Example: A software engineer opening at a company may receive applications from 20 platform users.

---

**ApplicationUser → JobApplication**
One user can apply to many job offers.
```
ApplicationUser 1 ─── many JobApplication
```
Example: A user may apply to five different job offers over time.

---

### 2. One-to-One Relationships

---

**ApplicationUser → CV**
One user can have zero or one CV profile record in the system.
```
ApplicationUser 1 ─── 0 or 1 CV
```
Explanation: Not every user has created a CV yet. Access to the CV feature is restricted to Intermediate and Advanced level users. When created, each user has exactly one CV record that they update over time. Enforced in the database by a unique index on CV.UserId.

---

### 3. Many-to-Many Relationships

All junction tables in this system are **associative entities** — they are not simple connectors. Each one stores additional attributes beyond the two foreign keys, such as progress percentage, score, or completion date. This makes them richer than a pure bridge table.

---

**ApplicationUser ↔ Assessment**
A user can take many assessments, and an assessment can be taken by many users.
This many-to-many relationship is implemented using the UserAssessment table.
```
ApplicationUser many ─── many Assessment
```
Implemented as:
```
ApplicationUser 1 ─── many UserAssessment
Assessment      1 ─── many UserAssessment
```

---

**ApplicationUser ↔ Course**
A user can enrol in many courses, and a course can have many users enrolled in it.
This many-to-many relationship is implemented using the UserCourse table.
```
ApplicationUser many ─── many Course
```
Implemented as:
```
ApplicationUser 1 ─── many UserCourse
Course          1 ─── many UserCourse
```

---

**ApplicationUser ↔ Lesson**
A user can complete many lessons, and a lesson can be completed by many users.
This many-to-many relationship is implemented using the LessonCompletion table.
```
ApplicationUser many ─── many Lesson
```
Implemented as:
```
ApplicationUser 1 ─── many LessonCompletion
Lesson          1 ─── many LessonCompletion
```

---

**ApplicationUser ↔ CodingExercise**
A user can submit code for many exercises, and an exercise can receive submissions from many users.
This many-to-many relationship is implemented using the ExerciseSubmission table.
```
ApplicationUser many ─── many CodingExercise
```
Implemented as:
```
ApplicationUser  1 ─── many ExerciseSubmission
CodingExercise   1 ─── many ExerciseSubmission
```

---

**ApplicationUser ↔ Quiz**
A user can attempt many quizzes, and a quiz can be attempted by many users.
This many-to-many relationship is implemented using the UserQuizAttempt table.
```
ApplicationUser many ─── many Quiz
```
Implemented as:
```
ApplicationUser 1 ─── many UserQuizAttempt
Quiz            1 ─── many UserQuizAttempt
```

---

**ApplicationUser ↔ JobOffer**
A user can apply to many job offers, and a job offer can receive applications from many users.
This many-to-many relationship is implemented using the JobApplication table.
```
ApplicationUser many ─── many JobOffer
```
Implemented as:
```
ApplicationUser 1 ─── many JobApplication
JobOffer        1 ─── many JobApplication
```

---

**Special Case — UserAnswer (Three-Way Associative Entity)**
UserAnswer is an associative table that records the answer selected by a user during a specific assessment attempt. Instead of connecting two entities like a standard junction table, it connects three simultaneously.

It connects:
```
UserAssessment 1 ─── many UserAnswer
Question       1 ─── many UserAnswer
AnswerOption   1 ─── many UserAnswer
```
Important rule: A UserAnswer record is created for every question in the assessment when the user submits. It permanently records which option was chosen and whether it was correct.

---

**Special Case — UserQuizAnswer (Three-Way Associative Entity)**
UserQuizAnswer is an associative table that records the answer selected by a user during a specific quiz attempt. Instead of connecting two entities like a standard junction table, it connects three simultaneously.

It connects:
```
UserQuizAttempt  1 ─── many UserQuizAnswer
QuizQuestion     1 ─── many UserQuizAnswer
QuizAnswerOption 0 or 1 ─── many UserQuizAnswer
```
Important rule: SelectedAnswerOptionId is nullable to support the case where a user submits without answering all questions.

---

### 4. Relationship Summary Table

| Relationship | Type | Implemented Through |
|---|---|---|
| ApplicationUser → UserAssessment | One-to-Many | UserAssessment.UserId |
| Assessment → Question | One-to-Many | Question.AssessmentId |
| Assessment → UserAssessment | One-to-Many | UserAssessment.AssessmentId |
| Question → AnswerOption | One-to-Many | AnswerOption.QuestionId |
| UserAssessment → UserAnswer | One-to-Many | UserAnswer.UserAssessmentId |
| Question → UserAnswer | One-to-Many | UserAnswer.QuestionId |
| AnswerOption → UserAnswer | One-to-Many | UserAnswer.AnswerOptionId |
| ApplicationUser → UserCourse | One-to-Many | UserCourse.UserId |
| Course → UserCourse | One-to-Many | UserCourse.CourseId |
| Course → Lesson | One-to-Many | Lesson.CourseId |
| Course → Quiz | One-to-Many | Quiz.CourseId |
| Lesson → CodingExercise | One-to-Many | CodingExercise.LessonId |
| Lesson → LessonCompletion | One-to-Many | LessonCompletion.LessonId |
| ApplicationUser → LessonCompletion | One-to-Many | LessonCompletion.UserId |
| CodingExercise → ExerciseTestCase | One-to-Many | ExerciseTestCase.ExerciseId |
| CodingExercise → ExerciseSubmission | One-to-Many | ExerciseSubmission.ExerciseId |
| ApplicationUser → ExerciseSubmission | One-to-Many | ExerciseSubmission.UserId |
| Quiz → QuizQuestion | One-to-Many | QuizQuestion.QuizId |
| QuizQuestion → QuizAnswerOption | One-to-Many | QuizAnswerOption.QuizQuestionId |
| Quiz → UserQuizAttempt | One-to-Many | UserQuizAttempt.QuizId |
| ApplicationUser → UserQuizAttempt | One-to-Many | UserQuizAttempt.UserId |
| UserQuizAttempt → UserQuizAnswer | One-to-Many | UserQuizAnswer.UserQuizAttemptId |
| QuizQuestion → UserQuizAnswer | One-to-Many | UserQuizAnswer.QuizQuestionId |
| QuizAnswerOption → UserQuizAnswer | One-to-Many | UserQuizAnswer.SelectedAnswerOptionId |
| ApplicationUser → Project | One-to-Many | Project.UserId |
| JobOffer → JobApplication | One-to-Many | JobApplication.JobOfferId |
| ApplicationUser → JobApplication | One-to-Many | JobApplication.UserId |
| ApplicationUser → CV | Optional One-to-One | CV.UserId (unique index) |
| ApplicationUser ↔ Assessment | Many-to-Many | UserAssessment |
| ApplicationUser ↔ Course | Many-to-Many | UserCourse |
| ApplicationUser ↔ Lesson | Many-to-Many | LessonCompletion |
| ApplicationUser ↔ CodingExercise | Many-to-Many | ExerciseSubmission |
| ApplicationUser ↔ Quiz | Many-to-Many | UserQuizAttempt |
| ApplicationUser ↔ JobOffer | Many-to-Many | JobApplication |
| UserAssessment ↔ Question ↔ AnswerOption | Many-to-Many (Three-Way) | UserAnswer |
| UserQuizAttempt ↔ QuizQuestion ↔ QuizAnswerOption | Many-to-Many (Three-Way) | UserQuizAnswer |
