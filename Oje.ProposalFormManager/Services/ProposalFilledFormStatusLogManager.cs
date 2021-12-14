using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Models.View;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Services
{
    public class ProposalFilledFormStatusLogManager : IProposalFilledFormStatusLogManager
    {
        readonly ProposalFormDBContext db = null;
        readonly IProposalFilledFormAdminBaseQueryManager ProposalFilledFormAdminBaseQueryManager = null;
        public ProposalFilledFormStatusLogManager(ProposalFormDBContext db, IProposalFilledFormAdminBaseQueryManager ProposalFilledFormAdminBaseQueryManager)
        {
            this.db = db;
            this.ProposalFilledFormAdminBaseQueryManager = ProposalFilledFormAdminBaseQueryManager;
        }

        public void Create(long? proposalFilledFormId, ProposalFilledFormStatus? status, DateTime now, long? userId, string description)
        {
            if (proposalFilledFormId.ToLongReturnZiro() > 0 && status != null && userId.ToLongReturnZiro() > 0)
            {
                db.Entry(new ProposalFilledFormStatusLog()
                {
                    CreateDate = now,
                    ProposalFilledFormId = proposalFilledFormId.Value,
                    Type = status.Value,
                    UserId = userId.Value
                }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();
            }
        }

        public object GetList(ProposalFilledFormLogMainGrid searchInput, int? siteSettingId, long? userId)
        {
            if (searchInput == null)
                searchInput = new ProposalFilledFormLogMainGrid();

            var qureResult = ProposalFilledFormAdminBaseQueryManager
                .getProposalFilledFormBaseQuery(siteSettingId, userId)
                .Where(t => t.Id == searchInput.pKey)
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
                    userFullname = t.User.Firstname + " " + t.User.Lastname
                })
                .ToList()
                .Select(t => new
                {
                    row = ++row,
                    id = t.ppfId + "_" + ((int)t.status) + "_" + t.createDate.Ticks.ToString(),
                    status = t.status.GetEnumDisplayName(),
                    createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("hh:mm"),
                    t.userFullname
                })
                .ToList()
            };
        }
    }
}
