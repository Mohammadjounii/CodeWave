// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
async function loadLesson(lessonId) {
    try {
        const res = await fetch(`/LearningPath/Lesson/${lessonId}`);

        if (!res.ok) {
            throw new Error(`Failed to load lesson. Status: ${res.status}`);
        }

        const lesson = await res.json();

        const lessonTitle = document.getElementById("lessonTitle");
        const lessonContent = document.getElementById("lessonContent");
        const lessonVideo = document.getElementById("lessonVideo");
        const codeEditor = document.getElementById("codeEditor");

        if (lessonTitle) lessonTitle.innerHTML = lesson.title || "";
        if (lessonContent) lessonContent.innerHTML = lesson.content || "";
        if (lessonVideo) lessonVideo.src = lesson.videoUrl || "";

        const exercise = lesson.exercises && lesson.exercises.length > 0
            ? lesson.exercises[0]
            : null;

        if (codeEditor && exercise) {
            codeEditor.value = exercise.starterCode || "";
            codeEditor.dataset.exerciseId = exercise.id || "";
            codeEditor.dataset.language = exercise.language || lesson.language || "";
        }
    } catch (error) {
        console.error("Error loading lesson:", error);
        alert("Failed to load lesson.");
    }
}

async function runCode() {
    const codeEditor = document.getElementById("codeEditor");
    const terminalOutput = document.getElementById("terminalOutput");

    if (!codeEditor || !terminalOutput) {
        console.error("Code editor or terminal output element not found.");
        return;
    }

    const code = codeEditor.value;
    const exerciseId = codeEditor.dataset.exerciseId || null;
    const language = codeEditor.dataset.language || null;

    if (!code || !code.trim()) {
        terminalOutput.textContent = "Please enter some code first.";
        return;
    }

    terminalOutput.textContent = "Running...";

    try {
        const response = await fetch("/api/code/run", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                language: language,
                code: code,
                exerciseId: exerciseId && exerciseId !== "" ? exerciseId : null
            })
        });

        const data = await response.json();

        if (!response.ok) {
            terminalOutput.textContent = data.message || data.output || "Run failed.";
            return;
        }

        terminalOutput.textContent = data.output || "(no output)";
    } catch (error) {
        console.error("Run code error:", error);
        terminalOutput.textContent = "Network or server error while running code.";
    }
}

async function completeLesson(lessonId) {
    if (!lessonId) {
        alert("Invalid lesson ID.");
        return false;
    }

    try {
        const response = await fetch("/api/progress/completeLesson", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                lessonId: lessonId
            })
        });

        const data = await response.json();

        if (!response.ok || !data.success) {
            alert(data.message || "Failed to complete lesson.");
            return false;
        }

        alert("Lesson Completed!");
        return true;
    } catch (error) {
        console.error("Complete lesson error:", error);
        alert("Network or server error while completing lesson.");
        return false;
    }
}