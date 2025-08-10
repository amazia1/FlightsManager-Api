using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Notifications
{
    public interface INotificationService
    {
        Task NotifyAllAsync<T>(string methodName, T payload);
    }
}