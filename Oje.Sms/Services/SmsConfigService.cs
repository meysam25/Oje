using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.View;
using Oje.Sms.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Services
{
    public class SmsConfigService: ISmsConfigService
    {
        readonly SmsDBContext db = null;
        public SmsConfigService(SmsDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateSmsConfigVM input, int? siteSettingId)
        {
            CreateUpdateValidation(input, siteSettingId);

            Models.DB.SmsConfig newItem = new Models.DB.SmsConfig() 
            {
                Domain = input.domain,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Password = input.smsPassword.Encrypt(),
                SiteSettingId = siteSettingId.Value,
                Username = input.smsUsername,
                Type = input.type.Value
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateUpdateValidation(CreateUpdateSmsConfigVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.type == null)
                throw BException.GenerateNewException(BMessages.Please_Select_Type);
            if (input.type == Infrastructure.Enums.SmsConfigType.Magfa)
                magfaInputValidation(input, siteSettingId);
            if (db.SmsConfigs.Any(t => t.Id != input.id && t.SiteSettingId == siteSettingId && t.Type == input.type))
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
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            Models.DB.SmsConfig foundItem = db.SmsConfigs.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.SmsConfigs.Where(t => t.Id == id && t.SiteSettingId == siteSettingId).Select(t => new 
            {
                id = t.Id,
                smsUsername = t.Username,
                domain = t.Domain,
                isActive = t.IsActive,
                type = (int) t.Type
            }).FirstOrDefault();
        }

        public GridResultVM<SmsConfigMainGridResultVM> GetList(SmsConfigMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new SmsConfigMainGrid();

            var qureResult = db.SmsConfigs.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.smsUsername))
                qureResult = qureResult.Where(t => t.Username.Contains(searchInput.smsUsername));
            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<SmsConfigMainGridResultVM> 
            { 
                total = qureResult.Count() ,
                data = qureResult.OrderByDescending(t => t.Id)
                    .Skip(searchInput.skip)
                    .Take(searchInput.take)
                    .Select(t => new 
                    {
                        id = t.Id,
                        username = t.Username,
                        type = t.Type,
                        isActive = t.IsActive
                    })
                    .ToList()
                    .Select(t => new SmsConfigMainGridResultVM 
                    { 
                        row = ++row,
                        id = t.id,
                        isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName()  : BMessages.InActive.GetEnumDisplayName(),
                        type = t.type.GetEnumDisplayName(),
                        smsUsername = t.username
                    })
                    .ToList()
            };
        }

        public ApiResult Update(CreateUpdateSmsConfigVM input, int? siteSettingId)
        {
            CreateUpdateValidation(input, siteSettingId);

            Models.DB.SmsConfig foundItem = db.SmsConfigs.Where(t => t.Id == input.id && t.SiteSettingId == siteSettingId).FirstOrDefault();
            foundItem.Domain = input.domain;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            if (!string.IsNullOrEmpty(input.smsPassword))
                foundItem.Password = input.smsPassword.Encrypt();
            foundItem.Username = input.smsUsername;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
