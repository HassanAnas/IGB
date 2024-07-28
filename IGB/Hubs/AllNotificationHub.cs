
using IGB.Models;
using Microsoft.AspNetCore.SignalR;

namespace IGB.Hubs
{
    public class AllNotificationHub : Hub
    {
        public void RefreshAdmin(List<AllNotification> notifications)
        {
            Clients.All.SendAsync("RefreshAdmin", notifications);
        }
        public void RefreshTutor(List<AllNotification> notifications)
        {
            Clients.All.SendAsync("RefreshTutor", notifications);
        }
        public void RefreshStudent(List<AllNotification> notifications)
        {
            Clients.All.SendAsync("RefreshStudent", notifications);
        }
    }
}