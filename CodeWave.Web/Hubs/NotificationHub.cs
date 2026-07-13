using Microsoft.AspNetCore.SignalR;

namespace CodeWave.Web.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotification(string userId, string message, string type = "info")
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", new
        {
            Message = message,
            Type = type,
            Timestamp = DateTime.UtcNow
        });
    }

    public async Task SendProgressUpdate(string userId, string courseId, int progress)
    {
        await Clients.User(userId).SendAsync("ProgressUpdated", new
        {
            CourseId = courseId,
            Progress = progress,
            Timestamp = DateTime.UtcNow
        });
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnDisconnectedAsync(exception);
    }
}
