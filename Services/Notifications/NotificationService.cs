

using Microsoft.AspNetCore.SignalR;

namespace API.Services.Notifications
{
  public class NotificationService : INotificationService
  {
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationService(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public Task NotifyAllAsync<T>(string methodName, T data)
        {
            return _hub.Clients
                   .All
                   .SendAsync(methodName, data);
        }
  }
}