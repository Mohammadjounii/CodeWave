using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Data.Seed
{
    public static class WebSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            const string beginnerIdStr     = "33333333-3333-3333-3333-333333333333";
            const string intermediateIdStr = "44444444-4444-4444-4444-444444444444";
            const string advancedIdStr     = "55555555-5555-5555-5555-555555555555";
            var beginnerCourseId = Guid.Parse(beginnerIdStr);

            // ── COURSES ──────────────────────────────────────────────────────────
            // Web Development is a single Beginner-to-Advanced path, same shape as Python/Java —
            // the old 3-tier split is retired here; its lessons/quizzes/progress are re-homed below.
            await context.Database.ExecuteSqlRawAsync($@"
                UPDATE Courses SET IsDeleted = 1 WHERE Id IN (
                    '33334444-4444-4444-4444-444444444444',
                    '33335555-5555-5555-5555-555555555555'
                );

                IF NOT EXISTS (SELECT 1 FROM Courses WHERE Id = '{beginnerIdStr}')
                    INSERT INTO Courses (Id,Title,Description,DifficultyLevel,LearningPath,CreatedAt,IsDeleted,ProgrammingLanguage)
                    VALUES ('{beginnerIdStr}','Web Development Mastery Path: Beginner to Advanced',
                        'A complete web development path from HTML, CSS and JavaScript fundamentals through CSS Grid, animations, ES6+, the Fetch API and async/await, all the way to JS modules, Node.js, React, REST APIs and deployment. Includes 30 lessons and 60 coding exercises.',
                        'Beginner to Advanced','Web Development','2024-01-01',0,2)
                ELSE
                    UPDATE Courses SET Title='Web Development Mastery Path: Beginner to Advanced',
                        DifficultyLevel='Beginner to Advanced',
                        Description='A complete web development path from HTML, CSS and JavaScript fundamentals through CSS Grid, animations, ES6+, the Fetch API and async/await, all the way to JS modules, Node.js, React, REST APIs and deployment. Includes 30 lessons and 60 coding exercises.',
                        LearningPath='Web Development',IsDeleted=0,ProgrammingLanguage=2,CreatedAt='2024-01-01'
                    WHERE Id='{beginnerIdStr}';

                UPDATE Courses SET IsDeleted = 1 WHERE Id IN ('{intermediateIdStr}','{advancedIdStr}');

                UPDATE Quizzes SET CourseId = '{beginnerIdStr}'
                    WHERE CourseId IN ('{intermediateIdStr}','{advancedIdStr}');

                UPDATE UserCourses SET CourseId = '{beginnerIdStr}'
                    WHERE CourseId IN ('{intermediateIdStr}','{advancedIdStr}')
                      AND NOT EXISTS (
                          SELECT 1 FROM UserCourses uc2
                          WHERE uc2.UserId = UserCourses.UserId AND uc2.CourseId = '{beginnerIdStr}'
                      );
            ");

            // ── BEGINNER LESSONS ─────────────────────────────────────────────────
            var beginnerLessons = new List<Lesson>
            {
                new Lesson { Id=Guid.Parse("e1100001-0000-0000-0000-000000000001"), CourseId=beginnerCourseId,
                    Title="Introduction to Web Development", OrderNumber=1,
                    ImageUrl="https://images.unsplash.com/photo-1547658719-da2b51169166?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/ysEN5RaKOlA", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Welcome to Web Development</h2>
  <p>The web is built on three core technologies that work together on every website you visit.</p>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">The Three Pillars</h3>
  <ul class=""list-disc list-inside space-y-2"">
    <li><strong class=""text-cyan-300"">HTML</strong> — Structure: defines content and layout.</li>
    <li><strong class=""text-cyan-300"">CSS</strong> — Style: controls colours, fonts, and spacing.</li>
    <li><strong class=""text-cyan-300"">JavaScript</strong> — Behaviour: makes pages interactive.</li>
  </ul>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Your First HTML File</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>&lt;!DOCTYPE html&gt;
&lt;html lang=""en""&gt;
  &lt;head&gt;&lt;title&gt;My Page&lt;/title&gt;&lt;/head&gt;
  &lt;body&gt;&lt;h1&gt;Hello, World!&lt;/h1&gt;&lt;/body&gt;
&lt;/html&gt;</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("e1100002-0000-0000-0000-000000000002"), CourseId=beginnerCourseId,
                    Title="HTML Basics — Structure and Elements", OrderNumber=2,
                    ImageUrl="https://images.unsplash.com/photo-1621839673705-6617adf9e890?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/UB1O30fR-EE", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">HTML Elements and Tags</h2>
  <p>HTML is made of elements. Most have an opening tag, content, and a closing tag.</p>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>&lt;h1&gt;Main Heading&lt;/h1&gt;
&lt;p&gt;A paragraph.&lt;/p&gt;
&lt;strong&gt;Bold&lt;/strong&gt; &lt;em&gt;Italic&lt;/em&gt;</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Semantic HTML</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>&lt;header&gt;Site header&lt;/header&gt;
&lt;nav&gt;Navigation&lt;/nav&gt;
&lt;main&gt;Main content&lt;/main&gt;
&lt;footer&gt;Site footer&lt;/footer&gt;</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("e1100003-0000-0000-0000-000000000003"), CourseId=beginnerCourseId,
                    Title="HTML Links, Images and Lists", OrderNumber=3,
                    ImageUrl="https://images.unsplash.com/photo-1498050108023-c5249f4df085?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/kX3isnerI_8", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Linking and Media</h2>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Hyperlinks</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>&lt;a href=""https://google.com"" target=""_blank""&gt;Google&lt;/a&gt;
&lt;a href=""about.html""&gt;About Us&lt;/a&gt;</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Images &amp; Lists</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>&lt;img src=""logo.png"" alt=""Logo"" width=""200"" /&gt;
&lt;ul&gt;&lt;li&gt;HTML&lt;/li&gt;&lt;li&gt;CSS&lt;/li&gt;&lt;/ul&gt;
&lt;ol&gt;&lt;li&gt;Learn HTML&lt;/li&gt;&lt;li&gt;Learn CSS&lt;/li&gt;&lt;/ol&gt;</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("e1100004-0000-0000-0000-000000000004"), CourseId=beginnerCourseId,
                    Title="HTML Forms and Input Elements", OrderNumber=4,
                    ImageUrl="https://images.unsplash.com/photo-1516321318423-f06f85e504b3?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/fNcJuPIZ2WE", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Collecting User Input</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>&lt;form action=""/submit"" method=""POST""&gt;
  &lt;label for=""name""&gt;Name:&lt;/label&gt;
  &lt;input type=""text"" id=""name"" name=""name"" required /&gt;
  &lt;input type=""email"" name=""email"" /&gt;
  &lt;textarea rows=""4"" name=""msg""&gt;&lt;/textarea&gt;
  &lt;button type=""submit""&gt;Submit&lt;/button&gt;
&lt;/form&gt;</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("e1100005-0000-0000-0000-000000000005"), CourseId=beginnerCourseId,
                    Title="CSS Basics — Selectors and Properties", OrderNumber=5,
                    ImageUrl="https://images.unsplash.com/photo-1523437113738-bbd3cc89fb19?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/yfoY53QXEnI", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Styling Your Pages</h2>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">CSS Selectors</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>p      { color: white; }       /* element */
.card  { background: #1e1e2e; } /* class   */
#hero  { font-size: 2rem; }     /* id      */
a:hover{ color: cyan; }         /* pseudo  */</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Common Properties</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>h1 { color: #22d3ee; font-size: 2rem;
     margin: 16px; padding: 12px 24px;
     border-radius: 8px; }</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("e1100006-0000-0000-0000-000000000006"), CourseId=beginnerCourseId,
                    Title="CSS Box Model and Layout", OrderNumber=6,
                    ImageUrl="https://images.unsplash.com/photo-1555066931-4365d14bab8c?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/rIO5326FgPE", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">The Box Model</h2>
  <p>Every element is a box: content → padding → border → margin.</p>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>div { width:300px; padding:16px;
      border:2px solid cyan;
      margin:24px; box-sizing:border-box; }</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Display &amp; Position</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>.box  { display: block; }   /* full width */
.tag  { display: inline; }  /* flows with text */
.nav  { position: fixed; top:0; width:100%; }</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("e1100007-0000-0000-0000-000000000007"), CourseId=beginnerCourseId,
                    Title="CSS Flexbox and Responsive Design", OrderNumber=7,
                    ImageUrl="https://images.unsplash.com/photo-1517694712202-14dd9538aa97?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/fYq5PXgSsbE", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Flexbox Layouts</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>.container { display:flex; gap:16px;
  justify-content:center; align-items:center;
  flex-wrap:wrap; }</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Media Queries</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>.container { flex-direction:column; }
@media (min-width:768px) {
  .container { flex-direction:row; }
}</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("e1100008-0000-0000-0000-000000000008"), CourseId=beginnerCourseId,
                    Title="JavaScript Basics — Variables and Data Types", OrderNumber=8,
                    ImageUrl="https://images.unsplash.com/photo-1579468118864-1b9ea3c0db4a?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/hdI2bqOjy3c", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">JavaScript: Language of the Web</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>let age = 25;          // reassignable
const name = ""Alice""; // constant
let user = { name:""Alice"", age:25 };
let items = [1,2,3];</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Template Literals</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>console.log(`Welcome ${name}, age ${age}!`);</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("e1100009-0000-0000-0000-000000000009"), CourseId=beginnerCourseId,
                    Title="JavaScript Functions and Control Flow", OrderNumber=9,
                    ImageUrl="https://images.unsplash.com/photo-1593720219276-0b1eacd0aef4?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/jS4aFq5-91M", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Functions and Decisions</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>function greet(name) { return `Hello, ${name}!`; }
const greet = name => `Hello, ${name}!`;</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">If / Loops</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>if (score >= 90) console.log(""A"");
else if (score >= 80) console.log(""B"");
else console.log(""C"");
for (let i=1; i<=5; i++) console.log(i);</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("e1100010-0000-0000-0000-000000000010"), CourseId=beginnerCourseId,
                    Title="JavaScript DOM Manipulation and Events", OrderNumber=10,
                    ImageUrl="https://images.unsplash.com/photo-1461749280684-dccba630e2f6?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/y17RuWkWdn8", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Making Pages Interactive</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const title = document.getElementById(""title"");
const btn   = document.querySelector("".btn"");
title.textContent = ""New Title"";
title.style.color = ""cyan"";
title.classList.toggle(""active"");</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Event Listeners</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>btn.addEventListener(""click"", () => {
  console.log(""clicked!"");
});</code></pre>
</div>" }
            };

            // ── INTERMEDIATE LESSONS ─────────────────────────────────────────────
            var intermediateLessons = new List<Lesson>
            {
                new Lesson { Id=Guid.Parse("a1200000-0000-0000-0000-000000000001"), CourseId=beginnerCourseId,
                    Title="Advanced HTML5 and Semantic Elements", OrderNumber=11,
                    ImageUrl="https://images.unsplash.com/photo-1542831371-29b0f74f9713?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/kGW8Al_cga4", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">HTML5 Semantic & Accessibility</h2>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Rich Semantic Elements</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>&lt;article&gt;A self-contained piece of content&lt;/article&gt;
&lt;aside&gt;Related sidebar content&lt;/aside&gt;
&lt;figure&gt;
  &lt;img src=""chart.png"" alt=""Sales chart"" /&gt;
  &lt;figcaption&gt;Q3 Sales&lt;/figcaption&gt;
&lt;/figure&gt;
&lt;details&gt;&lt;summary&gt;More info&lt;/summary&gt;Hidden text&lt;/details&gt;</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">ARIA &amp; Data Attributes</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>&lt;button aria-label=""Close menu"" aria-expanded=""false""&gt;X&lt;/button&gt;
&lt;div role=""alert""&gt;Error: field required&lt;/div&gt;
&lt;li data-id=""42"" data-category=""web""&gt;Item&lt;/li&gt;</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("a1200000-0000-0000-0000-000000000002"), CourseId=beginnerCourseId,
                    Title="CSS Grid Layout", OrderNumber=12,
                    ImageUrl="https://images.unsplash.com/photo-1507238691740-187a5b1d37b8?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/jV8B24rSN5o", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">CSS Grid — Two-Dimensional Layouts</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>.grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  grid-template-rows: auto;
  gap: 24px;
}</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Named Grid Areas</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>.layout {
  display: grid;
  grid-template-areas:
    ""header header""
    ""sidebar main""
    ""footer footer"";
}
header { grid-area: header; }
aside  { grid-area: sidebar; }
main   { grid-area: main; }</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Spanning Items</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>.hero { grid-column: 1 / -1; } /* full width */
.tall { grid-row: span 2; }</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("a1200000-0000-0000-0000-000000000003"), CourseId=beginnerCourseId,
                    Title="CSS Animations and Transitions", OrderNumber=13,
                    ImageUrl="https://images.unsplash.com/photo-1550745165-9bc0b252726f?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/zHUpx90NerM", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Motion and Animation</h2>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Transitions</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>.btn {
  background: #9b59f5;
  transition: background 0.3s ease, transform 0.2s;
}
.btn:hover { background: #b07eff; transform: scale(1.05); }</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">@keyframes</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>@keyframes bounce {
  0%, 100% { transform: translateY(0); }
  50%       { transform: translateY(-20px); }
}
.ball { animation: bounce 1s ease infinite; }</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Transform</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>.box { transform: rotate(45deg) scale(1.2) translateX(20px); }</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("a1200000-0000-0000-0000-000000000004"), CourseId=beginnerCourseId,
                    Title="Responsive Design with Media Queries", OrderNumber=14,
                    ImageUrl="https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/srvUrASNj0s", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Responsive Web Design</h2>
  <p>Mobile-first: write styles for small screens first, then add breakpoints for larger ones.</p>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Viewport Meta Tag</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>&lt;meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" /&gt;</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Breakpoints</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>/* Mobile default */
.card { width: 100%; font-size: 1rem; }

@media (min-width: 768px)  { .card { width: 50%; } }   /* tablet */
@media (min-width: 1024px) { .card { width: 33%; } }   /* desktop */</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Fluid Units</h3>
  <ul class=""list-disc list-inside space-y-1"">
    <li><strong class=""text-cyan-300"">rem</strong> — relative to root font size (16px)</li>
    <li><strong class=""text-cyan-300"">vw / vh</strong> — percentage of viewport width/height</li>
    <li><strong class=""text-cyan-300"">%</strong> — relative to parent element</li>
  </ul>
</div>" },

                new Lesson { Id=Guid.Parse("a1200000-0000-0000-0000-000000000005"), CourseId=beginnerCourseId,
                    Title="JavaScript ES6+ Modern Features", OrderNumber=15,
                    ImageUrl="https://images.unsplash.com/photo-1579468118864-1b9ea3c0db4a?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/nZ1DMMsyVyI", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Modern JavaScript (ES6+)</h2>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Destructuring</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const { name, age } = user;
const { name: fullName } = user; // rename
const [first, second] = [10, 20];</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Spread &amp; Rest</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const merged = [...arr1, ...arr2];
const updated = { ...user, age: 26 };
function sum(...nums) { return nums.reduce((a,b)=>a+b,0); }</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Optional Chaining &amp; Nullish</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const city = user?.address?.city ?? ""Unknown"";</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("a1200000-0000-0000-0000-000000000006"), CourseId=beginnerCourseId,
                    Title="Arrays — map, filter, reduce", OrderNumber=16,
                    ImageUrl="https://images.unsplash.com/photo-1461749280684-dccba630e2f6?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/R8rmfD9Y5-c", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Functional Array Methods</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const nums = [1, 2, 3, 4, 5];

const doubled  = nums.map(n => n * 2);        // [2,4,6,8,10]
const evens    = nums.filter(n => n % 2 === 0); // [2,4]
const total    = nums.reduce((acc,n) => acc+n, 0); // 15</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Search &amp; Check</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const found = nums.find(n => n > 3);   // 4
const hasNeg = nums.some(n => n < 0);  // false
const allPos = nums.every(n => n > 0); // true
const idx    = nums.indexOf(3);        // 2</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("a1200000-0000-0000-0000-000000000007"), CourseId=beginnerCourseId,
                    Title="JavaScript Objects and JSON", OrderNumber=17,
                    ImageUrl="https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/wDWW7fVyKnM", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Objects and JSON</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const user = { name: ""Alice"", age: 25, active: true };

Object.keys(user);    // [""name"",""age"",""active""]
Object.values(user);  // [""Alice"",25,true]
Object.entries(user); // [[""name"",""Alice""],[""age"",25],...]</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">JSON</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const json = JSON.stringify(user);
// '{""name"":""Alice"",""age"":25,""active"":true}'
const parsed = JSON.parse(json);
// back to a JS object</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("a1200000-0000-0000-0000-000000000008"), CourseId=beginnerCourseId,
                    Title="The Fetch API and Promises", OrderNumber=18,
                    ImageUrl="https://images.unsplash.com/photo-1558494949-ef010cbdcc31?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/cuEtnrL9-H0", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Fetching Data from APIs</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>fetch(""https://api.example.com/users"")
  .then(res => res.json())
  .then(data => console.log(data))
  .catch(err => console.error(""Error:"", err));</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">POST Request</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>fetch(""/api/users"", {
  method: ""POST"",
  headers: { ""Content-Type"": ""application/json"" },
  body: JSON.stringify({ name: ""Alice"" })
}).then(res => res.json()).then(console.log);</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("a1200000-0000-0000-0000-000000000009"), CourseId=beginnerCourseId,
                    Title="Async/Await and Error Handling", OrderNumber=19,
                    ImageUrl="https://images.unsplash.com/photo-1593720219276-0b1eacd0aef4?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/V_Kr9OSfDeU", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Async/Await — Cleaner Async Code</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>async function getUser(id) {
  try {
    const res  = await fetch(`/api/users/${id}`);
    const data = await res.json();
    return data;
  } catch (err) {
    console.error(""Failed:"", err);
  } finally {
    console.log(""Done"");
  }
}</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Promise.all</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const [users, posts] = await Promise.all([
  fetch(""/api/users"").then(r=>r.json()),
  fetch(""/api/posts"").then(r=>r.json())
]);</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("a1200000-0000-0000-0000-000000000010"), CourseId=beginnerCourseId,
                    Title="Browser Storage and Web APIs", OrderNumber=20,
                    ImageUrl="https://images.unsplash.com/photo-1526374965328-7f61d4dc18c5?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/AUOzvFzdIk4", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Storing Data in the Browser</h2>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">localStorage</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>localStorage.setItem(""user"", JSON.stringify({ name:""Alice"" }));
const user = JSON.parse(localStorage.getItem(""user""));
localStorage.removeItem(""user"");
localStorage.clear();</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">sessionStorage &amp; Geolocation</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>sessionStorage.setItem(""token"", ""abc123""); // clears on tab close

navigator.geolocation.getCurrentPosition(pos => {
  console.log(pos.coords.latitude, pos.coords.longitude);
});</code></pre>
</div>" }
            };

            // ── ADVANCED LESSONS ─────────────────────────────────────────────────
            var advancedLessons = new List<Lesson>
            {
                new Lesson { Id=Guid.Parse("b1300000-0000-0000-0000-000000000001"), CourseId=beginnerCourseId,
                    Title="JavaScript Modules (ESM)", OrderNumber=21,
                    ImageUrl="https://images.unsplash.com/photo-1581472723648-909f4851d4ae?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/cRHQNNcYf6s", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">JavaScript Modules</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>// math.js — named exports
export const add = (a,b) => a+b;
export const PI  = 3.14159;

// main.js — import
import { add, PI } from ""./math.js"";
console.log(add(2,3)); // 5</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Default Export</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>// logger.js
export default function log(msg) { console.log(`[LOG] ${msg}`); }

// main.js
import log from ""./logger.js"";
log(""App started"");</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Dynamic Import</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const { add } = await import(""./math.js""); // lazy load</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("b1300000-0000-0000-0000-000000000002"), CourseId=beginnerCourseId,
                    Title="Introduction to Node.js", OrderNumber=22,
                    ImageUrl="https://images.unsplash.com/photo-1558494949-ef010cbdcc31?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/fBNz5xF-Kx4", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Node.js — JavaScript on the Server</h2>
  <p>Node.js runs JavaScript outside the browser using Chrome's V8 engine. It's event-driven and non-blocking.</p>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Built-in Modules</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const fs   = require(""fs"");
const path = require(""path"");
const http = require(""http"");

const content = fs.readFileSync(""data.txt"", ""utf8"");</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Basic HTTP Server</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const server = http.createServer((req, res) => {
  res.writeHead(200, { ""Content-Type"": ""text/plain"" });
  res.end(""Hello from Node!"");
});
server.listen(3000, () => console.log(""Listening on 3000""));</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("b1300000-0000-0000-0000-000000000003"), CourseId=beginnerCourseId,
                    Title="NPM and Package Management", OrderNumber=23,
                    ImageUrl="https://images.unsplash.com/photo-1614741118887-7a4ee193a5fa?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/jHDhaSSKmB0", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">NPM — Node Package Manager</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>npm init -y            # create package.json
npm install axios      # runtime dependency
npm install -D vite    # dev-only dependency
npm uninstall axios
npm run build          # run a script</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">package.json Scripts</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>{
  ""scripts"": {
    ""dev"":   ""vite"",
    ""build"": ""vite build"",
    ""lint"":  ""eslint src""
  },
  ""dependencies"":    { ""axios"": ""^1.6.0"" },
  ""devDependencies"": { ""vite"":  ""^5.0.0"" }
}</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("b1300000-0000-0000-0000-000000000004"), CourseId=beginnerCourseId,
                    Title="Introduction to React", OrderNumber=24,
                    ImageUrl="https://images.unsplash.com/photo-1633356122544-f134324a6cee?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/w7ejDZ8SWv8", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">React — UI Library by Meta</h2>
  <p>React builds UIs using components — reusable pieces of HTML + logic written in JSX.</p>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">JSX Syntax</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>const element = &lt;h1 className=""title""&gt;Hello React!&lt;/h1&gt;;
// JSX compiles to: React.createElement(""h1"", {className:""title""}, ""Hello React!"")</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Your First Component</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>function App() {
  return (
    &lt;div&gt;
      &lt;h1&gt;Hello World&lt;/h1&gt;
      &lt;p&gt;Welcome to React&lt;/p&gt;
    &lt;/div&gt;
  );
}
export default App;</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("b1300000-0000-0000-0000-000000000005"), CourseId=beginnerCourseId,
                    Title="React Components, Props and Composition", OrderNumber=25,
                    ImageUrl="https://images.unsplash.com/photo-1555949963-aa79dcee981c?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/Ke90Tje7VS0", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Components and Props</h2>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>function Card({ title, description, imageUrl }) {
  return (
    &lt;div className=""card""&gt;
      &lt;img src={imageUrl} alt={title} /&gt;
      &lt;h2&gt;{title}&lt;/h2&gt;
      &lt;p&gt;{description}&lt;/p&gt;
    &lt;/div&gt;
  );
}

// Usage:
&lt;Card title=""React"" description=""A UI library"" imageUrl=""/react.png"" /&gt;</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Children Prop</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>function Layout({ children }) {
  return &lt;main className=""layout""&gt;{children}&lt;/main&gt;;
}</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("b1300000-0000-0000-0000-000000000006"), CourseId=beginnerCourseId,
                    Title="React State and Hooks", OrderNumber=26,
                    ImageUrl="https://images.unsplash.com/photo-1504639725590-34d0984388bd?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/TNhaISOUy6Q", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">State with Hooks</h2>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">useState</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>import { useState } from ""react"";

function Counter() {
  const [count, setCount] = useState(0);
  return (
    &lt;button onClick={() => setCount(count + 1)}&gt;
      Count: {count}
    &lt;/button&gt;
  );
}</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">useEffect</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>import { useEffect } from ""react"";

useEffect(() => {
  document.title = `Count: ${count}`;
}, [count]); // runs when count changes</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("b1300000-0000-0000-0000-000000000007"), CourseId=beginnerCourseId,
                    Title="REST API Design and Integration", OrderNumber=27,
                    ImageUrl="https://images.unsplash.com/photo-1451187580459-43490279c0fa?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/fgTGADljAeg", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">REST APIs</h2>
  <ul class=""list-disc list-inside space-y-1"">
    <li><strong class=""text-cyan-300"">GET</strong> — retrieve data</li>
    <li><strong class=""text-cyan-300"">POST</strong> — create new resource</li>
    <li><strong class=""text-cyan-300"">PUT/PATCH</strong> — update resource</li>
    <li><strong class=""text-cyan-300"">DELETE</strong> — remove resource</li>
  </ul>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Full CRUD Example</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>// GET
const users = await fetch(""/api/users"").then(r=>r.json());

// POST
await fetch(""/api/users"", {
  method:""POST"",
  headers:{""Content-Type"":""application/json""},
  body: JSON.stringify({ name:""Alice"" })
});

// DELETE
await fetch(""/api/users/42"", { method:""DELETE"" });</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("b1300000-0000-0000-0000-000000000008"), CourseId=beginnerCourseId,
                    Title="Authentication — JWT and Sessions", OrderNumber=28,
                    ImageUrl="https://images.unsplash.com/photo-1555066931-4365d14bab8c?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/7nafaH9SddU", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Authentication with JWT</h2>
  <p>A JWT (JSON Web Token) is a compact, self-contained token: <code class=""text-cyan-300"">header.payload.signature</code></p>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Login Flow</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>// 1. User logs in
const res = await fetch(""/api/login"", {
  method:""POST"", body: JSON.stringify({ email, password })
});
const { token } = await res.json();

// 2. Store token
localStorage.setItem(""token"", token);

// 3. Send on protected requests
fetch(""/api/profile"", {
  headers: { Authorization: `Bearer ${token}` }
});</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Best Practice</h3>
  <p class=""text-slate-300"">Store tokens in httpOnly cookies (not localStorage) to protect against XSS attacks.</p>
</div>" },

                new Lesson { Id=Guid.Parse("b1300000-0000-0000-0000-000000000009"), CourseId=beginnerCourseId,
                    Title="Web Performance Optimization", OrderNumber=29,
                    ImageUrl="https://images.unsplash.com/photo-1460925895917-afdab827c52f?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/AQwgX2l-CpE", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Making Sites Fast</h2>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Core Web Vitals</h3>
  <ul class=""list-disc list-inside space-y-1"">
    <li><strong class=""text-cyan-300"">LCP</strong> — Largest Contentful Paint (loading speed)</li>
    <li><strong class=""text-cyan-300"">FID</strong> — First Input Delay (interactivity)</li>
    <li><strong class=""text-cyan-300"">CLS</strong> — Cumulative Layout Shift (visual stability)</li>
  </ul>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Key Techniques</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>// Lazy load images
&lt;img src=""photo.jpg"" loading=""lazy"" alt=""..."" /&gt;

// Memoize expensive function
function memoize(fn) {
  const cache = new Map();
  return (x) => cache.has(x) ? cache.get(x) : cache.set(x,fn(x)).get(x);
}

// Code splitting (React)
const Dashboard = React.lazy(() => import(""./Dashboard""));</code></pre>
</div>" },

                new Lesson { Id=Guid.Parse("b1300000-0000-0000-0000-000000000010"), CourseId=beginnerCourseId,
                    Title="Deploying Web Applications", OrderNumber=30,
                    ImageUrl="https://images.unsplash.com/photo-1451187580459-43490279c0fa?auto=format&fit=crop&w=800&q=80",
                    VideoUrl="https://www.youtube.com/embed/l134cBAJCuc", CreatedAt=DateTime.UtcNow,
                    Content=@"<div class=""lesson-html space-y-4 leading-relaxed text-slate-100 text-sm sm:text-base"">
  <h2 class=""text-2xl font-bold mb-3 text-white"">Shipping to Production</h2>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Build Process</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code>npm run build   # creates /dist folder — minified, optimised</code></pre>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Hosting Options</h3>
  <ul class=""list-disc list-inside space-y-1"">
    <li><strong class=""text-cyan-300"">Vercel</strong> — push to GitHub, auto-deploys</li>
    <li><strong class=""text-cyan-300"">Netlify</strong> — drag-and-drop or Git integration</li>
    <li><strong class=""text-cyan-300"">GitHub Pages</strong> — free for static sites</li>
  </ul>
  <h3 class=""text-lg font-semibold mt-4 mb-2 text-slate-100"">Environment Variables</h3>
  <pre class=""bg-slate-900/80 border border-slate-700 rounded-xl p-4 text-xs overflow-x-auto""><code># .env
VITE_API_URL=https://api.example.com

// In code
const api = import.meta.env.VITE_API_URL;</code></pre>
</div>" }
            };

            // ── UPSERT ALL LESSONS ───────────────────────────────────────────────
            var allLessons = new List<Lesson>();
            allLessons.AddRange(beginnerLessons);
            allLessons.AddRange(intermediateLessons);
            allLessons.AddRange(advancedLessons);

            foreach (var lesson in allLessons)
            {
                var existing = await context.Lessons.FirstOrDefaultAsync(l => l.Id == lesson.Id);
                if (existing == null)
                    await context.Lessons.AddAsync(lesson);
                else
                {
                    existing.Title       = lesson.Title;
                    existing.Content     = lesson.Content;
                    existing.OrderNumber = lesson.OrderNumber;
                    existing.CourseId    = lesson.CourseId;
                    existing.ImageUrl    = lesson.ImageUrl;
                    existing.VideoUrl    = lesson.VideoUrl;
                    existing.isDeleted   = false;
                }
            }
            await context.SaveChangesAsync();

            // ── EXERCISES ────────────────────────────────────────────────────────
            var exercises = new List<CodingExercise>
            {
                // ── BEGINNER ──────────────────────────────────────────────────────
                new CodingExercise { Id=Guid.Parse("e2200001-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("e1100001-0000-0000-0000-000000000001"),
                    Title="The Three Pillars",
                    Description="Print the three pillars of web development, one per line:\nHTML\nCSS\nJavaScript",
                    StarterCode="// Print HTML, CSS, JavaScript each on its own line\n",
                    SolutionCode="console.log(\"HTML\");\nconsole.log(\"CSS\");\nconsole.log(\"JavaScript\");\n",
                    ExpectedOutput="HTML\nCSS\nJavaScript" },
                new CodingExercise { Id=Guid.Parse("e2200001-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("e1100001-0000-0000-0000-000000000001"),
                    Title="Web Welcome Message",
                    Description="Declare siteName = \"CodeWave\" and print:\nWelcome to CodeWave — powered by HTML, CSS and JavaScript!",
                    StarterCode="const siteName = \"CodeWave\";\n// Use a template literal\n",
                    SolutionCode="const siteName = \"CodeWave\";\nconsole.log(`Welcome to ${siteName} — powered by HTML, CSS and JavaScript!`);\n",
                    ExpectedOutput="Welcome to CodeWave — powered by HTML, CSS and JavaScript!" },

                new CodingExercise { Id=Guid.Parse("e2200002-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("e1100002-0000-0000-0000-000000000002"),
                    Title="Build an HTML Tag",
                    Description="Write wrapTag(content, tag) that returns <tag>content</tag>.\nPrint wrapTag(\"Hello World\", \"h1\")",
                    StarterCode="function wrapTag(content, tag) {\n  // Return <tag>content</tag>\n}\nconsole.log(wrapTag(\"Hello World\", \"h1\"));\n",
                    SolutionCode="function wrapTag(content, tag) {\n  return `<${tag}>${content}</${tag}>`;\n}\nconsole.log(wrapTag(\"Hello World\", \"h1\"));\n",
                    ExpectedOutput="<h1>Hello World</h1>" },
                new CodingExercise { Id=Guid.Parse("e2200002-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("e1100002-0000-0000-0000-000000000002"),
                    Title="Semantic Tags List",
                    Description="Print each tag in [\"header\",\"nav\",\"main\",\"footer\"] wrapped in < >:\n<header>\n<nav>\n<main>\n<footer>",
                    StarterCode="const tags = [\"header\", \"nav\", \"main\", \"footer\"];\n// Loop and print each as <tagname>\n",
                    SolutionCode="const tags = [\"header\", \"nav\", \"main\", \"footer\"];\ntags.forEach(tag => console.log(`<${tag}>`));\n",
                    ExpectedOutput="<header>\n<nav>\n<main>\n<footer>" },

                new CodingExercise { Id=Guid.Parse("e2200003-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("e1100003-0000-0000-0000-000000000003"),
                    Title="Print a List",
                    Description="Print each item in [\"HTML\",\"CSS\",\"JavaScript\"] prefixed with '- ':\n- HTML\n- CSS\n- JavaScript",
                    StarterCode="const languages = [\"HTML\", \"CSS\", \"JavaScript\"];\n// Print each prefixed with '- '\n",
                    SolutionCode="const languages = [\"HTML\", \"CSS\", \"JavaScript\"];\nlanguages.forEach(lang => console.log(`- ${lang}`));\n",
                    ExpectedOutput="- HTML\n- CSS\n- JavaScript" },
                new CodingExercise { Id=Guid.Parse("e2200003-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("e1100003-0000-0000-0000-000000000003"),
                    Title="Build an Anchor Tag",
                    Description="Write buildLink(text, url) returning <a href=\"url\">text</a>.\nPrint buildLink(\"CodeWave\", \"https://codewave.com\")",
                    StarterCode="function buildLink(text, url) {\n  // Return <a href=\"url\">text</a>\n}\nconsole.log(buildLink(\"CodeWave\", \"https://codewave.com\"));\n",
                    SolutionCode="function buildLink(text, url) {\n  return `<a href=\"${url}\">${text}</a>`;\n}\nconsole.log(buildLink(\"CodeWave\", \"https://codewave.com\"));\n",
                    ExpectedOutput="<a href=\"https://codewave.com\">CodeWave</a>" },

                new CodingExercise { Id=Guid.Parse("e2200004-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("e1100004-0000-0000-0000-000000000004"),
                    Title="Form Data Object",
                    Description="Create formData with name:\"Alice\" and email:\"alice@example.com\".\nPrint: Name: Alice, Email: alice@example.com",
                    StarterCode="const formData = { name: \"Alice\", email: \"alice@example.com\" };\n// Print: Name: Alice, Email: alice@example.com\n",
                    SolutionCode="const formData = { name: \"Alice\", email: \"alice@example.com\" };\nconsole.log(`Name: ${formData.name}, Email: ${formData.email}`);\n",
                    ExpectedOutput="Name: Alice, Email: alice@example.com" },
                new CodingExercise { Id=Guid.Parse("e2200004-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("e1100004-0000-0000-0000-000000000004"),
                    Title="Email Validator",
                    Description="Write isValidEmail(email) returning true if it contains '@'.\nPrint results for \"user@test.com\" then \"notanemail\":\ntrue\nfalse",
                    StarterCode="function isValidEmail(email) {\n  // Return true if email contains '@'\n}\nconsole.log(isValidEmail(\"user@test.com\"));\nconsole.log(isValidEmail(\"notanemail\"));\n",
                    SolutionCode="function isValidEmail(email) {\n  return email.includes('@');\n}\nconsole.log(isValidEmail(\"user@test.com\"));\nconsole.log(isValidEmail(\"notanemail\"));\n",
                    ExpectedOutput="true\nfalse" },

                new CodingExercise { Id=Guid.Parse("e2200005-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("e1100005-0000-0000-0000-000000000005"),
                    Title="CSS Rule Builder",
                    Description="Write cssRule(selector, property, value) returning: selector { property: value; }\nPrint cssRule(\"h1\", \"font-size\", \"2rem\")",
                    StarterCode="function cssRule(selector, property, value) {\n  // Return: selector { property: value; }\n}\nconsole.log(cssRule(\"h1\", \"font-size\", \"2rem\"));\n",
                    SolutionCode="function cssRule(selector, property, value) {\n  return `${selector} { ${property}: ${value}; }`;\n}\nconsole.log(cssRule(\"h1\", \"font-size\", \"2rem\"));\n",
                    ExpectedOutput="h1 { font-size: 2rem; }" },
                new CodingExercise { Id=Guid.Parse("e2200005-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("e1100005-0000-0000-0000-000000000005"),
                    Title="CSS Selector Types",
                    Description="Print the three CSS selector types:\nElement: p\nClass: .card\nID: #header",
                    StarterCode="// Print the three CSS selector types\n",
                    SolutionCode="console.log(\"Element: p\");\nconsole.log(\"Class: .card\");\nconsole.log(\"ID: #header\");\n",
                    ExpectedOutput="Element: p\nClass: .card\nID: #header" },

                new CodingExercise { Id=Guid.Parse("e2200006-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("e1100006-0000-0000-0000-000000000006"),
                    Title="Box Model Calculator",
                    Description="content=200, paddingEach=20, borderEach=2.\nTotal = content + paddingEach*2 + borderEach*2.\nPrint: Total width: 244px",
                    StarterCode="const content = 200, paddingEach = 20, borderEach = 2;\n// Calculate and print: Total width: Xpx\n",
                    SolutionCode="const content = 200, paddingEach = 20, borderEach = 2;\nconst total = content + paddingEach * 2 + borderEach * 2;\nconsole.log(`Total width: ${total}px`);\n",
                    ExpectedOutput="Total width: 244px" },
                new CodingExercise { Id=Guid.Parse("e2200006-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("e1100006-0000-0000-0000-000000000006"),
                    Title="border-box Content Width",
                    Description="width=300, paddingEach=20. Content area = width - paddingEach*2.\nPrint: Content area: 260px",
                    StarterCode="const width = 300, paddingEach = 20;\n// Print: Content area: Xpx\n",
                    SolutionCode="const width = 300, paddingEach = 20;\nconst contentArea = width - paddingEach * 2;\nconsole.log(`Content area: ${contentArea}px`);\n",
                    ExpectedOutput="Content area: 260px" },

                new CodingExercise { Id=Guid.Parse("e2200007-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("e1100007-0000-0000-0000-000000000007"),
                    Title="Flex Items Total Width",
                    Description="Sum widths [100, 200, 150] and print: Total flex width: 450",
                    StarterCode="const itemWidths = [100, 200, 150];\n// Sum and print: Total flex width: X\n",
                    SolutionCode="const itemWidths = [100, 200, 150];\nconst total = itemWidths.reduce((a, b) => a + b, 0);\nconsole.log(`Total flex width: ${total}`);\n",
                    ExpectedOutput="Total flex width: 450" },
                new CodingExercise { Id=Guid.Parse("e2200007-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("e1100007-0000-0000-0000-000000000007"),
                    Title="Responsive Breakpoint",
                    Description="Write getLayout(width): \"mobile\" <768, \"tablet\" <1024, else \"desktop\".\nPrint getLayout(480) and getLayout(1200):\nmobile\ndesktop",
                    StarterCode="function getLayout(width) {\n  // mobile / tablet / desktop\n}\nconsole.log(getLayout(480));\nconsole.log(getLayout(1200));\n",
                    SolutionCode="function getLayout(width) {\n  if (width < 768) return \"mobile\";\n  if (width < 1024) return \"tablet\";\n  return \"desktop\";\n}\nconsole.log(getLayout(480));\nconsole.log(getLayout(1200));\n",
                    ExpectedOutput="mobile\ndesktop" },

                new CodingExercise { Id=Guid.Parse("e2200008-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("e1100008-0000-0000-0000-000000000008"),
                    Title="Hello, Web!",
                    Description="Print: Hello, Web!",
                    StarterCode="// Print Hello, Web!\nconsole.log(\"\");\n",
                    SolutionCode="console.log(\"Hello, Web!\");\n",
                    ExpectedOutput="Hello, Web!" },
                new CodingExercise { Id=Guid.Parse("e2200008-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("e1100008-0000-0000-0000-000000000008"),
                    Title="Template Literal",
                    Description="Use a template literal to print: Alice is 25 years old.",
                    StarterCode="const name = \"Alice\";\nconst age = 25;\nconsole.log(\"\");\n",
                    SolutionCode="const name = \"Alice\";\nconst age = 25;\nconsole.log(`${name} is ${age} years old.`);\n",
                    ExpectedOutput="Alice is 25 years old." },

                new CodingExercise { Id=Guid.Parse("e2200009-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("e1100009-0000-0000-0000-000000000009"),
                    Title="Greeting Function",
                    Description="Write greet(name) returning 'Hello, [name]!'. Print greet('World').",
                    StarterCode="function greet(name) {\n  // Return Hello, [name]!\n}\nconsole.log(greet(\"World\"));\n",
                    SolutionCode="function greet(name) {\n  return `Hello, ${name}!`;\n}\nconsole.log(greet(\"World\"));\n",
                    ExpectedOutput="Hello, World!" },
                new CodingExercise { Id=Guid.Parse("e2200009-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("e1100009-0000-0000-0000-000000000009"),
                    Title="Sum with Loop",
                    Description="Sum 1 to 10 with a for loop. Print: Sum: 55",
                    StarterCode="let total = 0;\nfor (let i = 1; i <= 10; i++) { total += i; }\nconsole.log(\"Sum: \" + total);\n",
                    SolutionCode="let total = 0;\nfor (let i = 1; i <= 10; i++) { total += i; }\nconsole.log(\"Sum: \" + total);\n",
                    ExpectedOutput="Sum: 55" },

                new CodingExercise { Id=Guid.Parse("e2200010-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("e1100010-0000-0000-0000-000000000010"),
                    Title="Grade Checker",
                    Description="Write getGrade(score): 'A' >=90, 'B' >=80, 'C' >=70, 'F' otherwise.\nPrint getGrade(85).",
                    StarterCode="function getGrade(score) {\n  // if/else\n}\nconsole.log(getGrade(85));\n",
                    SolutionCode="function getGrade(score) {\n  if (score >= 90) return \"A\";\n  if (score >= 80) return \"B\";\n  if (score >= 70) return \"C\";\n  return \"F\";\n}\nconsole.log(getGrade(85));\n",
                    ExpectedOutput="B" },
                new CodingExercise { Id=Guid.Parse("e2200010-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("e1100010-0000-0000-0000-000000000010"),
                    Title="DOM Event Simulator",
                    Description="Write handleClick(buttonId) that prints: Button clicked: [buttonId]\nPrint handleClick(\"submitBtn\")",
                    StarterCode="function handleClick(buttonId) {\n  // Print: Button clicked: [buttonId]\n}\nhandleClick(\"submitBtn\");\n",
                    SolutionCode="function handleClick(buttonId) {\n  console.log(`Button clicked: ${buttonId}`);\n}\nhandleClick(\"submitBtn\");\n",
                    ExpectedOutput="Button clicked: submitBtn" },

                // ── INTERMEDIATE ──────────────────────────────────────────────────
                new CodingExercise { Id=Guid.Parse("a2200001-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000001"),
                    Title="Data Attribute Reader",
                    Description="Given an object el = { dataset: { id: \"42\", category: \"web\" } }, print:\nid: 42\ncategory: web",
                    StarterCode="const el = { dataset: { id: \"42\", category: \"web\" } };\n// Print each data attribute\n",
                    SolutionCode="const el = { dataset: { id: \"42\", category: \"web\" } };\nObject.entries(el.dataset).forEach(([k, v]) => console.log(`${k}: ${v}`));\n",
                    ExpectedOutput="id: 42\ncategory: web" },
                new CodingExercise { Id=Guid.Parse("a2200001-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000001"),
                    Title="ARIA Label Builder",
                    Description="Write ariaButton(label, expanded) returning: <button aria-label=\"[label]\" aria-expanded=\"[expanded]\">Menu</button>\nPrint ariaButton(\"Close menu\", \"false\")",
                    StarterCode="function ariaButton(label, expanded) {\n  // Build the button string\n}\nconsole.log(ariaButton(\"Close menu\", \"false\"));\n",
                    SolutionCode="function ariaButton(label, expanded) {\n  return `<button aria-label=\"${label}\" aria-expanded=\"${expanded}\">Menu</button>`;\n}\nconsole.log(ariaButton(\"Close menu\", \"false\"));\n",
                    ExpectedOutput="<button aria-label=\"Close menu\" aria-expanded=\"false\">Menu</button>" },

                new CodingExercise { Id=Guid.Parse("a2200002-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000002"),
                    Title="Grid Columns Calculator",
                    Description="A grid has containerWidth=900 and gap=24. Calculate the width of each column in a 3-column grid:\ncolumnWidth = (containerWidth - gap * 2) / 3\nPrint: Column width: 284px",
                    StarterCode="const containerWidth = 900, gap = 24, cols = 3;\n// Calculate and print: Column width: Xpx\n",
                    SolutionCode="const containerWidth = 900, gap = 24, cols = 3;\nconst columnWidth = (containerWidth - gap * 2) / cols;\nconsole.log(`Column width: ${columnWidth}px`);\n",
                    ExpectedOutput="Column width: 284px" },
                new CodingExercise { Id=Guid.Parse("a2200002-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000002"),
                    Title="Grid Area Names",
                    Description="Store grid area names [\"header\",\"sidebar\",\"main\",\"footer\"] and print each on its own line.",
                    StarterCode="const areas = [\"header\", \"sidebar\", \"main\", \"footer\"];\n// Print each area name\n",
                    SolutionCode="const areas = [\"header\", \"sidebar\", \"main\", \"footer\"];\nareas.forEach(area => console.log(area));\n",
                    ExpectedOutput="header\nsidebar\nmain\nfooter" },

                new CodingExercise { Id=Guid.Parse("a2200003-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000003"),
                    Title="Animation Duration",
                    Description="An animation runs for 1.5 seconds and repeats 3 times.\nTotal duration = duration * iterations.\nPrint: Total animation time: 4.5s",
                    StarterCode="const duration = 1.5, iterations = 3;\n// Print: Total animation time: Xs\n",
                    SolutionCode="const duration = 1.5, iterations = 3;\nconst total = duration * iterations;\nconsole.log(`Total animation time: ${total}s`);\n",
                    ExpectedOutput="Total animation time: 4.5s" },
                new CodingExercise { Id=Guid.Parse("a2200003-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000003"),
                    Title="Transform Steps",
                    Description="Print these CSS transform values one per line:\ntranslateY(0px)\ntranslateY(-20px)\ntranslateY(0px)",
                    StarterCode="const steps = [\"translateY(0px)\", \"translateY(-20px)\", \"translateY(0px)\"];\n// Print each step\n",
                    SolutionCode="const steps = [\"translateY(0px)\", \"translateY(-20px)\", \"translateY(0px)\"];\nsteps.forEach(step => console.log(step));\n",
                    ExpectedOutput="translateY(0px)\ntranslateY(-20px)\ntranslateY(0px)" },

                new CodingExercise { Id=Guid.Parse("a2200004-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000004"),
                    Title="Breakpoint Detector",
                    Description="Write getBreakpoint(width): \"sm\" <640, \"md\" <768, \"lg\" <1024, \"xl\" otherwise.\nPrint getBreakpoint(500) and getBreakpoint(1200):\nsm\nxl",
                    StarterCode="function getBreakpoint(width) {\n  // sm / md / lg / xl\n}\nconsole.log(getBreakpoint(500));\nconsole.log(getBreakpoint(1200));\n",
                    SolutionCode="function getBreakpoint(width) {\n  if (width < 640) return \"sm\";\n  if (width < 768) return \"md\";\n  if (width < 1024) return \"lg\";\n  return \"xl\";\n}\nconsole.log(getBreakpoint(500));\nconsole.log(getBreakpoint(1200));\n",
                    ExpectedOutput="sm\nxl" },
                new CodingExercise { Id=Guid.Parse("a2200004-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000004"),
                    Title="px to rem Converter",
                    Description="Write pxToRem(px, base=16) returning px/base.\nPrint pxToRem(32) and pxToRem(48):\n2\n3",
                    StarterCode="function pxToRem(px, base = 16) {\n  // Return px / base\n}\nconsole.log(pxToRem(32));\nconsole.log(pxToRem(48));\n",
                    SolutionCode="function pxToRem(px, base = 16) {\n  return px / base;\n}\nconsole.log(pxToRem(32));\nconsole.log(pxToRem(48));\n",
                    ExpectedOutput="2\n3" },

                new CodingExercise { Id=Guid.Parse("a2200005-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000005"),
                    Title="Destructuring Objects",
                    Description="Destructure { name, age } from user = { name:\"Bob\", age:30, city:\"London\" }.\nPrint: Bob is 30 years old.",
                    StarterCode="const user = { name: \"Bob\", age: 30, city: \"London\" };\n// Destructure name and age, then print\n",
                    SolutionCode="const user = { name: \"Bob\", age: 30, city: \"London\" };\nconst { name, age } = user;\nconsole.log(`${name} is ${age} years old.`);\n",
                    ExpectedOutput="Bob is 30 years old." },
                new CodingExercise { Id=Guid.Parse("a2200005-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000005"),
                    Title="Spread Arrays",
                    Description="Merge [1,2,3] and [4,5,6] using spread and print each number on its own line:\n1\n2\n3\n4\n5\n6",
                    StarterCode="const a = [1,2,3];\nconst b = [4,5,6];\n// Merge with spread and print each\n",
                    SolutionCode="const a = [1,2,3];\nconst b = [4,5,6];\n[...a, ...b].forEach(n => console.log(n));\n",
                    ExpectedOutput="1\n2\n3\n4\n5\n6" },

                new CodingExercise { Id=Guid.Parse("a2200006-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000006"),
                    Title="Double with map()",
                    Description="Use .map() to double [3,6,9] and print each result:\n6\n12\n18",
                    StarterCode="const nums = [3, 6, 9];\n// Use .map() to double each number, then print\n",
                    SolutionCode="const nums = [3, 6, 9];\nnums.map(n => n * 2).forEach(n => console.log(n));\n",
                    ExpectedOutput="6\n12\n18" },
                new CodingExercise { Id=Guid.Parse("a2200006-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000006"),
                    Title="Filter and Sum",
                    Description="From [5, -3, 8, -1, 2], filter positives then sum them.\nPrint: Sum of positives: 15",
                    StarterCode="const nums = [5, -3, 8, -1, 2];\n// Filter positives, then reduce to sum\n",
                    SolutionCode="const nums = [5, -3, 8, -1, 2];\nconst sum = nums.filter(n => n > 0).reduce((a, b) => a + b, 0);\nconsole.log(`Sum of positives: ${sum}`);\n",
                    ExpectedOutput="Sum of positives: 15" },

                new CodingExercise { Id=Guid.Parse("a2200007-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000007"),
                    Title="JSON Stringify",
                    Description="Stringify { name:\"Alice\", score:95 } and print the JSON string:\n{\"name\":\"Alice\",\"score\":95}",
                    StarterCode="const data = { name: \"Alice\", score: 95 };\n// JSON.stringify and print\n",
                    SolutionCode="const data = { name: \"Alice\", score: 95 };\nconsole.log(JSON.stringify(data));\n",
                    ExpectedOutput="{\"name\":\"Alice\",\"score\":95}" },
                new CodingExercise { Id=Guid.Parse("a2200007-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000007"),
                    Title="Object Keys",
                    Description="Print the keys of { html:1, css:2, js:3 } one per line:\nhtml\ncss\njs",
                    StarterCode="const skills = { html: 1, css: 2, js: 3 };\n// Print each key\n",
                    SolutionCode="const skills = { html: 1, css: 2, js: 3 };\nObject.keys(skills).forEach(k => console.log(k));\n",
                    ExpectedOutput="html\ncss\njs" },

                new CodingExercise { Id=Guid.Parse("a2200008-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000008"),
                    Title="Promise Chain",
                    Description="Create a Promise that resolves with 42, then chain .then(v => v * 2) and print the result.\nExpected output: 84",
                    StarterCode="const p = Promise.resolve(42);\np.then(v => v * 2).then(result => console.log(result));\n",
                    SolutionCode="const p = Promise.resolve(42);\np.then(v => v * 2).then(result => console.log(result));\n",
                    ExpectedOutput="84" },
                new CodingExercise { Id=Guid.Parse("a2200008-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000008"),
                    Title="Build Fetch Headers",
                    Description="Create a headers object with Content-Type:\"application/json\" and Authorization:\"Bearer mytoken123\".\nPrint: Content-Type: application/json\nAuthorization: Bearer mytoken123",
                    StarterCode="const headers = {\n  \"Content-Type\": \"application/json\",\n  \"Authorization\": \"Bearer mytoken123\"\n};\n// Print each header\n",
                    SolutionCode="const headers = {\n  \"Content-Type\": \"application/json\",\n  \"Authorization\": \"Bearer mytoken123\"\n};\nObject.entries(headers).forEach(([k, v]) => console.log(`${k}: ${v}`));\n",
                    ExpectedOutput="Content-Type: application/json\nAuthorization: Bearer mytoken123" },

                new CodingExercise { Id=Guid.Parse("a2200009-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000009"),
                    Title="Async Function Simulation",
                    Description="Use an async IIFE with await Promise.resolve(\"data loaded\") and print the result.\nExpected: data loaded",
                    StarterCode="(async () => {\n  const result = await Promise.resolve(\"data loaded\");\n  // Print result\n})();\n",
                    SolutionCode="(async () => {\n  const result = await Promise.resolve(\"data loaded\");\n  console.log(result);\n})();\n",
                    ExpectedOutput="data loaded" },
                new CodingExercise { Id=Guid.Parse("a2200009-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000009"),
                    Title="Promise.all Results",
                    Description="Use Promise.all with [Promise.resolve(10), Promise.resolve(20), Promise.resolve(30)] and print their sum.\nExpected: Total: 60",
                    StarterCode="(async () => {\n  const results = await Promise.all([\n    Promise.resolve(10),\n    Promise.resolve(20),\n    Promise.resolve(30)\n  ]);\n  // Print: Total: X\n})();\n",
                    SolutionCode="(async () => {\n  const results = await Promise.all([\n    Promise.resolve(10),\n    Promise.resolve(20),\n    Promise.resolve(30)\n  ]);\n  const total = results.reduce((a, b) => a + b, 0);\n  console.log(`Total: ${total}`);\n})();\n",
                    ExpectedOutput="Total: 60" },

                new CodingExercise { Id=Guid.Parse("a2200010-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000010"),
                    Title="Storage Simulator",
                    Description="Simulate localStorage: store { theme:\"dark\" } as JSON, retrieve it, and print: theme: dark",
                    StarterCode="const storage = {};\nstorage[\"settings\"] = JSON.stringify({ theme: \"dark\" });\nconst settings = JSON.parse(storage[\"settings\"]);\n// Print: theme: [value]\n",
                    SolutionCode="const storage = {};\nstorage[\"settings\"] = JSON.stringify({ theme: \"dark\" });\nconst settings = JSON.parse(storage[\"settings\"]);\nconsole.log(`theme: ${settings.theme}`);\n",
                    ExpectedOutput="theme: dark" },
                new CodingExercise { Id=Guid.Parse("a2200010-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("a1200000-0000-0000-0000-000000000010"),
                    Title="Key Exists Check",
                    Description="Write hasKey(storage, key) returning true/false.\nPrint hasKey({name:\"Alice\"}, \"name\") and hasKey({name:\"Alice\"}, \"age\"):\ntrue\nfalse",
                    StarterCode="function hasKey(storage, key) {\n  // Return true if key exists in storage\n}\nconsole.log(hasKey({ name: \"Alice\" }, \"name\"));\nconsole.log(hasKey({ name: \"Alice\" }, \"age\"));\n",
                    SolutionCode="function hasKey(storage, key) {\n  return key in storage;\n}\nconsole.log(hasKey({ name: \"Alice\" }, \"name\"));\nconsole.log(hasKey({ name: \"Alice\" }, \"age\"));\n",
                    ExpectedOutput="true\nfalse" },

                // ── ADVANCED ──────────────────────────────────────────────────────
                new CodingExercise { Id=Guid.Parse("b2200001-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000001"),
                    Title="Named Export Simulation",
                    Description="Create an object module with exported functions add and multiply.\nPrint module.add(3,4) and module.multiply(3,4):\n7\n12",
                    StarterCode="const module = {\n  add: (a, b) => a + b,\n  multiply: (a, b) => a * b\n};\n// Print results\n",
                    SolutionCode="const module = {\n  add: (a, b) => a + b,\n  multiply: (a, b) => a * b\n};\nconsole.log(module.add(3, 4));\nconsole.log(module.multiply(3, 4));\n",
                    ExpectedOutput="7\n12" },
                new CodingExercise { Id=Guid.Parse("b2200001-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000001"),
                    Title="Default Export Logger",
                    Description="Write a log(msg) function that prints: [LOG] msg\nPrint log(\"App started\")",
                    StarterCode="function log(msg) {\n  // Print: [LOG] msg\n}\nlog(\"App started\");\n",
                    SolutionCode="function log(msg) {\n  console.log(`[LOG] ${msg}`);\n}\nlog(\"App started\");\n",
                    ExpectedOutput="[LOG] App started" },

                new CodingExercise { Id=Guid.Parse("b2200002-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000002"),
                    Title="Server Response Simulator",
                    Description="Write respond(statusCode, body) that prints: HTTP [statusCode]: [body]\nPrint respond(200, \"OK\") and respond(404, \"Not Found\"):\nHTTP 200: OK\nHTTP 404: Not Found",
                    StarterCode="function respond(statusCode, body) {\n  // Print: HTTP [statusCode]: [body]\n}\nrespond(200, \"OK\");\nrespond(404, \"Not Found\");\n",
                    SolutionCode="function respond(statusCode, body) {\n  console.log(`HTTP ${statusCode}: ${body}`);\n}\nrespond(200, \"OK\");\nrespond(404, \"Not Found\");\n",
                    ExpectedOutput="HTTP 200: OK\nHTTP 404: Not Found" },
                new CodingExercise { Id=Guid.Parse("b2200002-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000002"),
                    Title="Path Join",
                    Description="Join path segments [\"/api\", \"users\", \"42\"] with '/' separator.\nPrint: /api/users/42",
                    StarterCode="const segments = [\"/api\", \"users\", \"42\"];\n// Join and print the path\n",
                    SolutionCode="const segments = [\"/api\", \"users\", \"42\"];\nconsole.log(segments.join(\"/\"));\n",
                    ExpectedOutput="/api/users/42" },

                new CodingExercise { Id=Guid.Parse("b2200003-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000003"),
                    Title="Package Version Check",
                    Description="Given pkg = { name:\"myapp\", version:\"2.1.0\", dependencies:{\"axios\":\"^1.6.0\"} }, print:\nPackage: myapp v2.1.0\nDependencies: 1",
                    StarterCode="const pkg = { name:\"myapp\", version:\"2.1.0\", dependencies:{\"axios\":\"^1.6.0\"} };\n// Print package info\n",
                    SolutionCode="const pkg = { name:\"myapp\", version:\"2.1.0\", dependencies:{\"axios\":\"^1.6.0\"} };\nconsole.log(`Package: ${pkg.name} v${pkg.version}`);\nconsole.log(`Dependencies: ${Object.keys(pkg.dependencies).length}`);\n",
                    ExpectedOutput="Package: myapp v2.1.0\nDependencies: 1" },
                new CodingExercise { Id=Guid.Parse("b2200003-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000003"),
                    Title="Script Runner",
                    Description="Given scripts = { dev:\"vite\", build:\"vite build\", lint:\"eslint src\" }, print each script as: [name]: [command]:\ndev: vite\nbuild: vite build\nlint: eslint src",
                    StarterCode="const scripts = { dev:\"vite\", build:\"vite build\", lint:\"eslint src\" };\n// Print each script\n",
                    SolutionCode="const scripts = { dev:\"vite\", build:\"vite build\", lint:\"eslint src\" };\nObject.entries(scripts).forEach(([name, cmd]) => console.log(`${name}: ${cmd}`));\n",
                    ExpectedOutput="dev: vite\nbuild: vite build\nlint: eslint src" },

                new CodingExercise { Id=Guid.Parse("b2200004-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000004"),
                    Title="JSX Tag Builder",
                    Description="Write jsxTag(component, prop, value) returning: <Component prop=\"value\" />\nPrint jsxTag(\"Button\", \"className\", \"btn-primary\")",
                    StarterCode="function jsxTag(component, prop, value) {\n  // Return <Component prop=\"value\" />\n}\nconsole.log(jsxTag(\"Button\", \"className\", \"btn-primary\"));\n",
                    SolutionCode="function jsxTag(component, prop, value) {\n  return `<${component} ${prop}=\"${value}\" />`;\n}\nconsole.log(jsxTag(\"Button\", \"className\", \"btn-primary\"));\n",
                    ExpectedOutput="<Button className=\"btn-primary\" />" },
                new CodingExercise { Id=Guid.Parse("b2200004-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000004"),
                    Title="Virtual DOM Concept",
                    Description="Create a vNode = { type:\"h1\", props:{ className:\"title\" }, children:\"Hello React\" }.\nPrint: type: h1, children: Hello React",
                    StarterCode="const vNode = { type:\"h1\", props:{ className:\"title\" }, children:\"Hello React\" };\n// Print type and children\n",
                    SolutionCode="const vNode = { type:\"h1\", props:{ className:\"title\" }, children:\"Hello React\" };\nconsole.log(`type: ${vNode.type}, children: ${vNode.children}`);\n",
                    ExpectedOutput="type: h1, children: Hello React" },

                new CodingExercise { Id=Guid.Parse("b2200005-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000005"),
                    Title="Props Renderer",
                    Description="Write renderCard({ title, description }) that prints:\nCard: [title] — [description]\nPrint renderCard({ title:\"React\", description:\"A UI library\" })",
                    StarterCode="function renderCard({ title, description }) {\n  // Print: Card: [title] — [description]\n}\nrenderCard({ title: \"React\", description: \"A UI library\" });\n",
                    SolutionCode="function renderCard({ title, description }) {\n  console.log(`Card: ${title} — ${description}`);\n}\nrenderCard({ title: \"React\", description: \"A UI library\" });\n",
                    ExpectedOutput="Card: React — A UI library" },
                new CodingExercise { Id=Guid.Parse("b2200005-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000005"),
                    Title="Component List",
                    Description="Given components = [\"Header\",\"Sidebar\",\"Main\",\"Footer\"], print each as: <Component />:\n<Header />\n<Sidebar />\n<Main />\n<Footer />",
                    StarterCode="const components = [\"Header\", \"Sidebar\", \"Main\", \"Footer\"];\n// Print each as <Name />\n",
                    SolutionCode="const components = [\"Header\", \"Sidebar\", \"Main\", \"Footer\"];\ncomponents.forEach(c => console.log(`<${c} />`));\n",
                    ExpectedOutput="<Header />\n<Sidebar />\n<Main />\n<Footer />" },

                new CodingExercise { Id=Guid.Parse("b2200006-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000006"),
                    Title="State Counter Simulation",
                    Description="Simulate useState: start count=0, call setCount(c=>c+1) three times, print final count.\nExpected: Count: 3",
                    StarterCode="let count = 0;\nconst setCount = (fn) => { count = fn(count); };\nsetCount(c => c + 1);\nsetCount(c => c + 1);\nsetCount(c => c + 1);\n// Print: Count: X\n",
                    SolutionCode="let count = 0;\nconst setCount = (fn) => { count = fn(count); };\nsetCount(c => c + 1);\nsetCount(c => c + 1);\nsetCount(c => c + 1);\nconsole.log(`Count: ${count}`);\n",
                    ExpectedOutput="Count: 3" },
                new CodingExercise { Id=Guid.Parse("b2200006-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000006"),
                    Title="Effect Dependencies",
                    Description="Write shouldRunEffect(prev, curr) returning true if any value changed.\nPrint shouldRunEffect([1,2], [1,3]) and shouldRunEffect([1,2], [1,2]):\ntrue\nfalse",
                    StarterCode="function shouldRunEffect(prev, curr) {\n  return prev.some((val, i) => val !== curr[i]);\n}\nconsole.log(shouldRunEffect([1,2], [1,3]));\nconsole.log(shouldRunEffect([1,2], [1,2]));\n",
                    SolutionCode="function shouldRunEffect(prev, curr) {\n  return prev.some((val, i) => val !== curr[i]);\n}\nconsole.log(shouldRunEffect([1,2], [1,3]));\nconsole.log(shouldRunEffect([1,2], [1,2]));\n",
                    ExpectedOutput="true\nfalse" },

                new CodingExercise { Id=Guid.Parse("b2200007-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000007"),
                    Title="HTTP Method Mapper",
                    Description="Write getMethod(action): \"GET\" for read, \"POST\" for create, \"DELETE\" for remove.\nPrint getMethod(\"read\"), getMethod(\"create\"), getMethod(\"remove\"):\nGET\nPOST\nDELETE",
                    StarterCode="function getMethod(action) {\n  // Map action to HTTP method\n}\nconsole.log(getMethod(\"read\"));\nconsole.log(getMethod(\"create\"));\nconsole.log(getMethod(\"remove\"));\n",
                    SolutionCode="function getMethod(action) {\n  if (action === \"read\") return \"GET\";\n  if (action === \"create\") return \"POST\";\n  if (action === \"remove\") return \"DELETE\";\n}\nconsole.log(getMethod(\"read\"));\nconsole.log(getMethod(\"create\"));\nconsole.log(getMethod(\"remove\"));\n",
                    ExpectedOutput="GET\nPOST\nDELETE" },
                new CodingExercise { Id=Guid.Parse("b2200007-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000007"),
                    Title="Status Code Checker",
                    Description="Write getStatus(code): \"Success\" for 200-299, \"Redirect\" for 300-399, \"Error\" for 400+.\nPrint getStatus(200) and getStatus(404):\nSuccess\nError",
                    StarterCode="function getStatus(code) {\n  // Success / Redirect / Error\n}\nconsole.log(getStatus(200));\nconsole.log(getStatus(404));\n",
                    SolutionCode="function getStatus(code) {\n  if (code >= 200 && code < 300) return \"Success\";\n  if (code >= 300 && code < 400) return \"Redirect\";\n  return \"Error\";\n}\nconsole.log(getStatus(200));\nconsole.log(getStatus(404));\n",
                    ExpectedOutput="Success\nError" },

                new CodingExercise { Id=Guid.Parse("b2200008-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000008"),
                    Title="Bearer Token Builder",
                    Description="Write authHeader(token) returning: Authorization: Bearer [token]\nPrint authHeader(\"abc123\")",
                    StarterCode="function authHeader(token) {\n  // Return: Authorization: Bearer [token]\n}\nconsole.log(authHeader(\"abc123\"));\n",
                    SolutionCode="function authHeader(token) {\n  return `Authorization: Bearer ${token}`;\n}\nconsole.log(authHeader(\"abc123\"));\n",
                    ExpectedOutput="Authorization: Bearer abc123" },
                new CodingExercise { Id=Guid.Parse("b2200008-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000008"),
                    Title="Token Expiry Check",
                    Description="Write isExpired(expiry, now) returning true if expiry <= now.\nPrint isExpired(1000, 1500) and isExpired(2000, 1500):\ntrue\nfalse",
                    StarterCode="function isExpired(expiry, now) {\n  // Return true if expired\n}\nconsole.log(isExpired(1000, 1500));\nconsole.log(isExpired(2000, 1500));\n",
                    SolutionCode="function isExpired(expiry, now) {\n  return expiry <= now;\n}\nconsole.log(isExpired(1000, 1500));\nconsole.log(isExpired(2000, 1500));\n",
                    ExpectedOutput="true\nfalse" },

                new CodingExercise { Id=Guid.Parse("b2200009-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000009"),
                    Title="Memoize Function",
                    Description="Write memoize(fn). Call it with a slow square function, call twice with 5.\nPrint: 25 then cache hit: 25",
                    StarterCode="function memoize(fn) {\n  const cache = {};\n  return (x) => {\n    if (x in cache) { console.log(\"cache hit: \" + cache[x]); return cache[x]; }\n    return (cache[x] = fn(x));\n  };\n}\nconst sq = memoize(x => x * x);\nconsole.log(sq(5));\nsq(5);\n",
                    SolutionCode="function memoize(fn) {\n  const cache = {};\n  return (x) => {\n    if (x in cache) { console.log(\"cache hit: \" + cache[x]); return cache[x]; }\n    return (cache[x] = fn(x));\n  };\n}\nconst sq = memoize(x => x * x);\nconsole.log(sq(5));\nsq(5);\n",
                    ExpectedOutput="25\ncache hit: 25" },
                new CodingExercise { Id=Guid.Parse("b2200009-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000009"),
                    Title="Lazy Load Checker",
                    Description="Write shouldLazyLoad(position, viewportHeight) returning true if position > viewportHeight.\nPrint shouldLazyLoad(900, 768) and shouldLazyLoad(500, 768):\ntrue\nfalse",
                    StarterCode="function shouldLazyLoad(position, viewportHeight) {\n  return position > viewportHeight;\n}\nconsole.log(shouldLazyLoad(900, 768));\nconsole.log(shouldLazyLoad(500, 768));\n",
                    SolutionCode="function shouldLazyLoad(position, viewportHeight) {\n  return position > viewportHeight;\n}\nconsole.log(shouldLazyLoad(900, 768));\nconsole.log(shouldLazyLoad(500, 768));\n",
                    ExpectedOutput="true\nfalse" },

                new CodingExercise { Id=Guid.Parse("b2200010-0000-0000-0000-000000000001"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000010"),
                    Title="Env Variable Reader",
                    Description="Given env = { VITE_API_URL:\"https://api.example.com\", NODE_ENV:\"production\" }, print each variable:\nVITE_API_URL: https://api.example.com\nNODE_ENV: production",
                    StarterCode="const env = { VITE_API_URL:\"https://api.example.com\", NODE_ENV:\"production\" };\n// Print each env variable\n",
                    SolutionCode="const env = { VITE_API_URL:\"https://api.example.com\", NODE_ENV:\"production\" };\nObject.entries(env).forEach(([k, v]) => console.log(`${k}: ${v}`));\n",
                    ExpectedOutput="VITE_API_URL: https://api.example.com\nNODE_ENV: production" },
                new CodingExercise { Id=Guid.Parse("b2200010-0000-0000-0000-000000000002"),
                    LessonId=Guid.Parse("b1300000-0000-0000-0000-000000000010"),
                    Title="Build Config Validator",
                    Description="Write isValidConfig(config) returning true if config has both 'entry' and 'output'.\nPrint isValidConfig({entry:\"src/main.js\",output:\"dist\"}) and isValidConfig({entry:\"src/main.js\"}):\ntrue\nfalse",
                    StarterCode="function isValidConfig(config) {\n  return \"entry\" in config && \"output\" in config;\n}\nconsole.log(isValidConfig({ entry:\"src/main.js\", output:\"dist\" }));\nconsole.log(isValidConfig({ entry:\"src/main.js\" }));\n",
                    SolutionCode="function isValidConfig(config) {\n  return \"entry\" in config && \"output\" in config;\n}\nconsole.log(isValidConfig({ entry:\"src/main.js\", output:\"dist\" }));\nconsole.log(isValidConfig({ entry:\"src/main.js\" }));\n",
                    ExpectedOutput="true\nfalse" }
            };

            foreach (var ex in exercises)
            {
                var existing = await context.CodingExercises.FirstOrDefaultAsync(e => e.Id == ex.Id);
                if (existing == null)
                    await context.CodingExercises.AddAsync(ex);
                else
                {
                    existing.Title          = ex.Title;
                    existing.Description    = ex.Description;
                    existing.StarterCode    = ex.StarterCode;
                    existing.SolutionCode   = ex.SolutionCode;
                    existing.ExpectedOutput = ex.ExpectedOutput;
                    existing.isDeleted      = false;
                }
            }
            await context.SaveChangesAsync();

            // All 6 quizzes now belong to the single consolidated Web course.
            await SeedWebQuizzesAsync(context, beginnerCourseId, beginnerCourseId, beginnerCourseId);
        }

        private static async Task SeedWebQuizzesAsync(ApplicationDbContext context,
            Guid beginnerCourseId, Guid intermediateCourseId, Guid advancedCourseId)
        {
            // ── Quiz 1: Web Beginner ─────────────────────────────────────────────
            var wq1Id = Guid.Parse("eb100001-0000-0000-0000-000000000001");
            if (!await context.Quizzes.AnyAsync(q => q.Id == wq1Id))
            {
                await context.Quizzes.AddAsync(new Quiz
                {
                    Id = wq1Id, CourseId = beginnerCourseId,
                    Title = "Web Basics Quiz", Description = "Test your knowledge of HTML, CSS and basic JavaScript.",
                    TimeLimitMinutes = 10, PassingScore = 70, CreatedAt = DateTime.UtcNow, IsDeleted = false
                });
                await context.SaveChangesAsync();

                (Guid qId, string text, string diff, int order, (Guid aId, string aText, bool correct)[] opts)[] q1s =
                {
                    (Guid.Parse("eb100011-0000-0000-0000-000000000001"), "What does HTML stand for?", "Easy", 1, new[]{
                        (Guid.Parse("eb100011-0001-0000-0000-000000000001"), "HyperText Markup Language", true),
                        (Guid.Parse("eb100011-0002-0000-0000-000000000001"), "HyperText Machine Language", false),
                        (Guid.Parse("eb100011-0003-0000-0000-000000000001"), "HighText Markup Language", false),
                        (Guid.Parse("eb100011-0004-0000-0000-000000000001"), "HyperTool Markup Language", false) }),
                    (Guid.Parse("eb100012-0000-0000-0000-000000000001"), "Which CSS property changes text colour?", "Easy", 2, new[]{
                        (Guid.Parse("eb100012-0001-0000-0000-000000000001"), "font-color", false),
                        (Guid.Parse("eb100012-0002-0000-0000-000000000001"), "text-color", false),
                        (Guid.Parse("eb100012-0003-0000-0000-000000000001"), "color", true),
                        (Guid.Parse("eb100012-0004-0000-0000-000000000001"), "foreground-color", false) }),
                    (Guid.Parse("eb100013-0000-0000-0000-000000000001"), "Which tag creates a hyperlink in HTML?", "Easy", 3, new[]{
                        (Guid.Parse("eb100013-0001-0000-0000-000000000001"), "<link>", false),
                        (Guid.Parse("eb100013-0002-0000-0000-000000000001"), "<a>", true),
                        (Guid.Parse("eb100013-0003-0000-0000-000000000001"), "<href>", false),
                        (Guid.Parse("eb100013-0004-0000-0000-000000000001"), "<url>", false) }),
                    (Guid.Parse("eb100014-0000-0000-0000-000000000001"), "How do you write a single-line comment in JavaScript?", "Easy", 4, new[]{
                        (Guid.Parse("eb100014-0001-0000-0000-000000000001"), "<!-- comment -->", false),
                        (Guid.Parse("eb100014-0002-0000-0000-000000000001"), "# comment", false),
                        (Guid.Parse("eb100014-0003-0000-0000-000000000001"), "// comment", true),
                        (Guid.Parse("eb100014-0004-0000-0000-000000000001"), "** comment", false) }),
                    (Guid.Parse("eb100015-0000-0000-0000-000000000001"), "Which HTML element defines the document body?", "Easy", 5, new[]{
                        (Guid.Parse("eb100015-0001-0000-0000-000000000001"), "<head>", false),
                        (Guid.Parse("eb100015-0002-0000-0000-000000000001"), "<section>", false),
                        (Guid.Parse("eb100015-0003-0000-0000-000000000001"), "<main>", false),
                        (Guid.Parse("eb100015-0004-0000-0000-000000000001"), "<body>", true) }),
                };
                foreach (var (qId, text, diff, order, opts) in q1s)
                {
                    await context.QuizQuestions.AddAsync(new QuizQuestion { Id = qId, QuizId = wq1Id, Text = text, Difficulty = diff, OrderNumber = order, CreatedAt = DateTime.UtcNow, IsDeleted = false });
                    foreach (var (aId, aText, correct) in opts)
                        await context.QuizAnswerOptions.AddAsync(new QuizAnswerOption { Id = aId, QuizQuestionId = qId, Text = aText, IsCorrect = correct, CreatedAt = DateTime.UtcNow, IsDeleted = false });
                }
                await context.SaveChangesAsync();
            }

            // ── Quiz 2: Web Intermediate ─────────────────────────────────────────
            var wq2Id = Guid.Parse("eb200001-0000-0000-0000-000000000002");
            if (!await context.Quizzes.AnyAsync(q => q.Id == wq2Id))
            {
                await context.Quizzes.AddAsync(new Quiz
                {
                    Id = wq2Id, CourseId = intermediateCourseId,
                    Title = "Web Intermediate Quiz", Description = "Test your knowledge of CSS layouts, DOM manipulation and ES6.",
                    TimeLimitMinutes = 10, PassingScore = 70, CreatedAt = DateTime.UtcNow, IsDeleted = false
                });
                await context.SaveChangesAsync();

                (Guid qId, string text, string diff, int order, (Guid aId, string aText, bool correct)[] opts)[] q2s =
                {
                    (Guid.Parse("eb200011-0000-0000-0000-000000000002"), "Which CSS property makes a flex container wrap its children?", "Medium", 1, new[]{
                        (Guid.Parse("eb200011-0001-0000-0000-000000000002"), "flex-direction: wrap", false),
                        (Guid.Parse("eb200011-0002-0000-0000-000000000002"), "flex-wrap: wrap", true),
                        (Guid.Parse("eb200011-0003-0000-0000-000000000002"), "flex-flow: nowrap", false),
                        (Guid.Parse("eb200011-0004-0000-0000-000000000002"), "flex-wrap: nowrap", false) }),
                    (Guid.Parse("eb200012-0000-0000-0000-000000000002"), "What does document.querySelector() return?", "Medium", 2, new[]{
                        (Guid.Parse("eb200012-0001-0000-0000-000000000002"), "All matching elements", false),
                        (Guid.Parse("eb200012-0002-0000-0000-000000000002"), "An array of elements", false),
                        (Guid.Parse("eb200012-0003-0000-0000-000000000002"), "The first matching element", true),
                        (Guid.Parse("eb200012-0004-0000-0000-000000000002"), "A NodeList", false) }),
                    (Guid.Parse("eb200013-0000-0000-0000-000000000002"), "Which keyword declares a block-scoped variable in ES6?", "Medium", 3, new[]{
                        (Guid.Parse("eb200013-0001-0000-0000-000000000002"), "var", false),
                        (Guid.Parse("eb200013-0002-0000-0000-000000000002"), "let", true),
                        (Guid.Parse("eb200013-0003-0000-0000-000000000002"), "define", false),
                        (Guid.Parse("eb200013-0004-0000-0000-000000000002"), "set", false) }),
                    (Guid.Parse("eb200014-0000-0000-0000-000000000002"), "What is the correct syntax for an arrow function?", "Medium", 4, new[]{
                        (Guid.Parse("eb200014-0001-0000-0000-000000000002"), "function => (x) { }", false),
                        (Guid.Parse("eb200014-0002-0000-0000-000000000002"), "(x) => { }", true),
                        (Guid.Parse("eb200014-0003-0000-0000-000000000002"), "(x) -> { }", false),
                        (Guid.Parse("eb200014-0004-0000-0000-000000000002"), "=> x { }", false) }),
                    (Guid.Parse("eb200015-0000-0000-0000-000000000002"), "Which CSS value centers a block element horizontally?", "Medium", 5, new[]{
                        (Guid.Parse("eb200015-0001-0000-0000-000000000002"), "margin: center", false),
                        (Guid.Parse("eb200015-0002-0000-0000-000000000002"), "text-align: center", false),
                        (Guid.Parse("eb200015-0003-0000-0000-000000000002"), "margin: 0 auto", true),
                        (Guid.Parse("eb200015-0004-0000-0000-000000000002"), "align: center", false) }),
                };
                foreach (var (qId, text, diff, order, opts) in q2s)
                {
                    await context.QuizQuestions.AddAsync(new QuizQuestion { Id = qId, QuizId = wq2Id, Text = text, Difficulty = diff, OrderNumber = order, CreatedAt = DateTime.UtcNow, IsDeleted = false });
                    foreach (var (aId, aText, correct) in opts)
                        await context.QuizAnswerOptions.AddAsync(new QuizAnswerOption { Id = aId, QuizQuestionId = qId, Text = aText, IsCorrect = correct, CreatedAt = DateTime.UtcNow, IsDeleted = false });
                }
                await context.SaveChangesAsync();
            }

            // ── Quiz 3: Web Advanced ─────────────────────────────────────────────
            var wq3Id = Guid.Parse("eb300001-0000-0000-0000-000000000003");
            if (!await context.Quizzes.AnyAsync(q => q.Id == wq3Id))
            {
                await context.Quizzes.AddAsync(new Quiz
                {
                    Id = wq3Id, CourseId = advancedCourseId,
                    Title = "Web Advanced Quiz", Description = "Test your knowledge of async JS, modules, APIs and performance.",
                    TimeLimitMinutes = 15, PassingScore = 70, CreatedAt = DateTime.UtcNow, IsDeleted = false
                });
                await context.SaveChangesAsync();

                (Guid qId, string text, string diff, int order, (Guid aId, string aText, bool correct)[] opts)[] q3s =
                {
                    (Guid.Parse("eb300011-0000-0000-0000-000000000003"), "What does async/await do in JavaScript?", "Hard", 1, new[]{
                        (Guid.Parse("eb300011-0001-0000-0000-000000000003"), "Creates a new thread", false),
                        (Guid.Parse("eb300011-0002-0000-0000-000000000003"), "Runs code synchronously", false),
                        (Guid.Parse("eb300011-0003-0000-0000-000000000003"), "Makes asynchronous code look synchronous", true),
                        (Guid.Parse("eb300011-0004-0000-0000-000000000003"), "Blocks the event loop", false) }),
                    (Guid.Parse("eb300012-0000-0000-0000-000000000003"), "Which HTTP method is used to update an existing resource?", "Hard", 2, new[]{
                        (Guid.Parse("eb300012-0001-0000-0000-000000000003"), "GET", false),
                        (Guid.Parse("eb300012-0002-0000-0000-000000000003"), "POST", false),
                        (Guid.Parse("eb300012-0003-0000-0000-000000000003"), "PUT", true),
                        (Guid.Parse("eb300012-0004-0000-0000-000000000003"), "DELETE", false) }),
                    (Guid.Parse("eb300013-0000-0000-0000-000000000003"), "What is the output of: console.log(typeof null)?", "Hard", 3, new[]{
                        (Guid.Parse("eb300013-0001-0000-0000-000000000003"), "null", false),
                        (Guid.Parse("eb300013-0002-0000-0000-000000000003"), "undefined", false),
                        (Guid.Parse("eb300013-0003-0000-0000-000000000003"), "object", true),
                        (Guid.Parse("eb300013-0004-0000-0000-000000000003"), "string", false) }),
                    (Guid.Parse("eb300014-0000-0000-0000-000000000003"), "Which statement correctly exports a default function in an ES module?", "Hard", 4, new[]{
                        (Guid.Parse("eb300014-0001-0000-0000-000000000003"), "module.export = fn", false),
                        (Guid.Parse("eb300014-0002-0000-0000-000000000003"), "exports.default = fn", false),
                        (Guid.Parse("eb300014-0003-0000-0000-000000000003"), "export default fn", true),
                        (Guid.Parse("eb300014-0004-0000-0000-000000000003"), "export fn", false) }),
                    (Guid.Parse("eb300015-0000-0000-0000-000000000003"), "What does the Fetch API return?", "Hard", 5, new[]{
                        (Guid.Parse("eb300015-0001-0000-0000-000000000003"), "An XMLHttpRequest", false),
                        (Guid.Parse("eb300015-0002-0000-0000-000000000003"), "A Promise", true),
                        (Guid.Parse("eb300015-0003-0000-0000-000000000003"), "A callback", false),
                        (Guid.Parse("eb300015-0004-0000-0000-000000000003"), "A JSON object", false) }),
                };
                foreach (var (qId, text, diff, order, opts) in q3s)
                {
                    await context.QuizQuestions.AddAsync(new QuizQuestion { Id = qId, QuizId = wq3Id, Text = text, Difficulty = diff, OrderNumber = order, CreatedAt = DateTime.UtcNow, IsDeleted = false });
                    foreach (var (aId, aText, correct) in opts)
                        await context.QuizAnswerOptions.AddAsync(new QuizAnswerOption { Id = aId, QuizQuestionId = qId, Text = aText, IsCorrect = correct, CreatedAt = DateTime.UtcNow, IsDeleted = false });
                }
                await context.SaveChangesAsync();
            }

            // ── Quiz 4: Web Beginner #2 ──────────────────────────────────────────
            var wq4Id = Guid.Parse("eb100002-0000-0000-0000-000000000001");
            if (!await context.Quizzes.AnyAsync(q => q.Id == wq4Id))
            {
                await context.Quizzes.AddAsync(new Quiz
                {
                    Id = wq4Id, CourseId = beginnerCourseId,
                    Title = "HTML & CSS Fundamentals", Description = "Deepen your understanding of HTML elements and CSS properties.",
                    TimeLimitMinutes = 10, PassingScore = 70, CreatedAt = DateTime.UtcNow, IsDeleted = false
                });
                await context.SaveChangesAsync();

                (Guid qId, string text, string diff, int order, (Guid aId, string aText, bool correct)[] opts)[] q4s =
                {
                    (Guid.Parse("eb100021-0000-0000-0000-000000000001"), "Which HTML attribute specifies an image URL?", "Easy", 1, new[]{
                        (Guid.Parse("eb100021-0001-0000-0000-000000000001"), "src", true),
                        (Guid.Parse("eb100021-0002-0000-0000-000000000001"), "href", false),
                        (Guid.Parse("eb100021-0003-0000-0000-000000000001"), "url", false),
                        (Guid.Parse("eb100021-0004-0000-0000-000000000001"), "link", false) }),
                    (Guid.Parse("eb100022-0000-0000-0000-000000000001"), "Which CSS property controls the space inside an element?", "Easy", 2, new[]{
                        (Guid.Parse("eb100022-0001-0000-0000-000000000001"), "margin", false),
                        (Guid.Parse("eb100022-0002-0000-0000-000000000001"), "spacing", false),
                        (Guid.Parse("eb100022-0003-0000-0000-000000000001"), "padding", true),
                        (Guid.Parse("eb100022-0004-0000-0000-000000000001"), "border", false) }),
                    (Guid.Parse("eb100023-0000-0000-0000-000000000001"), "What does the HTML <title> tag define?", "Easy", 3, new[]{
                        (Guid.Parse("eb100023-0001-0000-0000-000000000001"), "The main heading of the page", false),
                        (Guid.Parse("eb100023-0002-0000-0000-000000000001"), "The browser tab title", true),
                        (Guid.Parse("eb100023-0003-0000-0000-000000000001"), "A tooltip text", false),
                        (Guid.Parse("eb100023-0004-0000-0000-000000000001"), "The page footer", false) }),
                    (Guid.Parse("eb100024-0000-0000-0000-000000000001"), "Which CSS unit is relative to the font size of the element?", "Easy", 4, new[]{
                        (Guid.Parse("eb100024-0001-0000-0000-000000000001"), "px", false),
                        (Guid.Parse("eb100024-0002-0000-0000-000000000001"), "em", true),
                        (Guid.Parse("eb100024-0003-0000-0000-000000000001"), "vw", false),
                        (Guid.Parse("eb100024-0004-0000-0000-000000000001"), "cm", false) }),
                    (Guid.Parse("eb100025-0000-0000-0000-000000000001"), "Which JavaScript method selects an element by its ID?", "Easy", 5, new[]{
                        (Guid.Parse("eb100025-0001-0000-0000-000000000001"), "document.getElement()", false),
                        (Guid.Parse("eb100025-0002-0000-0000-000000000001"), "document.findById()", false),
                        (Guid.Parse("eb100025-0003-0000-0000-000000000001"), "document.getElementById()", true),
                        (Guid.Parse("eb100025-0004-0000-0000-000000000001"), "document.selectId()", false) }),
                };
                foreach (var (qId, text, diff, order, opts) in q4s)
                {
                    await context.QuizQuestions.AddAsync(new QuizQuestion { Id = qId, QuizId = wq4Id, Text = text, Difficulty = diff, OrderNumber = order, CreatedAt = DateTime.UtcNow, IsDeleted = false });
                    foreach (var (aId, aText, correct) in opts)
                        await context.QuizAnswerOptions.AddAsync(new QuizAnswerOption { Id = aId, QuizQuestionId = qId, Text = aText, IsCorrect = correct, CreatedAt = DateTime.UtcNow, IsDeleted = false });
                }
                await context.SaveChangesAsync();
            }

            // ── Quiz 5: Web Intermediate #2 ─────────────────────────────────────
            var wq5Id = Guid.Parse("eb200002-0000-0000-0000-000000000002");
            if (!await context.Quizzes.AnyAsync(q => q.Id == wq5Id))
            {
                await context.Quizzes.AddAsync(new Quiz
                {
                    Id = wq5Id, CourseId = intermediateCourseId,
                    Title = "DOM & ES6 Deep Dive", Description = "Test your skills with DOM events, ES6 features, and CSS Grid.",
                    TimeLimitMinutes = 10, PassingScore = 70, CreatedAt = DateTime.UtcNow, IsDeleted = false
                });
                await context.SaveChangesAsync();

                (Guid qId, string text, string diff, int order, (Guid aId, string aText, bool correct)[] opts)[] q5s =
                {
                    (Guid.Parse("eb200021-0000-0000-0000-000000000002"), "Which method removes an event listener in JavaScript?", "Medium", 1, new[]{
                        (Guid.Parse("eb200021-0001-0000-0000-000000000002"), "removeEvent()", false),
                        (Guid.Parse("eb200021-0002-0000-0000-000000000002"), "detachEventListener()", false),
                        (Guid.Parse("eb200021-0003-0000-0000-000000000002"), "removeEventListener()", true),
                        (Guid.Parse("eb200021-0004-0000-0000-000000000002"), "deleteEvent()", false) }),
                    (Guid.Parse("eb200022-0000-0000-0000-000000000002"), "What does the spread operator (...) do in JavaScript?", "Medium", 2, new[]{
                        (Guid.Parse("eb200022-0001-0000-0000-000000000002"), "Creates a new array constructor", false),
                        (Guid.Parse("eb200022-0002-0000-0000-000000000002"), "Expands an iterable into individual elements", true),
                        (Guid.Parse("eb200022-0003-0000-0000-000000000002"), "Merges two functions", false),
                        (Guid.Parse("eb200022-0004-0000-0000-000000000002"), "Declares a rest parameter", false) }),
                    (Guid.Parse("eb200023-0000-0000-0000-000000000002"), "Which CSS property defines the number of columns in a grid?", "Medium", 3, new[]{
                        (Guid.Parse("eb200023-0001-0000-0000-000000000002"), "grid-columns", false),
                        (Guid.Parse("eb200023-0002-0000-0000-000000000002"), "grid-template-columns", true),
                        (Guid.Parse("eb200023-0003-0000-0000-000000000002"), "columns", false),
                        (Guid.Parse("eb200023-0004-0000-0000-000000000002"), "flex-columns", false) }),
                    (Guid.Parse("eb200024-0000-0000-0000-000000000002"), "What is destructuring in JavaScript?", "Medium", 4, new[]{
                        (Guid.Parse("eb200024-0001-0000-0000-000000000002"), "Breaking a function into smaller pieces", false),
                        (Guid.Parse("eb200024-0002-0000-0000-000000000002"), "Unpacking values from arrays or objects into variables", true),
                        (Guid.Parse("eb200024-0003-0000-0000-000000000002"), "Deleting properties from an object", false),
                        (Guid.Parse("eb200024-0004-0000-0000-000000000002"), "Converting a string to an array", false) }),
                    (Guid.Parse("eb200025-0000-0000-0000-000000000002"), "Which event fires when a user submits an HTML form?", "Medium", 5, new[]{
                        (Guid.Parse("eb200025-0001-0000-0000-000000000002"), "click", false),
                        (Guid.Parse("eb200025-0002-0000-0000-000000000002"), "change", false),
                        (Guid.Parse("eb200025-0003-0000-0000-000000000002"), "submit", true),
                        (Guid.Parse("eb200025-0004-0000-0000-000000000002"), "input", false) }),
                };
                foreach (var (qId, text, diff, order, opts) in q5s)
                {
                    await context.QuizQuestions.AddAsync(new QuizQuestion { Id = qId, QuizId = wq5Id, Text = text, Difficulty = diff, OrderNumber = order, CreatedAt = DateTime.UtcNow, IsDeleted = false });
                    foreach (var (aId, aText, correct) in opts)
                        await context.QuizAnswerOptions.AddAsync(new QuizAnswerOption { Id = aId, QuizQuestionId = qId, Text = aText, IsCorrect = correct, CreatedAt = DateTime.UtcNow, IsDeleted = false });
                }
                await context.SaveChangesAsync();
            }

            // ── Quiz 6: Web Advanced #2 ──────────────────────────────────────────
            var wq6Id = Guid.Parse("eb300002-0000-0000-0000-000000000003");
            if (!await context.Quizzes.AnyAsync(q => q.Id == wq6Id))
            {
                await context.Quizzes.AddAsync(new Quiz
                {
                    Id = wq6Id, CourseId = advancedCourseId,
                    Title = "APIs, Modules & Performance", Description = "Advanced questions on REST APIs, ES modules, closures, and web performance.",
                    TimeLimitMinutes = 15, PassingScore = 70, CreatedAt = DateTime.UtcNow, IsDeleted = false
                });
                await context.SaveChangesAsync();

                (Guid qId, string text, string diff, int order, (Guid aId, string aText, bool correct)[] opts)[] q6s =
                {
                    (Guid.Parse("eb300021-0000-0000-0000-000000000003"), "What is a closure in JavaScript?", "Hard", 1, new[]{
                        (Guid.Parse("eb300021-0001-0000-0000-000000000003"), "A way to close the browser tab", false),
                        (Guid.Parse("eb300021-0002-0000-0000-000000000003"), "A function that retains access to its outer scope after the outer function returns", true),
                        (Guid.Parse("eb300021-0003-0000-0000-000000000003"), "A method to end a Promise chain", false),
                        (Guid.Parse("eb300021-0004-0000-0000-000000000003"), "An IIFE that runs once", false) }),
                    (Guid.Parse("eb300022-0000-0000-0000-000000000003"), "What HTTP status code means 'Not Found'?", "Hard", 2, new[]{
                        (Guid.Parse("eb300022-0001-0000-0000-000000000003"), "200", false),
                        (Guid.Parse("eb300022-0002-0000-0000-000000000003"), "401", false),
                        (Guid.Parse("eb300022-0003-0000-0000-000000000003"), "500", false),
                        (Guid.Parse("eb300022-0004-0000-0000-000000000003"), "404", true) }),
                    (Guid.Parse("eb300023-0000-0000-0000-000000000003"), "Which keyword is used to import a named export in ES modules?", "Hard", 3, new[]{
                        (Guid.Parse("eb300023-0001-0000-0000-000000000003"), "require", false),
                        (Guid.Parse("eb300023-0002-0000-0000-000000000003"), "include", false),
                        (Guid.Parse("eb300023-0003-0000-0000-000000000003"), "import", true),
                        (Guid.Parse("eb300023-0004-0000-0000-000000000003"), "use", false) }),
                    (Guid.Parse("eb300024-0000-0000-0000-000000000003"), "What does CORS stand for?", "Hard", 4, new[]{
                        (Guid.Parse("eb300024-0001-0000-0000-000000000003"), "Cross-Origin Resource Sharing", true),
                        (Guid.Parse("eb300024-0002-0000-0000-000000000003"), "Client-Origin Request System", false),
                        (Guid.Parse("eb300024-0003-0000-0000-000000000003"), "Cross-Object Response Security", false),
                        (Guid.Parse("eb300024-0004-0000-0000-000000000003"), "Cache-Override Rendering Strategy", false) }),
                    (Guid.Parse("eb300025-0000-0000-0000-000000000003"), "Which tool is commonly used to bundle JavaScript modules for the browser?", "Hard", 5, new[]{
                        (Guid.Parse("eb300025-0001-0000-0000-000000000003"), "Babel", false),
                        (Guid.Parse("eb300025-0002-0000-0000-000000000003"), "ESLint", false),
                        (Guid.Parse("eb300025-0003-0000-0000-000000000003"), "Webpack", true),
                        (Guid.Parse("eb300025-0004-0000-0000-000000000003"), "Prettier", false) }),
                };
                foreach (var (qId, text, diff, order, opts) in q6s)
                {
                    await context.QuizQuestions.AddAsync(new QuizQuestion { Id = qId, QuizId = wq6Id, Text = text, Difficulty = diff, OrderNumber = order, CreatedAt = DateTime.UtcNow, IsDeleted = false });
                    foreach (var (aId, aText, correct) in opts)
                        await context.QuizAnswerOptions.AddAsync(new QuizAnswerOption { Id = aId, QuizQuestionId = qId, Text = aText, IsCorrect = correct, CreatedAt = DateTime.UtcNow, IsDeleted = false });
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
