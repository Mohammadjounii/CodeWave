// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
async function loadLesson(lessonId) {
    const res = await fetch(`/LearningPath/Lesson/${lessonId}`);
    const lesson = await res.json();

    document.getElementById("lessonTitle").innerHTML = lesson.title;
    document.getElementById("lessonContent").innerHTML = lesson.content;
    document.getElementById("lessonVideo").src = lesson.videoUrl;

    const exercise = lesson.exercises[0];

    document.getElementById("codeEditor").value = exercise.starterCode;
}
async function runCode() {
    const code = document.getElementById("codeEditor").value;

    const response = await fetch("/api/code/run", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ code })
    });

    const data = await response.json();
    document.getElementById("terminalOutput").textContent = data.output;
}
async function completeLesson(lessonId) {
    await fetch("/api/progress/completeLesson?lessonId=" + lessonId, {
        method: "POST"
    });

    alert("Lesson Completed!");
}
