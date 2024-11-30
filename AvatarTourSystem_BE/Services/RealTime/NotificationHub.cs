using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RealTime
{
    using Microsoft.AspNetCore.SignalR;
    public class NotificationHub : Hub
    {
        // Hàm để client gửi tin nhắn đến server
        public async Task SendMessage(string user, string message)
        {
            // Phát lại tin nhắn đến tất cả các client khác
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        // Hàm client yêu cầu join vào một group
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        // Hàm client yêu cầu rời khỏi một group
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        // Hàm thực hiện logic khi client kết nối
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "System", "Welcome to the Notification Hub!");
            await base.OnConnectedAsync();
        }

        // Hàm thực hiện logic khi client ngắt kết nối
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} has disconnected.");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task NotifyNewDailyTour(string dailyTourId, string dailyTourName)
        {
            // Gửi thông báo tới tất cả client kết nối
            await Clients.All.SendAsync("ReceiveNewDailyTour", dailyTourId, dailyTourName);
        }
    }
}
