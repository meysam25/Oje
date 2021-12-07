using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.View;
using Oje.ProposalFormManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Services
{
    public class ProposalFilledFormAdminManager : IProposalFilledFormAdminManager
    {
        readonly ProposalFormDBContext db = null;
        readonly AccountManager.Interfaces.IUserManager UserManager = null;
        public ProposalFilledFormAdminManager(
                ProposalFormDBContext db,
                AccountManager.Interfaces.IUserManager UserManager
            )
        {
            this.UserManager = UserManager;
            this.db = db;
        }

        public ApiResult Delete(long? id, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            var allChildUserId = UserManager.GetChildsUserId(userId.ToLongReturnZiro());
            var foundItem = db.ProposalFilledForms
                .Where(t => t.SiteSettingId == siteSettingId && t.Id == id && t.Status == status &&
                            (allChildUserId == null || t.ProposalFilledFormUsers.Any(tt => allChildUserId.Contains(tt.UserId)))
                        )
                .FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.IsDelete = true;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public GridResultVM<ProposalFilledFormMainGridResult> GetList(ProposalFilledFormMainGrid searchInput, int? siteSettingId, long? userId, ProposalFilledFormStatus status)
        {
            if (searchInput == null)
                searchInput = new ProposalFilledFormMainGrid();

            var allChildUserId = UserManager.GetChildsUserId(userId.ToLongReturnZiro());
            var qureResult = db.ProposalFilledForms.Where(t => t.IsDelete != true && t.SiteSettingId == siteSettingId && t.Status == status && (allChildUserId == null || t.ProposalFilledFormUsers.Any(tt => allChildUserId.Contains(tt.UserId))));

            if (searchInput.cId.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.ProposalFilledFormCompanies.Any(tt => tt.CompanyId == searchInput.cId));
            if (!string.IsNullOrEmpty(searchInput.ppfTitle))
                qureResult = qureResult.Where(t => t.ProposalForm.Title.Contains(searchInput.ppfTitle));
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDT = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDT.Year && t.CreateDate.Month == targetDT.Month && t.CreateDate.Day == targetDT.Day);
            }
            if (searchInput.price.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Price == searchInput.price);
            if (!string.IsNullOrEmpty(searchInput.agentFullname))
                qureResult = qureResult.Where(t => t.ProposalFilledFormUsers.Any(tt => tt.Type == ProposalFilledFormUserType.Agent && (tt.User.Firstname + " " + tt.User.Lastname).Contains(searchInput.agentFullname)));
            if (!string.IsNullOrEmpty(searchInput.targetUserfullname))
                qureResult = qureResult.Where(t => t.ProposalFilledFormUsers.Any(tt => tt.Type == ProposalFilledFormUserType.OwnerUser && (tt.User.Firstname + " " + tt.User.Lastname).Contains(searchInput.targetUserfullname)));
            if (!string.IsNullOrEmpty(searchInput.createUserfullname))
                qureResult = qureResult.Where(t => t.ProposalFilledFormUsers.Any(tt => tt.Type == ProposalFilledFormUserType.CreateUser && (tt.User.Firstname + " " + tt.User.Lastname).Contains(searchInput.createUserfullname)));
            if (!string.IsNullOrEmpty(searchInput.fromCreateDate) && searchInput.fromCreateDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDT = searchInput.fromCreateDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate >= targetDT);
            }
            if (!string.IsNullOrEmpty(searchInput.toCreateDate) && searchInput.toCreateDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDT = searchInput.toCreateDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate <= targetDT);
            }
            if (!string.IsNullOrEmpty(searchInput.fromIssueDate) && searchInput.fromIssueDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDT = searchInput.fromIssueDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.IssueDate != null && t.IssueDate >= targetDT);
            }
            if (!string.IsNullOrEmpty(searchInput.toIssueDate) && searchInput.toIssueDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDT = searchInput.toIssueDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.IssueDate != null && t.IssueDate <= targetDT);
            }
            if(!string.IsNullOrEmpty(searchInput.targetUserMobileNumber))
                qureResult = qureResult.Where(t => t.ProposalFilledFormUsers.Any(tt => tt.Type == ProposalFilledFormUserType.CreateUser && tt.User.Username.Contains(searchInput.createUserfullname)));

            int row = searchInput.skip;

            return new GridResultVM<ProposalFilledFormMainGridResult>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    cId = t.ProposalFilledFormCompanies.Select(tt => tt.Company.Title).ToList(),
                    ppfTitle = t.ProposalForm.Title,
                    createDate = t.CreateDate,
                    price = t.Price,
                    agentFullname = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.Agent).Select(tt => tt.User.Firstname + " " + tt.User.Lastname).FirstOrDefault(),
                    targetUserfullname = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.OwnerUser).Select(tt => tt.User.Firstname + " " + tt.User.Lastname).FirstOrDefault(),
                    targetUserMobileNumber = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.OwnerUser).Select(tt => tt.User.Username).FirstOrDefault(),
                    createUserfullname = t.ProposalFilledFormUsers.Where(tt => tt.Type == ProposalFilledFormUserType.CreateUser).Select(tt => tt.User.Firstname + " " + tt.User.Lastname).FirstOrDefault(),
                    issueDate = t.IssueDate,
                    startDate = t.InsuranceStartDate,
                    endDate = t.InsuranceEndDate
                })
                .ToList()
                .Select(t => new ProposalFilledFormMainGridResult
                {
                    row = ++row,
                    id = t.id,
                    cId = String.Join(",", t.cId),
                    ppfTitle = t.ppfTitle,
                    createDate = t.createDate.ToFaDate(),
                    price = t.price > 0 ? t.price.ToString("###,###") : "0",
                    agentFullname = t.agentFullname,
                    targetUserfullname = t.targetUserfullname,
                    createUserfullname = t.createUserfullname,
                    targetUserMobileNumber = t.targetUserMobileNumber,
                    issueDate = t.issueDate != null?  t.issueDate.ToFaDate() : "",
                    startDate = t.startDate != null ? t.startDate.ToFaDate() : "",
                    endDate = t.endDate != null ? t.endDate.ToFaDate() : ""
                })
                .ToList()
            };
        }
    }
}
