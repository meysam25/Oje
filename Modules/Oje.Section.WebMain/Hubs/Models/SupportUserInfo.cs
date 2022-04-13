using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Hubs.Models
{
    public class SupportUserInfo
    {
        public SupportUserInfo()
        {
            UserMessages = new();
        }

        public string connectionId { get; set; }
        public DateTime createDate { get; set; }
        public LoginUserVM loginUserObj { get; set; }
        public IpSections clientIp { get; set; }
        public bool isAdmin { get; set; }
        public int siteSettingId { get; set; }
        public string clientId { get; set; }
        public long clientIdLong { get; set; }
        public SupportUserInfo AdminUser { get; set; }
        public List<UserMessage> UserMessages { get; set; }

        public static List<SupportUserInfo> SupportUserInfos { get; set; }


        public static SupportUserInfo addNewUser(LoginUserVM newUser, string connectionId, IpSections clientIp, int? siteSettingId, string clientId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var clientHash = clientId.GetHashCode();
                var foundItem = SupportUserInfos.Where(t => t.clientIdLong == clientHash && t.siteSettingId == siteSettingId).FirstOrDefault();
                if (foundItem != null)
                {
                    foundItem.connectionId = connectionId;
                    foundItem.loginUserObj = newUser;
                    return foundItem;
                }
                else
                {
                    var newItem = new SupportUserInfo()
                    {
                        clientId = clientId,
                        clientIdLong = clientHash,
                        connectionId = connectionId,
                        loginUserObj = newUser,
                        createDate = DateTime.Now,
                        clientIp = clientIp,
                        isAdmin = newUser != null && newUser.roles != null && newUser.roles.Any(t => !string.IsNullOrEmpty(t) && GlobalConfig.GetValidRoleForSupports().Contains(t.ToLower()) && t.ToLower() != "user") ? true : false,
                        siteSettingId = siteSettingId.Value
                    };
                    SupportUserInfos.Add(newItem);
                    return newItem;
                }
            }
        }

        public static string AddUserMessage(string message, int siteSettingId, string connectionId, IpSections clientIp, long clientId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null)
                {
                    foundUser.UserMessages.Add(new UserMessage() { CreateDate = DateTime.Now, IsAdmin = false, Message = message, ClientIp = clientIp, connectionId = connectionId });
                    return foundUser.AdminUser?.connectionId;
                }
            }

            return null;
        }

        public static string AddUserFile(string link, int siteSettingId, string connectionId, IpSections clientIp, long clientId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null)
                {
                    foundUser.UserMessages.Add(new UserMessage() { CreateDate = DateTime.Now, IsAdmin = false, Link = link, ClientIp = clientIp, connectionId = connectionId });
                    return foundUser.AdminUser?.connectionId;
                }
            }

            return null;
        }

        internal static void UpdateAdminClientIdForUser(long clientId, long toClientId, int? siteSettingId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == toClientId).FirstOrDefault();
                var foundAdmin = SupportUserInfos.Where(t => t != null && t.isAdmin == true && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null && foundAdmin != null)
                    foundUser.AdminUser = foundAdmin;
            }
        }

        internal static object GetMessageList(long clientId, int? siteSettingId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null && foundUser.UserMessages != null)
                {
                    return foundUser.UserMessages
                        .OrderBy(t => t.CreateDate)
                        .Select(t => new
                        {
                            message = t.Message,
                            isAdmin = t.IsAdmin,
                            type = !string.IsNullOrEmpty(t.voiceLink) ? "voice" : t.MapObj != null ? "map" : !string.IsNullOrEmpty(t.Message) ? "text" : "file",
                            cTime = t.CreateDate.ToString("HH:mm"),
                            link = !string.IsNullOrEmpty(t.voiceLink) ? t.voiceLink : t.Link,
                            mapLat = t.MapObj?.mapLat,
                            mapLon = t.MapObj?.mapLon,
                            mapZoom = t.MapObj?.mapZoom
                        }).ToList();
                }
            }

            return null;
        }

        internal static object GetMessageListForAdmin(long clientId, int? siteSettingId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null && foundUser.UserMessages != null)
                {
                    return foundUser.UserMessages
                        .OrderBy(t => t.CreateDate)
                        .Select(t => new
                        {
                            message = t.Message,
                            isAdmin = !t.IsAdmin,
                            type = !string.IsNullOrEmpty(t.voiceLink) ? "voice" : t.MapObj != null ? "map" : !string.IsNullOrEmpty(t.Message) ? "text" : "file",
                            cTime = t.CreateDate.ToString("HH:mm"),
                            link = !string.IsNullOrEmpty(t.voiceLink) ? t.voiceLink : t.Link,
                            mapLat = t.MapObj?.mapLat,
                            mapLon = t.MapObj?.mapLon,
                            mapZoom = t.MapObj?.mapZoom
                        }).ToList();
                }
            }

            return null;
        }

        internal static string AddUserVoice(string link, int siteSettingId, string connectionId, IpSections clientIp, int clientId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null)
                {
                    foundUser.UserMessages.Add(new UserMessage() { CreateDate = DateTime.Now, IsAdmin = false, ClientIp = clientIp, connectionId = connectionId, voiceLink = link });
                    return foundUser.AdminUser?.connectionId;
                }
            }

            return null;
        }

        internal static string AddUserMap(MapObj mapObj, int siteSettingId, string connectionId, IpSections clientIp, int clientId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null)
                {
                    foundUser.UserMessages.Add(new UserMessage() { CreateDate = DateTime.Now, IsAdmin = false, ClientIp = clientIp, connectionId = connectionId, MapObj = mapObj });
                    return foundUser.AdminUser?.connectionId;
                }
            }

            return null;
        }

        internal static string AddUserVoiceForAdmin(string link, int siteSettingId, string connectionId, IpSections clientIp, int clientId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null)
                {
                    foundUser.UserMessages.Add(new UserMessage() { CreateDate = DateTime.Now, IsAdmin = true, ClientIp = clientIp, connectionId = connectionId, voiceLink = link });
                    return foundUser.connectionId;
                }
            }

            return null;
        }

        internal static object Delete(long clientId, int? siteSettingId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();
            lock (SupportUserInfos)
            {
                long clientIdLong = clientId.GetHashCode();
                var foundItem = SupportUserInfos.Where(t => t != null && t.clientIdLong == clientIdLong && t.siteSettingId == siteSettingId).FirstOrDefault();
                if (foundItem != null)
                    SupportUserInfos.Remove(foundItem);
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        internal static string AddUserMapForAdmin(MapObj mapObj, int siteSettingId, string connectionId, IpSections clientIp, int clientId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null)
                {
                    foundUser.UserMessages.Add(new UserMessage() { CreateDate = DateTime.Now, IsAdmin = true, ClientIp = clientIp, connectionId = connectionId, MapObj = mapObj });
                    return foundUser.connectionId;
                }
            }

            return null;
        }

        internal static object GetList(OnlineSupportMainGrid searchInput, int? siteSettingId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();
            var quiryResult = SupportUserInfos.Where(t => t != null && t.siteSettingId == siteSettingId && t.isAdmin == false && t.UserMessages.Count() > 0);

            int row = searchInput.skip;

            return new
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.createDate)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    row = ++row,
                    id = t.clientId,
                    createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("HH:mm"),
                    ipAddress = t.clientIp != null ? t.clientIp.ToString() : "",
                    mCount = t.UserMessages.Count(),
                    lastMessageDate = t.UserMessages.Where(t => t != null && t.IsAdmin == false).Count() > 0 ? t.UserMessages.Where(t => t != null && t.IsAdmin == false).OrderByDescending(tt => tt.CreateDate).Select(t => t.CreateDate as DateTime?).FirstOrDefault() : null,
                    isYouAnswered = t.UserMessages.OrderByDescending(tt => tt.CreateDate).Select(t => t.IsAdmin).FirstOrDefault()
                })
                .ToList()
                .Select(t => new
                {
                    t.row,
                    t.id,
                    t.createDate,
                    t.ipAddress,
                    t.mCount,
                    lastMessageDate = t.lastMessageDate != null ? t.lastMessageDate.ToFaDate() + " " + t.lastMessageDate.Value.ToString("HH:mm") : "",
                    isYouAnswered = t.isYouAnswered == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        internal static long? GetLoginUserId(long clientId, int siteSettingId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null)
                    return foundUser.loginUserObj?.UserId;
            }
            return null;
        }

        internal static string GetUserConnectionId(int clientId, int siteSettingId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null)
                    return foundUser.connectionId;
            }
            return null;
        }

        internal static string GetUserConnectionIdForAdmin(int clientId, int siteSettingId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null)
                    return foundUser.AdminUser?.connectionId;
            }
            return null;
        }

        public static void removeUserBy()
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();
            lock (SupportUserInfos)
            {
                SupportUserInfos = SupportUserInfos.Where(t => t != null && (DateTime.Now - t.createDate).TotalHours <= 10).ToList();
            }
        }

        internal static bool IsAdmin(long userId, int? siteSettingId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();
            lock (SupportUserInfos)
            {
                return SupportUserInfos.Any(t => t != null && t.loginUserObj != null && t.loginUserObj.UserId == userId && t.isAdmin == true && t.siteSettingId == siteSettingId);
            }
        }

        internal static string AddUserMessageForAdmin(string message, int siteSettingId, string connectionId, IpSections clientIp, long clientId, long userId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null)
                {
                    var foundAdminUser = SupportUserInfos.Where(t => t.loginUserObj != null && t.loginUserObj.UserId == userId && t.siteSettingId == siteSettingId).FirstOrDefault();
                    if (foundAdminUser == null)
                        return null;
                    foundUser.UserMessages.Add(new UserMessage() { CreateDate = DateTime.Now, IsAdmin = true, Message = message, ClientIp = clientIp, connectionId = connectionId });
                    return foundUser.connectionId;
                }
            }

            return null;
        }

        internal static string AddFileForAdmin(string link, int siteSettingId, string connectionId, IpSections clientIp, long clientId, long userId)
        {
            if (SupportUserInfos == null)
                SupportUserInfos = new();

            lock (SupportUserInfos)
            {
                var foundUser = SupportUserInfos.Where(t => t != null && t.isAdmin == false && t.siteSettingId == siteSettingId && t.clientIdLong == clientId).FirstOrDefault();
                if (foundUser != null)
                {
                    var foundAdminUser = SupportUserInfos.Where(t => t.loginUserObj != null && t.loginUserObj.UserId == userId && t.siteSettingId == siteSettingId).FirstOrDefault();
                    if (foundAdminUser == null)
                        return null;
                    foundUser.UserMessages.Add(new UserMessage() { CreateDate = DateTime.Now, IsAdmin = true, Link = link, ClientIp = clientIp, connectionId = connectionId });
                    return foundUser.connectionId;
                }
            }

            return null;
        }
    }
}
