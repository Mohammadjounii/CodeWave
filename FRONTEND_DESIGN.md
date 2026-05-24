# CodeWave Platform — Front-End Design

The system has two main roles: **Admin** and **Learner**. Each role has different access rights and a different set of pages.

Learners can have one of three proficiency levels: **Beginner**, **Intermediate**, or **Advanced**. These levels are not separate user types — they are progress states within the Learner role that affect which features the learner can access.

Note: CV and Jobs pages are locked for Beginner-level learners. They are only accessible to Intermediate and Advanced learners. These restrictions are enforced both in the interface and in backend authorization checks.

---

## 1. General Layout

Most authenticated learner pages use a fixed left sidebar and a scrollable main content area. The exception is the three learning path pages (Python, Java, and Web Development), which use a top navigation bar layout instead of a sidebar — see the Learning Path Layout Structure diagram below.

### Learner Main Layout Structure

```
--------------------------------------------------
| Sidebar                  | Main Content Area    |
|                          |                      |
| [Logo] CodeWave          | Page title           |
|                          | Cards / Tables /     |
| [Avatar] Name            | Forms / Editors      |
| Level Label              |                      |
|                          |                      |
| Dashboard                |                      |
| Learning Paths           |                      |
| CV (locked if Beginner)  |                      |
| Jobs (locked if Beginner)|                      |
| Projects                 |                      |
| Quizzes                  |                      |
| Settings                 |                      |
|                          |                      |
| [Dark / Light Mode]      |                      |
| [Log Out]                |                      |
--------------------------------------------------
```

### Admin Main Layout Structure

```
--------------------------------------------------
| Sidebar                  | Main Content Area    |
|                          |                      |
| [Logo] CodeWave          | Page title           |
| Admin Panel              | Cards / Charts /     |
|                          | Tables / Forms       |
| Dashboard                |                      |
| User Management          |                      |
| Courses                  |                      |
| Jobs                     |                      |
| Reports                  |                      |
|                          |                      |
| [Back to Site]           |                      |
| [Admin Name] — Admin     |                      |
--------------------------------------------------
```

### Learning Path Layout Structure

The three learning path pages (Python, Java, Web Development) use a different layout — a top navigation bar instead of a sidebar.

```
--------------------------------------------------
| [Logo] CodeWave  Dashboard  Learning Paths      |
|                  Projects   Jobs    [Avatar]     |
--------------------------------------------------
| Lessons Panel          | Lesson Detail Panel    |
|                        |                        |
| [v] Lesson 1           | CURRENT LESSON         |
| [v] Lesson 2           | Lesson Title           |
| [>] Lesson 3  <        | Short description...   |
| [x] Lesson 4           |                        |
| [x] Lesson 5           | [Lesson] [Code Editor] |
|                        |                        |
| Overall Progress: 40%  | Lesson content / code  |
| ████████░░░░           | editor loads here      |
|                        |                        |
|                        | [Run Code]             |
|                        | [Mark as Complete ->]  |
--------------------------------------------------
```

---

## 2. Authentication Pages

### 2.1 Login Page

**Purpose**
Allows platform users to sign in securely using email/password or OAuth providers.

**Page Elements**

| Element | Description |
|---------|-------------|
| CodeWave Logo | Displayed at the top left of the page. |
| Platform Subtitle | "Interactive Coding Platform" shown beside the logo. |
| Welcome Heading | "Welcome Back!" displayed inside the login card. |
| Subtext | "Login to access your personalized learning path and project tracking." |
| Email Field | User enters their registered email address. |
| Password Field | User enters their password. |
| Forgot Password Link | Navigates to the Forgot Password page. |
| Login Button | Submits the login form. |
| Google Login Button | Authenticates the user via Google OAuth. |
| GitHub Login Button | Authenticates the user via GitHub OAuth. |
| Sign Up Link | Navigates to the registration page. |
| Validation Error Area | Displays field-level and summary error messages. |
| Loading Overlay | Shown after form submit with a progress bar and rotating motivational messages. |

**Suggested Design**
```
--------------------------------------------------
|  [Logo] CodeWave    Interactive Coding Platform |
--------------------------------------------------
|                                                |
|         [ CodeWave Logo Icon ]                 |
|         Welcome Back!                          |
|         Login to access your personalized      |
|         learning path and project tracking.    |
|                                                |
|         Email address                          |
|         [mail icon] [______________________]   |
|                                                |
|         Password            Forgot Password?   |
|         [lock icon] [______________________]   |
|                                                |
|         [ Login -> ]                           |
|                                                |
|         -------- Or continue with --------    |
|                                                |
|         [ Google ]         [ GitHub ]          |
|                                                |
|         Don't have an account? Sign up         |
--------------------------------------------------
```

---

### 2.2 Sign Up Page

**Purpose**
Allows new users to create an account.

**Page Elements**

| Element | Description |
|---------|-------------|
| CodeWave Logo | Displayed at the top left. |
| First Name Field | User's first name. |
| Last Name Field | User's last name. |
| Email Field | User's email address. |
| Password Field | User's chosen password. |
| Register Button | Submits the registration form. |
| Google Sign Up Button | Registers via Google OAuth. |
| GitHub Sign Up Button | Registers via GitHub OAuth. |
| Login Link | Navigates back to the login page for existing users. |
| Validation Error Area | Displays field-level validation messages. |

**Suggested Design**
```
--------------------------------------------------
|  [Logo] CodeWave    Interactive Coding Platform |
--------------------------------------------------
|                                                |
|         [ CodeWave Logo Icon ]                 |
|         Create your account                    |
|                                                |
|         First Name [________]                  |
|         Last Name  [________]                  |
|         Email      [____________________]      |
|         Password   [____________________]      |
|                                                |
|         [ Create Account -> ]                  |
|                                                |
|         -------- Or continue with --------    |
|         [ Google ]         [ GitHub ]          |
|                                                |
|         Already have an account? Login         |
--------------------------------------------------
```

---

### 2.3 Forgot Password Page

**Purpose**
Allows users to request a password reset link sent to their email.

**Page Elements**

| Element | Description |
|---------|-------------|
| Heading | "Forgot Password?" displayed at the top. |
| Instruction Text | Explains that a reset link will be sent to the provided email. |
| Email Field | User enters their registered email address. |
| Send Reset Link Button | Submits the request and sends the password reset email. |

**Suggested Design**
```
--------------------------------------------------
|                                                |
|         Forgot Password?                       |
|         Enter your email and we'll send you    |
|         a link to reset your password.         |
|                                                |
|         Email address                          |
|         [____________________________________] |
|                                                |
|         [ Send Reset Link ]                    |
|                                                |
|         <- Back to Login                       |
--------------------------------------------------
```

---

### 2.4 Forgot Password Confirmation Page

**Purpose**
Shown immediately after the user submits the Forgot Password form. Confirms the reset link has been generated and provides a link for development environments.

**Page Elements**

| Element | Description |
|---------|-------------|
| Email Icon | Large green email icon displayed at the top. |
| Heading | "Check Your Email" displayed prominently. |
| Subtext | "Password reset link has been generated." |
| Reset Link Panel | In development mode: shows the reset link directly on screen with the target email address. In production: shows a success message only. |
| Back to Login Button | Navigates back to the Login page. |

**Suggested Design**
```
--------------------------------------------------
|                                                |
|         [✉ Green Email Icon]                  |
|         Check Your Email                       |
|         Password reset link has been generated.|
|                                                |
|         [Reset link shown here in dev mode]    |
|         In production: a success message is    |
|         shown — no email address is revealed.  |
|                                                |
|         [ Back to Login ]                      |
--------------------------------------------------
```

---

### 2.5 Reset Password Page

**Purpose**
Allows users to set a new password using the reset token received by email.

**Page Elements**

| Element | Description |
|---------|-------------|
| Heading | "Reset Password" displayed at the top. |
| New Password Field | User enters their new password. |
| Confirm Password Field | User confirms the new password. |
| Reset Button | Submits the new password and completes the reset. |

**Suggested Design**
```
--------------------------------------------------
|                                                |
|         Reset Password                         |
|                                                |
|         New Password                           |
|         [____________________________________] |
|                                                |
|         Confirm New Password                   |
|         [____________________________________] |
|                                                |
|         [ Reset Password ]                     |
--------------------------------------------------
```

---

### 2.6 GitHub Email Entry Page

**Purpose**
Shown during GitHub OAuth sign-up when GitHub does not share the user's email address. Asks the user to enter their email manually to complete account setup.

**Page Elements**

| Element | Description |
|---------|-------------|
| CodeWave Logo | Top left header. |
| GitHub Icon | Displayed inside the form card. |
| Heading | "One More Step" displayed inside the card. |
| Subtext | Explains that GitHub did not share an email and asks the user to provide it. |
| Email Field | User enters their email address. |
| Complete Sign Up Button | Submits the email and finalises account creation. |
| Back to Login Link | Navigates back to the Login page. |

**Suggested Design**
```
--------------------------------------------------
|  [Logo] CodeWave    Interactive Coding Platform |
--------------------------------------------------
|                                                |
|         [ GitHub Icon ]                        |
|         One More Step                          |
|         GitHub did not share your email.       |
|         Please enter it below.                 |
|                                                |
|         Email address                          |
|         [mail icon] [______________________]   |
|                                                |
|         [ Complete Sign Up -> ]                |
|                                                |
|         Changed your mind? Back to Login       |
--------------------------------------------------
```

---

## 3. Onboarding Pages

### 3.1 Welcome / Overview Page

**Purpose**
Public-facing landing page that introduces the CodeWave platform to visitors before they sign in or register.

**Page Elements**

| Element | Description |
|---------|-------------|
| Navigation Bar | Platform name, links to features, login and register buttons. |
| Hero Section | Large headline and platform description with a call-to-action button. |
| Features Section | Cards describing platform features such as learning paths, coding exercises, quizzes, and CV generation. |
| Learning Paths Section | Preview cards for Python, Java, and Web Development paths. |

**Suggested Design**
```
--------------------------------------------------
| CodeWave                  Login    Get Started  |
--------------------------------------------------
|                                                |
|    Learn to Code. Build Your Future.           |
|    Master Python, Java, and Web Development    |
|    through interactive lessons and projects.   |
|                                                |
|    [ Get Started ]   [ Learn More ]            |
--------------------------------------------------
|  Features                                      |
|  [Learning Paths] [Coding Exercises]           |
|  [Quizzes]        [CV Builder]                 |
|  [Job Offers]     [Projects]                   |
--------------------------------------------------
```

---

### 3.2 Onboarding Flow

**Purpose**
A multi-step onboarding flow completed by new users immediately after registration. Collects preferences used to personalize the learning experience.

**Onboarding Steps**

| Step | Page | Purpose |
|------|------|---------|
| 1 | Language Interest | User selects their preferred programming language: Python, Java, or Web Development. |
| 2 | User Interest | User selects programming interests such as Data Science, Web Development, or Game Development. |
| 3 | User Skill | User self-reports their current skill level: Beginner, Intermediate, or Advanced. This is an initial self-assessment only. |
| 4 | User Goal | User selects their learning goal, such as career change, skill improvement, or academic study. |
| 5 | User Motivation | User selects their motivation for learning, such as getting a job, personal growth, or academic requirements. |
| 6 | User Schedule | User selects how many hours per week they plan to dedicate to learning. |

**Suggested Design (per step)**
```
--------------------------------------------------
| CodeWave                                        |
--------------------------------------------------
|                                                |
|    Step 1 of 6                                 |
|    ████████░░░░░░░░░░  (progress bar)          |
|                                                |
|    Which programming language interests        |
|    you the most?                               |
|                                                |
|    [ Python        ]                           |
|    [ Java          ]                           |
|    [ Web Dev       ]                           |
|                                                |
|                          [ Next -> ]           |
--------------------------------------------------
```

---

### 3.3 Skills Assessment Page

**Purpose**
Presents the initial skills assessment one question at a time. The score determines the user's proficiency level (Beginner, Intermediate, or Advanced) and assigns a learning path. Note: during onboarding (Step 3), the user may self-report their skill level as a starting point. The skills assessment then confirms or updates that level based on actual performance — the assessment result takes precedence.

**Page Elements**

| Element | Description |
|---------|-------------|
| CodeWave Header | Platform logo and assessment title shown at the top. |
| Progress Bar | Shows current question number out of total questions. |
| Question Number Label | Displays the current question index and total count. |
| Question Text | Full text of the current assessment question. |
| Answer Options | Multiple-choice radio buttons with answer option text. |
| Next Button | Submits the current answer and loads the next question. |

**Suggested Design**
```
--------------------------------------------------
| [Logo]  CodeWave Assessment                    |
--------------------------------------------------
|                                                |
|  Question 3 of 10                              |
|  ████████████░░░░░░░░  30%                     |
|                                                |
|  What is the output of print(type(3.14))?      |
|                                                |
|  ( ) <class 'int'>                             |
|  ( ) <class 'str'>                             |
|  (*) <class 'float'>                           |
|  ( ) <class 'double'>                          |
|                                                |
|                          [ Next Question -> ]  |
--------------------------------------------------
```

---

## 4. Learner Pages

### 4.1 User Dashboard

**Purpose**
The main dashboard for logged-in learners. Shows a snapshot of the user's learning progress, recommended next step, per-skill mastery, and quick-access job recommendations.

**Dashboard Cards**

| Card | Description |
|------|-------------|
| Completed Courses | Number of courses the user has fully completed. |
| Overall Progress | Overall progress percentage across all courses in the user's learning path. Shown with a mini progress bar. |
| Learning Path | The name of the user's assigned learning path (Python, Java, or Web Development). |
| Quizzes | Number of quizzes passed out of total attempts. Shows average score if available. Clicking navigates to the Quiz list. |

**Dashboard Sections**

| Section | Description |
|---------|-------------|
| Your Next Step | Recommended lesson card with course name and a Start Lesson button. If no lesson is found, shows a Get Started prompt to explore learning paths. |
| Skills Progress | Shows the overall learning path progress bar (lessons, exercises, quizzes, study time breakdown) followed by individual skill cards each with a mastery percentage bar. A "View All Skills" button expands additional skills. |
| Job Recommendations | Filterable panel showing sample job cards (Frontend, Backend, Full-Stack, Remote filters). Each card shows job title, company, location, and required skills. Beginner-level users can see the job preview cards but the View Offer button is locked — they cannot open the job details or apply until they reach Intermediate level. |

**Profile Picture Modal**

| Element | Description |
|---------|-------------|
| Avatar Click Trigger | Clicking the user's avatar in the sidebar opens the modal. |
| Large Profile Picture | Full-size preview of the current profile picture. |
| Change Picture Button | Opens a file picker to upload a new image (JPG, PNG, GIF — max 5 MB). |

**Suggested Design**
```
--------------------------------------------------
| Sidebar              | Dashboard               |
|                      |                         |
| [Logo] CodeWave      | Welcome back, Ali! 👋   |
|                      |                         |
| [Avatar]             | [Completed Courses    ] |
| Ali Hassan           | [Overall Progress    %] |
| Intermediate         | [Learning Path Name   ] |
|                      | [Quizzes Passed/Total ] |
| Dashboard <          |                         |
| Learning Paths       | Your Next Step          |
| CV                   | [Recommended Lesson   ] |
| Jobs                 | [Your Course Card     ] |
| Projects             | [ Start Lesson -> ]     |
| Quizzes              |                         |
| Settings             | Skills Progress         |
|                      | Python  ████████  75%   |
| [Dark Mode]          | Functions ██████  60%   |
| [Log Out]            | OOP       ████    40%   |
|                      | [ View All Skills ]     |
|                      |                         |
|                      | Job Recommendations     |
|                      | [All][Frontend][Backend]|
|                      | [Full-Stack][Remote]    |
|                      | Frontend Dev — Stripe   |
|                      | React, JS  [View Offer] |
--------------------------------------------------
```

---

### 4.2 Learning Path Overview Pages

**Purpose**
Three dedicated pages — one each for Python, Java, and Web Development — show all lessons across the entire selected learning path. Each page uses a top navigation bar layout (not the sidebar layout). Lessons are listed on the left. Clicking a lesson loads its content in a right-side panel dynamically via AJAX without a page reload. Note: this page covers the entire path, not a single course. To view lessons within one specific course, see section 4.3 (Course Page).

**Page Elements**

| Element | Description |
|---------|-------------|
| Top Navigation Bar | CodeWave logo, nav links (Dashboard, Learning Paths, Projects, Jobs), user avatar, and notifications button. |
| Overall Progress Bar | Shows completion percentage for the entire path at the top of the lessons list. |
| Lessons List (Left Panel) | Ordered list of all lessons. Each item shows an icon: green check (completed), primary play arrow (current), grey lock (locked). Clicking a lesson loads it in the right panel. |
| Lesson Detail Panel (Right Panel) | Displays the currently selected lesson: title, short description, and a tabbed interface with Lesson tab (content) and Code Editor tab. |
| Lesson Content | Full lesson HTML rendered inside the right panel. Optional embedded video. |
| Code Editor | Shown in the Code Editor tab if the lesson has an exercise. Pre-loaded with starter code. Includes a Run Code button. |
| Mark as Complete Button | Marks the lesson complete via a POST request and reloads the page to update progress. |

**Suggested Design**
```
--------------------------------------------------
| [Logo] CodeWave  Dashboard  Learning Paths      |
|                  Projects   Jobs    [Avatar]     |
--------------------------------------------------
| Python Learning Path       | CURRENT LESSON      |
|                            |                     |
| Overall: ████████░░  75%   | Variables           |
|                            | Learn how Python    |
| [v] Variables              | variables work...   |
| [v] Data Types             |                     |
| [>] Functions   <          | [Lesson] [Code Ed.] |
| [x] OOP                    |                     |
| [x] Modules                | Variable is a       |
| [x] File I/O               | container for       |
| [x] Exceptions             | storing values...   |
|                            |                     |
|                            | [Run Code]          |
|                            | [Mark as Complete ->]|
--------------------------------------------------
```

---

### 4.3 Course Page

**Purpose**
Displays all lessons within one specific course. Users can view their progress per lesson and navigate to any available lesson. Completed lessons are marked. Locked lessons require completing the previous lesson first. Note: unlike section 4.2 (Learning Path Overview), which spans the entire learning path and uses a top navigation bar, this page is scoped to a single course and uses the standard sidebar layout.

**Page Elements**

| Element | Description |
|---------|-------------|
| Course Sidebar | Shows the CodeWave logo, Dashboard link, Back to Path link, and the ordered list of lessons. |
| Course Title | Displayed at the top of the main content area. |
| Course Description | Brief summary of what the course covers. |
| Lesson List | Ordered list of lessons with title, completion status, and a Start/Continue button. |
| Progress Indicator | Shows how many lessons have been completed out of the total. |
| Quiz Button | If a quiz exists for the course, a button to open it is shown. |

**Suggested Design**
```
--------------------------------------------------
| [Logo] CodeWave      | Python Basics           |
|                      | Learn Python from the   |
| Dashboard            | ground up.              |
| <- Back to Path      |                         |
|                      | Progress: 3 / 8 lessons |
| Lessons:             | ████████░░░░░░ 37%      |
| [v] Variables        |                         |
| [v] Data Types       | Lesson 1 - Variables    |
| [v] Loops            | Completed               |
| [>] Functions  <     | [Review Lesson]         |
| [x] OOP              |                         |
| [x] Modules          | Lesson 4 - Functions    |
| [x] File I/O         | In Progress             |
| [x] Exceptions       | [Continue ->]           |
|                      |                         |
|                      | [Take Quiz]             |
--------------------------------------------------
```

---

### 4.4 Focus Mode — Lesson Page

**Purpose**
The main learning interface. Displays the full lesson content on the left and a Monaco code editor with a coding exercise on the right. Users can read the lesson, write and run code, submit their solution, and interact with the AI chatbot.

**Page Sections**

| Section | Description |
|---------|-------------|
| Top Navigation Bar | Shows CodeWave logo, lesson title, and a Back to Course button. |
| Lesson Content Panel | Displays the full lesson text, theory, and optional images. |
| Code Editor Panel | Monaco editor pre-loaded with starter code for the exercise. |
| Run Button | Executes the user's code and displays output in the console. |
| Submit Button | Runs all test cases and records the submission result. |
| Console Output Area | Shows the output or error returned from the code execution. |
| Test Cases Panel | Lists individual test cases with their pass or fail status. |
| AI Chatbot Button | Opens a floating popup chatbot for help with the lesson or exercise. |
| Mark Complete Button | Marks the lesson as completed and unlocks the next lesson. |
| Navigation Arrows | Previous and Next lesson buttons. |

**Suggested Design**
```
--------------------------------------------------
| [Logo] <- Back to Course    Functions - Lesson |
--------------------------------------------------
| Lesson Content         | Code Editor            |
|                        |                        |
| ## Functions in Python | def add(a, b):         |
|                        |     return a + b       |
| A function is a block  |                        |
| of reusable code...    | result = add(3, 5)     |
|                        | print(result)          |
| [Image / Diagram]      |                        |
|                        | [Run] [Submit]         |
|                        |                        |
|                        | Console Output:        |
|                        | > 8                    |
|                        |                        |
|                        | Test Cases:            |
|                        | [v] add(1,2) = 3       |
|                        | [v] add(0,0) = 0       |
|                        | [x] add(-1,1) = 0      |
--------------------------------------------------
| [<- Prev Lesson]  [Mark Complete]  [Next ->]   |
|                               [AI Help]         |
--------------------------------------------------
```

---

### 4.5 Standalone Exercise Page

**Purpose**
A simplified standalone page for a single coding exercise, accessible outside of Focus Mode.

**Page Elements**

| Element | Description |
|---------|-------------|
| Exercise Title | Name of the coding exercise. |
| Exercise Description | Problem statement and instructions. |
| Monaco Code Editor | Editable code area pre-loaded with starter code. |
| Timeout Selector | Dropdown to select code execution timeout: 3s, 5s, or 10s. |
| Run Button | Executes the code and shows output without validating against test cases. |
| Submit Button | Validates the code against test cases and records the result. |
| Console Output Area | Displays the output or errors from code execution. |
| Submission Result Area | Shows whether the submission passed or failed after submit. |

**Suggested Design**
```
--------------------------------------------------
| Reverse a String                                |
| Write a function that reverses a string.        |
--------------------------------------------------
| Code                                            |
| [_________________________________________]     |
| [_________________________________________]     |
| [_________________________________________]     |
--------------------------------------------------
| [Run] [Submit] Timeout [5s v]                   |
--------------------------------------------------
| Console output                                  |
| > Output will appear here...                    |
--------------------------------------------------
```

---

### 4.6 About Us Page

**Purpose**
Informational page describing the CodeWave platform's mission, vision, and key features. Accessible from the main layout.

**Page Elements**

| Element | Description |
|---------|-------------|
| Page Title | "About CodeWave" shown as the main heading. |
| Mission Card | Describes the platform's mission to provide accessible, engaging coding education for Python, Java, and Web Development. |
| Vision Card | Describes the vision of empowering anyone to become a developer regardless of background. |
| Platform Features Grid | Six feature cards: Interactive Courses, Real Code Execution, Progress Tracking, Quizzes and Assessments, Auto-Generated CV, Job Opportunities. |
| Footer | Copyright line: "© 2025 CodeWave — Built to empower the next generation of developers." |

**Suggested Design**
```
--------------------------------------------------
|                                                |
|              About CodeWave                    |
|   Empowering learners to master programming    |
|   through interactive lessons and projects.    |
--------------------------------------------------
|  Our Mission          |  Our Vision            |
|  Accessible coding    |  Anyone can become     |
|  education through    |  a developer through   |
|  hands-on practice.   |  structured learning.  |
--------------------------------------------------
|  What CodeWave Offers                          |
|  [Interactive Courses] [Real Code Execution]   |
|  [Progress Tracking]   [Quizzes & Assessments] |
|  [Auto-Generated CV]   [Job Opportunities]     |
--------------------------------------------------
```

---

## 5. Quiz Pages

### 5.1 Quiz List Page

**Purpose**
Displays available quizzes associated with courses on the user's learning path. Users can see quiz details and start an attempt.

**Page Elements**

| Element | Description |
|---------|-------------|
| Page Title | "Quizzes" shown as the heading. |
| Quiz Cards | Each card shows quiz title, associated course, time limit, passing score, and question count. |
| Start Quiz Button | Opens the quiz taking page. |

**Suggested Design**
```
--------------------------------------------------
| Sidebar              | Quizzes                 |
|                      |                         |
|                      | Python Basics Quiz      |
|                      | Course: Python Basics   |
|                      | 15 Questions | 30 min   |
|                      | Passing Score: 70%      |
|                      | [Start Quiz]            |
|                      |                         |
|                      | OOP Concepts Quiz       |
|                      | Course: Python OOP      |
|                      | 10 Questions | 20 min   |
|                      | Passing Score: 70%      |
|                      | [Start Quiz]            |
--------------------------------------------------
```

---

### 5.2 Take Quiz Page

**Purpose**
Allows the user to answer all quiz questions within a time limit. Displays a countdown timer, all questions with multiple-choice options, and a submit button.

**Page Elements**

| Element | Description |
|---------|-------------|
| Quiz Title | Name of the quiz being taken. |
| Course Name | The course this quiz belongs to. |
| Countdown Timer | Live timer counting down from the quiz time limit in minutes. |
| Question List | All quiz questions displayed sequentially with multiple-choice radio options. |
| Difficulty Badge | Shows the difficulty of each question: Easy, Medium, or Hard. |
| Submit Quiz Button | Submits all answers and redirects to the results page. |

**Suggested Design**
```
--------------------------------------------------
| Python Basics Quiz                   [T] 28:45 |
| Course: Python Basics                          |
--------------------------------------------------
|                                                |
| Question 1  [Easy]                             |
| What is the output of print(2 ** 3)?           |
| ( ) 6                                          |
| (*) 8                                          |
| ( ) 9                                          |
| ( ) 16                                         |
|                                                |
| Question 2  [Medium]                           |
| Which method removes the last item in a list?  |
| ( ) remove()                                   |
| ( ) delete()                                   |
| (*) pop()                                      |
| ( ) discard()                                  |
|                                                |
--------------------------------------------------
|                         [ Submit Quiz ]        |
--------------------------------------------------
```

---

### 5.3 Quiz Results Page

**Purpose**
Shows the outcome of a completed quiz attempt including score, pass/fail status, time spent, and a detailed breakdown of each question with the user's answer and the correct answer.

**Page Elements**

| Element | Description |
|---------|-------------|
| Result Heading | "Passed!" or "Failed" displayed prominently. |
| Score Display | Percentage score achieved in the attempt. |
| Time Spent | Total minutes spent on the quiz. |
| Passing Score | The minimum score required to pass. |
| Answer Review | Each question listed with the user's selected answer and the correct answer highlighted. |
| Retake Button | Allows the user to start a new attempt. |
| Back to Course Button | Returns the user to the course page. |

**Suggested Design**
```
--------------------------------------------------
| Passed!                                        |
--------------------------------------------------
| Score: 85%   |  Time: 18 min  |  Passed: Yes   |
| Passing Score: 70%                             |
--------------------------------------------------
| Answer Review                                  |
|                                                |
| Q1: What is the output of print(2 ** 3)?       |
| Your Answer:    8        [v]                   |
| Correct Answer: 8                              |
|                                                |
| Q2: Which method removes the last list item?   |
| Your Answer:    remove() [x]                   |
| Correct Answer: pop()                          |
--------------------------------------------------
| [Retake Quiz]                [Back to Course]  |
--------------------------------------------------
```

---

## 6. CV Pages

### 6.1 CV Builder Page

**Purpose**
Allows Intermediate and Advanced users to build and manage their professional CV. Contains multiple form sections and a template selector. Beginners see a locked state.

**Form Sections**

Section 1: Personal Information

| Field | Type |
|-------|------|
| Full Name | Text |
| Age | Number |
| Location | Text |
| Email | Text |
| Phone | Text |
| LinkedIn URL | Text |
| GitHub URL | Text |
| CV Profile Picture | File Upload |

Section 2: Professional Summary

| Field | Type |
|-------|------|
| Summary | Text Area |

Section 3: Education

| Field | Type |
|-------|------|
| Institution | Text |
| Education Details (Degree, Field, Year) | Text Area |

Section 4: Skills

| Field | Type |
|-------|------|
| General Skills | Text |
| Programming Languages | Text |
| Spoken Languages | Text |

Section 5: Experience and Certifications

| Field | Type |
|-------|------|
| Work Experience | Text Area |
| Certifications | Text Area |

Section 6: CV Template

| Field | Type |
|-------|------|
| Template Selector | Dropdown (modern, classic, creative, minimalist, executive) |

Section 7: Upload Existing CV

| Field | Type |
|-------|------|
| Upload CV File (PDF/DOCX) | File Upload |

**Suggested Design**
```
--------------------------------------------------
| Sidebar              | CV Builder              |
|                      |                         |
|                      | Personal Information    |
|                      | Full Name [__________]  |
|                      | Age [___] Location [__] |
|                      | Email [______________]  |
|                      | Phone [______________]  |
|                      | LinkedIn [____________] |
|                      | GitHub [______________] |
|                      | [Upload Photo]          |
|                      |                         |
|                      | Summary                 |
|                      | [___________________]   |
|                      |                         |
|                      | Education               |
|                      | Institution [_________] |
|                      | Details [_____________] |
|                      |                         |
|                      | Skills                  |
|                      | General [_____________] |
|                      | Languages [___________] |
|                      | Spoken [______________] |
|                      |                         |
|                      | Template [modern v]     |
|                      |                         |
|                      | [Save CV] [Preview CV]  |
--------------------------------------------------
```

---

### 6.2 CV View Page

**Purpose**
Renders the user's CV in a styled, printable layout based on the selected template. Provides options to download as PDF.

**Page Elements**

| Element | Description |
|---------|-------------|
| CV Preview | Fully rendered CV layout using the selected template. |
| Download PDF Button | Generates and downloads the CV as a PDF file. |
| Edit CV Button | Navigates back to the CV Builder page. |

**Suggested Design**
```
--------------------------------------------------
| Sidebar              | Your CV Preview         |
|                      |                         |
|                      | [Edit CV] [Download PDF]|
|                      |                         |
|                      | +---------------------+ |
|                      | | Ali Hassan          | |
|                      | | Intermediate Dev    | |
|                      | | ali@email.com       | |
|                      | |---------------------| |
|                      | | Summary: ...        | |
|                      | |---------------------| |
|                      | | Skills: Python, Java| |
|                      | | Education: ...      | |
|                      | +---------------------+ |
--------------------------------------------------
```

---

## 7. Jobs Pages

### 7.1 Job Offers Page

**Purpose**
Displays all active job offers posted by the admin. Shows a skill-match percentage calculated by comparing the job's required skills against the user's profile. Users can apply with an optional cover letter.

**Page Elements**

| Element | Description |
|---------|-------------|
| Search Bar | Search job offers by title, company, or keyword. |
| Job List Panel | Left panel showing all available job cards with title, company, and match percentage. |
| Job Detail Panel | Right panel showing the full job description, required skills, deadline, and apply button. |
| Match Percentage Badge | Calculated percentage showing how well the user's skills match the job. |
| Apply Button | Opens an apply form with an optional cover letter field. |
| Deadline Indicator | Shows the application deadline date. Apply button is disabled after the deadline. |

**Suggested Design**
```
--------------------------------------------------
| Sidebar   | Job Offers              | Job Detail|
|           |                        |           |
|           | Search [____________]  | Company X |
|           |                        | Full Stack|
|           | [Full Stack Dev      ] | Developer |
|           | Company X | 85% match  |           |
|           | Deadline: Jan 30       | Required: |
|           |                        | Python,JS |
|           | [Backend Engineer    ] | React     |
|           | Company Y | 60% match  |           |
|           | Deadline: Feb 10       | Match: 85%|
|           |                        |           |
|           | [Mobile Developer    ] | Deadline: |
|           | Company Z | 40% match  | Jan 30    |
|           |                        |           |
|           |                        | Cover:    |
|           |                        | [_______] |
|           |                        |           |
|           |                        | [Apply]   |
--------------------------------------------------
```

---

### 7.2 Applied Jobs Page

**Purpose**
Shows all job applications submitted by the current user. Displays the status of each application.

**Page Elements**

| Element | Description |
|---------|-------------|
| Page Title | "My Applications" shown as heading. |
| Applications Table | Lists all submitted applications with job title, company, applied date, and status. |
| Status Badge | Shows the current status: Pending, Reviewed, Accepted, or Rejected. |

**Suggested Design**
```
--------------------------------------------------
| Sidebar              | My Applications         |
|                      |                                      |
|                      | Job Title | Company | Date   | Status   |
|                      |-----------|---------|--------|----------|
|                      | Full Stack| Corp X  | Jan 5  | Pending  |
|                      | Backend   | Corp Y  | Jan 8  | Reviewed |
|                      | Mobile Dev| Corp Z  | Jan 9  | Accepted |
--------------------------------------------------
```

---

## 8. Projects Pages

### 8.1 Projects Page

**Purpose**
Displays all portfolio projects added by the user. Projects are used as portfolio items and can appear in the CV.

**Page Elements**

| Element | Description |
|---------|-------------|
| Page Title | "My Projects" shown as heading. |
| Add Project Button | Opens the project creation form. |
| Project Cards | Each card shows title, description, completion date, and result. |
| Edit Button | Opens the project edit form. |
| Delete Button | Removes the project after confirmation. |

**Suggested Design**
```
--------------------------------------------------
| Sidebar              | My Projects  [+ Add Project] |
|                      |                         |
|                      | [Calculator App       ] |
|                      | Built with Python.      |
|                      | Completed: Dec 2024     |
|                      | Result: A               |
|                      | [Edit] [Delete]         |
|                      |                         |
|                      | [To-Do List App       ] |
|                      | Built with HTML/CSS/JS  |
|                      | Completed: Jan 2025     |
|                      | Result: B+              |
|                      | [Edit] [Delete]         |
--------------------------------------------------
```

---

### 8.2 Add / Edit Project Page

**Purpose**
Form to create a new project or update an existing one.

**Form Fields**

| Field | Type |
|-------|------|
| Title | Text |
| Description | Text Area |
| Completion Date | Date Picker |
| Result | Text |

**Suggested Design**
```
--------------------------------------------------
| Sidebar              | Add New Project         |
|                      |                         |
|                      | Title [________________]|
|                      |                         |
|                      | Description             |
|                      | [_____________________] |
|                      | [_____________________] |
|                      |                         |
|                      | Completion Date [____]  |
|                      |                         |
|                      | Result [_______________]|
|                      |                         |
|                      | [Cancel]       [Save]   |
--------------------------------------------------
```

---

## 9. Settings Page

**Purpose**
Allows the user to update their account password and manage their profile picture.

**Page Elements**

| Element | Description |
|---------|-------------|
| Page Title | "Settings" shown as heading. |
| Current Password Field | User enters their existing password. |
| New Password Field | User enters the desired new password. |
| Confirm New Password Field | User re-enters the new password for confirmation. |
| Change Password Button | Submits the password change form. |
| Profile Picture Upload | Allows the user to upload or change their profile picture. |
| Validation Error Area | Shows password mismatch or incorrect password errors. |

**Suggested Design**
```
--------------------------------------------------
| Sidebar              | Settings                |
|                      |                         |
|                      | Change Password         |
|                      |                         |
|                      | Current Password        |
|                      | [____________________]  |
|                      |                         |
|                      | New Password            |
|                      | [____________________]  |
|                      |                         |
|                      | Confirm New Password    |
|                      | [____________________]  |
|                      |                         |
|                      | [ Change Password ]     |
|                      |                         |
|                      | Profile Picture         |
|                      | [Upload Photo]          |
--------------------------------------------------
```

---

## 10. Admin Pages

### 10.1 Admin Dashboard

**Purpose**
Overview page for the administrator. Displays platform-wide KPI cards and a user engagement chart.

**Dashboard Cards**

| Card | Description |
|------|-------------|
| Total Learners | Total number of registered users on the platform. |
| Active Courses | Total number of available courses. |
| Active Job Offers | Total number of active job offers posted. |
| Avg. Project Score | Average project score across all user submissions. |

**Chart**

| Chart | Description |
|-------|-------------|
| User Engagement | Line chart showing active sessions over the last 1 day, 7 days, or 30 days. Filterable by time range. |

**Suggested Design**
```
--------------------------------------------------
| Admin Sidebar     | Dashboard                  |
|                   | Welcome back, Admin.        |
| [Logo] CodeWave   |                            |
| Admin Panel       | [Total Learners]           |
|                   | [Active Courses]           |
| Dashboard <       | [Active Job Offers]        |
| User Management   | [Avg. Project Score]       |
| Courses           |                            |
| Jobs              | User Engagement Chart      |
| Reports           | [30 Days] [7 Days] [24hr]  |
|                   | +----------------------+   |
| [Back to Site]    | |  /\      /\          |   |
| [Admin Name] Admin| | /  \    /  \         |   |
|                   | +----------------------+   |
--------------------------------------------------
```

---

### 10.2 Admin User Management Page

**Purpose**
Allows the admin to view, search, and manage all registered users. Provides access to individual user details and an edit form.

**Page Elements**

| Element | Description |
|---------|-------------|
| Page Title | "User Management" shown as heading. |
| Search Bar | Search by name or email. |
| Users Table | Displays all users with name, email, learning path, level, and registration date. |
| View Details Button | Opens the user details page. |
| Edit Button | Opens the Edit User form (see 10.3). |

**Suggested Design**
```
--------------------------------------------------
| Admin Sidebar     | User Management             |
|                   |                             |
|                   | Search [_______________]    |
|                   |                             |
|                   | Name | Email | Path  | Level | Actions |
|                   |------|-------|-------|-------|---------|
|                   | Ali  | ali@. | Python| Inter.| View Edit|
|                   | Sara | sara@ | Java  | Begin.| View Edit|
|                   | Omar | omar@ | Web   | Advan.| View Edit|
--------------------------------------------------
```

---

### 10.3 Admin User Details Page

**Purpose**
Shows the full profile of a specific user including their level, learning path, progress, and quiz attempts.

**Page Elements**

| Element | Description |
|---------|-------------|
| User Identity Card | Name, email, level, learning path, and profile picture. |
| Onboarding Data | Shows the user's selected interests, goal, motivation, skill level, and weekly hours. |
| Enrolled Courses | Lists courses the user is enrolled in with progress percentage. |
| Quiz Attempts | Lists the user's quiz attempts with score and pass status. |

**Suggested Design**
```
--------------------------------------------------
| Admin Sidebar     | User Details               |
|                   |                            |
|                   | [Avatar] Ali Hassan        |
|                   | ali@email.com              |
|                   | Level: Intermediate        |
|                   | Path: Python               |
|                   |                            |
|                   | Interests: Data Science    |
|                   | Goal: Career Change        |
|                   | Weekly Hours: 10           |
|                   |                            |
|                   | Courses:                   |
|                   | Python Basics -- 75%       |
|                   | Python OOP    -- 30%       |
|                   |                            |
|                   | Quiz Attempts:             |
|                   | Python Basics Quiz -- 80% [v] |
--------------------------------------------------
```

---

### 10.4 Admin Create / Edit User Page

**Purpose**
Form for creating a new user or editing an existing one. Opened from the Edit button on the User Management page.

**Page Elements**

| Element | Description |
|---------|-------------|
| Breadcrumb | Dashboard → Users → Edit User. |
| Page Title | "Edit User" shown as heading. |
| User Fields | Editable fields for the user's profile: name, email, level, learning path, admin flag, and other profile data. |
| Import CSV Button | Allows bulk user import via CSV file. |
| Save Button | Saves the updated user record. |
| Cancel Link | Returns to the User Management page without saving. |

**Suggested Design**
```
--------------------------------------------------
| Admin Sidebar     | Edit User                  |
|                   | Dashboard > Users > Edit   |
|                   |                            |
|                   | First Name [_____________] |
|                   | Last Name  [_____________] |
|                   | Email      [_____________] |
|                   | Level      [Intermediate v]|
|                   | Learning Path [Python    v]|
|                   | Is Admin   [ ] Yes         |
|                   |                            |
|                   | [Import CSV]               |
|                   |                            |
|                   | [Cancel]      [Save User]  |
--------------------------------------------------
```

---

### 10.5 Admin Course Management Page

**Purpose**
Allows the admin to view all courses and navigate to create or edit course records.

**Page Elements**

| Element | Description |
|---------|-------------|
| Page Title | "Course Management" shown as heading. |
| Add Course Button | Opens the Create Course form (see 10.6). |
| Courses Table | Displays all courses with title, learning path, difficulty level, and lesson count. |
| Edit Button | Opens the Edit Course form (see 10.6). |
| Delete Button | Soft-deletes the course and hides it from users. |

**Suggested Design**
```
--------------------------------------------------
| Admin Sidebar     | Course Management  [+ Add] |
|                   |                            |
|                   | Title    | Path   | Difficulty | Actions |
|                   |----------|--------|------------|---------|
|                   | Py Basics| Python | Beginner   | Edit Del|
|                   | Py OOP   | Python | Interm.    | Edit Del|
|                   | Java Adv | Java   | Advanced   | Edit Del|
--------------------------------------------------
```

---

### 10.6 Admin Create / Edit Course Page

**Purpose**
Form for creating a new course or editing an existing one. Opened from the Add Course button or the Edit button on the Course Management page.

**Page Elements**

| Element | Description |
|---------|-------------|
| Breadcrumb | Dashboard → Courses → Create New / Edit. |
| Page Title | "Create New Course" or "Edit Course" shown as heading. |
| Course Title Field | Text field for the course name. |
| Description Field | Text area for the course description. |
| Difficulty Level | Selector for Beginner, Intermediate, or Advanced. |
| Learning Path | Selector for Python, Java, or Web Development. |
| Lessons Section | Inline lesson management: add, reorder, and remove lessons from within the course form. |
| Save Course Button | Submits the form and saves the course. |
| Cancel Link | Returns to the Course Management page without saving. |

**Suggested Design**
```
--------------------------------------------------
| Admin Sidebar     | Create New Course          |
|                   | Dashboard > Courses > New  |
|                   |                            |
|                   | Title [__________________] |
|                   |                            |
|                   | Description               |
|                   | [______________________]   |
|                   |                            |
|                   | Difficulty [Beginner     v]|
|                   | Path       [Python       v]|
|                   |                            |
|                   | Lessons                    |
|                   | [Lesson 1 Title ________] |
|                   | [+ Add Lesson]             |
|                   |                            |
|                   | [Cancel]   [Save Course]   |
--------------------------------------------------
```

---

### 10.7 Admin Job Offers Page

**Purpose**
Allows the admin to view, create, edit, and delete job offers posted on the platform.

**Page Elements**

| Element | Description |
|---------|-------------|
| Page Title | "Job Offers" shown as heading. |
| Add Job Offer Button | Opens the Create Job Offer form (see 10.8). |
| Job Offers Table | Displays all offers with title, company, deadline, and status. |
| Edit Button | Opens the Edit Job Offer form (see 10.8). |
| Delete Button | Soft-deletes the job offer and hides it from users. |

**Suggested Design**
```
--------------------------------------------------
| Admin Sidebar     | Job Offers         [+ Add] |
|                   |                            |
|                   | Title   | Company | Deadline | Actions |
|                   |---------|---------|----------|---------|
|                   | Full St.| Corp X  | Jan 30   | Edit Del|
|                   | Backend | Corp Y  | Feb 10   | Edit Del|
--------------------------------------------------
```

---

### 10.8 Admin Create / Edit Job Offer Page

**Purpose**
Form for creating a new job offer or editing an existing one. Opened from the Add Job Offer button or the Edit button on the Job Offers page.

**Page Elements**

| Element | Description |
|---------|-------------|
| Breadcrumb | Dashboard → Jobs → Create New / Edit. |
| Page Title | "Create New Job Offer" or "Edit Job Offer" shown as heading. |
| Core Details Section | Job title, company name, location, job type (Remote/On-site), and description. |
| Requirements Section | Required skills and qualifications text area. |
| Application Details | Application deadline date picker and application instructions. |
| Save Job Offer Button | Submits the form and saves the job offer. |
| Cancel Link | Returns to the Job Offers page without saving. |

**Suggested Design**
```
--------------------------------------------------
| Admin Sidebar     | Create New Job Offer       |
|                   | Dashboard > Jobs > New     |
|                   |                            |
|                   | Job Title [______________] |
|                   | Company   [______________] |
|                   | Location  [______________] |
|                   | Type      [Remote        v]|
|                   |                            |
|                   | Description               |
|                   | [______________________]   |
|                   |                            |
|                   | Required Skills           |
|                   | [______________________]   |
|                   |                            |
|                   | Deadline [Date Picker]     |
|                   |                            |
|                   | [Cancel] [Save Job Offer]  |
--------------------------------------------------
```

---

### 10.9 Admin Reports Page

**Purpose**
Provides the admin with platform-wide reporting and statistics.

**Report Types**

| Report | Description |
|--------|-------------|
| User Report | Number of users by learning path, level, and registration date. |
| Course Progress Report | Average progress percentage per course. |
| Quiz Performance Report | Pass rate and average score per quiz. |
| Job Application Report | Number of applications per job offer and status breakdown. |

**Suggested Design**
```
--------------------------------------------------
| Admin Sidebar     | Reports                    |
|                   |                            |
|                   | Report Type [v]            |
|                   | From Date [____]           |
|                   | To Date   [____]           |
|                   |                            |
|                   | [Generate Report]          |
|                   |                            |
|                   | +----------------------+   |
|                   | |  Data / Chart Area   |   |
|                   | +----------------------+   |
|                   |                            |
|                   | [Export PDF] [Export Excel]|
--------------------------------------------------
```

---

## 11. Role-Based Sidebar Menus

### Learner Sidebar (Intermediate / Advanced)

```
Dashboard
Learning Paths
CV
Jobs
  My Applications (visible when on Jobs page)
Projects
Quizzes
Settings
-----------------
Dark / Light Mode
Log Out
```

### Learner Sidebar (Beginner)

```
Dashboard
Learning Paths
CV   (Locked — not clickable)
Jobs (Locked — not clickable)
Projects
Quizzes
Settings
-----------------
Dark / Light Mode
Log Out
```

### Admin Sidebar

```
Dashboard
User Management
Courses
Jobs
Reports
-----------------
Back to Site
[Admin Name] — Admin
```

---

## 12. Color and UI Style

### Color Palette

| Element | Color |
|---------|-------|
| Primary Color | Purple — #b887e3 / #9b59f5 |
| Primary Dark | #9d6fd9 |
| Background Light | White / Light gray #f7f6f8 |
| Background Dark | Deep dark #19131f / #0f0f1a |
| Admin Primary | #9e47eb |
| Admin Background Light | #f7f6f8 |
| Admin Surface Dark | #2d2438 |
| Error / Failed | Red |
| Success / Passed | Green |
| Scrollbar | Glowing purple #9b59f5 with purple shadow |

### UI Style

The platform uses a clean, modern developer-focused interface:
- Dark mode default with light mode toggle in the sidebar
- Purple accent color used for buttons, badges, active states, and scrollbar
- Frosted glass effect on login and sign-up cards
- Rounded cards and form fields throughout
- Monaco editor for all coding exercise and focus mode pages
- Confetti animation on quiz pass and exercise completion
- Loading overlay with progress bar and motivational messages on login
- Status badges for quiz results (Passed / Failed), job applications, and lesson completion

---

## 13. UI Components

| Component | Used For |
|-----------|----------|
| Sidebar Navigation | All authenticated pages — learner and admin layouts |
| Top Navigation Bar | Learning Path pages (Python, Java, Web Development) |
| Progress Bar | Assessment questions, course lessons, quiz timer, login loading, skills mastery |
| Monaco Code Editor | Focus Mode lesson page and coding exercise page |
| Status Badge | Quiz results, job application status, lesson completion |
| Countdown Timer | Quiz taking page |
| Confetti Animation | Quiz passed result and correct exercise submission |
| AI Chatbot Popup | Focus Mode lesson page |
| Dark / Light Mode Toggle | Sidebar bottom section on all learner pages |
| File Upload | CV profile picture, CV document upload, profile picture via dashboard modal |
| Skill Match Percentage Badge | Job offers list and job detail panel |
| Skill Mastery Progress Bar | Dashboard Skills Progress section — one bar per skill |
| Job Recommendation Card | Dashboard right panel — filterable by type and location |
| Profile Picture Modal | Dashboard — opens on avatar click, allows photo upload |
| Lock Indicator | CV and Jobs sidebar items when user is Beginner level |
| Loading Overlay | Login page after form submission |
| OAuth Buttons | Login and Sign Up pages (Google and GitHub) |
| Data Table | Admin user management, course management, job offers, applied jobs |
| Cards | Dashboard summary, course list, quiz list, project portfolio |
| Breadcrumb Navigation | Admin CRUD pages (Create/Edit Course, Job Offer, User) |
