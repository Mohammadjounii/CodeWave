// SignalR Client for CodeWave
// Provides real-time notifications and updates

let connection = null;

// Initialize SignalR connection
function initializeSignalR() {
    if (connection) {
        return; // Already initialized
    }

    connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .withAutomaticReconnect()
        .build();

    // Handle notifications
    connection.on("ReceiveNotification", function (notification) {
        showNotification(notification.message, notification.type || "info");
    });

    // Handle progress updates
    connection.on("ProgressUpdated", function (data) {
        updateProgressDisplay(data.courseId, data.progress);
    });

    // Start connection
    connection.start()
        .then(function () {
            console.log("SignalR connected successfully");
        })
        .catch(function (err) {
            console.error("SignalR connection error: ", err);
        });

    // Handle reconnection
    connection.onreconnecting(function () {
        console.log("SignalR reconnecting...");
    });

    connection.onreconnected(function () {
        console.log("SignalR reconnected");
    });
}

// Show notification to user
function showNotification(message, type) {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `fixed top-4 right-4 p-4 rounded-lg shadow-lg z-50 max-w-sm animate-fade-in ${
        type === 'error' ? 'bg-red-500 text-white' :
        type === 'success' ? 'bg-green-500 text-white' :
        type === 'warning' ? 'bg-yellow-500 text-white' :
        'bg-blue-500 text-white'
    }`;
    notification.innerHTML = `
        <div class="flex items-center justify-between">
            <span>${escapeHtml(message)}</span>
            <button onclick="this.parentElement.parentElement.remove()" class="ml-4 text-white hover:text-gray-200">×</button>
        </div>
    `;
    
    document.body.appendChild(notification);
    
    // Auto-remove after 5 seconds
    setTimeout(() => {
        if (notification.parentElement) {
            notification.remove();
        }
    }, 5000);
}

// Update progress display
function updateProgressDisplay(courseId, progress) {
    // Find progress elements and update them
    const progressElements = document.querySelectorAll(`[data-course-id="${courseId}"] .progress-bar`);
    progressElements.forEach(element => {
        element.style.width = `${progress}%`;
        element.setAttribute('aria-valuenow', progress);
        element.textContent = `${progress}%`;
    });
}

// Helper function to escape HTML
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initializeSignalR);
} else {
    initializeSignalR();
}
