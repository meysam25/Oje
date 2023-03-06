using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Models.View;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFilledFormStatusLogService : IProposalFilledFormStatusLogService
    {
        readonly ProposalFormDBContext db = null;
        readonly IProposalFilledFormAdminBaseQueryService ProposalFilledFormAdminBaseQueryService = null;
        readonly IProposalFilledFormStatusLogFileService ProposalFilledFormStatusLogFileService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public ProposalFilledFormStatusLogService
            (
                ProposalFormDBContext db, 
                IProposalFilledFormAdminBaseQueryService ProposalFilledFormAdminBaseQueryService,
                IProposalFilledFormStatusLogFileService ProposalFilledFormStatusLogFileService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.db = db;
            this.ProposalFilledFormAdminBaseQueryService = ProposalFilledFormAdminBaseQueryService;
            this.ProposalFilledFormStatusLogFileService = ProposalFilledFormStatusLogFileService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public void Create(long? proposalFilledFormId, ProposalFilledFormStatus? status, DateTime now, long? userId, string description, string fullname, List<ProposalFilledFormChangeStatusFileVM> fileList, int? siteSettingId)
        {

            if (proposalFilledFormId.ToLongReturnZiro() > 0 && status != null && userId.ToLongReturnZiro() > 0)
            {
                if (!string.IsNullOrEmpty(description) && description.Length > 4000)
                    throw BException.GenerateNewException(BMessages.Description_Length_Can_Not_Be_More_Then_4000);

                var newLog = new ProposalFilledFormStatusLog()
                {
                    CreateDate = now,
                    ProposalFilledFormId = proposalFilledFormId.Value,
                    Type = status.Value,
                    UserId = userId.Value,
                    Description = description,
                    FullName = fullname,
                };

                db.Entry(newLog).State = EntityState.Added;
                db.SaveChanges();

                newLog.FilledSignature();
                db.SaveChanges();

                if (fileList != null && fileList.Count > 0)
                    foreach(var file in fileList)
                        ProposalFilledFormStatusLogFileService.Create(newLog.Id, file, siteSettingId, userId);
            }
        }

        public object GetList(ProposalFilledFormLogMainGrid searchInput, int? siteSettingId, long? userId, List<ProposalFilledFormStatus> validStatus = null)
        {
            if (searchInput == null)
                searchInput = new ProposalFilledFormLogMainGrid();

            var qureResult = ProposalFilledFormAdminBaseQueryService
                .getProposalFilledFormBaseQuery(siteSettingId, userId, HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites)
                .Where(t => t.Id == searchInput.pKey)
                .Where(t => validStatus == null || validStatus.Contains(t.Status))
                .SelectMany(t => t.ProposalFilledFormStatusLogs);

            if (!string.IsNullOrEmpty(searchInput.userFullname))
                qureResult = qureResult.Where(t => (t.User.Firstname + " " + t.User.Lastname).Contains(searchInput.userFullname));
            if (searchInput.status != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.status);
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }

            int row = searchInput.skip;
            return new
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.CreateDate).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    ppfId = t.ProposalFilledFormId,
                    status = t.Type,
                    createDate = t.CreateDate,
                    userFullname = t.User.Firstname + " " + t.User.Lastname,
                    desc = t.Description
                })
                .ToList()
                .Select(t => new
                {
                    row = ++row,
                    id = t.ppfId + "_" + ((int)t.status) + "_" + t.createDate.Ticks.ToString(),
                    status = t.status.GetEnumDisplayName(),
                    createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("hh:mm"),
                    t.userFullname,
                    t.desc
                })
                .ToList()
            };
        }
    }
}
