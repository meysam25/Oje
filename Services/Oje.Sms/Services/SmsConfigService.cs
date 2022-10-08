using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.View;
using Oje.Sms.Services.EContext;

namespace Oje.Sms.Services
{
    public class SmsConfigService : ISmsConfigService
    {
        readonly SmsDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        public SmsConfigService(SmsDBContext db, IHttpContextAccessor HttpContextAccessor)
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ApiResult Create(CreateUpdateSmsConfigVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            CreateUpdateValidation(input, siteSettingId, canSetSiteSetting);

            if (input.isActive == true)
            {
                var allItems = db.SmsConfigs.Where(t => t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId)).ToList();
                foreach (var item in allItems)
                    item.IsActive = false;
            }

            Models.DB.SmsConfig newItem = new Models.DB.SmsConfig()
            {
                Domain = input.domain,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Password = input.smsPassword.Encrypt(),
                SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value,
                Username = input.smsUsername,
                Type = input.type.Value,
                PhoneNumber = input.ph
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateUpdateValidation(CreateUpdateSmsConfigVM input, int? siteSettingId, bool? canSetSiteSetting)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (input.type == Infrastructure.Enums.SmsConfigType.Magfa)
                magfaInputValidation(input, siteSettingId);


            if (db.SmsConfigs.Any(t => t.Id != input.id && t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId) && t.Type == input.type))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        private void magfaInputValidation(CreateUpdateSmsConfigVM input, int? siteSettingId)
        {
            if (string.IsNullOrEmpty(input.smsUsername))
                throw BException.GenerateNewException(BMessages.Please_Enter_Username);
            if (string.IsNullOrEmpty(input.smsPassword) && input.id.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Password);
            if (string.IsNullOrEmpty(input.domain))
                throw BException.GenerateNewException(BMessages.Please_Enter_Domain);
            if (string.IsNullOrEmpty(input.ph))
                throw BException.GenerateNewException(BMessages.Please_Enter_PhoneNumber);
            if (input.ph.Length > 20)
                throw BException.GenerateNewException(BMessages.PhoneNumber_Can_Not_Be_More_Then_20_chars);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            Models.DB.SmsConfig foundItem = db.SmsConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.SmsConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    smsUsername = t.Username,
                    domain = t.Domain,
                    isActive = t.IsActive,
                    type = (int)t.Type,
                    ph = t.PhoneNumber,
                    cSOWSiteSettingId = t.SiteSettingId,
                    cSOWSiteSettingId_Title = t.SiteSetting.Title
                }).FirstOrDefault();
        }

        public GridResultVM<SmsConfigMainGridResultVM> GetList(SmsConfigMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new SmsConfigMainGrid();

            var qureResult = db.SmsConfigs.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.smsUsername))
                qureResult = qureResult.Where(t => t.Username.Contains(searchInput.smsUsername));
            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.ph))
                qureResult = qureResult.Where(t => t.PhoneNumber.Contains(searchInput.ph));
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            int row = searchInput.skip;

            return new GridResultVM<SmsConfigMainGridResultVM>
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id)
                    .Skip(searchInput.skip)
                    .Take(searchInput.take)
                    .Select(t => new
                    {
                        id = t.Id,
                        username = t.Username,
                        type = t.Type,
                        isActive = t.IsActive,
                        ph = t.PhoneNumber,
                        siteTitleMN2 = t.SiteSetting.Title
                    })
                    .ToList()
                    .Select(t => new SmsConfigMainGridResultVM
                    {
                        row = ++row,
                        id = t.id,
                        isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                        type = t.type.GetEnumDisplayName(),
                        smsUsername = t.username,
                        ph = t.ph,
                        siteTitleMN2 = t.siteTitleMN2
                    })
                    .ToList()
            };
        }

        public ApiResult Update(CreateUpdateSmsConfigVM input, int? siteSettingId)
        {
            bool? canSetSiteSetting = HttpContextAccessor.HttpContext?.GetLoginUser()?.canSeeOtherWebsites;
            CreateUpdateValidation(input, siteSettingId, canSetSiteSetting);

            Models.DB.SmsConfig foundItem = db.SmsConfigs
                .getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId)
                .Where(t => t.Id == input.id)
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (input.isActive == true)
            {
                var allItems = db.SmsConfigs.Where(t => t.SiteSettingId == (canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId : siteSettingId) && t.Id != input.id).ToList();
                foreach (var item in allItems)
                    item.IsActive = false;
            }

            foundItem.Domain = input.domain;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            if (!string.IsNullOrEmpty(input.smsPassword))
                foundItem.Password = input.smsPassword.Encrypt();
            foundItem.Username = input.smsUsername;
            foundItem.PhoneNumber = input.ph;
            foundItem.SiteSettingId = canSetSiteSetting == true && input.cSOWSiteSettingId.ToIntReturnZiro() > 0 ? input.cSOWSiteSettingId.Value : siteSettingId.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public Models.DB.SmsConfig GetActive(int? siteSettingId)
        {
            return db.SmsConfigs.Where(t => t.SiteSettingId == siteSettingId && t.IsActive == true).FirstOrDefault();
        }
    }
}
