using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.DB;
using Oje.Section.WebMain.Models.View;
using Oje.Section.WebMain.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Services
{
    public class ProposalFormReminderService: IProposalFormReminderService
    {
        readonly WebMainDBContext db = null;
        public ProposalFormReminderService
            (
                WebMainDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(ReminderCreateVM input, int? siteSettingId, IpSections ipSections)
        {
            CreateValidation(input, siteSettingId, ipSections);

            db.Entry(new ProposalFormReminder() 
            {
                CreateDate = DateTime.Now,
                Description = input.summery,
                Ip1 = ipSections.Ip1,
                Ip2 = ipSections.Ip2,
                Ip3 = ipSections.Ip3,
                Ip4 = ipSections.Ip4,
                Mobile = input.mobile.ToLongReturnZiro(),
                ProposalFormId = input.fid.ToIntReturnZiro(),
                SiteSettingId = siteSettingId.Value,
                TargetDate = input.targetDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(ReminderCreateVM input, int? siteSettingId, IpSections ipSections)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if(siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (ipSections == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (input.fid.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_ProposalForm);
            if (string.IsNullOrEmpty(input.targetDate))
                throw BException.GenerateNewException(BMessages.Please_Select_Date);
            if (input.targetDate.ConvertPersianNumberToEnglishNumber().ToEnDate() == null)
                throw BException.GenerateNewException(BMessages.Invalid_Date);
            if (!string.IsNullOrEmpty(input.summery) && input.summery.Length > 4000)
                throw BException.GenerateNewException(BMessages.To_Much_Summery);
            if (string.IsNullOrEmpty(input.mobile))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!input.mobile.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (input.targetDate.ConvertPersianNumberToEnglishNumber().ToEnDate() <= DateTime.Now)
                throw BException.GenerateNewException(BMessages.Date_Should_Be_From_Tomarow);
        }
    }
}
