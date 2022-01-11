using Microsoft.AspNetCore.SignalR;
using Oje.AccountService.Hubs.Models;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Hubs
{
    public class NotificationHub : Hub
    {
        public NotificationHub()
        {

        }

        public override Task OnConnectedAsync()
        {
            var loginUser = Context.GetHttpContext().GetLoginUser();
            if (loginUser == null)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);

            UserInfo.addNewUser(loginUser, Context.ConnectionId);

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            UserInfo.removeUserBy(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public static void SendNotification(IHubContext<NotificationHub> context, string subject, string body, List<long> userIds, int siteSettingId)
        {
            if (context != null && !string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(body) && userIds != null && userIds.Count > 0 && siteSettingId > 0)
            {
                List<UserInfo> allConnections = UserInfo.GetBy(userIds, siteSettingId);
                foreach (UserInfo info in allConnections)
                {
                    context.Clients.Client(info.connectionId).SendAsync("notify", subject, body);
                }
            }
        }
    }
}
