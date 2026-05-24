using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Data.Seed
{
    public static class JavaSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var javaCourseId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            // ---------------------------------------------------------
            // 1) COURSE (UPSERT)
            // ---------------------------------------------------------
            var course = await context.Courses
                .FirstOrDefaultAsync(c => c.Id == javaCourseId);

            if (course == null)
            {
                course = new Course
                {
                    Id = javaCourseId,
                    Title = "Java Mastery Path: Beginner to Advanced",
                    Description =
                        "A comprehensive, university-level Java course taking you from absolute beginner to advanced developer. " +
                        "Master Java syntax, OOP principles, collections, generics, exception handling, file I/O, " +
                        "multithreading, design patterns, and algorithmic problem-solving. " +
                        "Includes 20+ detailed lessons, 50+ coding exercises, and 200+ test cases. " +
                        "Perfect for aspiring software engineers seeking a deep understanding of Java programming.",
                    DifficultyLevel = "Beginner → Advanced",
                    LearningPath = "Java Development Path",
                    ProgrammingLanguage = Language.Java,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await context.Courses.AddAsync(course);
            }
            else
            {
                course.Title = "Java Mastery Path: Beginner to Advanced";
                course.Description =
                    "A comprehensive, university-level Java course taking you from absolute beginner to advanced developer. " +
                    "Master Java syntax, OOP principles, collections, generics, exception handling, file I/O, " +
                    "multithreading, design patterns, and algorithmic problem-solving. " +
                    "Includes 20+ detailed lessons, 50+ coding exercises, and 200+ test cases. " +
                    "Perfect for aspiring software engineers seeking a deep understanding of Java programming.";
                course.DifficultyLevel = "Beginner → Advanced";
                course.LearningPath = "Java Development Path";
                course.ProgrammingLanguage = Language.Java;
                course.IsDeleted = false;
            }

            await context.SaveChangesAsync();

            // ---------------------------------------------------------
            // 2) LESSONS (20 modules: from basics to multithreading)
            // ---------------------------------------------------------

            var lessons = new List<Lesson>
            {
                // 1 – INTRO
                new Lesson
                {
                    Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                    CourseId = javaCourseId,
                    Title = "Introduction to Java & the JVM World",
                    OrderNumber = 1,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Welcome to Java</h2>
  <p class=""text-slate-200"">Java is a popular, high-level, object-oriented programming language originally developed by James Gosling at Sun Microsystems (now Oracle).</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Why Java Is Still Relevant</h3>
  <ul class=""list-disc list-inside space-y-1 text-slate-200"">
    <li><strong>Enterprise standard:</strong> millions of backend services and APIs use Java.</li>
    <li><strong>Android development:</strong> especially older and legacy apps.</li>
    <li><strong>Big data &amp; cloud:</strong> tools like Hadoop, Spark and Kafka are built with Java/Scala.</li>
    <li><strong>Strong ecosystem:</strong> libraries, frameworks and tooling (IDE support, build tools, etc.).</li>
  </ul>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Java Platform Layers</h3>
  <ol class=""list-decimal list-inside space-y-1 text-slate-200"">
    <li><strong>Java Source Code:</strong> <code class=""bg-slate-900/70 px-1 rounded text-xs"">.java</code> files you write in an editor or IDE.</li>
    <li><strong>Java Compiler (javac):</strong> converts source into <em>bytecode</em> (<code class=""bg-slate-900/70 px-1 rounded text-xs"">.class</code> files).</li>
    <li><strong>Java Virtual Machine (JVM):</strong> executes bytecode on any supported OS.</li>
    <li><strong>Java Runtime Environment (JRE):</strong> JVM + standard libraries.</li>
  </ol>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Key Principle: Write Once, Run Anywhere</h3>
  <p class=""text-slate-200"">The same compiled bytecode can run on Windows, Linux, or macOS as long as a compatible JVM is installed.</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Mini Challenge</h3>
  <p class=""text-slate-200"">In your head (or notes), list 3 applications you use that are likely written in Java or run on the JVM (hint: developer tools, servers, IDEs, Android apps...).</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1516116216624-53e697fedbea?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/mAtkPQO1FcA",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 2 – BASIC SYNTAX
                new Lesson
                {
                    Id = Guid.Parse("a2222222-2222-2222-2222-222222222222"),
                    CourseId = javaCourseId,
                    Title = "Your First Java Program & Basic Syntax",
                    OrderNumber = 2,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Your First Java Program</h2>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">public class Main {
    public static void main(String[] args) {
        System.out.println(""Hello, CodeWave!"");
    }
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Code Breakdown</h3>
  <ol class=""list-decimal list-inside space-y-1 text-slate-200"">
    <li><code class=""bg-slate-900/70 px-1 rounded text-xs"">public class Main</code>: declares a class named <code class=""bg-slate-900/70 px-1 rounded text-xs"">Main</code>. File name must match.</li>
    <li><code class=""bg-slate-900/70 px-1 rounded text-xs"">public static void main(String[] args)</code>: application entry point.</li>
    <li><code class=""bg-slate-900/70 px-1 rounded text-xs"">System.out.println(...)</code>: prints text followed by a newline.</li>
  </ol>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Compiling and Running</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code>javac Main.java     // Compile -&gt; creates Main.class
java Main           // Run via JVM
</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Common Mistakes</h3>
  <ul class=""list-disc list-inside space-y-1 text-slate-200"">
    <li>Forgetting the semicolon <code class=""bg-slate-900/70 px-1 rounded text-xs"">;</code> at the end of statements.</li>
    <li>Mismatched curly braces <code class=""bg-slate-900/70 px-1 rounded text-xs"">{ }</code>.</li>
    <li>File name and public class name not matching.</li>
  </ul>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Try It</h3>
  <p class=""text-slate-200"">Change the message in <code class=""bg-slate-900/70 px-1 rounded text-xs"">println</code> to display your name and your goal in learning Java.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/grEKMHGYyns",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 3 – VARIABLES & TYPES
                new Lesson
                {
                    Id = Guid.Parse("a3333333-3333-3333-3333-333333333333"),
                    CourseId = javaCourseId,
                    Title = "Variables, Data Types & Expressions",
                    OrderNumber = 3,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Variables in Java</h2>
  <p class=""text-slate-200"">Variables are named containers that hold values in memory.</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Primitive Types</h3>
  <ul class=""list-disc list-inside space-y-1 text-slate-200"">
    <li><code class=""bg-slate-900/70 px-1 rounded text-xs"">int</code> – whole numbers (e.g. 42)</li>
    <li><code class=""bg-slate-900/70 px-1 rounded text-xs"">double</code> – floating-point numbers (e.g. 3.14)</li>
    <li><code class=""bg-slate-900/70 px-1 rounded text-xs"">boolean</code> – true/false</li>
    <li><code class=""bg-slate-900/70 px-1 rounded text-xs"">char</code> – single character (e.g. 'A')</li>
  </ul>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Reference Types</h3>
  <p class=""text-slate-200"">Any non-primitive type (e.g., <code class=""bg-slate-900/70 px-1 rounded text-xs"">String</code>, arrays, custom classes) is stored by reference.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">int age = 25;
double price = 19.99;
boolean isStudent = true;
char grade = 'A';
String name = ""Tima"";

System.out.println(name + "" is "" + age + "" years old."");</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Operators &amp; Expressions</h3>
  <ul class=""list-disc list-inside space-y-1 text-slate-200"">
    <li>Arithmetic: <code class=""bg-slate-900/70 px-1 rounded text-xs"">+ - * / %</code></li>
    <li>Comparison: <code class=""bg-slate-900/70 px-1 rounded text-xs"">== != &gt; &lt; &gt;= &lt;=</code></li>
    <li>Logical: <code class=""bg-slate-900/70 px-1 rounded text-xs"">&amp;&amp; || !</code></li>
  </ul>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Quick Exercise</h3>
  <p class=""text-slate-200"">Declare variables to hold your name, favorite programming language and years of experience. Print a sentence combining them.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1542831371-29b0f74f9713?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/BBpAmxU_NQo",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 4 – CONTROL FLOW
                new Lesson
                {
                    Id = Guid.Parse("a4444444-4444-4444-4444-444444444444"),
                    CourseId = javaCourseId,
                    Title = "Control Flow: If, Else, Switch",
                    OrderNumber = 4,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Making Decisions in Java</h2>
  <p class=""text-slate-200"">Control flow structures let your program choose different paths based on conditions.</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">If / Else</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">int score = 78;

if (score &gt;= 90) {
    System.out.println(""A grade"");
} else if (score &gt;= 80) {
    System.out.println(""B grade"");
} else if (score &gt;= 70) {
    System.out.println(""C grade"");
} else {
    System.out.println(""Needs improvement"");
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Switch Statement</h3>
  <p class=""text-slate-200"">Useful when you have multiple discrete values to check.</p>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">int day = 3;
switch (day) {
    case 1 -&gt; System.out.println(""Monday"");
    case 2 -&gt; System.out.println(""Tuesday"");
    case 3 -&gt; System.out.println(""Wednesday"");
    default -&gt; System.out.println(""Unknown"");
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise Idea</h3>
  <p class=""text-slate-200"">Try these two exercises: (1) Read a numeric score and print its letter grade — A (≥90), B (≥80), C (≥70), or F (below 70) — using if/else if. (2) Read a day number (1–7) and use a switch statement to print the full day name (Monday through Sunday).</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1526498460520-4c246339dccb?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/8cm1x4bC610",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 5 – LOOPS
                new Lesson
                {
                    Id = Guid.Parse("a5555555-5555-5555-5555-555555555555"),
                    CourseId = javaCourseId,
                    Title = "Loops: For, While & Do-While",
                    OrderNumber = 5,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Repeating Work with Loops</h2>
  <p class=""text-slate-200"">Loops let you repeat code multiple times until a condition is met.</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">For Loop</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">for (int i = 1; i &lt;= 5; i++) {
    System.out.println(""Iteration "" + i);
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">While Loop</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">int count = 0;
while (count &lt; 3) {
    System.out.println(""Count: "" + count);
    count++;
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Do-While Loop</h3>
  <p class=""text-slate-200"">Executes the block at least once.</p>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">int number = 5;
do {
    System.out.println(""Number is "" + number);
    number--;
} while (number &gt; 0);</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise Idea</h3>
  <p class=""text-slate-200"">Try these two exercises: (1) Given an array of integers, use a <code class=""bg-slate-900/70 px-1 rounded text-xs"">for</code> loop to compute the sum and average. (2) Use nested <code class=""bg-slate-900/70 px-1 rounded text-xs"">for</code> loops to print a right triangle of stars — row 1 prints 1 star, row 2 prints 2 stars, and so on up to 5 rows.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1523475472560-d2df97ec485c?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/1uChuWUUH6I",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 6 – METHODS
                new Lesson
                {
                    Id = Guid.Parse("a6666666-6666-6666-6666-666666666666"),
                    CourseId = javaCourseId,
                    Title = "Methods & Parameters",
                    OrderNumber = 6,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Methods: Reusable Blocks of Logic</h2>
  <p class=""text-slate-200"">Methods let you group related instructions and call them by name.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">public static int add(int a, int b) {
    return a + b;
}

public static void main(String[] args) {
    int result = add(3, 5);
    System.out.println(""Result: "" + result);
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Key Concepts</h3>
  <ul class=""list-disc list-inside space-y-1 text-slate-200"">
    <li><strong>Return type:</strong> what the method gives back (or <code class=""bg-slate-900/70 px-1 rounded text-xs"">void</code>).</li>
    <li><strong>Parameters:</strong> input values.</li>
    <li><strong>Overloading:</strong> multiple methods with same name but different parameters.</li>
  </ul>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Create a method <code class=""bg-slate-900/70 px-1 rounded text-xs"">greet(String name)</code> that prints a personalized greeting. Overload it with a version that takes both <code class=""bg-slate-900/70 px-1 rounded text-xs"">name</code> and <code class=""bg-slate-900/70 px-1 rounded text-xs"">timeOfDay</code>.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/kz6bqG2gQzY",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 7 – INTRO OOP
                new Lesson
                {
                    Id = Guid.Parse("a7777777-7777-7777-7777-777777777777"),
                    CourseId = javaCourseId,
                    Title = "Introduction to OOP: Classes & Objects",
                    OrderNumber = 7,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Object-Oriented Programming</h2>
  <p class=""text-slate-200"">OOP models software as a collection of <strong>objects</strong> that interact. Each object bundles <em>state</em> (fields) and <em>behavior</em> (methods).</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Defining a Class</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">public class Student {
    String name;
    int age;

    public void introduce() {
        System.out.println(""Hi, I'm "" + name + "" and I'm "" + age + "" years old."");
    }
}

public class Main {
    public static void main(String[] args) {
        Student s = new Student();
        s.name = ""Tima"";
        s.age = 20;
        s.introduce();
    }
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Create a <code class=""bg-slate-900/70 px-1 rounded text-xs"">Book</code> class with fields <code class=""bg-slate-900/70 px-1 rounded text-xs"">title</code>, <code class=""bg-slate-900/70 px-1 rounded text-xs"">author</code> and <code class=""bg-slate-900/70 px-1 rounded text-xs"">price</code>, and a method <code class=""bg-slate-900/70 px-1 rounded text-xs"">printInfo()</code> that prints all details.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1515879218367-8466d910aaa4?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/HhQ8ZlQ4C1w",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 8 – ENCAPSULATION
                new Lesson
                {
                    Id = Guid.Parse("a8888888-8888-8888-8888-888888888888"),
                    CourseId = javaCourseId,
                    Title = "Encapsulation, Getters & Setters",
                    OrderNumber = 8,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Encapsulation</h2>
  <p class=""text-slate-200"">Encapsulation means hiding internal details and exposing a clean public API. In Java we use <code class=""bg-slate-900/70 px-1 rounded text-xs"">private</code> fields and <code class=""bg-slate-900/70 px-1 rounded text-xs"">public</code> getters/setters.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">public class BankAccount {
    private double balance;

    public BankAccount(double initialBalance) {
        if (initialBalance &gt;= 0) {
            this.balance = initialBalance;
        }
    }

    public double getBalance() {
        return balance;
    }

    public void deposit(double amount) {
        if (amount &gt; 0) balance += amount;
    }

    public void withdraw(double amount) {
        if (amount &gt; 0 &amp;&amp; amount &lt;= balance) balance -= amount;
    }
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Add validation in setters for a <code class=""bg-slate-900/70 px-1 rounded text-xs"">Student</code> class (e.g., age must be positive, name cannot be empty).</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1517411032315-54ef2cb783bb?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/Lk-uVEVGxOA",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 9 – INHERITANCE
                new Lesson
                {
                    Id = Guid.Parse("a9999999-9999-9999-9999-999999999999"),
                    CourseId = javaCourseId,
                    Title = "Inheritance: Extending Classes",
                    OrderNumber = 9,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Inheritance</h2>
  <p class=""text-slate-200"">Inheritance lets you create new classes that reuse, extend or modify behavior of existing ones.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">public class Animal {
    public void speak() {
        System.out.println(""The animal makes a sound."");
    }
}

public class Dog extends Animal {
    @Override
    public void speak() {
        System.out.println(""Woof!"");
    }
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Create a <code class=""bg-slate-900/70 px-1 rounded text-xs"">Vehicle</code> base class and subclasses <code class=""bg-slate-900/70 px-1 rounded text-xs"">Car</code> and <code class=""bg-slate-900/70 px-1 rounded text-xs"">Bicycle</code>. Override a method <code class=""bg-slate-900/70 px-1 rounded text-xs"">move()</code>.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1526498460520-4c246339dccb?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/wN0x9eZLix4",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 10 – POLYMORPHISM & ABSTRACT
                new Lesson
                {
                    Id = Guid.Parse("a1010101-0000-0000-0000-000000000000"),
                    CourseId = javaCourseId,
                    Title = "Polymorphism & Abstract Classes",
                    OrderNumber = 10,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Polymorphism</h2>
  <p class=""text-slate-200"">Polymorphism allows one interface to be used for a general class of actions. The specific action is determined at runtime.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">abstract class Shape {
    abstract double area();
}

class Circle extends Shape {
    double radius;
    Circle(double r) { this.radius = r; }

    @Override
    double area() { return Math.PI * radius * radius; }
}

class Rectangle extends Shape {
    double w, h;
    Rectangle(double w, double h) { this.w = w; this.h = h; }

    @Override
    double area() { return w * h; }
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Create an abstract class <code class=""bg-slate-900/70 px-1 rounded text-xs"">Employee</code> with abstract method <code class=""bg-slate-900/70 px-1 rounded text-xs"">calculatePay()</code>. Implement <code class=""bg-slate-900/70 px-1 rounded text-xs"">HourlyEmployee</code> and <code class=""bg-slate-900/70 px-1 rounded text-xs"">SalariedEmployee</code>.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1509228468518-180dd4864904?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/7psHffSdf1U",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 11 – INTERFACES
                new Lesson
                {
                    Id = Guid.Parse("a1110101-0000-0000-0000-000000000000"),
                    CourseId = javaCourseId,
                    Title = "Interfaces & Dependency Inversion",
                    OrderNumber = 11,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Interfaces</h2>
  <p class=""text-slate-200"">An interface defines a contract that classes can implement.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">public interface Notifier {
    void send(String message);
}

public class EmailNotifier implements Notifier {
    @Override
    public void send(String message) {
        System.out.println(""Sending EMAIL: "" + message);
    }
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Create a <code class=""bg-slate-900/70 px-1 rounded text-xs"">Logger</code> interface and implement <code class=""bg-slate-900/70 px-1 rounded text-xs"">ConsoleLogger</code> and <code class=""bg-slate-900/70 px-1 rounded text-xs"">FileLogger</code> (just simulate file logging with <code class=""bg-slate-900/70 px-1 rounded text-xs"">System.out</code>).</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1523475472560-d2df97ec485c?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/TH8qWSkudWY",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 12 – COLLECTIONS
                new Lesson
                {
                    Id = Guid.Parse("a1212121-0000-0000-0000-000000000000"),
                    CourseId = javaCourseId,
                    Title = "Collections: Lists, Sets & Maps",
                    OrderNumber = 12,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Java Collections Framework</h2>
  <p class=""text-slate-200"">Collections store groups of objects.</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">List</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">List&lt;String&gt; names = new ArrayList&lt;&gt;();
names.add(""Tima"");
names.add(""Haidar"");
names.add(""Nour"");
</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Set</h3>
  <p class=""text-slate-200"">No duplicates, no guaranteed order (for <code class=""bg-slate-900/70 px-1 rounded text-xs"">HashSet</code>).</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Map</h3>
  <p class=""text-slate-200"">Key–value pairs using <code class=""bg-slate-900/70 px-1 rounded text-xs"">Map&lt;K,V&gt;</code>.</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Use a <code class=""bg-slate-900/70 px-1 rounded text-xs"">Map&lt;String, Integer&gt;</code> to store programming languages and years of experience, then print them sorted by value.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1518933165971-611dbc9c412d?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/qQJ4U8mStDQ",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 13 – GENERICS
                new Lesson
                {
                    Id = Guid.Parse("a1313131-0000-0000-0000-000000000000"),
                    CourseId = javaCourseId,
                    Title = "Generics: Type-Safe Collections",
                    OrderNumber = 13,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Generics</h2>
  <p class=""text-slate-200"">Generics provide type safety at compile time.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">public class Box&lt;T&gt; {
    private T value;

    public void set(T value) { this.value = value; }
    public T get() { return value; }
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Create a generic class <code class=""bg-slate-900/70 px-1 rounded text-xs"">Pair&lt;T,U&gt;</code> holding two values of possibly different types, and test it with <code class=""bg-slate-900/70 px-1 rounded text-xs"">Pair&lt;String, Integer&gt;</code>.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1517245386807-bb43f82c33c4?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/K1iu1kXkVoA",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 14 – EXCEPTIONS
                new Lesson
                {
                    Id = Guid.Parse("a1414141-0000-0000-0000-000000000000"),
                    CourseId = javaCourseId,
                    Title = "Error Handling & Exceptions",
                    OrderNumber = 14,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Exceptions</h2>
  <p class=""text-slate-200"">Exceptions signal that something went wrong during execution.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">try {
    int x = 10 / 0;
} catch (ArithmeticException ex) {
    System.out.println(""Cannot divide by zero: "" + ex.getMessage());
} finally {
    System.out.println(""This always runs."");
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Write a method that parses a <code class=""bg-slate-900/70 px-1 rounded text-xs"">String</code> to <code class=""bg-slate-900/70 px-1 rounded text-xs"">int</code>, catching <code class=""bg-slate-900/70 px-1 rounded text-xs"">NumberFormatException</code> and returning a default value if parsing fails.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1517245386807-bb43f82c33c4?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/uvT9sOZo6o0",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 15 – FILE I/O
                new Lesson
                {
                    Id = Guid.Parse("a1515151-0000-0000-0000-000000000000"),
                    CourseId = javaCourseId,
                    Title = "File I/O: Reading & Writing Files",
                    OrderNumber = 15,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">File I/O</h2>
  <p class=""text-slate-200"">Java provides multiple APIs for working with files, such as <code class=""bg-slate-900/70 px-1 rounded text-xs"">java.nio.file</code>.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">Path path = Paths.get(""notes.txt"");
List&lt;String&gt; lines = Files.readAllLines(path);
for (String line : lines) {
    System.out.println(line);
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Write a program that appends a timestamped log line to a <code class=""bg-slate-900/70 px-1 rounded text-xs"">log.txt</code> file each time it runs.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1515879218367-8466d910aaa4?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/Sz6qDxzPdeo",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 16 – SIMPLE ALGORITHMS
                new Lesson
                {
                    Id = Guid.Parse("a1616161-0000-0000-0000-000000000000"),
                    CourseId = javaCourseId,
                    Title = "Algorithmic Thinking: Searching & Sorting",
                    OrderNumber = 16,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Algorithmic Thinking</h2>
  <p class=""text-slate-200"">As a Java developer, you'll often implement or use classic algorithms.</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Linear Search</h3>
  <p class=""text-slate-200"">Scan each element until you find the target.</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Bubble Sort (Learning Purposes)</h3>
  <p class=""text-slate-200"">Repeatedly swap adjacent elements that are in the wrong order.</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Implement a method <code class=""bg-slate-900/70 px-1 rounded text-xs"">int indexOf(int[] arr, int target)</code> that returns the index of <code class=""bg-slate-900/70 px-1 rounded text-xs"">target</code> or <code class=""bg-slate-900/70 px-1 rounded text-xs"">-1</code> if not found.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1523475472560-d2df97ec485c?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/8hly31xKli0",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 17 – THREADS
                new Lesson
                {
                    Id = Guid.Parse("a1717171-0000-0000-0000-000000000000"),
                    CourseId = javaCourseId,
                    Title = "Intro to Multithreading",
                    OrderNumber = 17,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Multithreading Basics</h2>
  <p class=""text-slate-200"">Threads allow your program to do multiple things seemingly at the same time.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">class Worker extends Thread {
    private final String name;

    Worker(String name) { this.name = name; }

    @Override
    public void run() {
        for (int i = 0; i &lt; 3; i++) {
            System.out.println(name + "" working... "" + i);
        }
    }
}</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Create a small demo where two threads print messages interleaved. Observe the non-deterministic order.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1553877522-43269d4ea984?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/QlK7-t2pE5Y",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 18 – EXECUTOR SERVICE
                new Lesson
                {
                    Id = Guid.Parse("a1818181-0000-0000-0000-000000000000"),
                    CourseId = javaCourseId,
                    Title = "Executors & Runnable",
                    OrderNumber = 18,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">ExecutorService</h2>
  <p class=""text-slate-200"">A higher-level abstraction for managing threads.</p>

  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs sm:text-sm overflow-x-auto mb-3"">
<code class=""language-java"">ExecutorService executor = Executors.newFixedThreadPool(2);

Runnable task = () -&gt; {
    System.out.println(""Running in "" + Thread.currentThread().getName());
};

executor.submit(task);
executor.submit(task);
executor.shutdown();</code></pre>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Exercise</h3>
  <p class=""text-slate-200"">Create 5 tasks that simulate downloading files by printing messages and sleeping for 500 ms.</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1542838132-92c53300491e?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/EglTzE7pG8M",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 19 – MINI-PROJECT
                new Lesson
                {
                    Id = Guid.Parse("a1919191-0000-0000-0000-000000000000"),
                    CourseId = javaCourseId,
                    Title = "Mini Project: Console To-Do Manager",
                    OrderNumber = 19,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Mini Project: To-Do Manager</h2>
  <p class=""text-slate-200"">Combine collections, loops, methods and basic I/O to build a console app:</p>

  <ul class=""list-disc list-inside space-y-1 text-slate-200"">
    <li>Add tasks with a title and priority.</li>
    <li>List tasks sorted by priority.</li>
    <li>Mark tasks as completed.</li>
  </ul>

  <p class=""text-slate-200"">Think about how you will represent tasks (class), where you store them (list) and how users will interact (menu loop).</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1515879218367-8466d910aaa4?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/I5srDu75h_M",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                },

                // 20 – FINAL PROJECT
                new Lesson
                {
                    Id = Guid.Parse("a2020202-0000-0000-0000-000000000000"),
                    CourseId = javaCourseId,
                    Title = "Final Project: Student Course Tracker",
                    OrderNumber = 20,
                    Content = @"
<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Final Project: Student Course Tracker</h2>
  <p class=""text-slate-200"">Build a simple in-memory system to track students, courses and enrollments.</p>

  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Requirements</h3>
  <ul class=""list-disc list-inside space-y-1 text-slate-200"">
    <li>Model <strong>Student</strong>, <strong>Course</strong> and <strong>Enrollment</strong> classes.</li>
    <li>Add new students and courses.</li>
    <li>Enroll students into courses.</li>
    <li>List all students with their enrolled courses.</li>
  </ul>

  <p class=""text-slate-200"">This mirrors a tiny version of what CodeWave does behind the scenes!</p>
</div>",
                    ImageUrl = "https://images.unsplash.com/photo-1523475472560-d2df97ec485c?auto=format&fit=crop&w=800&q=80",
                    VideoUrl = "https://www.youtube.com/embed/eIrMbAQSU34",
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                }
            };

            // UPSERT lessons
            foreach (var lesson in lessons)
            {
                var existing = await context.Lessons
                    .FirstOrDefaultAsync(l => l.Id == lesson.Id);

                if (existing == null)
                {
                    await context.Lessons.AddAsync(lesson);
                }
                else
                {
                    existing.Title = lesson.Title;
                    existing.Content = lesson.Content;
                    existing.ImageUrl = lesson.ImageUrl;
                    existing.VideoUrl = lesson.VideoUrl;
                    existing.OrderNumber = lesson.OrderNumber;
                    existing.isDeleted = false;
                }
            }

            await context.SaveChangesAsync();

            // ---------------------------------------------------------
            // 3) CODING EXERCISES (same as before)
            // ---------------------------------------------------------

            var exercises = new List<CodingExercise>
            {
                // Lesson 1 – concept questions
                new CodingExercise
                {
                    Id = Guid.Parse("b1111111-1111-1111-1111-111111111111"),
                    LessonId = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                    Title = "Identify Java Use Cases",
                    Description = "Print three types of real-world applications where Java is commonly used.",
                    StarterCode = "public class Main {\n    public static void main(String[] args) {\n        // Print three real-world Java use cases below\n        System.out.println(\"\");\n        System.out.println(\"\");\n        System.out.println(\"\");\n    }\n}\n",
                    ExpectedOutput = "e.g. Web servers, Android apps, Enterprise backends"
                },

                new CodingExercise
                {
                    Id = Guid.Parse("b1111111-1111-1111-1111-111111111112"),
                    LessonId = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                    Title = "Explain JVM in Your Own Words",
                    Description = "Write a short comment in the code explaining what the JVM is and why it's important.",
                    StarterCode = "public class Main {\n    public static void main(String[] args) {\n        // Write your explanation of the JVM here as comments\n        // What is it? Why is it important?\n    }\n}\n",
                    ExpectedOutput = "A short explanation comment describing JVM and bytecode."
                },

                // Lesson 2 – Hello World, syntax
                new CodingExercise
                {
                    Id = Guid.Parse("b2222222-2222-2222-2222-222222222221"),
                    LessonId = Guid.Parse("a2222222-2222-2222-2222-222222222222"),
                    Title = "Customize Hello Message",
                    Description = "Change the message so it prints your name and your goal in learning Java. Use the exact format: \"Hello, my name is <name> and I want to <goal>\".",
                    StarterCode =
"public class Main {\n" +
"    public static void main(String[] args) {\n" +
"        // TODO: Replace the empty strings with your real name and your goal\n" +
"        String name = \"\";\n" +
"        String goal = \"\";\n" +
"\n" +
"        // TODO: Print one line in the format:\n" +
"        //   Hello, my name is <name> and I want to <goal>\n" +
"        // Use System.out.println(...) and string concatenation with +\n" +
"\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "Hello, my name is <your name> and I want to <your goal>"
                },

                new CodingExercise
                {
                    Id = Guid.Parse("b2222222-2222-2222-2222-222222222222"),
                    LessonId = Guid.Parse("a2222222-2222-2222-2222-222222222222"),
                    Title = "Fix the Compilation Error",
                    Description = "This program has 2 syntax errors. Find and fix them so it compiles and prints: Hello World",
                    StarterCode =
"public class Main\n" +
"    public static void main(String[] args) {\n" +
"        System.out.println(\"Hello World\")\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "Hello World"
                },

                // Lesson 3 – variables
                new CodingExercise
                {
                    Id = Guid.Parse("b3333333-3333-3333-3333-333333333331"),
                    LessonId = Guid.Parse("a3333333-3333-3333-3333-333333333333"),
                    Title = "Introduce Yourself with Variables",
                    Description = "Declare a String variable for your name, an int for your age, and a String for your favorite language. Then print them in this format: <name> is <age> and loves <language>.",
                    StarterCode =
"public class Main {\n" +
"    public static void main(String[] args) {\n" +
"        // TODO: Replace with your own values\n" +
"        String name = \"\";\n" +
"        int age = 0;\n" +
"        String language = \"\";\n" +
"\n" +
"        // TODO: Print in the format: <name> is <age> and loves <language>.\n" +
"\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "is and loves"
                },

                new CodingExercise
                {
                    Id = Guid.Parse("b3333333-3333-3333-3333-333333333332"),
                    LessonId = Guid.Parse("a3333333-3333-3333-3333-333333333333"),
                    Title = "BMI Calculator",
                    Description = "Read weight (kg) and height (m) from input, compute the BMI and print it formatted to 2 decimal places. Formula: BMI = weight / (height * height). Print format: BMI: <value>",
                    StarterCode =
"import java.util.Scanner;\n" +
"public class Main {\n" +
"    public static void main(String[] args) {\n" +
"        Scanner sc = new Scanner(System.in);\n" +
"        double weightKg = sc.nextDouble();\n" +
"        double heightM = sc.nextDouble();\n" +
"\n" +
"        // TODO: Calculate BMI using the formula: weight / (height * height)\n" +
"        double bmi = 0;\n" +
"\n" +
"        // TODO: Print the result in this exact format: BMI: <value>\n" +
"        // Hint: use System.out.printf(\"BMI: %.2f%n\", bmi);\n" +
"\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "BMI: 22.86"
                },

                // Lesson 4 – if/switch
                new CodingExercise
                {
                    Id = Guid.Parse("b4444444-4444-4444-4444-444444444441"),
                    LessonId = Guid.Parse("a4444444-4444-4444-4444-444444444444"),
                    Title = "Grade Calculator",
                    Description = "Read a score from input, then print the corresponding letter grade: A (>=90), B (>=80), C (>=70), or F (below 70).",
                    StarterCode =
"import java.util.Scanner;\n" +
"public class Main {\n" +
"    public static void main(String[] args) {\n" +
"        Scanner sc = new Scanner(System.in);\n" +
"        int score = sc.nextInt();\n" +
"        String grade = \"?\";\n" +
"        // TODO: Use if/else if to assign the correct letter grade\n" +
"        // A (score >= 90), B (score >= 80), C (score >= 70), F (below 70)\n" +
"        System.out.println(\"Grade: \" + grade);\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "Grade: C"
                },

                new CodingExercise
                {
                    Id = Guid.Parse("b4444444-4444-4444-4444-444444444442"),
                    LessonId = Guid.Parse("a4444444-4444-4444-4444-444444444444"),
                    Title = "Day of Week via Switch",
                    Description = "Use switch to print the name of the day based on an int.",
                    StarterCode =
"import java.util.Scanner;\n" +
"public class Main {\n" +
"    public static void main(String[] args) {\n" +
"        Scanner sc = new Scanner(System.in);\n" +
"        int day = sc.nextInt(); // 1=Mon, 2=Tue, 3=Wed, 4=Thu, 5=Fri, 6=Sat, 7=Sun\n" +
"        String dayName;\n" +
"        // TODO: Use a switch statement to assign dayName based on day\n" +
"        // case 1 -> \"Monday\", 2 -> \"Tuesday\", 3 -> \"Wednesday\"\n" +
"        // case 4 -> \"Thursday\", 5 -> \"Friday\", 6 -> \"Saturday\"\n" +
"        // case 7 -> \"Sunday\", default -> \"Invalid\"\n" +
"        dayName = \"\"; // replace with your switch\n" +
"        System.out.println(\"For \" + day + \" -> \" + dayName);\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "For 3 -> Wednesday"
                },

                // Lesson 5 – loops
                new CodingExercise
                {
                    Id = Guid.Parse("b5555555-5555-5555-5555-555555555551"),
                    LessonId = Guid.Parse("a5555555-5555-5555-5555-555555555555"),
                    Title = "Sum of Array",
                    Description = "Compute sum and average of an array using a for loop.",
                    StarterCode =
"public class Main {\n" +
"    public static void main(String[] args) {\n" +
"        int[] numbers = {10, 20, 30, 40, 50};\n" +
"        int sum = 0;\n" +
"        // TODO: Use a for loop to add each element of numbers to sum\n" +
"\n" +
"        double avg = 0;\n" +
"        // TODO: Calculate the average: (double) sum / numbers.length\n" +
"\n" +
"        System.out.println(\"Sum: \" + sum + \", Avg: \" + avg);\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "Sum: 150, Avg: 30"
                },

                new CodingExercise
                {
                    Id = Guid.Parse("b5555555-5555-5555-5555-555555555552"),
                    LessonId = Guid.Parse("a5555555-5555-5555-5555-555555555555"),
                    Title = "Print a Triangle",
                    Description = "Use nested loops to print a right triangle of stars.",
                    StarterCode =
"public class Main {\n" +
"    public static void main(String[] args) {\n" +
"        int rows = 5;\n" +
"        // TODO: Use nested for loops to print a right triangle of stars\n" +
"        // Row 1 prints 1 star, row 2 prints 2 stars, ..., row 5 prints 5 stars\n" +
"        // Use System.out.print(\"*\") for each star and System.out.println() for newlines\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "*\n**\n***\n****\n*****"
                },

                // Lesson 6 – Methods & Parameters
                new CodingExercise
                {
                    Id = Guid.Parse("b6666666-6666-6666-6666-666666666661"),
                    LessonId = Guid.Parse("a6666666-6666-6666-6666-666666666666"),
                    Title = "Greet Method & Overloading",
                    Description = "Create a method greet(String name) that prints a personalized greeting. Overload it with greet(String name, String timeOfDay) that includes the time of day.",
                    StarterCode =
"public class Main {\n" +
"    public static void main(String[] args) {\n" +
"        greet(\"Alice\");\n" +
"        greet(\"Bob\", \"morning\");\n" +
"    }\n\n" +
"    public static void greet(String name) {\n" +
"        // TODO: Print \"Hello, [name]!\"\n" +
"    }\n\n" +
"    public static void greet(String name, String timeOfDay) {\n" +
"        // TODO: Print \"Good [timeOfDay], [name]!\"\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "Hello, Alice!\nGood morning, Bob!"
                },

                // Lesson 7 – OOP: Book Class
                new CodingExercise
                {
                    Id = Guid.Parse("b7777777-7777-7777-7777-777777777771"),
                    LessonId = Guid.Parse("a7777777-7777-7777-7777-777777777777"),
                    Title = "Book Class",
                    Description = "Create a Book class with fields title (String), author (String), and price (double). Add a constructor and a printInfo() method that prints 'Title: [title]', 'Author: [author]', and 'Price: [price]' on separate lines. The main method creates a Book and calls printInfo().",
                    StarterCode =
"public class Main {\n" +
"    static class Book {\n" +
"        String title;\n" +
"        String author;\n" +
"        double price;\n" +
"\n" +
"        // TODO: Add constructor Book(String title, String author, double price)\n" +
"\n" +
"        // TODO: Add printInfo() that prints:\n" +
"        // System.out.println(\"Title: \" + title);\n" +
"        // System.out.println(\"Author: \" + author);\n" +
"        // System.out.println(\"Price: \" + price);\n" +
"    }\n" +
"\n" +
"    public static void main(String[] args) {\n" +
"        Book book = new Book(\"Clean Code\", \"Robert Martin\", 29.99);\n" +
"        book.printInfo();\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "Title: Clean Code\nAuthor: Robert Martin\nPrice: 29.99"
                },

                // Lesson 8 – Encapsulation: Student Class
                new CodingExercise
                {
                    Id = Guid.Parse("b8888888-8888-8888-8888-888888888881"),
                    LessonId = Guid.Parse("a8888888-8888-8888-8888-888888888888"),
                    Title = "Student with Getters & Setters",
                    Description = "Implement setName(String name) — only set if name is not null or empty. Implement setAge(int age) — only set if age > 0. The constructor calls both setters. Main creates Student(\"Alice\", 20) and prints 'Name: [name]' and 'Age: [age]'.",
                    StarterCode =
"public class Main {\n" +
"    static class Student {\n" +
"        private String name;\n" +
"        private int age;\n" +
"\n" +
"        Student(String name, int age) {\n" +
"            setName(name);\n" +
"            setAge(age);\n" +
"        }\n" +
"\n" +
"        // TODO: setName - only set if name is not null or empty\n" +
"        public void setName(String name) {\n" +
"        }\n" +
"\n" +
"        // TODO: setAge - only set if age > 0\n" +
"        public void setAge(int age) {\n" +
"        }\n" +
"\n" +
"        public String getName() { return name; }\n" +
"        public int getAge()     { return age; }\n" +
"    }\n" +
"\n" +
"    public static void main(String[] args) {\n" +
"        Student s = new Student(\"Alice\", 20);\n" +
"        System.out.println(\"Name: \" + s.getName());\n" +
"        System.out.println(\"Age: \"  + s.getAge());\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "Name: Alice\nAge: 20"
                },

                // Lesson 9 – Inheritance: Vehicle, Car, Bicycle
                new CodingExercise
                {
                    Id = Guid.Parse("b9999999-9999-9999-9999-999999999991"),
                    LessonId = Guid.Parse("a9999999-9999-9999-9999-999999999999"),
                    Title = "Vehicle Inheritance",
                    Description = "Create Car and Bicycle classes that extend Vehicle. Override move() in Car to print 'Car drives on roads' and in Bicycle to print 'Bicycle rides on paths'. Main creates a Car and a Bicycle and calls move() on each.",
                    StarterCode =
"public class Main {\n" +
"    static class Vehicle {\n" +
"        String brand;\n" +
"        Vehicle(String brand) { this.brand = brand; }\n" +
"        void move() { System.out.println(\"The vehicle moves\"); }\n" +
"    }\n" +
"\n" +
"    // TODO: Create Car class extending Vehicle\n" +
"    // Constructor: Car(String brand) calls super(brand)\n" +
"    // Override move() to print: \"Car drives on roads\"\n" +
"\n" +
"    // TODO: Create Bicycle class extending Vehicle\n" +
"    // Constructor: Bicycle(String brand) calls super(brand)\n" +
"    // Override move() to print: \"Bicycle rides on paths\"\n" +
"\n" +
"    public static void main(String[] args) {\n" +
"        Vehicle car  = new Car(\"Toyota\");\n" +
"        Vehicle bike = new Bicycle(\"Trek\");\n" +
"        car.move();\n" +
"        bike.move();\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "Car drives on roads\nBicycle rides on paths"
                },

                // Lesson 10 – Polymorphism: Abstract Employee
                new CodingExercise
                {
                    Id = Guid.Parse("b1010101-0000-0000-0000-000000000001"),
                    LessonId = Guid.Parse("a1010101-0000-0000-0000-000000000000"),
                    Title = "Abstract Employee & Pay Calculation",
                    Description = "Create HourlyEmployee(String name, int hours, double rate) where calculatePay() returns hours * rate. Create SalariedEmployee(String name, double salary) where calculatePay() returns salary. Main prints '[name] earns: [pay]' for Alice (40h @ $25) and Bob ($3500 salary).",
                    StarterCode =
"public class Main {\n" +
"    abstract static class Employee {\n" +
"        String name;\n" +
"        Employee(String name) { this.name = name; }\n" +
"        abstract double calculatePay();\n" +
"    }\n" +
"\n" +
"    // TODO: Create HourlyEmployee(String name, int hours, double rate)\n" +
"    // calculatePay() returns hours * rate\n" +
"\n" +
"    // TODO: Create SalariedEmployee(String name, double salary)\n" +
"    // calculatePay() returns salary\n" +
"\n" +
"    public static void main(String[] args) {\n" +
"        Employee e1 = new HourlyEmployee(\"Alice\", 40, 25.0);\n" +
"        Employee e2 = new SalariedEmployee(\"Bob\", 3500.0);\n" +
"        System.out.println(e1.name + \" earns: \" + e1.calculatePay());\n" +
"        System.out.println(e2.name + \" earns: \" + e2.calculatePay());\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "Alice earns: 1000.0\nBob earns: 3500.0"
                },

                // Lesson 11 – Interfaces: Logger
                new CodingExercise
                {
                    Id = Guid.Parse("b1110101-0000-0000-0000-000000000001"),
                    LessonId = Guid.Parse("a1110101-0000-0000-0000-000000000000"),
                    Title = "Logger Interface",
                    Description = "Declare the log(String message) method in the Logger interface. Implement ConsoleLogger to print '[CONSOLE] message' and FileLogger to print '[FILE] message'. Main logs 'System started' with both implementations.",
                    StarterCode =
"public class Main {\n" +
"    interface Logger {\n" +
"        // TODO: declare void log(String message)\n" +
"    }\n" +
"\n" +
"    // TODO: ConsoleLogger implements Logger\n" +
"    // log() prints \"[CONSOLE] \" + message\n" +
"\n" +
"    // TODO: FileLogger implements Logger\n" +
"    // log() prints \"[FILE] \" + message\n" +
"\n" +
"    public static void main(String[] args) {\n" +
"        Logger console = new ConsoleLogger();\n" +
"        Logger file    = new FileLogger();\n" +
"        console.log(\"System started\");\n" +
"        file.log(\"System started\");\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "[CONSOLE] System started\n[FILE] System started"
                },

                // Lesson 12 – Collections: Word Frequency
                new CodingExercise
                {
                    Id = Guid.Parse("b1212121-0000-0000-0000-000000000001"),
                    LessonId = Guid.Parse("a1212121-0000-0000-0000-000000000000"),
                    Title = "Word Frequency Counter",
                    Description = "Use a TreeMap<String, Integer> to count how many times each word appears in the sentence 'java is fun and java is powerful'. Print each word and its count as 'word: count', one per line, in alphabetical order.",
                    StarterCode =
"import java.util.*;\n" +
"\n" +
"public class Main {\n" +
"    public static void main(String[] args) {\n" +
"        String sentence = \"java is fun and java is powerful\";\n" +
"        String[] words  = sentence.split(\" \");\n" +
"\n" +
"        // TODO: Use a TreeMap<String, Integer> to count word frequencies\n" +
"        Map<String, Integer> freq = new TreeMap<>();\n" +
"\n" +
"        // TODO: For each word, increment its count in the map\n" +
"\n" +
"        // TODO: Print each entry as: word + \": \" + count\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "and: 1\nfun: 1\nis: 2\njava: 2\npowerful: 1"
                },

                // Lesson 13 – Generics: Pair<T, U>
                new CodingExercise
                {
                    Id = Guid.Parse("b1313131-0000-0000-0000-000000000001"),
                    LessonId = Guid.Parse("a1313131-0000-0000-0000-000000000000"),
                    Title = "Generic Pair Class",
                    Description = "Create a generic class Pair<T, U> with private fields first and second, a constructor Pair(T first, U second), and getters getFirst() and getSecond(). Main creates Pair<String, Integer>(\"Java\", 2024) and prints 'First: Java, Second: 2024'.",
                    StarterCode =
"public class Main {\n" +
"    // TODO: Create generic class Pair<T, U>\n" +
"    // Fields: private T first; private U second;\n" +
"    // Constructor: Pair(T first, U second)\n" +
"    // Getters: getFirst(), getSecond()\n" +
"\n" +
"    public static void main(String[] args) {\n" +
"        Pair<String, Integer> pair = new Pair<>(\"Java\", 2024);\n" +
"        System.out.println(\"First: \" + pair.getFirst() + \", Second: \" + pair.getSecond());\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "First: Java, Second: 2024"
                },

                // Lesson 14 – Exceptions: Safe Integer Parser
                new CodingExercise
                {
                    Id = Guid.Parse("b1414141-0000-0000-0000-000000000001"),
                    LessonId = Guid.Parse("a1414141-0000-0000-0000-000000000000"),
                    Title = "Safe Integer Parser",
                    Description = "Implement parseIntSafe(String s, int defaultValue) using try-catch. It should return Integer.parseInt(s) if successful, or defaultValue if a NumberFormatException occurs. Main calls it three times and prints each result.",
                    StarterCode =
"public class Main {\n" +
"    // TODO: Implement parseIntSafe\n" +
"    // Return Integer.parseInt(s), or defaultValue on NumberFormatException\n" +
"    public static int parseIntSafe(String s, int defaultValue) {\n" +
"        // TODO\n" +
"        return defaultValue;\n" +
"    }\n" +
"\n" +
"    public static void main(String[] args) {\n" +
"        System.out.println(parseIntSafe(\"42\",  0));\n" +
"        System.out.println(parseIntSafe(\"abc\", -1));\n" +
"        System.out.println(parseIntSafe(\"100\", 0));\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "42\n-1\n100"
                },

                // Lesson 15 – File I/O: Log Writer Simulation
                new CodingExercise
                {
                    Id = Guid.Parse("b1515151-0000-0000-0000-000000000001"),
                    LessonId = Guid.Parse("a1515151-0000-0000-0000-000000000000"),
                    Title = "Log Writer (I/O Simulation)",
                    Description = "Implement addLog(String message) to add '[LOG] message' to an internal list, and printAll() to print every log entry. This simulates writing log entries to a file. Main adds 3 logs then calls printAll().",
                    StarterCode =
"import java.util.*;\n" +
"\n" +
"public class Main {\n" +
"    static class LogWriter {\n" +
"        private List<String> logs = new ArrayList<>();\n" +
"\n" +
"        // TODO: addLog(String message)\n" +
"        // Adds \"[LOG] \" + message to the logs list\n" +
"        public void addLog(String message) {\n" +
"        }\n" +
"\n" +
"        // TODO: printAll()\n" +
"        // Prints every entry in logs\n" +
"        public void printAll() {\n" +
"        }\n" +
"    }\n" +
"\n" +
"    public static void main(String[] args) {\n" +
"        LogWriter writer = new LogWriter();\n" +
"        writer.addLog(\"Application started\");\n" +
"        writer.addLog(\"User logged in\");\n" +
"        writer.addLog(\"Application stopped\");\n" +
"        writer.printAll();\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "[LOG] Application started\n[LOG] User logged in\n[LOG] Application stopped"
                },

                // Lesson 16 – Algorithms: Linear Search
                new CodingExercise
                {
                    Id = Guid.Parse("b1616161-0000-0000-0000-000000000001"),
                    LessonId = Guid.Parse("a1616161-0000-0000-0000-000000000000"),
                    Title = "Linear Search: indexOf",
                    Description = "Implement indexOf(int[] arr, int target) that scans the array and returns the index of the first occurrence of target, or -1 if not found. Main tests it on {5, 3, 8, 1, 9} with targets 8 (→ 2), 7 (→ -1), and 5 (→ 0).",
                    StarterCode =
"public class Main {\n" +
"    // TODO: Implement indexOf\n" +
"    // Scan arr from index 0; return the index where arr[i] == target, or -1\n" +
"    public static int indexOf(int[] arr, int target) {\n" +
"        return -1; // TODO\n" +
"    }\n" +
"\n" +
"    public static void main(String[] args) {\n" +
"        int[] arr = {5, 3, 8, 1, 9};\n" +
"        System.out.println(indexOf(arr, 8));  // 2\n" +
"        System.out.println(indexOf(arr, 7));  // -1\n" +
"        System.out.println(indexOf(arr, 5));  // 0\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "2\n-1\n0"
                },

                // Lesson 17 – Threads: Two Threads with join()
                new CodingExercise
                {
                    Id = Guid.Parse("b1717171-0000-0000-0000-000000000001"),
                    LessonId = Guid.Parse("a1717171-0000-0000-0000-000000000000"),
                    Title = "Two Threads with join()",
                    Description = "Create two threads using lambda expressions. Thread t1 prints 'Thread A running' and thread t2 prints 'Thread B running'. Start t1, join it (wait for completion), then start and join t2 to guarantee deterministic output.",
                    StarterCode =
"public class Main {\n" +
"    public static void main(String[] args) throws InterruptedException {\n" +
"        // TODO: Create Thread t1 with a lambda that prints \"Thread A running\"\n" +
"        // TODO: t1.start(); t1.join();\n" +
"\n" +
"        // TODO: Create Thread t2 with a lambda that prints \"Thread B running\"\n" +
"        // TODO: t2.start(); t2.join();\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "Thread A running\nThread B running"
                },

                // Lesson 18 – Executors: Single-Thread Pool
                new CodingExercise
                {
                    Id = Guid.Parse("b1818181-0000-0000-0000-000000000001"),
                    LessonId = Guid.Parse("a1818181-0000-0000-0000-000000000000"),
                    Title = "Executor Service: Task Queue",
                    Description = "Use Executors.newSingleThreadExecutor() to run 3 tasks in order. Each task prints 'Task [n] completed' (n = 1, 2, 3). Call executor.shutdown() and executor.awaitTermination(5, TimeUnit.SECONDS) to wait for all tasks.",
                    StarterCode =
"import java.util.concurrent.*;\n" +
"\n" +
"public class Main {\n" +
"    public static void main(String[] args) throws Exception {\n" +
"        // TODO: Create a single-thread executor\n" +
"        // ExecutorService executor = Executors.newSingleThreadExecutor();\n" +
"\n" +
"        // TODO: Submit 3 tasks (lambdas) that print:\n" +
"        // \"Task 1 completed\", \"Task 2 completed\", \"Task 3 completed\"\n" +
"\n" +
"        // TODO: executor.shutdown();\n" +
"        // TODO: executor.awaitTermination(5, TimeUnit.SECONDS);\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "Task 1 completed\nTask 2 completed\nTask 3 completed"
                },

                // Lesson 19 – Mini Project: Todo List
                new CodingExercise
                {
                    Id = Guid.Parse("b1919191-0000-0000-0000-000000000001"),
                    LessonId = Guid.Parse("a1919191-0000-0000-0000-000000000000"),
                    Title = "Todo List Manager",
                    Description = "Implement add(Task t) to store a task, and printAll() to print each task as '[DONE] title' if completed or '[TODO] title' if not. Main adds 3 tasks (Buy groceries: not done, Write code: done, Read a book: not done) and prints them.",
                    StarterCode =
"import java.util.*;\n" +
"\n" +
"public class Main {\n" +
"    static class Task {\n" +
"        String title;\n" +
"        boolean completed;\n" +
"        Task(String title, boolean completed) {\n" +
"            this.title     = title;\n" +
"            this.completed = completed;\n" +
"        }\n" +
"    }\n" +
"\n" +
"    static class TodoList {\n" +
"        private List<Task> tasks = new ArrayList<>();\n" +
"\n" +
"        // TODO: add(Task t) - adds the task to the list\n" +
"        public void add(Task t) {\n" +
"        }\n" +
"\n" +
"        // TODO: printAll() - prints \"[DONE] title\" or \"[TODO] title\"\n" +
"        public void printAll() {\n" +
"        }\n" +
"    }\n" +
"\n" +
"    public static void main(String[] args) {\n" +
"        TodoList list = new TodoList();\n" +
"        list.add(new Task(\"Buy groceries\", false));\n" +
"        list.add(new Task(\"Write code\",    true));\n" +
"        list.add(new Task(\"Read a book\",   false));\n" +
"        list.printAll();\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "[TODO] Buy groceries\n[DONE] Write code\n[TODO] Read a book"
                },

                // Lesson 20 – Final Project: Student Course Tracker
                new CodingExercise
                {
                    Id = Guid.Parse("b2020202-0000-0000-0000-000000000001"),
                    LessonId = Guid.Parse("a2020202-0000-0000-0000-000000000000"),
                    Title = "Student Course Tracker",
                    Description = "Implement enroll(Student s, String course) to add a course to a student's list, and printStudents() to print 'name enrolled in: course1, course2' for each student. Main enrolls Alice in Java and OOP, Bob in Java, then prints all students.",
                    StarterCode =
"import java.util.*;\n" +
"\n" +
"public class Main {\n" +
"    static class Student {\n" +
"        String name;\n" +
"        List<String> courses = new ArrayList<>();\n" +
"        Student(String name) { this.name = name; }\n" +
"    }\n" +
"\n" +
"    static class CourseTracker {\n" +
"        private List<Student> students = new ArrayList<>();\n" +
"\n" +
"        public void addStudent(Student s) { students.add(s); }\n" +
"\n" +
"        // TODO: enroll(Student s, String course)\n" +
"        // Add course to s.courses\n" +
"        public void enroll(Student s, String course) {\n" +
"        }\n" +
"\n" +
"        // TODO: printStudents()\n" +
"        // For each student print: name + \" enrolled in: \" + String.join(\", \", student.courses)\n" +
"        public void printStudents() {\n" +
"        }\n" +
"    }\n" +
"\n" +
"    public static void main(String[] args) {\n" +
"        CourseTracker tracker = new CourseTracker();\n" +
"        Student alice = new Student(\"Alice\");\n" +
"        Student bob   = new Student(\"Bob\");\n" +
"        tracker.addStudent(alice);\n" +
"        tracker.addStudent(bob);\n" +
"        tracker.enroll(alice, \"Java\");\n" +
"        tracker.enroll(alice, \"OOP\");\n" +
"        tracker.enroll(bob,   \"Java\");\n" +
"        tracker.printStudents();\n" +
"    }\n" +
"}\n",
                    ExpectedOutput = "Alice enrolled in: Java, OOP\nBob enrolled in: Java"
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
                    existingEx.LessonId = ex.LessonId;
                    existingEx.Title = ex.Title;
                    existingEx.Description = ex.Description;
                    existingEx.StarterCode = ex.StarterCode;
                    existingEx.ExpectedOutput = ex.ExpectedOutput;
                    existingEx.isDeleted = false;
                }
            }

            await context.SaveChangesAsync();

            // ---------------------------------------------------------
            // 3.2) CLEAN UP STALE LESSON 6 EXERCISES
            // Any exercise for lesson 6 that is NOT the canonical one must be
            // soft-deleted so it never appears alongside the real greet exercise.
            // ---------------------------------------------------------
            var lesson6Id      = Guid.Parse("a6666666-6666-6666-6666-666666666666");
            var greetExerciseId = Guid.Parse("b6666666-6666-6666-6666-666666666661");
            var staleLesson6Exercises = await context.CodingExercises
                .Where(e => e.LessonId == lesson6Id && e.Id != greetExerciseId && !e.isDeleted)
                .ToListAsync();
            foreach (var stale in staleLesson6Exercises)
            {
                stale.isDeleted = true;
            }
            if (staleLesson6Exercises.Any())
            {
                await context.SaveChangesAsync();
                Console.WriteLine($"[JavaSeed] Removed {staleLesson6Exercises.Count} stale exercise(s) for lesson 6.");
            }

            // ---------------------------------------------------------
            // 3.5) EXERCISE TEST CASES
            // ---------------------------------------------------------
            await SeedJavaExerciseTestCasesAsync(context);

            // ---------------------------------------------------------
            // 4) QUIZZES
            // ---------------------------------------------------------
            await SeedJavaQuizzesAsync(context, javaCourseId);
        }

        private static async Task SeedJavaExerciseTestCasesAsync(ApplicationDbContext context)
        {
            // Exercise 1: Identify Java Use Cases - Multiple test cases
            var exercise1Id = Guid.Parse("b1111111-1111-1111-1111-111111111111");
            var existingTestCases1 = await context.ExerciseTestCases
                .Where(tc => tc.ExerciseId == exercise1Id && !tc.IsDeleted)
                .AnyAsync();
            
            if (!existingTestCases1)
            {
                var testCases1 = new List<ExerciseTestCase>
                {
                    new ExerciseTestCase
                    {
                        Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                        ExerciseId = exercise1Id,
                        Input = "",
                        ExpectedOutput = "Web servers",
                        Description = "Test case 1: Should mention web servers",
                        OrderNumber = 1,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new ExerciseTestCase
                    {
                        Id = Guid.Parse("a1111111-1111-1111-1111-111111111112"),
                        ExerciseId = exercise1Id,
                        Input = "",
                        ExpectedOutput = "Android",
                        Description = "Test case 2: Should mention Android",
                        OrderNumber = 2,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new ExerciseTestCase
                    {
                        Id = Guid.Parse("a1111111-1111-1111-1111-111111111113"),
                        ExerciseId = exercise1Id,
                        Input = "",
                        ExpectedOutput = "Enterprise",
                        Description = "Test case 3: Should mention enterprise applications",
                        OrderNumber = 3,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    }
                };
                await context.ExerciseTestCases.AddRangeAsync(testCases1);
            }

            // Exercise 2 (Lesson 1): Explain JVM in Your Own Words - keyword checks in code text
            var jvmExerciseId = Guid.Parse("b1111111-1111-1111-1111-111111111112");
            var existingJvmTestCases = await context.ExerciseTestCases
                .Where(tc => tc.ExerciseId == jvmExerciseId && !tc.IsDeleted)
                .AnyAsync();

            if (!existingJvmTestCases)
            {
                var jvmTestCases = new List<ExerciseTestCase>
                {
                    new ExerciseTestCase
                    {
                        Id = Guid.Parse("c1111111-1111-1111-1111-111111111111"),
                        ExerciseId = jvmExerciseId,
                        Input = "__code__",
                        ExpectedOutput = "JVM",
                        Description = "Your explanation must mention JVM",
                        OrderNumber = 1,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new ExerciseTestCase
                    {
                        Id = Guid.Parse("c1111111-1111-1111-1111-111111111112"),
                        ExerciseId = jvmExerciseId,
                        Input = "__code__",
                        ExpectedOutput = "bytecode",
                        Description = "Your explanation must mention bytecode",
                        OrderNumber = 2,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new ExerciseTestCase
                    {
                        Id = Guid.Parse("c1111111-1111-1111-1111-111111111113"),
                        ExerciseId = jvmExerciseId,
                        Input = "__code__",
                        ExpectedOutput = "platform",
                        Description = "Your explanation must mention platform independence",
                        OrderNumber = 3,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    }
                };
                await context.ExerciseTestCases.AddRangeAsync(jvmTestCases);
            }

            // Lesson 2 Exercise 1: Customize Hello Message - output checks
            var customizeHelloId = Guid.Parse("b2222222-2222-2222-2222-222222222221");
            var existingCustomizeHelloTcs = await context.ExerciseTestCases
                .Where(tc => tc.ExerciseId == customizeHelloId && !tc.IsDeleted)
                .AnyAsync();

            if (!existingCustomizeHelloTcs)
            {
                var customizeHelloTcs = new List<ExerciseTestCase>
                {
                    new ExerciseTestCase
                    {
                        Id = Guid.Parse("c2222222-2222-2222-2222-222222222221"),
                        ExerciseId = customizeHelloId,
                        Input = "",
                        ExpectedOutput = "Hello, my name is",
                        Description = "Output should start with 'Hello, my name is'",
                        OrderNumber = 1,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new ExerciseTestCase
                    {
                        Id = Guid.Parse("c2222222-2222-2222-2222-222222222222"),
                        ExerciseId = customizeHelloId,
                        Input = "",
                        ExpectedOutput = "and I want to",
                        Description = "Output must contain 'and I want to'",
                        OrderNumber = 2,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new ExerciseTestCase
                    {
                        Id = Guid.Parse("c2222222-2222-2222-2222-222222222223"),
                        ExerciseId = customizeHelloId,
                        Input = "__code__",
                        ExpectedOutput = "System.out.println",
                        Description = "Your code must call System.out.println",
                        OrderNumber = 3,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    }
                };
                await context.ExerciseTestCases.AddRangeAsync(customizeHelloTcs);
            }

            // Exercise 2: Fix Compilation Error — upsert so changes always take effect
            var exercise2Id = Guid.Parse("b2222222-2222-2222-2222-222222222222");

            // Remove false-positive submissions created when old test cases matched
            // compilation error text. Safe to run repeatedly — only targets submissions
            // where the stored output proves the code never actually compiled.
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b2222222-2222-2222-2222-222222222222'
                AND Output LIKE '%Compilation Error%'");

            // TC1: verify student added the opening brace to the class declaration
            await UpsertTestCase(context,
                Guid.Parse("a2222222-2222-2222-2222-222222222221"),
                exercise2Id,
                "__code__", "public class Main {",
                "Your code must have 'public class Main {' with the opening brace", 1);
            // TC2: verify student added the semicolon after println
            await UpsertTestCase(context,
                Guid.Parse("a2222222-2222-2222-2222-222222222222"),
                exercise2Id,
                "__code__", "println(\"Hello World\");",
                "Your println statement must end with a semicolon", 2);

            // Lesson 3 Exercise 1: Introduce Yourself with Variables
            var introduceYourselfId = Guid.Parse("b3333333-3333-3333-3333-333333333331");

            // Remove any false-positive submissions (old ExpectedOutput had "Example:" prefix)
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b3333333-3333-3333-3333-333333333331'
                AND Output LIKE '%Example%'");

            await UpsertTestCase(context,
                Guid.Parse("d3333333-0000-0000-0000-000000000001"),
                introduceYourselfId,
                "__code__", "String name",
                "You must declare a String variable called 'name'", 1);
            await UpsertTestCase(context,
                Guid.Parse("d3333333-0000-0000-0000-000000000002"),
                introduceYourselfId,
                "__code__", "int age",
                "You must declare an int variable called 'age'", 2);
            await UpsertTestCase(context,
                Guid.Parse("d3333333-0000-0000-0000-000000000003"),
                introduceYourselfId,
                "__code__", "System.out.println",
                "You must print your output using System.out.println", 3);
            await UpsertTestCase(context,
                Guid.Parse("d3333333-0000-0000-0000-000000000004"),
                introduceYourselfId,
                "", "is",
                "Your output must contain the word 'is'", 4);
            await UpsertTestCase(context,
                Guid.Parse("d3333333-0000-0000-0000-000000000005"),
                introduceYourselfId,
                "", "loves",
                "Your output must contain the word 'loves'", 5);

            // Exercise 3: BMI Calculator - Multiple test cases with different inputs
            var exercise3Id = Guid.Parse("b3333333-3333-3333-3333-333333333332");
            var existingTestCases3 = await context.ExerciseTestCases
                .Where(tc => tc.ExerciseId == exercise3Id && !tc.IsDeleted)
                .AnyAsync();
            
            if (!existingTestCases3)
            {
                var testCases3 = new List<ExerciseTestCase>
                {
                    new ExerciseTestCase
                    {
                        Id = Guid.Parse("a3333333-3333-3333-3333-333333333331"),
                        ExerciseId = exercise3Id,
                        Input = "70 1.75",
                        ExpectedOutput = "22.86",
                        Description = "Test case 1: BMI for weight 70kg, height 1.75m should be approximately 22.86",
                        OrderNumber = 1,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new ExerciseTestCase
                    {
                        Id = Guid.Parse("a3333333-3333-3333-3333-333333333332"),
                        ExerciseId = exercise3Id,
                        Input = "80 1.80",
                        ExpectedOutput = "24.69",
                        Description = "Test case 2: BMI for weight 80kg, height 1.80m should be approximately 24.69",
                        OrderNumber = 2,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new ExerciseTestCase
                    {
                        Id = Guid.Parse("a3333333-3333-3333-3333-333333333333"),
                        ExerciseId = exercise3Id,
                        Input = "70 1.75",
                        ExpectedOutput = "BMI",
                        Description = "Test case 3: Should output BMI value",
                        OrderNumber = 3,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    }
                };
                await context.ExerciseTestCases.AddRangeAsync(testCases3);
            }
            else
            {
                // Fix existing records that were seeded with wrong key=value input format
                var bmitc1 = await context.ExerciseTestCases.FindAsync(Guid.Parse("a3333333-3333-3333-3333-333333333331"));
                if (bmitc1 != null && bmitc1.Input != "70 1.75") bmitc1.Input = "70 1.75";
                var bmitc2 = await context.ExerciseTestCases.FindAsync(Guid.Parse("a3333333-3333-3333-3333-333333333332"));
                if (bmitc2 != null && bmitc2.Input != "80 1.80") bmitc2.Input = "80 1.80";
                var bmitc3 = await context.ExerciseTestCases.FindAsync(Guid.Parse("a3333333-3333-3333-3333-333333333333"));
                if (bmitc3 != null && bmitc3.Input != "70 1.75") bmitc3.Input = "70 1.75";
            }

            // Grade Calculator: delete false-positive submissions from when starter code was the full solution
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b4444444-4444-4444-4444-444444444441'
                AND IsCorrect = 1");

            // Grade Calculator test cases — always upsert so input format stays correct
            await UpsertTestCase(context,
                Guid.Parse("a4444444-4444-4444-4444-444444444441"),
                Guid.Parse("b4444444-4444-4444-4444-444444444441"),
                "73", "Grade: C",
                "Score 73 should output Grade: C", 1);
            await UpsertTestCase(context,
                Guid.Parse("a4444444-4444-4444-4444-444444444442"),
                Guid.Parse("b4444444-4444-4444-4444-444444444441"),
                "85", "Grade: B",
                "Score 85 should output Grade: B", 2);
            await UpsertTestCase(context,
                Guid.Parse("a4444444-4444-4444-4444-444444444443"),
                Guid.Parse("b4444444-4444-4444-4444-444444444441"),
                "95", "Grade: A",
                "Score 95 should output Grade: A", 3);

            // Day of Week: delete false-positive submissions
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b4444444-4444-4444-4444-444444444442'
                AND IsCorrect = 1");

            // Day of Week test cases — exercise now uses Scanner so we can test multiple days
            await UpsertTestCase(context,
                Guid.Parse("a4444444-4444-4444-4444-44444444444a"),
                Guid.Parse("b4444444-4444-4444-4444-444444444442"),
                "3", "Wednesday",
                "Day 3 should output Wednesday", 1);
            await UpsertTestCase(context,
                Guid.Parse("a4444444-4444-4444-4444-44444444444b"),
                Guid.Parse("b4444444-4444-4444-4444-444444444442"),
                "5", "Friday",
                "Day 5 should output Friday", 2);
            await UpsertTestCase(context,
                Guid.Parse("a4444444-4444-4444-4444-44444444444c"),
                Guid.Parse("b4444444-4444-4444-4444-444444444442"),
                "1", "Monday",
                "Day 1 should output Monday", 3);

            // Sum of Array: delete false-positive submissions
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b5555555-5555-5555-5555-555555555551'
                AND IsCorrect = 1");

            // Sum of Array test cases — upsert so TC3 gets fixed to __code__ check
            await UpsertTestCase(context,
                Guid.Parse("a5555555-5555-5555-5555-555555555551"),
                Guid.Parse("b5555555-5555-5555-5555-555555555551"),
                "", "Sum: 150",
                "Output must contain 'Sum: 150'", 1);
            await UpsertTestCase(context,
                Guid.Parse("a5555555-5555-5555-5555-555555555552"),
                Guid.Parse("b5555555-5555-5555-5555-555555555551"),
                "", "Avg: 30.0",
                "Output must contain 'Avg: 30.0'", 2);
            await UpsertTestCase(context,
                Guid.Parse("a5555555-5555-5555-5555-555555555553"),
                Guid.Parse("b5555555-5555-5555-5555-555555555551"),
                "__code__", "for",
                "You must use a for loop", 3);

            // Print a Triangle: delete false-positive submissions
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b5555555-5555-5555-5555-555555555552'
                AND IsCorrect = 1");

            // Print a Triangle test cases
            await UpsertTestCase(context,
                Guid.Parse("a5555555-5555-5555-5555-55555555555a"),
                Guid.Parse("b5555555-5555-5555-5555-555555555552"),
                "", "*\n**\n***\n****\n*****",
                "Output must print all 5 rows of the triangle correctly", 4);
            await UpsertTestCase(context,
                Guid.Parse("a5555555-5555-5555-5555-55555555555b"),
                Guid.Parse("b5555555-5555-5555-5555-555555555552"),
                "__code__", "for",
                "You must use nested for loops", 5);

            // Lesson 6: delete false-positive submissions from when starter code had the full solution
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b6666666-6666-6666-6666-666666666661'
                AND IsCorrect = 1");

           // Lesson 6: Greet Method & Overloading - Multiple test cases
var exercise6Id = Guid.Parse("b6666666-6666-6666-6666-666666666661");

// soft-delete any stale lesson 6 test cases for this exercise except the canonical two
var lesson6CanonicalTc1 = Guid.Parse("a6666666-6666-6666-6666-666666666661");
var lesson6CanonicalTc2 = Guid.Parse("a6666666-6666-6666-6666-666666666662");

var staleLesson6TestCases = await context.ExerciseTestCases
    .Where(tc => tc.ExerciseId == exercise6Id
        && tc.Id != lesson6CanonicalTc1
        && tc.Id != lesson6CanonicalTc2
        && !tc.IsDeleted)
    .ToListAsync();

foreach (var stale in staleLesson6TestCases)
{
    stale.IsDeleted = true;
}

var tc1 = await context.ExerciseTestCases.FindAsync(lesson6CanonicalTc1);
if (tc1 == null)
{
    tc1 = new ExerciseTestCase
    {
        Id = lesson6CanonicalTc1,
        ExerciseId = exercise6Id,
        Input = "",
        ExpectedOutput = "Hello, Alice!\nGood morning, Bob!",
        Description = "Test case 1: both greet methods should print the correct lines",
        OrderNumber = 1,
        CreatedAt = DateTime.UtcNow,
        IsDeleted = false
    };

    await context.ExerciseTestCases.AddAsync(tc1);
}
else
{
    tc1.ExerciseId = exercise6Id;
    tc1.Input = "";
    tc1.ExpectedOutput = "Hello, Alice!\nGood morning, Bob!";
    tc1.Description = "Test case 1: both greet methods should print the correct lines";
    tc1.OrderNumber = 1;
    tc1.IsDeleted = false;
}

var tc2 = await context.ExerciseTestCases.FindAsync(lesson6CanonicalTc2);
if (tc2 == null)
{
    tc2 = new ExerciseTestCase
    {
        Id = lesson6CanonicalTc2,
        ExerciseId = exercise6Id,
        Input = "",
        ExpectedOutput = "Good morning, Bob!",
        Description = "Test case 2: greet(\"Bob\", \"morning\") should print Good morning, Bob!",
        OrderNumber = 2,
        CreatedAt = DateTime.UtcNow,
        IsDeleted = true // merged into tc1; redundant with empty input
    };

    await context.ExerciseTestCases.AddAsync(tc2);
}
else
{
    tc2.ExerciseId = exercise6Id;
    tc2.Input = "";
    tc2.ExpectedOutput = "Good morning, Bob!";
    tc2.Description = "Test case 2: greet(\"Bob\", \"morning\") should print Good morning, Bob!";
    tc2.OrderNumber = 2;
    tc2.IsDeleted = true; // merged into tc1; redundant with empty input
}

            // ---- Lessons 7-20 test cases ----

            // Lesson 7: Book Class — delete false-positive submissions from when starter code had the full solution
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b7777777-7777-7777-7777-777777777771'
                AND IsCorrect = 1");

            // Lesson 7: Book Class
            await UpsertTestCase(context,
                Guid.Parse("c7777777-7777-7777-7777-777777777771"),
                Guid.Parse("b7777777-7777-7777-7777-777777777771"),
                "", "Title: Clean Code\nAuthor: Robert Martin\nPrice: 29.99",
                "Full output: Title, Author and Price lines", 1);

            // Lesson 8: Student Encapsulation
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b8888888-8888-8888-8888-888888888881' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c8888888-8888-8888-8888-888888888881"),
                Guid.Parse("b8888888-8888-8888-8888-888888888881"),
                "", "Name: Alice\nAge: 20",
                "Prints Name and Age using validated setters", 1);

            // Lesson 9: Vehicle Inheritance
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b9999999-9999-9999-9999-999999999991' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c9999999-9999-9999-9999-999999999991"),
                Guid.Parse("b9999999-9999-9999-9999-999999999991"),
                "", "Car drives on roads\nBicycle rides on paths",
                "Car.move() and Bicycle.move() produce correct lines", 1);

            // Lesson 10: Abstract Employee
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b1010101-0000-0000-0000-000000000001' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c1010101-0000-0000-0000-000000000001"),
                Guid.Parse("b1010101-0000-0000-0000-000000000001"),
                "", "Alice earns: 1000.0\nBob earns: 3500.0",
                "HourlyEmployee 40h*25.0=1000.0, SalariedEmployee 3500.0", 1);

            // Lesson 11: Logger Interface
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b1110101-0000-0000-0000-000000000001' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c1110101-0000-0000-0000-000000000001"),
                Guid.Parse("b1110101-0000-0000-0000-000000000001"),
                "", "[CONSOLE] System started\n[FILE] System started",
                "ConsoleLogger and FileLogger both log the message", 1);

            // Lesson 12: Word Frequency
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b1212121-0000-0000-0000-000000000001' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c1212121-0000-0000-0000-000000000001"),
                Guid.Parse("b1212121-0000-0000-0000-000000000001"),
                "", "and: 1\nfun: 1\nis: 2\njava: 2\npowerful: 1",
                "TreeMap word counts in alphabetical order", 1);

            // Lesson 13: Generic Pair
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b1313131-0000-0000-0000-000000000001' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c1313131-0000-0000-0000-000000000001"),
                Guid.Parse("b1313131-0000-0000-0000-000000000001"),
                "", "First: Java, Second: 2024",
                "Pair<String,Integer> getters print correctly", 1);

            // Lesson 14: Safe Integer Parser
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b1414141-0000-0000-0000-000000000001' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c1414141-0000-0000-0000-000000000001"),
                Guid.Parse("b1414141-0000-0000-0000-000000000001"),
                "", "42\n-1\n100",
                "parseIntSafe returns parsed value or default on exception", 1);

            // Lesson 15: Log Writer
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b1515151-0000-0000-0000-000000000001' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c1515151-0000-0000-0000-000000000001"),
                Guid.Parse("b1515151-0000-0000-0000-000000000001"),
                "", "[LOG] Application started\n[LOG] User logged in\n[LOG] Application stopped",
                "addLog prepends [LOG] prefix; printAll outputs all entries", 1);

            // Lesson 16: Linear Search
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b1616161-0000-0000-0000-000000000001' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c1616161-0000-0000-0000-000000000001"),
                Guid.Parse("b1616161-0000-0000-0000-000000000001"),
                "", "2\n-1\n0",
                "indexOf returns correct index or -1 for missing element", 1);

            // Lesson 17: Threads
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b1717171-0000-0000-0000-000000000001' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c1717171-0000-0000-0000-000000000001"),
                Guid.Parse("b1717171-0000-0000-0000-000000000001"),
                "", "Thread A running\nThread B running",
                "t1 joined before t2 starts guarantees deterministic order", 1);

            // Lesson 18: Executor Service
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b1818181-0000-0000-0000-000000000001' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c1818181-0000-0000-0000-000000000001"),
                Guid.Parse("b1818181-0000-0000-0000-000000000001"),
                "", "Task 1 completed\nTask 2 completed\nTask 3 completed",
                "Single-thread executor runs tasks in submission order", 1);

            // Lesson 19: Todo List
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b1919191-0000-0000-0000-000000000001' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c1919191-0000-0000-0000-000000000001"),
                Guid.Parse("b1919191-0000-0000-0000-000000000001"),
                "", "[TODO] Buy groceries\n[DONE] Write code\n[TODO] Read a book",
                "printAll formats completed and pending tasks correctly", 1);

            // Lesson 20: Course Tracker
            await context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM ExerciseSubmissions
                WHERE ExerciseId = 'b2020202-0000-0000-0000-000000000001' AND IsCorrect = 1");
            await UpsertTestCase(context,
                Guid.Parse("c2020202-0000-0000-0000-000000000001"),
                Guid.Parse("b2020202-0000-0000-0000-000000000001"),
                "", "Alice enrolled in: Java, OOP\nBob enrolled in: Java",
                "printStudents shows each student with joined course list", 1);

            await context.SaveChangesAsync();
            Console.WriteLine("Completed seeding Java exercise test cases");
        }

        private static async Task UpsertTestCase(
            ApplicationDbContext context,
            Guid id, Guid exerciseId,
            string input, string expectedOutput,
            string description, int orderNumber)
        {
            var existing = await context.ExerciseTestCases.FindAsync(id);
            if (existing == null)
            {
                await context.ExerciseTestCases.AddAsync(new ExerciseTestCase
                {
                    Id = id,
                    ExerciseId = exerciseId,
                    Input = input,
                    ExpectedOutput = expectedOutput,
                    Description = description,
                    OrderNumber = orderNumber,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                });
            }
            else
            {
                existing.ExerciseId = exerciseId;
                existing.Input = input;
                existing.ExpectedOutput = expectedOutput;
                existing.Description = description;
                existing.OrderNumber = orderNumber;
                existing.IsDeleted = false;
            }
        }

        private static async Task SeedJavaQuizzesAsync(ApplicationDbContext context, Guid courseId)
        {
            // Quiz 1: Java Basics Quiz
            var quiz1Id = Guid.Parse("a1111111-1111-1111-1111-111111111111");
            var quiz1 = await context.Quizzes.FirstOrDefaultAsync(q => q.Id == quiz1Id);
            
            if (quiz1 == null)
            {
                quiz1 = new Quiz
                {
                    Id = quiz1Id,
                    CourseId = courseId,
                    Title = "Java Basics Quiz",
                    Description = "Test your understanding of Java syntax, variables, and data types",
                    TimeLimitMinutes = 15,
                    PassingScore = 70,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.Quizzes.AddAsync(quiz1);
                await context.SaveChangesAsync();

                // Question 1
                var q1 = new QuizQuestion
                {
                    Id = Guid.Parse("a9111111-1111-1111-1111-111111111111"),
                    QuizId = quiz1Id,
                    Text = "What is the entry point of a Java application?",
                    Difficulty = "Easy",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q1);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("aa111111-1111-1111-1111-111111111111"), QuizQuestionId = q1.Id, Text = "main()", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111112-1111-1111-1111-111111111111"), QuizQuestionId = q1.Id, Text = "public static void main(String[] args)", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111113-1111-1111-1111-111111111111"), QuizQuestionId = q1.Id, Text = "start()", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111114-1111-1111-1111-111111111111"), QuizQuestionId = q1.Id, Text = "run()", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 2
                var q2 = new QuizQuestion
                {
                    Id = Guid.Parse("a9111112-1111-1111-1111-111111111111"),
                    QuizId = quiz1Id,
                    Text = "Which data type is used for whole numbers in Java?",
                    Difficulty = "Easy",
                    OrderNumber = 2,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q2);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("aa111121-1111-1111-1111-111111111111"), QuizQuestionId = q2.Id, Text = "float", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111122-1111-1111-1111-111111111111"), QuizQuestionId = q2.Id, Text = "int", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111123-1111-1111-1111-111111111111"), QuizQuestionId = q2.Id, Text = "double", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111124-1111-1111-1111-111111111111"), QuizQuestionId = q2.Id, Text = "String", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 3
                var q3 = new QuizQuestion
                {
                    Id = Guid.Parse("a9111113-1111-1111-1111-111111111111"),
                    QuizId = quiz1Id,
                    Text = "What is the output of: System.out.println(5 / 2);",
                    Difficulty = "Easy",
                    OrderNumber = 3,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q3);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("aa111131-1111-1111-1111-111111111111"), QuizQuestionId = q3.Id, Text = "2.5", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111132-1111-1111-1111-111111111111"), QuizQuestionId = q3.Id, Text = "2", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111133-1111-1111-1111-111111111111"), QuizQuestionId = q3.Id, Text = "2.0", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111134-1111-1111-1111-111111111111"), QuizQuestionId = q3.Id, Text = "Error", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 4
                var q4 = new QuizQuestion
                {
                    Id = Guid.Parse("a9111114-1111-1111-1111-111111111111"),
                    QuizId = quiz1Id,
                    Text = "Which keyword is used to declare a constant in Java?",
                    Difficulty = "Medium",
                    OrderNumber = 4,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q4);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("aa111141-1111-1111-1111-111111111111"), QuizQuestionId = q4.Id, Text = "const", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111142-1111-1111-1111-111111111111"), QuizQuestionId = q4.Id, Text = "final", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111143-1111-1111-1111-111111111111"), QuizQuestionId = q4.Id, Text = "static", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111144-1111-1111-1111-111111111111"), QuizQuestionId = q4.Id, Text = "constant", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 5
                var q5 = new QuizQuestion
                {
                    Id = Guid.Parse("a9111115-1111-1111-1111-111111111111"),
                    QuizId = quiz1Id,
                    Text = "What is the size of an int in Java?",
                    Difficulty = "Medium",
                    OrderNumber = 5,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q5);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("aa111151-1111-1111-1111-111111111111"), QuizQuestionId = q5.Id, Text = "2 bytes", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111152-1111-1111-1111-111111111111"), QuizQuestionId = q5.Id, Text = "4 bytes", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111153-1111-1111-1111-111111111111"), QuizQuestionId = q5.Id, Text = "8 bytes", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa111154-1111-1111-1111-111111111111"), QuizQuestionId = q5.Id, Text = "Depends on the system", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                await context.SaveChangesAsync();
            }

            // Quiz 2: Java OOP Quiz
            var quiz2Id = Guid.Parse("a2222222-2222-2222-2222-222222222222");
            var quiz2 = await context.Quizzes.FirstOrDefaultAsync(q => q.Id == quiz2Id);
            
            if (quiz2 == null)
            {
                quiz2 = new Quiz
                {
                    Id = quiz2Id,
                    CourseId = courseId,
                    Title = "Java OOP Quiz",
                    Description = "Test your knowledge of object-oriented programming in Java",
                    TimeLimitMinutes = 20,
                    PassingScore = 75,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.Quizzes.AddAsync(quiz2);
                await context.SaveChangesAsync();

                // Question 1
                var q6 = new QuizQuestion
                {
                    Id = Guid.Parse("a9222221-2222-2222-2222-222222222222"),
                    QuizId = quiz2Id,
                    Text = "What are the four pillars of OOP?",
                    Difficulty = "Easy",
                    OrderNumber = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q6);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("aa222221-2222-2222-2222-222222222222"), QuizQuestionId = q6.Id, Text = "Inheritance, Polymorphism, Abstraction, Encapsulation", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222222-2222-2222-2222-222222222222"), QuizQuestionId = q6.Id, Text = "Class, Object, Method, Variable", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222223-2222-2222-2222-222222222222"), QuizQuestionId = q6.Id, Text = "Public, Private, Protected, Default", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222224-2222-2222-2222-222222222222"), QuizQuestionId = q6.Id, Text = "Static, Final, Abstract, Interface", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 2
                var q7 = new QuizQuestion
                {
                    Id = Guid.Parse("a9222222-2222-2222-2222-222222222222"),
                    QuizId = quiz2Id,
                    Text = "Which keyword is used for inheritance in Java?",
                    Difficulty = "Easy",
                    OrderNumber = 2,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q7);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("aa222231-2222-2222-2222-222222222222"), QuizQuestionId = q7.Id, Text = "extends", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222232-2222-2222-2222-222222222222"), QuizQuestionId = q7.Id, Text = "implements", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222233-2222-2222-2222-222222222222"), QuizQuestionId = q7.Id, Text = "inherits", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222234-2222-2222-2222-222222222222"), QuizQuestionId = q7.Id, Text = "super", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 3
                var q8 = new QuizQuestion
                {
                    Id = Guid.Parse("a9222223-2222-2222-2222-222222222222"),
                    QuizId = quiz2Id,
                    Text = "What is method overriding?",
                    Difficulty = "Medium",
                    OrderNumber = 3,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q8);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("aa222241-2222-2222-2222-222222222222"), QuizQuestionId = q8.Id, Text = "Providing a new implementation of a method in a subclass", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222242-2222-2222-2222-222222222222"), QuizQuestionId = q8.Id, Text = "Creating multiple methods with the same name", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222243-2222-2222-2222-222222222222"), QuizQuestionId = q8.Id, Text = "Hiding a method from the parent class", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222244-2222-2222-2222-222222222222"), QuizQuestionId = q8.Id, Text = "Calling a method from another class", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 4
                var q9 = new QuizQuestion
                {
                    Id = Guid.Parse("a9222224-2222-2222-2222-222222222222"),
                    QuizId = quiz2Id,
                    Text = "What is the default access modifier in Java?",
                    Difficulty = "Medium",
                    OrderNumber = 4,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q9);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("aa222251-2222-2222-2222-222222222222"), QuizQuestionId = q9.Id, Text = "public", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222252-2222-2222-2222-222222222222"), QuizQuestionId = q9.Id, Text = "private", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222253-2222-2222-2222-222222222222"), QuizQuestionId = q9.Id, Text = "package-private (no modifier)", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222254-2222-2222-2222-222222222222"), QuizQuestionId = q9.Id, Text = "protected", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                // Question 5
                var q10 = new QuizQuestion
                {
                    Id = Guid.Parse("a9222225-2222-2222-2222-222222222222"),
                    QuizId = quiz2Id,
                    Text = "What keyword is used to refer to the current object?",
                    Difficulty = "Easy",
                    OrderNumber = 5,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await context.QuizQuestions.AddAsync(q10);
                await context.QuizAnswerOptions.AddRangeAsync(new[]
                {
                    new QuizAnswerOption { Id = Guid.Parse("aa222261-2222-2222-2222-222222222222"), QuizQuestionId = q10.Id, Text = "this", IsCorrect = true, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222262-2222-2222-2222-222222222222"), QuizQuestionId = q10.Id, Text = "self", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222263-2222-2222-2222-222222222222"), QuizQuestionId = q10.Id, Text = "current", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false },
                    new QuizAnswerOption { Id = Guid.Parse("aa222264-2222-2222-2222-222222222222"), QuizQuestionId = q10.Id, Text = "object", IsCorrect = false, CreatedAt = DateTime.UtcNow, IsDeleted = false }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}


