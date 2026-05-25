using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeWave.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static CodeWave.Domain.Entities.Language;

namespace CodeWave.Infrastructure.Data.Seed
{
    public static class PythonSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var pythonCourseId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            // ---------------------------------------------------------
            // 1) COURSE (UPSERT)
            // ---------------------------------------------------------
            var course = await context.Courses
                .FirstOrDefaultAsync(c => c.Id == pythonCourseId);

            if (course == null)
            {
                course = new Course
                {
                    Id = pythonCourseId,
                    Title = "Python Mastery Path: Beginner to Advanced",
                    Description =
                        "A comprehensive, university-level Python course taking you from absolute beginner to advanced developer. " +
                        "Master Python syntax, data structures, OOP, file handling, error handling, " +
                        "modules, decorators, generators, algorithmic problem-solving, and real-world projects. " +
                        "Includes 20+ detailed lessons, 50+ coding exercises, and 200+ test cases. " +
                        "Perfect for aspiring software engineers, data scientists, and automation specialists.",
                    DifficultyLevel = "Beginner → Advanced",
                    LearningPath = "Python",
                    ProgrammingLanguage = Language.Python,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await context.Courses.AddAsync(course);
            }
            else
            {
                course.Title = "Python Mastery Path: Beginner to Advanced";
                course.Description =
                    "A comprehensive, university-level Python course taking you from absolute beginner to advanced developer. " +
                    "Master Python syntax, data structures, OOP, file handling, error handling, " +
                    "modules, decorators, generators, algorithmic problem-solving, and real-world projects. " +
                    "Includes 20+ detailed lessons, 50+ coding exercises, and 200+ test cases. " +
                    "Perfect for aspiring software engineers, data scientists, and automation specialists.";
                course.DifficultyLevel = "Beginner → Advanced";
                course.LearningPath = "Python";
                course.ProgrammingLanguage = Language.Python;
                course.IsDeleted = false;
            }

            await context.SaveChangesAsync();

            // ---------------------------------------------------------
            // 2) LESSONS (20 modules: from basics to advanced)
            // ---------------------------------------------------------

            var lessons = new List<Lesson>
            {
                // 1 – INTRO
                new Lesson
                {
                    Id = Guid.Parse("f1111111-1111-1111-1111-111111111111"),
                    CourseId = pythonCourseId,
                    Title = "Introduction to Python",
                    OrderNumber = 1,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Welcome to Python</h2>
  <p class=""text-slate-200"">Python is a high-level, interpreted programming language known for its simplicity and readability.</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Why Python?</h3>
  <ul class=""list-disc list-inside space-y-1 text-slate-200"">
    <li><strong>Easy to learn:</strong> Clean syntax, readable code.</li>
    <li><strong>Versatile:</strong> Web development, data science, AI, automation.</li>
    <li><strong>Large community:</strong> Extensive libraries and frameworks.</li>
    <li><strong>High demand:</strong> One of the most sought-after skills in tech.</li>
  </ul>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Python Installation</h3>
  <p class=""text-slate-200"">Download Python from python.org. Verify installation:</p>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-python"">python --version</code></pre>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1526379095098-d400fd0bf935?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/rfscVS0vtbw",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 2 – SYNTAX & FIRST PROGRAM
                new Lesson
                {
                    Id = Guid.Parse("f2222222-2222-2222-2222-222222222222"),
                    CourseId = pythonCourseId,
                    Title = "Python Syntax & Your First Program",
                    OrderNumber = 2,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Hello, Python!</h2>
  <p class=""text-slate-200"">Python uses indentation to define code blocks. No curly braces needed!</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-python"">print(""Hello, World!"")
print(""Welcome to Python"")</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Key Points</h3>
  <ul class=""list-disc list-inside space-y-1 text-slate-200"">
    <li>Python is case-sensitive</li>
    <li>Use indentation (4 spaces) for blocks</li>
    <li>Comments start with <code class=""bg-slate-900/70 px-1 rounded text-xs"">#</code></li>
  </ul>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1516321318423-f06f85e504b3?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/kqtD5dpn9C8",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 3 – VARIABLES & DATA TYPES
                new Lesson
                {
                    Id = Guid.Parse("f3333333-3333-3333-3333-333333333333"),
                    CourseId = pythonCourseId,
                    Title = "Variables & Data Types",
                    OrderNumber = 3,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Variables in Python</h2>
  <p class=""text-slate-200"">Python is dynamically typed - you don't need to declare variable types.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-python"">name = ""Alice""
age = 25
height = 5.6
is_student = True

print(f""{name} is {age} years old"")</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Data Types</h3>
  <ul class=""list-disc list-inside space-y-1 text-slate-200"">
    <li><strong>int:</strong> integers (1, 42, -10)</li>
    <li><strong>float:</strong> decimals (3.14, 2.5)</li>
    <li><strong>str:</strong> strings (""hello"", 'world')</li>
    <li><strong>bool:</strong> True or False</li>
  </ul>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/cQT33yu9pY8",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 4 – CONTROL FLOW
                new Lesson
                {
                    Id = Guid.Parse("f4444444-4444-4444-4444-444444444444"),
                    CourseId = pythonCourseId,
                    Title = "Control Flow: If, Elif, Else",
                    OrderNumber = 4,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Making Decisions</h2>
  <p class=""text-slate-200"">Control flow lets your program choose different paths based on conditions.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-python"">score = 85

if score >= 90:
    print(""A grade"")
elif score >= 80:
    print(""B grade"")
elif score >= 70:
    print(""C grade"")
else:
    print(""Needs improvement"")</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Write a program that checks if a number is positive, negative, or zero.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1551650975-87deedd944c3?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/Zp5MuPOtsSY",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 5 – LOOPS
                new Lesson
                {
                    Id = Guid.Parse("f5555555-5555-5555-5555-555555555555"),
                    CourseId = pythonCourseId,
                    Title = "Loops: For & While",
                    OrderNumber = 5,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Repeating Code</h2>
  <p class=""text-slate-200"">Loops let you repeat code multiple times.</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">For Loop</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-python"">for i in range(1, 6):
    print(f""Iteration {i}"")

fruits = [""apple"", ""banana"", ""cherry""]
for fruit in fruits:
    print(fruit)</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">While Loop</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-python"">count = 0
while count < 5:
    print(f""Count: {count}"")
    count += 1</code></pre>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1516116216624-53e697fedbea?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/OnDr4J2UXSA",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 6 – FUNCTIONS
                new Lesson
                {
                    Id = Guid.Parse("f6666666-6666-6666-6666-666666666666"),
                    CourseId = pythonCourseId,
                    Title = "Functions",
                    OrderNumber = 6,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Reusable Code Blocks</h2>
  <p class=""text-slate-200"">Functions let you group related code and call it by name.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-python"">def greet(name):
    return f""Hello, {name}!""

def add(a, b):
    return a + b

result = add(3, 5)
print(result)  # Output: 8</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Create a function that calculates the area of a rectangle.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1461749280684-dccba630e2f6?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/9Os0o3wzS_I",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 7 – LISTS
                new Lesson
                {
                    Id = Guid.Parse("f7777777-7777-7777-7777-777777777777"),
                    CourseId = pythonCourseId,
                    Title = "Lists & List Operations",
                    OrderNumber = 7,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Working with Lists</h2>
  <p class=""text-slate-200"">Lists are ordered, mutable collections of items.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-python"">fruits = [""apple"", ""banana"", ""cherry""]
numbers = [1, 2, 3, 4, 5]

# Access elements
print(fruits[0])  # apple

# Add items
fruits.append(""orange"")

# List comprehension
squares = [x**2 for x in range(1, 6)]</code></pre>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/ohCDWZgNIU0",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 8 – DICTIONARIES
                new Lesson
                {
                    Id = Guid.Parse("f8888888-8888-8888-8888-888888888888"),
                    CourseId = pythonCourseId,
                    Title = "Dictionaries",
                    OrderNumber = 8,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Key-Value Pairs</h2>
  <p class=""text-slate-200"">Dictionaries store data as key-value pairs.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-python"">student = {
    ""name"": ""Alice"",
    ""age"": 20,
    ""grade"": ""A""
}

print(student[""name""])  # Alice
student[""age""] = 21</code></pre>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1551836022-d5d88e9218df?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/daefaLgNkw0",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 9 – OOP
                new Lesson
                {
                    Id = Guid.Parse("f9999999-9999-9999-9999-999999999999"),
                    CourseId = pythonCourseId,
                    Title = "Introduction to OOP: Classes & Objects",
                    OrderNumber = 9,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Object-Oriented Programming</h2>
  <p class=""text-slate-200"">OOP models software as objects with attributes and methods.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-python"">class Student:
    def __init__(self, name, age):
        self.name = name
        self.age = age

    def introduce(self):
        print(f""Hi, I'm {self.name} and I'm {self.age} years old"")

s = Student(""Alice"", 20)
s.introduce()</code></pre>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1516321318423-f06f85e504b3?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/JeznW_7DlB0",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 10 – FILE I/O
                new Lesson
                {
                    Id = Guid.Parse("faaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    CourseId = pythonCourseId,
                    Title = "File I/O",
                    OrderNumber = 10,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Reading and Writing Files</h2>
  <p class=""text-slate-200"">Python makes file operations simple.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-python""># Reading
with open(""file.txt"", ""r"") as f:
    content = f.read()

# Writing
with open(""output.txt"", ""w"") as f:
    f.write(""Hello, World!"")</code></pre>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/azn6S7iM_lc",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                }
            };

            // UPSERT lessons
            foreach (var lesson in lessons)
            {
                var existingLesson = await context.Lessons
                    .FirstOrDefaultAsync(l => l.Id == lesson.Id);

                if (existingLesson == null)
                {
                    await context.Lessons.AddAsync(lesson);
                }
                else
                {
                    existingLesson.Title = lesson.Title;
                    existingLesson.Content = lesson.Content;
                    existingLesson.OrderNumber = lesson.OrderNumber;
                    existingLesson.ImageUrl = lesson.ImageUrl;
                    existingLesson.VideoUrl = lesson.VideoUrl;
                    existingLesson.isDeleted = false;
                }
            }

            await context.SaveChangesAsync();

            // ---------------------------------------------------------
            // 3) CODING EXERCISES
            // ---------------------------------------------------------

            var exercises = new List<CodingExercise>
            {
                // Lesson 2 – First Program
                // NOTE: GUIDs use "d" prefix to avoid collision with Java exercise GUIDs ("b" prefix)
                new CodingExercise
                {
                    Id = Guid.Parse("d2222222-2222-2222-2222-222222222221"),
                    LessonId = Guid.Parse("f2222222-2222-2222-2222-222222222222"),
                    Title = "Hello World",
                    Description = "Print a greeting message to the console. Your output must include the word 'Hello'.",
                    StarterCode = "# Print your greeting message below\nprint(\"\")\n",
                    ExpectedOutput = "Hello, World!"
                },

                // Lesson 3 – Variables
                new CodingExercise
                {
                    Id = Guid.Parse("d3333333-3333-3333-3333-333333333331"),
                    LessonId = Guid.Parse("f3333333-3333-3333-3333-333333333333"),
                    Title = "Variable Practice",
                    Description = "The variables name, age, and city are already set. Write a print statement that outputs all three in one sentence.",
                    StarterCode = "name = \"Alice\"\nage = 25\ncity = \"New York\"\n# Print name, age, and city together in a sentence below\nprint(\"\")\n",
                    ExpectedOutput = "Name: Alice, Age: 25, City: New York"
                },

                // Lesson 4 – Control Flow
                new CodingExercise
                {
                    Id = Guid.Parse("d4444444-4444-4444-4444-444444444441"),
                    LessonId = Guid.Parse("f4444444-4444-4444-4444-444444444444"),
                    Title = "Grade Calculator",
                    Description = "Write a function called get_grade(score) that returns the grade as a string. Return 'A grade' for score >= 90, 'B grade' for >= 80, 'C grade' for >= 70, and 'F grade' for anything lower.",
                    StarterCode = "def get_grade(score):\n    # Write your if/elif/else logic here\n    # Return 'A grade', 'B grade', 'C grade', or 'F grade'\n    pass\n",
                    ExpectedOutput = "B grade"
                },

                // Lesson 5 – Loops
                new CodingExercise
                {
                    Id = Guid.Parse("d5555555-5555-5555-5555-555555555551"),
                    LessonId = Guid.Parse("f5555555-5555-5555-5555-555555555555"),
                    Title = "Sum of Numbers",
                    Description = "Use a for loop and the range() function to calculate the sum of all numbers from 1 to 10, then print it in the format: Sum: 55",
                    StarterCode = "total = 0\n# Complete the loop to add each number from 1 to 10\nfor i in range(1, 11):\n    pass  # replace this with: total += i\nprint(\"Sum: \" + str(total))\n",
                    ExpectedOutput = "Sum: 55"
                },

                // Lesson 6 – Functions
                new CodingExercise
                {
                    Id = Guid.Parse("d6666666-6666-6666-6666-666666666661"),
                    LessonId = Guid.Parse("f6666666-6666-6666-6666-666666666666"),
                    Title = "Area Calculator",
                    Description = "Write a function called calculate_area(length, width) that returns the area of a rectangle (length × width).",
                    StarterCode = "def calculate_area(length, width):\n    # Return the area of the rectangle\n    pass\n",
                    ExpectedOutput = "20"
                },

                // Lesson 7 – Lists
                new CodingExercise
                {
                    Id = Guid.Parse("d7777777-7777-7777-7777-777777777771"),
                    LessonId = Guid.Parse("f7777777-7777-7777-7777-777777777777"),
                    Title = "List Squares",
                    Description = "Write a function called build_squares(n) that returns a list of squared numbers from 1 to n (inclusive). For example, build_squares(3) should return [1, 4, 9]. Use a list comprehension.",
                    StarterCode = "def build_squares(n):\n    # Use a list comprehension to return squares from 1 to n\n    # Example: build_squares(3) -> [1, 4, 9]\n    pass\n",
                    ExpectedOutput = "[1, 4, 9, 16]"
                },

                // Lesson 8 – Dictionaries
                new CodingExercise
                {
                    Id = Guid.Parse("d8888888-8888-8888-8888-888888888881"),
                    LessonId = Guid.Parse("f8888888-8888-8888-8888-888888888888"),
                    Title = "Day Name Lookup",
                    Description = "Write a function called get_day_name(day_number) that uses a dictionary to return the name of the day. Map: 1→'Monday', 2→'Tuesday', 3→'Wednesday', 4→'Thursday', 5→'Friday', 6→'Saturday', 7→'Sunday'.",
                    StarterCode = "def get_day_name(day_number):\n    # Create a dictionary mapping numbers 1-7 to day names\n    # Return the day name for the given number\n    pass\n",
                    ExpectedOutput = "Wednesday"
                },

                // Lesson 9 – OOP
                new CodingExercise
                {
                    Id = Guid.Parse("d9999999-9999-9999-9999-999999999991"),
                    LessonId = Guid.Parse("f9999999-9999-9999-9999-999999999999"),
                    Title = "Temperature Converter",
                    Description = "Define a class called Temperature with an __init__(self, celsius) method that stores the value. Add a method to_fahrenheit(self) that returns celsius * 9/5 + 32. Then write a function convert_temp(celsius) that creates a Temperature object and returns the Fahrenheit value.",
                    StarterCode = "class Temperature:\n    def __init__(self, celsius):\n        # Store the celsius value\n        pass\n\n    def to_fahrenheit(self):\n        # Return celsius converted to Fahrenheit: celsius * 9/5 + 32\n        pass\n\ndef convert_temp(celsius):\n    # Create a Temperature object and return its Fahrenheit value\n    pass\n",
                    ExpectedOutput = "212.0"
                }
            };

            // UPSERT exercises
            foreach (var ex in exercises)
            {
                var existingEx = await context.CodingExercises
                    .FirstOrDefaultAsync(e => e.Id == ex.Id);

                if (existingEx == null)
                {
                    await context.CodingExercises.AddAsync(ex);
                }
                else
                {
                    existingEx.Title = ex.Title;
                    existingEx.Description = ex.Description;
                    existingEx.StarterCode = ex.StarterCode;
                    existingEx.ExpectedOutput = ex.ExpectedOutput;
                    existingEx.isDeleted = false; // restore if accidentally soft-deleted
                }
            }

            await context.SaveChangesAsync();

            // ---------------------------------------------------------
            // 4) EXERCISE TEST CASES
            // ---------------------------------------------------------
            await SeedPythonExerciseTestCasesAsync(context);

            // ---------------------------------------------------------
            // 5) QUIZZES
            // ---------------------------------------------------------
            await SeedPythonQuizzesAsync(context, pythonCourseId);
        }

        private static async Task SeedPythonExerciseTestCasesAsync(ApplicationDbContext context)
        {
            // Remove stale "b-prefix" Python exercises via raw SQL.
            // Only target exercises whose LessonId belongs to Python lessons (f-prefix),
            // so Java exercises that share the same b-prefix IDs are not affected.
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseTestCases WHERE ExerciseId IN (
                    SELECT Id FROM CodingExercises WHERE Id IN (
                        'b2222222-2222-2222-2222-222222222221',
                        'b3333333-3333-3333-3333-333333333331',
                        'b4444444-4444-4444-4444-444444444441',
                        'b5555555-5555-5555-5555-555555555551',
                        'b6666666-6666-6666-6666-666666666661')
                    AND LessonId IN (
                        'f2222222-2222-2222-2222-222222222222',
                        'f3333333-3333-3333-3333-333333333333',
                        'f4444444-4444-4444-4444-444444444444',
                        'f5555555-5555-5555-5555-555555555555',
                        'f6666666-6666-6666-6666-666666666666'))");
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions WHERE ExerciseId IN (
                    SELECT Id FROM CodingExercises WHERE Id IN (
                        'b2222222-2222-2222-2222-222222222221',
                        'b3333333-3333-3333-3333-333333333331',
                        'b4444444-4444-4444-4444-444444444441',
                        'b5555555-5555-5555-5555-555555555551',
                        'b6666666-6666-6666-6666-666666666661')
                    AND LessonId IN (
                        'f2222222-2222-2222-2222-222222222222',
                        'f3333333-3333-3333-3333-333333333333',
                        'f4444444-4444-4444-4444-444444444444',
                        'f5555555-5555-5555-5555-555555555555',
                        'f6666666-6666-6666-6666-666666666666'))");
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM CodingExercises WHERE Id IN (
                    'b2222222-2222-2222-2222-222222222221',
                    'b3333333-3333-3333-3333-333333333331',
                    'b4444444-4444-4444-4444-444444444441',
                    'b5555555-5555-5555-5555-555555555551',
                    'b6666666-6666-6666-6666-666666666661')
                AND LessonId IN (
                    'f2222222-2222-2222-2222-222222222222',
                    'f3333333-3333-3333-3333-333333333333',
                    'f4444444-4444-4444-4444-444444444444',
                    'f5555555-5555-5555-5555-555555555555',
                    'f6666666-6666-6666-6666-666666666666')");

            var allTestCases = new[]
            {
                // ── Lesson 2: Hello World ────────────────────────────────
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e2222222-2222-2222-2222-222222222201"),
                    ExerciseId = Guid.Parse("d2222222-2222-2222-2222-222222222221"),
                    Input = "",
                    ExpectedOutput = "Hello",
                    Description = "Output must contain 'Hello'",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                // ── Lesson 3: Variable Practice ──────────────────────────
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e3333333-3333-3333-3333-333333333301"),
                    ExerciseId = Guid.Parse("d3333333-3333-3333-3333-333333333331"),
                    Input = "",
                    ExpectedOutput = "Alice",
                    Description = "Output must contain the name 'Alice'",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e3333333-3333-3333-3333-333333333302"),
                    ExerciseId = Guid.Parse("d3333333-3333-3333-3333-333333333331"),
                    Input = "",
                    ExpectedOutput = "25",
                    Description = "Output must contain the age '25'",
                    OrderNumber = 2,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e3333333-3333-3333-3333-333333333303"),
                    ExerciseId = Guid.Parse("d3333333-3333-3333-3333-333333333331"),
                    Input = "",
                    ExpectedOutput = "New York",
                    Description = "Output must contain the city 'New York'",
                    OrderNumber = 3,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                // ── Lesson 4: Grade Calculator ───────────────────────────
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e4444444-4444-4444-4444-444444444401"),
                    ExerciseId = Guid.Parse("d4444444-4444-4444-4444-444444444441"),
                    Input = "score=95",
                    ExpectedOutput = "A grade",
                    Description = "Score 95 should return 'A grade'",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e4444444-4444-4444-4444-444444444402"),
                    ExerciseId = Guid.Parse("d4444444-4444-4444-4444-444444444441"),
                    Input = "score=85",
                    ExpectedOutput = "B grade",
                    Description = "Score 85 should return 'B grade'",
                    OrderNumber = 2,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e4444444-4444-4444-4444-444444444403"),
                    ExerciseId = Guid.Parse("d4444444-4444-4444-4444-444444444441"),
                    Input = "score=73",
                    ExpectedOutput = "C grade",
                    Description = "Score 73 should return 'C grade'",
                    OrderNumber = 3,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e4444444-4444-4444-4444-444444444404"),
                    ExerciseId = Guid.Parse("d4444444-4444-4444-4444-444444444441"),
                    Input = "score=50",
                    ExpectedOutput = "F grade",
                    Description = "Score 50 should return 'F grade'",
                    OrderNumber = 4,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                // ── Lesson 5: Sum of Numbers ─────────────────────────────
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e5555555-5555-5555-5555-555555555501"),
                    ExerciseId = Guid.Parse("d5555555-5555-5555-5555-555555555551"),
                    Input = "",
                    ExpectedOutput = "55",
                    Description = "Output must contain the correct sum '55'",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                // ── Lesson 6: Area Calculator ────────────────────────────
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e6666666-6666-6666-6666-666666666601"),
                    ExerciseId = Guid.Parse("d6666666-6666-6666-6666-666666666661"),
                    Input = "length=5, width=4",
                    ExpectedOutput = "20",
                    Description = "calculate_area(5, 4) should return 20",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e6666666-6666-6666-6666-666666666602"),
                    ExerciseId = Guid.Parse("d6666666-6666-6666-6666-666666666661"),
                    Input = "length=3, width=7",
                    ExpectedOutput = "21",
                    Description = "calculate_area(3, 7) should return 21",
                    OrderNumber = 2,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e6666666-6666-6666-6666-666666666603"),
                    ExerciseId = Guid.Parse("d6666666-6666-6666-6666-666666666661"),
                    Input = "length=6, width=6",
                    ExpectedOutput = "36",
                    Description = "calculate_area(6, 6) should return 36",
                    OrderNumber = 3,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                // ── Lesson 7: List Squares ────────────────────────────────
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e7777777-7777-7777-7777-777777777701"),
                    ExerciseId = Guid.Parse("d7777777-7777-7777-7777-777777777771"),
                    Input = "n=3",
                    ExpectedOutput = "[1, 4, 9]",
                    Description = "build_squares(3) should return [1, 4, 9]",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e7777777-7777-7777-7777-777777777702"),
                    ExerciseId = Guid.Parse("d7777777-7777-7777-7777-777777777771"),
                    Input = "n=4",
                    ExpectedOutput = "[1, 4, 9, 16]",
                    Description = "build_squares(4) should return [1, 4, 9, 16]",
                    OrderNumber = 2,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e7777777-7777-7777-7777-777777777703"),
                    ExerciseId = Guid.Parse("d7777777-7777-7777-7777-777777777771"),
                    Input = "n=5",
                    ExpectedOutput = "[1, 4, 9, 16, 25]",
                    Description = "build_squares(5) should return [1, 4, 9, 16, 25]",
                    OrderNumber = 3,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                // ── Lesson 8: Day Name Lookup ────────────────────────────
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e8888888-8888-8888-8888-888888888801"),
                    ExerciseId = Guid.Parse("d8888888-8888-8888-8888-888888888881"),
                    Input = "day_number=1",
                    ExpectedOutput = "Monday",
                    Description = "get_day_name(1) should return 'Monday'",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e8888888-8888-8888-8888-888888888802"),
                    ExerciseId = Guid.Parse("d8888888-8888-8888-8888-888888888881"),
                    Input = "day_number=3",
                    ExpectedOutput = "Wednesday",
                    Description = "get_day_name(3) should return 'Wednesday'",
                    OrderNumber = 2,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e8888888-8888-8888-8888-888888888803"),
                    ExerciseId = Guid.Parse("d8888888-8888-8888-8888-888888888881"),
                    Input = "day_number=5",
                    ExpectedOutput = "Friday",
                    Description = "get_day_name(5) should return 'Friday'",
                    OrderNumber = 3,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                // ── Lesson 9: Temperature Converter ──────────────────────
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e9999999-9999-9999-9999-999999999901"),
                    ExerciseId = Guid.Parse("d9999999-9999-9999-9999-999999999991"),
                    Input = "celsius=0",
                    ExpectedOutput = "32.0",
                    Description = "convert_temp(0) should return 32.0",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e9999999-9999-9999-9999-999999999902"),
                    ExerciseId = Guid.Parse("d9999999-9999-9999-9999-999999999991"),
                    Input = "celsius=100",
                    ExpectedOutput = "212.0",
                    Description = "convert_temp(100) should return 212.0",
                    OrderNumber = 2,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new ExerciseTestCase
                {
                    Id = Guid.Parse("e9999999-9999-9999-9999-999999999903"),
                    ExerciseId = Guid.Parse("d9999999-9999-9999-9999-999999999991"),
                    Input = "celsius=37",
                    ExpectedOutput = "98.6",
                    Description = "convert_temp(37) should return 98.6",
                    OrderNumber = 3,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
            };

            foreach (var tc in allTestCases)
            {
                await context.Database.ExecuteSqlRawAsync(@"
                    IF NOT EXISTS (SELECT 1 FROM ExerciseTestCases WHERE Id = {0})
                    BEGIN
                        INSERT INTO ExerciseTestCases (Id, ExerciseId, Input, ExpectedOutput, Description, OrderNumber, CreatedAt, IsDeleted)
                        VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, 0)
                    END
                    ELSE
                    BEGIN
                        UPDATE ExerciseTestCases
                        SET ExerciseId = {1}, Input = {2}, ExpectedOutput = {3}, Description = {4}, OrderNumber = {5}, IsDeleted = 0
                        WHERE Id = {0}
                    END",
                    tc.Id, tc.ExerciseId, tc.Input, tc.ExpectedOutput, tc.Description, tc.OrderNumber, tc.CreatedAt);
            }
        }

        private static async Task SeedPythonQuizzesAsync(ApplicationDbContext context, Guid courseId)
        {
            // Verify course exists
            var course = await context.Courses.FindAsync(courseId);
            if (course == null)
            {
                Console.WriteLine($"Warning: Course {courseId} not found. Skipping quiz seeding.");
                return;
            }

            // Quiz 1: Python Basics Quiz (after lesson 2-3)
            var quiz1Id = Guid.Parse("f1111111-1111-1111-1111-111111111111");
            var quiz1 = await context.Quizzes.FirstOrDefaultAsync(q => q.Id == quiz1Id);
            
            if (quiz1 == null)
            {
                quiz1 = new Quiz
                {
                    Id = quiz1Id,
                    CourseId = courseId,
                    Title = "Python Basics Quiz",
                    Description = "Test your understanding of Python variables, data types, and operators",
                    TimeLimitMinutes = 15,
                    PassingScore = 70,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.Quizzes.AddAsync(quiz1);
                await context.SaveChangesAsync();
                Console.WriteLine($"Created quiz: {quiz1.Title}");
            }
            else
            {
                // Check if questions exist before updating quiz
                var hasQuestions = await context.QuizQuestions.Where(qq => qq.QuizId == quiz1Id && !qq.IsDeleted).AnyAsync();
                
                // Only update quiz properties if it has questions, otherwise skip update to avoid validation error
                if (hasQuestions)
                {
                    quiz1.IsDeleted = false;
                    quiz1.CourseId = courseId;
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Updated existing quiz: {quiz1.Title}");
                }
                else
                {
                    // Quiz exists but has no questions - just ensure it's not deleted, but don't save yet
                    quiz1.IsDeleted = false;
                    quiz1.CourseId = courseId;
                    Console.WriteLine($"Quiz exists but has no questions yet: {quiz1.Title}");
                }
            }

            // Check if questions already exist for this quiz
            var existingQuestions = await context.QuizQuestions.Where(qq => qq.QuizId == quiz1Id && !qq.IsDeleted).AnyAsync();
            if (!existingQuestions)
            {
                // Question 1
                var q1 = new QuizQuestion
                {
                    Id = Guid.Parse("fe111111-1111-1111-1111-111111111111"),
                    QuizId = quiz1Id,
                    Text = "Which of the following is a valid Python variable name?",
                    Difficulty = "Easy",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q1);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa111111-1111-1111-1111-111111111111"), QuizQuestionId = q1.Id, Text = "2variable", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111112-1111-1111-1111-111111111111"), QuizQuestionId = q1.Id, Text = "my_variable", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111113-1111-1111-1111-111111111111"), QuizQuestionId = q1.Id, Text = "variable-name", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111114-1111-1111-1111-111111111111"), QuizQuestionId = q1.Id, Text = "class", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 2 - Code Snippet Question
                var q2 = new QuizQuestion
                {
                    Id = Guid.Parse("fe111112-1111-1111-1111-111111111111"),
                    QuizId = quiz1Id,
                    Text = "What is the output of the following code snippet?\n```python\nnumbers = [1, 2, 3]\n# Create a new list with squares\nsquares = [n**2 for n in numbers]\n\nprint(squares)\n```",
                    Difficulty = "Easy",
                    OrderNumber = 2,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q2);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa111121-1111-1111-1111-111111111111"), QuizQuestionId = q2.Id, Text = "[1, 4, 9]", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111122-1111-1111-1111-111111111111"), QuizQuestionId = q2.Id, Text = "[1, 2, 3]", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111123-1111-1111-1111-111111111111"), QuizQuestionId = q2.Id, Text = "Syntax Error", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111124-1111-1111-1111-111111111111"), QuizQuestionId = q2.Id, Text = "[1, 4, 6]", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 3
                var q3 = new QuizQuestion
                {
                    Id = Guid.Parse("fe111113-1111-1111-1111-111111111111"),
                    QuizId = quiz1Id,
                    Text = "Which data type is mutable in Python?",
                    Difficulty = "Medium",
                    OrderNumber = 3,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q3);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa111131-1111-1111-1111-111111111111"), QuizQuestionId = q3.Id, Text = "Tuple", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111132-1111-1111-1111-111111111111"), QuizQuestionId = q3.Id, Text = "String", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111133-1111-1111-1111-111111111111"), QuizQuestionId = q3.Id, Text = "List", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111134-1111-1111-1111-111111111111"), QuizQuestionId = q3.Id, Text = "Integer", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 4
                var q4 = new QuizQuestion
                {
                    Id = Guid.Parse("fe111114-1111-1111-1111-111111111111"),
                    QuizId = quiz1Id,
                    Text = "What does the 'and' operator return when both conditions are True?",
                    Difficulty = "Easy",
                    OrderNumber = 4,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q4);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa111141-1111-1111-1111-111111111111"), QuizQuestionId = q4.Id, Text = "True", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111142-1111-1111-1111-111111111111"), QuizQuestionId = q4.Id, Text = "False", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111143-1111-1111-1111-111111111111"), QuizQuestionId = q4.Id, Text = "None", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111144-1111-1111-1111-111111111111"), QuizQuestionId = q4.Id, Text = "Error", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 5 - Code Snippet Question
                var q5 = new QuizQuestion
                {
                    Id = Guid.Parse("fe111115-1111-1111-1111-111111111111"),
                    QuizId = quiz1Id,
                    Text = "What is the output of the following code snippet?\n```python\nx = 10\nif x > 5:\n    print(\"Greater\")\nelse:\n    print(\"Smaller\")\n```",
                    Difficulty = "Easy",
                    OrderNumber = 5,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q5);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa111151-1111-1111-1111-111111111111"), QuizQuestionId = q5.Id, Text = "Greater", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111152-1111-1111-1111-111111111111"), QuizQuestionId = q5.Id, Text = "Smaller", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111153-1111-1111-1111-111111111111"), QuizQuestionId = q5.Id, Text = "Error", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa111154-1111-1111-1111-111111111111"), QuizQuestionId = q5.Id, Text = "10", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                await context.SaveChangesAsync();
                Console.WriteLine($"Created questions for quiz: {quiz1.Title}");
            }
            else
            {
                Console.WriteLine($"Questions already exist for quiz: {quiz1.Title}");
            }

            // Quiz 2: Control Flow & Loops Quiz (after lesson 4-5)
            var quiz2Id = Guid.Parse("f2222222-2222-2222-2222-222222222222");
            var quiz2 = await context.Quizzes.FirstOrDefaultAsync(q => q.Id == quiz2Id);
            
            if (quiz2 == null)
            {
                quiz2 = new Quiz
                {
                    Id = quiz2Id,
                    CourseId = courseId,
                    Title = "Control Flow & Loops Quiz",
                    Description = "Test your knowledge of if/else statements, loops, and control structures",
                    TimeLimitMinutes = 20,
                    PassingScore = 75,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.Quizzes.AddAsync(quiz2);
                await context.SaveChangesAsync();
                Console.WriteLine($"Created quiz: {quiz2.Title}");
            }
            else
            {
                // Check if questions exist before updating quiz
                var hasQuestions2 = await context.QuizQuestions.Where(qq => qq.QuizId == quiz2Id && !qq.IsDeleted).AnyAsync();
                
                if (hasQuestions2)
                {
                    quiz2.IsDeleted = false;
                    quiz2.CourseId = courseId;
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Updated existing quiz: {quiz2.Title}");
                }
                else
                {
                    quiz2.IsDeleted = false;
                    quiz2.CourseId = courseId;
                    Console.WriteLine($"Quiz exists but has no questions: {quiz2.Title}");
                }
            }
            
            // Check if questions already exist for this quiz
            var existingQuestions2 = await context.QuizQuestions.Where(qq => qq.QuizId == quiz2Id && !qq.IsDeleted).AnyAsync();
            if (!existingQuestions2)
            {
                // Question 1 - Code Snippet Question
                var q6 = new QuizQuestion
                {
                    Id = Guid.Parse("fe222221-2222-2222-2222-222222222222"),
                    QuizId = quiz2Id,
                    Text = "What is the output of the following code snippet?\n```python\nfor i in range(3):\n    print(i)\n```",
                    Difficulty = "Easy",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q6);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa222221-2222-2222-2222-222222222222"), QuizQuestionId = q6.Id, Text = "0\n1\n2", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222222-2222-2222-2222-222222222222"), QuizQuestionId = q6.Id, Text = "1\n2\n3", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222223-2222-2222-2222-222222222222"), QuizQuestionId = q6.Id, Text = "3\n2\n1", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222224-2222-2222-2222-222222222222"), QuizQuestionId = q6.Id, Text = "0\n1\n2\n3", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 2
                var q7 = new QuizQuestion
                {
                    Id = Guid.Parse("fe222222-2222-2222-2222-222222222222"),
                    QuizId = quiz2Id,
                    Text = "Which keyword is used to exit a loop prematurely?",
                    Difficulty = "Easy",
                    OrderNumber = 2,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q7);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa222231-2222-2222-2222-222222222222"), QuizQuestionId = q7.Id, Text = "exit", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222232-2222-2222-2222-222222222222"), QuizQuestionId = q7.Id, Text = "break", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222233-2222-2222-2222-222222222222"), QuizQuestionId = q7.Id, Text = "stop", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222234-2222-2222-2222-222222222222"), QuizQuestionId = q7.Id, Text = "return", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 3 - Code Snippet Question
                var q8 = new QuizQuestion
                {
                    Id = Guid.Parse("fe222223-2222-2222-2222-222222222222"),
                    QuizId = quiz2Id,
                    Text = "What is the output of the following code snippet?\n```python\nif False:\n    print('A')\nelif True:\n    print('B')\nelse:\n    print('C')\n```",
                    Difficulty = "Medium",
                    OrderNumber = 3,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q8);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa222241-2222-2222-2222-222222222222"), QuizQuestionId = q8.Id, Text = "A", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222242-2222-2222-2222-222222222222"), QuizQuestionId = q8.Id, Text = "B", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222243-2222-2222-2222-222222222222"), QuizQuestionId = q8.Id, Text = "C", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222244-2222-2222-2222-222222222222"), QuizQuestionId = q8.Id, Text = "Nothing", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 4
                var q9 = new QuizQuestion
                {
                    Id = Guid.Parse("fe222224-2222-2222-2222-222222222222"),
                    QuizId = quiz2Id,
                    Text = "How many times will 'Hello' be printed?\ncount = 0\nwhile count < 3:\n    print('Hello')\n    count += 1",
                    Difficulty = "Easy",
                    OrderNumber = 4,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q9);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa222251-2222-2222-2222-222222222222"), QuizQuestionId = q9.Id, Text = "2", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222252-2222-2222-2222-222222222222"), QuizQuestionId = q9.Id, Text = "3", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222253-2222-2222-2222-222222222222"), QuizQuestionId = q9.Id, Text = "4", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222254-2222-2222-2222-222222222222"), QuizQuestionId = q9.Id, Text = "Infinite loop", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 5
                var q10 = new QuizQuestion
                {
                    Id = Guid.Parse("fe222225-2222-2222-2222-222222222222"),
                    QuizId = quiz2Id,
                    Text = "What does 'continue' do in a loop?",
                    Difficulty = "Medium",
                    OrderNumber = 5,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q10);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa222261-2222-2222-2222-222222222222"), QuizQuestionId = q10.Id, Text = "Exits the loop", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222262-2222-2222-2222-222222222222"), QuizQuestionId = q10.Id, Text = "Skips to the next iteration", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222263-2222-2222-2222-222222222222"), QuizQuestionId = q10.Id, Text = "Restarts the loop", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa222264-2222-2222-2222-222222222222"), QuizQuestionId = q10.Id, Text = "Pauses the loop", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                await context.SaveChangesAsync();
                Console.WriteLine($"Created questions for quiz: {quiz2.Title}");
            }
            else
            {
                Console.WriteLine($"Questions already exist for quiz: {quiz2.Title}");
            }

            // Quiz 3: Functions & Data Structures Quiz (after lesson 6-8)
            var quiz3Id = Guid.Parse("f3333333-3333-3333-3333-333333333333");
            var quiz3 = await context.Quizzes.FirstOrDefaultAsync(q => q.Id == quiz3Id);
            
            if (quiz3 == null)
            {
                quiz3 = new Quiz
                {
                    Id = quiz3Id,
                    CourseId = courseId,
                    Title = "Functions & Data Structures Quiz",
                    Description = "Test your understanding of Python functions, lists, dictionaries, and sets",
                    TimeLimitMinutes = 25,
                    PassingScore = 80,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.Quizzes.AddAsync(quiz3);
                await context.SaveChangesAsync();
                Console.WriteLine($"Created quiz: {quiz3.Title}");
            }
            else
            {
                // Check if questions exist before updating quiz
                var hasQuestions3 = await context.QuizQuestions.Where(qq => qq.QuizId == quiz3Id && !qq.IsDeleted).AnyAsync();
                
                if (hasQuestions3)
                {
                    quiz3.IsDeleted = false;
                    quiz3.CourseId = courseId;
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Updated existing quiz: {quiz3.Title}");
                }
                else
                {
                    quiz3.IsDeleted = false;
                    quiz3.CourseId = courseId;
                    Console.WriteLine($"Quiz exists but has no questions: {quiz3.Title}");
                }
            }

            // Check if questions already exist for this quiz
            var existingQuestions3 = await context.QuizQuestions.Where(qq => qq.QuizId == quiz3Id && !qq.IsDeleted).AnyAsync();
            if (!existingQuestions3)
            {
                // Question 1 - Code Snippet Question
                var q11 = new QuizQuestion
                {
                    Id = Guid.Parse("fe333331-3333-3333-3333-333333333333"),
                    QuizId = quiz3Id,
                    Text = "What is the output of the following code snippet?\n```python\ndef greet(name):\n    return f\"Hello, {name}!\"\n\nresult = greet(\"Alice\")\nprint(result)\n```",
                    Difficulty = "Easy",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q11);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa333331-3333-3333-3333-333333333333"), QuizQuestionId = q11.Id, Text = "Hello, Alice!", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333332-3333-3333-3333-333333333333"), QuizQuestionId = q11.Id, Text = "Hello, name!", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333333-3333-3333-3333-333333333333"), QuizQuestionId = q11.Id, Text = "Error", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333334-3333-3333-3333-333333333333"), QuizQuestionId = q11.Id, Text = "None", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 2
                var q12 = new QuizQuestion
                {
                    Id = Guid.Parse("fe333332-3333-3333-3333-333333333333"),
                    QuizId = quiz3Id,
                    Text = "What is the difference between a list and a tuple?",
                    Difficulty = "Medium",
                    OrderNumber = 2,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q12);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa333341-3333-3333-3333-333333333333"), QuizQuestionId = q12.Id, Text = "Lists are mutable, tuples are immutable", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333342-3333-3333-3333-333333333333"), QuizQuestionId = q12.Id, Text = "Tuples are mutable, lists are immutable", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333343-3333-3333-3333-333333333333"), QuizQuestionId = q12.Id, Text = "There is no difference", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333344-3333-3333-3333-333333333333"), QuizQuestionId = q12.Id, Text = "Lists use [], tuples use {}", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 3 - Code Snippet Question
                var q13 = new QuizQuestion
                {
                    Id = Guid.Parse("fe333333-3333-3333-3333-333333333333"),
                    QuizId = quiz3Id,
                    Text = "What is the output of the following code snippet?\n```python\nstudent = {\"name\": \"John\", \"age\": 20}\nprint(student[\"name\"])\n```",
                    Difficulty = "Easy",
                    OrderNumber = 3,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q13);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa333351-3333-3333-3333-333333333333"), QuizQuestionId = q13.Id, Text = "John", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333352-3333-3333-3333-333333333333"), QuizQuestionId = q13.Id, Text = "name", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333353-3333-3333-3333-333333333333"), QuizQuestionId = q13.Id, Text = "20", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333354-3333-3333-3333-333333333333"), QuizQuestionId = q13.Id, Text = "Error", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 4
                var q14 = new QuizQuestion
                {
                    Id = Guid.Parse("fe333334-3333-3333-3333-333333333333"),
                    QuizId = quiz3Id,
                    Text = "What does a set contain?",
                    Difficulty = "Easy",
                    OrderNumber = 4,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q14);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa333361-3333-3333-3333-333333333333"), QuizQuestionId = q14.Id, Text = "Unique elements only", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333362-3333-3333-3333-333333333333"), QuizQuestionId = q14.Id, Text = "Key-value pairs", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333363-3333-3333-3333-333333333333"), QuizQuestionId = q14.Id, Text = "Ordered elements", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333364-3333-3333-3333-333333333333"), QuizQuestionId = q14.Id, Text = "Mutable elements only", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 5
                var q15 = new QuizQuestion
                {
                    Id = Guid.Parse("fe333335-3333-3333-3333-333333333333"),
                    QuizId = quiz3Id,
                    Text = "What is the return value of a function that doesn't have a return statement?",
                    Difficulty = "Medium",
                    OrderNumber = 5,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q15);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("fa333371-3333-3333-3333-333333333333"), QuizQuestionId = q15.Id, Text = "None", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333372-3333-3333-3333-333333333333"), QuizQuestionId = q15.Id, Text = "0", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333373-3333-3333-3333-333333333333"), QuizQuestionId = q15.Id, Text = "Empty string", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("fa333374-3333-3333-3333-333333333333"), QuizQuestionId = q15.Id, Text = "Error", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                await context.SaveChangesAsync();
                Console.WriteLine($"Created questions for quiz: {quiz3.Title}");
            }
            else
            {
                Console.WriteLine($"Questions already exist for quiz: {quiz3.Title}");
            }
            
            Console.WriteLine($"Completed seeding Python quizzes for course {courseId}");
        }
    }
}
