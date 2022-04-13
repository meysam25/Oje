using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.DB;
using Oje.Section.WebMain.Services.EContext;
using Oje.Security.Interfaces;
using System;

namespace Oje.Section.WebMain.Services
{
    public class SubscribeEmailService : ISubscribeEmailService
    {
        readonly WebMainDBContext db = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        public SubscribeEmailService
            (
                WebMainDBContext db,
                IBlockAutoIpService BlockAutoIpService
            )
        {
            this.db = db;
            this.BlockAutoIpService = BlockAutoIpService;
        }

        public object Create(string email, IpSections clientIp, int? siteSettingId)
        {
            try
            {
                createValidation(email, clientIp, siteSettingId);

                BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.SubscribeNewEmail, BlockAutoIpAction.BeforeExecute, clientIp, siteSettingId);
                db.Entry(new SubscribeEmail() 
                {
                    Email = email,
                    SiteSettingId = siteSettingId.Value,
                    Ip1 = clientIp.Ip1,
                    Ip2 = clientIp.Ip2,
                    Ip3 = clientIp.Ip3,
                    Ip4 = clientIp.Ip4
                }).State = EntityState.Added;
                db.SaveChanges();
                BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.SubscribeNewEmail, BlockAutoIpAction.AfterExecute, clientIp, siteSettingId);
                return new { isError = false, message = BMessages.Operation_Was_Successfull.GetEnumDisplayName() };
            }
            catch (BException ex)
            {
                return new { isError = true, message = ex.Message };
            }
            catch (Exception )
            {
                return new { isError = true, message = BMessages.UnknownError.GetEnumDisplayName() };
            }
        }

        private void createValidation(string email, IpSections clientIp, int? siteSettingId)
        {
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(email))
                throw BException.GenerateNewException(BMessages.Please_Enter_Email);
            if (!email.IsValidEmail())
                throw BException.GenerateNewException(BMessages.Invalid_Email);
            if (clientIp == null)
                throw BException.GenerateNewException(BMessages.Invalid_Ip_Address);
        }
    }
}
