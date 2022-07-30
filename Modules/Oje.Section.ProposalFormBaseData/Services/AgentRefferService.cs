using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.DB;
using Oje.Section.ProposalFormBaseData.Models.View;
using Oje.Section.ProposalFormBaseData.Services.EContext;
using System;
using System.Linq;

namespace Oje.Section.ProposalFormBaseData.Services
{
    public class AgentRefferService : IAgentRefferService
    {
        readonly ProposalFormBaseDataDBContext db = null;
        public AgentRefferService(ProposalFormBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(AgentRefferCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            db.Entry(new AgentReffer()
            {
                Address = input.address,
                Code = input.code,
                CompanyId = input.cid.Value,
                FullName = input.fullname,
                Mobile = input.mobile,
                SiteSettingId = siteSettingId.Value,
                Tell = input.tell
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(AgentRefferCreateUpdateVM input, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (input.cid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (string.IsNullOrEmpty(input.fullname))
                throw BException.GenerateNewException(BMessages.Please_Select_FullName);
            if (string.IsNullOrEmpty(input.mobile))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!input.mobile.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (string.IsNullOrEmpty(input.address))
                throw BException.GenerateNewException(BMessages.Please_Enter_Address);
            if (input.address.Length > 4000)
                throw BException.GenerateNewException(BMessages.Address_Length_Can_Not_Be_More_Then_4000);
            if (string.IsNullOrEmpty(input.code))
                throw BException.GenerateNewException(BMessages.Please_Enter_Code);
            if (db.AgentReffers.Any(t => t.CompanyId == input.cid && t.SiteSettingId == siteSettingId && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id, int? siteSettingId)
        {
            var foundItem = db.AgentReffers.Where(t => t.SiteSettingId == siteSettingId && t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id, int? siteSettingId)
        {
            return db.AgentReffers
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    cid = t.CompanyId,
                    code = t.Code,
                    fullname = t.FullName,
                    mobile = t.Mobile,
                    address = t.Address,
                    tell = t.Tell
                })
                .FirstOrDefault();
        }

        public GridResultVM<AgentRefferMainGridResultVM> GetList(AgentRefferMainGrid searchInput, int? siteSettingId)
        {
            searchInput = searchInput ?? new AgentRefferMainGrid();

            var quiryResult = db.AgentReffers.Where(t => t.SiteSettingId == siteSettingId);

            if (!string.IsNullOrEmpty(searchInput.companyTitle))
                quiryResult = quiryResult.Where(t => t.Company.Title.Contains(searchInput.companyTitle));
            if (!string.IsNullOrEmpty(searchInput.code))
                quiryResult = quiryResult.Where(t => t.Code.Contains(searchInput.code));
            if (!string.IsNullOrEmpty(searchInput.fullname))
                quiryResult = quiryResult.Where(t => t.FullName.Contains(searchInput.fullname));
            if (!string.IsNullOrEmpty(searchInput.mobile))
                quiryResult = quiryResult.Where(t => t.Mobile == searchInput.mobile);

            int row = searchInput.skip;

            return new GridResultVM<AgentRefferMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    companyTitle = t.Company.Title,
                    code = t.Code,
                    fullname = t.FullName,
                    mobile = t.Mobile
                })
                .ToList()
                .Select(t => new AgentRefferMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    code = t.code,
                    fullname = t.fullname,
                    companyTitle = t.companyTitle,
                    mobile = t.mobile
                })
                .ToList()
            };
        }

        public ApiResult Update(AgentRefferCreateUpdateVM input, int? siteSettingId)
        {
            createUpdateValidation(input, siteSettingId);

            var foundItem = db.AgentReffers.Where(t => t.SiteSettingId == siteSettingId && t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Address = input.address;
            foundItem.Code = input.code;
            foundItem.CompanyId = input.cid.Value;
            foundItem.FullName = input.fullname;
            foundItem.Mobile = input.mobile;
            foundItem.Tell = input.tell;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}