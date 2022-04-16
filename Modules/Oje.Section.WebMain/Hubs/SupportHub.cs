using Microsoft.AspNetCore.SignalR;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.JoinServices.Interfaces;
using Oje.Section.WebMain.Hubs.Models;
using Oje.Section.WebMain.Interfaces;
using Oje.Security.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Oje.Section.WebMain.Hubs
{
    public class SupportHUb : Hub
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IAutoAnswerOnlineChatMessageService AutoAnswerOnlineChatMessageService = null;
        readonly ISubscribeEmailService SubscribeEmailService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly IUserNotifierService UserNotifierService = null;

        public SupportHUb(
                ISiteSettingService SiteSettingService,
                IAutoAnswerOnlineChatMessageService AutoAnswerOnlineChatMessageService,
                ISubscribeEmailService SubscribeEmailService,
                IBlockAutoIpService BlockAutoIpService,
                IUserNotifierService UserNotifierService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.AutoAnswerOnlineChatMessageService = AutoAnswerOnlineChatMessageService;
            this.SubscribeEmailService = SubscribeEmailService;
            this.BlockAutoIpService = BlockAutoIpService;
            this.UserNotifierService = UserNotifierService;
        }

        public override Task OnConnectedAsync()
        {

            return base.OnConnectedAsync();
        }

        public async Task GetMessageList(string clientId)
        {
            if (!string.IsNullOrEmpty(clientId))
            {
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                await Clients.Client(Context.ConnectionId).SendAsync("messageHistory", SupportUserInfo.GetMessageList(clientId.GetHashCode(), ssId));
            }
        }

        public async Task SubscribeEmail(string email)
        {
            var httpContext = Context.GetHttpContext();
            var ssId = SiteSettingService.GetSiteSetting()?.Id;
            var clientIp = httpContext.GetIpAddress();
            await Clients.Client(Context.ConnectionId).SendAsync("notification", SubscribeEmailService.Create(email, clientIp, ssId));
        }

        public async Task GetMessageListForAdmin(string clientId)
        {
            if (!string.IsNullOrEmpty(clientId))
            {
                var httpContext = Context.GetHttpContext();
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                var loginUser = httpContext.GetLoginUser();
                if (loginUser == null)
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                if (!SupportUserInfo.IsAdmin(loginUser.UserId, ssId))
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);

                await Clients.Client(Context.ConnectionId).SendAsync("messageHistory", SupportUserInfo.GetMessageListForAdmin(clientId.GetHashCode(), ssId));
            }
        }

        public async Task Join(string clientId, string toClientId)
        {
            if (!string.IsNullOrEmpty(clientId))
            {
                var httpContext = Context.GetHttpContext();
                BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.JoinOnlineRome, BlockAutoIpAction.BeforeExecute, httpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
                var loginUser = httpContext.GetLoginUser();
                var clientIp = httpContext.GetIpAddress();
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                if (clientIp == null)
                    throw BException.GenerateNewException(BMessages.Invalid_Ip_Address);
                if (ssId.ToIntReturnZiro() <= 0)
                    throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);

                var newUser = SupportUserInfo.addNewUser(loginUser, Context.ConnectionId, clientIp, ssId, clientId);
                if (newUser.isAdmin == false)
                    await Clients.Client(Context.ConnectionId).SendAsync("groupMessages", AutoAnswerOnlineChatMessageService.GetByParentId(null, newUser.siteSettingId));
                else
                {
                    if (!string.IsNullOrEmpty(toClientId))
                    {
                        if (loginUser == null)
                            throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                        if (!SupportUserInfo.IsAdmin(loginUser.UserId, ssId))
                            throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                        SupportUserInfo.UpdateAdminClientIdForUser(clientId.GetHashCode(), toClientId.GetHashCode(), ssId);
                    }
                }
                BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.JoinOnlineRome, BlockAutoIpAction.AfterExecute, httpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);

                await Clients.Client(Context.ConnectionId).SendAsync("joinSuccess", true);
            }
        }

        public async Task SelectGroup(int? groupId)
        {
            var ssId = SiteSettingService.GetSiteSetting()?.Id;
            await Clients.Client(Context.ConnectionId).SendAsync("groupMessages", AutoAnswerOnlineChatMessageService.GetByParentId(groupId, ssId));
        }

        public async Task SendMessage(string message, string clientId)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.SendMessage, BlockAutoIpAction.BeforeExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(clientId) && message.Length < 4000)
            {
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                var clientIp = Context.GetHttpContext().GetIpAddress();
                if (ssId.ToIntReturnZiro() > 0 && clientIp != null)
                {
                    string adminsConnectionId = SupportUserInfo.AddUserMessage(message, ssId.ToIntReturnZiro(), Context.ConnectionId, clientIp, clientId.GetHashCode());
                    UserNotifierService.Notify(null, UserNotificationType.NewOnlineMessageForAdmin, null, null, "پیام جدید", ssId, "/WebMainAdmin/OnlineSupport/Index" );
                    if (!string.IsNullOrEmpty(adminsConnectionId))
                    {
                        try
                        {
                            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.SendMessage, BlockAutoIpAction.AfterExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
                            await Clients.Client(adminsConnectionId).SendAsync("reciveUserMessageForAdmin", new { message = HttpUtility.HtmlEncode(message), type = "text", cTime = DateTime.Now.ToString("HH:mm") });
                        }
                        catch { }
                    }
                }
            }
        }

        public async Task SendMapForUser(MapObj mapObj, string clientId)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.SendMapForUser, BlockAutoIpAction.BeforeExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            if (mapObj != null && !string.IsNullOrEmpty(clientId))
            {
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                var clientIp = Context.GetHttpContext().GetIpAddress();
                if (ssId.ToIntReturnZiro() > 0 && clientIp != null)
                {
                    string adminsConnectionId = SupportUserInfo.AddUserMap(mapObj, ssId.ToIntReturnZiro(), Context.ConnectionId, clientIp, clientId.GetHashCode());
                    if (!string.IsNullOrEmpty(adminsConnectionId))
                    {
                        try
                        {
                            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.SendMapForUser, BlockAutoIpAction.AfterExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
                            await Clients.Client(adminsConnectionId).SendAsync("reciveUserMessageForAdmin", new { type = "map", cTime = DateTime.Now.ToString("HH:mm"), mapLat = mapObj.mapLat, mapLon = mapObj.mapLon, mapZoom = mapObj.mapZoom });
                        }
                        catch { }
                    }
                }
            }
        }

        public async Task UploadVoiceForUser(string clientId, string link)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.SendVoiceForUser, BlockAutoIpAction.BeforeExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            if (!string.IsNullOrEmpty(link) && !string.IsNullOrEmpty(clientId))
            {
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                var httpContext = Context.GetHttpContext();
                var loginUser = httpContext.GetLoginUser();
                var clientIp = httpContext.GetIpAddress();
                if (loginUser == null)
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                if (ssId.ToIntReturnZiro() > 0 && clientIp != null)
                {
                    string adminsConnectionId = SupportUserInfo.AddUserVoice(link, ssId.ToIntReturnZiro(), Context.ConnectionId, clientIp, clientId.GetHashCode());
                    if (!string.IsNullOrEmpty(adminsConnectionId))
                    {
                        try
                        {
                            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.SendVoiceForUser, BlockAutoIpAction.AfterExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
                            await Clients.Client(adminsConnectionId).SendAsync("reciveUserMessageForAdmin", new { type = "voice", cTime = DateTime.Now.ToString("HH:mm"), link = HttpUtility.HtmlEncode(link) });
                        }
                        catch { }
                    }
                }
            }
        }

        public async Task UploadVoiceForAdmin(string clientId, string link)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.UploadVoiceForAdmin, BlockAutoIpAction.BeforeExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            if (!string.IsNullOrEmpty(link) && !string.IsNullOrEmpty(clientId))
            {
                var httpContext = Context.GetHttpContext();
                var loginUser = httpContext.GetLoginUser();
                var clientIp = httpContext.GetIpAddress();
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                if (clientIp == null)
                    throw BException.GenerateNewException(BMessages.Invalid_Ip_Address);
                if (ssId.ToIntReturnZiro() <= 0)
                    throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
                if (loginUser == null)
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                if (!SupportUserInfo.IsAdmin(loginUser.UserId, ssId))
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                if (ssId.ToIntReturnZiro() > 0 && clientIp != null)
                {
                    string adminsConnectionId = SupportUserInfo.AddUserVoiceForAdmin(link, ssId.ToIntReturnZiro(), Context.ConnectionId, clientIp, clientId.GetHashCode());
                    if (!string.IsNullOrEmpty(adminsConnectionId))
                    {
                        try
                        {
                            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.UploadVoiceForAdmin, BlockAutoIpAction.AfterExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
                            await Clients.Client(adminsConnectionId).SendAsync("reciveUserMessageForAdmin", new { type = "voice", cTime = DateTime.Now.ToString("HH:mm"), link = HttpUtility.HtmlEncode(link) });
                        }
                        catch { }
                    }
                }
            }
        }

        public async Task sendMapForAdmin(MapObj mapObj, string clientId)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.SendMapForAdmin, BlockAutoIpAction.BeforeExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            if (mapObj != null && !string.IsNullOrEmpty(clientId))
            {
                var httpContext = Context.GetHttpContext();
                var loginUser = httpContext.GetLoginUser();
                var clientIp = httpContext.GetIpAddress();
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                if (clientIp == null)
                    throw BException.GenerateNewException(BMessages.Invalid_Ip_Address);
                if (ssId.ToIntReturnZiro() <= 0)
                    throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
                if (loginUser == null)
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                if (!SupportUserInfo.IsAdmin(loginUser.UserId, ssId))
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                if (ssId.ToIntReturnZiro() > 0 && clientIp != null)
                {
                    string adminsConnectionId = SupportUserInfo.AddUserMapForAdmin(mapObj, ssId.ToIntReturnZiro(), Context.ConnectionId, clientIp, clientId.GetHashCode());
                    if (!string.IsNullOrEmpty(adminsConnectionId))
                    {
                        try
                        {
                            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.SendMapForAdmin, BlockAutoIpAction.AfterExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
                            await Clients.Client(adminsConnectionId).SendAsync("reciveUserMessageForAdmin", new { type = "map", cTime = DateTime.Now.ToString("HH:mm"), mapLat = mapObj.mapLat, mapLon = mapObj.mapLon, mapZoom = mapObj.mapZoom });
                        }
                        catch { }
                    }
                }
            }
        }

        public async Task UserTyping(string clientId)
        {
            if (!string.IsNullOrEmpty(clientId))
            {
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                if (ssId.ToIntReturnZiro() > 0)
                {
                    string adminsConnectionId = SupportUserInfo.GetUserConnectionIdForAdmin(clientId.GetHashCode(), ssId.ToIntReturnZiro());
                    if (!string.IsNullOrEmpty(adminsConnectionId))
                    {
                        try
                        {
                            await Clients.Client(adminsConnectionId).SendAsync("typing");
                        }
                        catch { }
                    }
                }
            }
        }

        public async Task UserTypingForAdmin(string clientId)
        {
            if (!string.IsNullOrEmpty(clientId))
            {
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                if (ssId.ToIntReturnZiro() > 0)
                {
                    var loginUser = Context.GetHttpContext().GetLoginUser();
                    if (loginUser == null)
                        throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                    if (!SupportUserInfo.IsAdmin(loginUser.UserId, ssId))
                        throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                    string adminsConnectionId = SupportUserInfo.GetUserConnectionId(clientId.GetHashCode(), ssId.ToIntReturnZiro());
                    if (!string.IsNullOrEmpty(adminsConnectionId))
                    {
                        try
                        {
                            await Clients.Client(adminsConnectionId).SendAsync("typing");
                        }
                        catch { }
                    }
                }
            }
        }

        public async Task UploadFileUser(string clientId, string link)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.UploadFileForUser, BlockAutoIpAction.BeforeExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            if (!string.IsNullOrEmpty(link) && !string.IsNullOrEmpty(clientId) && link.Length < 4000)
            {
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                var clientIp = Context.GetHttpContext().GetIpAddress();
                if (ssId.ToIntReturnZiro() > 0 && clientIp != null)
                {
                    string adminsConnectionId = SupportUserInfo.AddUserFile(link, ssId.ToIntReturnZiro(), Context.ConnectionId, clientIp, clientId.GetHashCode());
                    if (!string.IsNullOrEmpty(adminsConnectionId))
                    {
                        try
                        {
                            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.UploadFileForUser, BlockAutoIpAction.AfterExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
                            await Clients.Client(adminsConnectionId).SendAsync("reciveUserMessageForAdmin", new { link = HttpUtility.HtmlEncode(link), type = "file", cTime = DateTime.Now.ToString("HH:mm") });
                        }
                        catch { }
                    }
                }
            }
        }

        public async Task UploadFileAdmin(string clientId, string link)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.UploadFileForAdmin, BlockAutoIpAction.BeforeExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            if (!string.IsNullOrEmpty(link) && link.Length < 4000 && !string.IsNullOrEmpty(clientId))
            {
                var httpContext = Context.GetHttpContext();
                var loginUser = httpContext.GetLoginUser();
                if (loginUser == null)
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                if (!SupportUserInfo.IsAdmin(loginUser.UserId, ssId))
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                var clientIp = httpContext.GetIpAddress();
                if (ssId.ToIntReturnZiro() > 0 && clientIp != null)
                {
                    string userConnectionIds = SupportUserInfo.AddFileForAdmin(link, ssId.ToIntReturnZiro(), Context.ConnectionId, clientIp, clientId.GetHashCode(), loginUser.UserId);
                    if (!string.IsNullOrEmpty(userConnectionIds))
                    {
                        try
                        {
                            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.UploadFileForAdmin, BlockAutoIpAction.AfterExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
                            await Clients.Client(userConnectionIds).SendAsync("reciveUserMessage", new { link = HttpUtility.HtmlEncode(link), type = "file", cTime = DateTime.Now.ToString("HH:mm") });
                        }
                        catch { }
                    }
                }
            }
        }

        public async Task GetUserId(string clientId)
        {
            if (!string.IsNullOrEmpty(clientId))
            {
                var httpContext = Context.GetHttpContext();
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                var loginUser = httpContext.GetLoginUser();
                if (loginUser == null)
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                if (!SupportUserInfo.IsAdmin(loginUser.UserId, ssId))
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                var clientIp = httpContext.GetIpAddress();
                if (ssId.ToIntReturnZiro() > 0 && clientIp != null)
                {
                    long? userLoginId = SupportUserInfo.GetLoginUserId(clientId.GetHashCode(), ssId.ToIntReturnZiro());
                    if (userLoginId.ToLongReturnZiro() > 0)
                        await Clients.Client(Context.ConnectionId).SendAsync("userIdForUploadingFile", userLoginId);
                    else
                    {
                        try
                        {
                            await Clients.Client(Context.ConnectionId).SendAsync("notification", new { isError = true, message = BMessages.Website_User_Is_Not_Login.GetEnumDisplayName() });
                        }
                        catch { }
                        try
                        {
                            var userConnectionId = SupportUserInfo.GetUserConnectionId(clientId.GetHashCode(), ssId.ToIntReturnZiro());
                            if (!string.IsNullOrEmpty(userConnectionId))
                                await Clients.Client(userConnectionId).SendAsync("reciveUserMessage", new { message = BMessages.Website_User_For_Recive_File_From_Admin_Need_To_Be_Login.GetEnumDisplayName(), cTime = DateTime.Now.ToString("HH:mm"), type = "loginQuestion" });
                        }
                        catch { }
                    }

                }
            }
        }

        public async Task GetUserIdForVoice(string clientId)
        {
            if (!string.IsNullOrEmpty(clientId))
            {
                var httpContext = Context.GetHttpContext();
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                var loginUser = httpContext.GetLoginUser();
                if (loginUser == null)
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                if (!SupportUserInfo.IsAdmin(loginUser.UserId, ssId))
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                var clientIp = httpContext.GetIpAddress();
                if (ssId.ToIntReturnZiro() > 0 && clientIp != null)
                {
                    long? userLoginId = SupportUserInfo.GetLoginUserId(clientId.GetHashCode(), ssId.ToIntReturnZiro());
                    if (userLoginId.ToLongReturnZiro() > 0)
                        await Clients.Client(Context.ConnectionId).SendAsync("userIdForUploadingVoice", userLoginId);
                    else
                    {
                        try
                        {
                            await Clients.Client(Context.ConnectionId).SendAsync("notification", new { isError = true, message = BMessages.Website_User_Is_Not_Login.GetEnumDisplayName() });
                        }
                        catch { }
                        try
                        {
                            var userConnectionId = SupportUserInfo.GetUserConnectionId(clientId.GetHashCode(), ssId.ToIntReturnZiro());
                            if (!string.IsNullOrEmpty(userConnectionId))
                                await Clients.Client(userConnectionId).SendAsync("reciveUserMessage", new { message = BMessages.Website_User_For_Recive_File_From_Admin_Need_To_Be_Login.GetEnumDisplayName(), cTime = DateTime.Now.ToString("HH:mm"), type = "loginQuestion" });
                        }
                        catch { }
                    }

                }
            }
        }

        public async Task SendMessageForAdmin(string message, string clientId)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.SendMessageForAdmin, BlockAutoIpAction.BeforeExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            if (!string.IsNullOrEmpty(message) && message.Length < 4000 && !string.IsNullOrEmpty(clientId))
            {
                var httpContext = Context.GetHttpContext();
                var loginUser = httpContext.GetLoginUser();
                if (loginUser == null)
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                var ssId = SiteSettingService.GetSiteSetting()?.Id;
                if (!SupportUserInfo.IsAdmin(loginUser.UserId, ssId))
                    throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
                var clientIp = httpContext.GetIpAddress();
                if (ssId.ToIntReturnZiro() > 0 && clientIp != null)
                {
                    string userConnectionIds = SupportUserInfo.AddUserMessageForAdmin(message, ssId.ToIntReturnZiro(), Context.ConnectionId, clientIp, clientId.GetHashCode(), loginUser.UserId);
                    if (!string.IsNullOrEmpty(userConnectionIds))
                    {
                        try
                        {
                            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.SendMessageForAdmin, BlockAutoIpAction.AfterExecute, Context.GetHttpContext().GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
                            await Clients.Client(userConnectionIds).SendAsync("reciveUserMessage", new { message = HttpUtility.HtmlEncode(message), type = "text", cTime = DateTime.Now.ToString("HH:mm") });
                        }
                        catch { }
                    }
                }
            }
        }

        public async Task LikeDislike(int? groupId, bool isLike)
        {
            var ssId = SiteSettingService.GetSiteSetting()?.Id;
            var clientIp = Context.GetHttpContext().GetIpAddress();
            await AutoAnswerOnlineChatMessageService.LikeOrDislike(groupId, isLike, ssId, clientIp);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            SupportUserInfo.removeUserBy();

            return base.OnDisconnectedAsync(exception);
        }
    }
}
