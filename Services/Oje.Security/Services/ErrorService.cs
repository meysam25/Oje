using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class ErrorService : IErrorService
    {
        readonly SecurityDBContext db = null;
        public ErrorService
            (
                SecurityDBContext db
            )
        {
            this.db = db;
        }

        public void Create(long? UserId, string requestId, ApiResultErrorCode? type, BMessages? bMessageCode, string cMessages, IpSections ip, string cLineNumbers, string cFilenames)
        {
            if (!string.IsNullOrEmpty(requestId) && !string.IsNullOrEmpty(cMessages) && ip != null && !string.IsNullOrEmpty(cLineNumbers) && !string.IsNullOrEmpty(cFilenames))
            {
                db.Entry(new Error()
                {
                    UserId = UserId,
                    RequestId = requestId,
                    Type = type,
                    BMessageCode = bMessageCode,
                    CreateDate = DateTime.Now,
                    FileName = cFilenames,
                    Ip1 = ip.Ip1,
                    Ip2 = ip.Ip2,
                    Ip3 = ip.Ip3,
                    Ip4 = ip.Ip4,
                    LineNumber = cLineNumbers,
                    Message = cMessages
                }).State = EntityState.Added;
                db.SaveChanges();
            }
        }

        public object GetBy(string requestId)
        {
            return db.Errors.Where(t => t.RequestId == requestId).Select(t => new
            {
                description = t.Message
            }).FirstOrDefault();
        }

        public GridResultVM<ErrorMainGridResultVM> GetList(ErrorMainGrid searchInput)
        {
            searchInput = searchInput ?? new ErrorMainGrid();

            var quiryResult = db.Errors.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.userFullname))
                quiryResult = quiryResult.Where(t => (t.UserId > 0 && !string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Firstname + " " + t.User.Lastname : t.UserId > 0 ? t.User.Username : "").Contains(searchInput.userFullname));
            if (!string.IsNullOrEmpty(searchInput.ip) && searchInput.ip.ToIp() != null)
            {
                var targetIp = searchInput.ip.ToIp();
                quiryResult = quiryResult.Where(t => t.Ip1 == targetIp.Ip1 && t.Ip2 == targetIp.Ip2 && t.Ip3 == targetIp.Ip3 && t.Ip4 == targetIp.Ip4);
            }
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.description))
                quiryResult = quiryResult.Where(t => t.Message.Contains(searchInput.description));
            if (!string.IsNullOrEmpty(searchInput.lineNumber))
                quiryResult = quiryResult.Where(t => t.LineNumber.Contains(searchInput.lineNumber));
            if (!string.IsNullOrEmpty(searchInput.fileName))
                quiryResult = quiryResult.Where(t => t.FileName.Contains(searchInput.fileName));
            if (searchInput.bMessageCode != null)
                quiryResult = quiryResult.Where(t => t.BMessageCode == searchInput.bMessageCode);
            if (searchInput.type != null)
                quiryResult = quiryResult.Where(t => t.Type == searchInput.type);


            switch (searchInput.sortField)
            {
                case "userFullname":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.UserId > 0 && !string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Firstname + " " + t.User.Lastname : t.UserId > 0 ? t.User.Username : "");
                    else
                        quiryResult = quiryResult.OrderBy(t => t.UserId > 0 && !string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Firstname + " " + t.User.Lastname : t.UserId > 0 ? t.User.Username : "");
                    break;
                case "ip":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.Ip1).ThenByDescending(t => t.Ip2).ThenByDescending(t => t.Ip3).ThenByDescending(t => t.Ip4);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.Ip1).ThenBy(t => t.Ip2).ThenBy(t => t.Ip3).ThenBy(t => t.Ip4);
                    break;
                case "createDate":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.CreateDate);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.CreateDate);
                    break;
                case "description":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.Message);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.Message);
                    break;
                case "bMessageCode":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.BMessageCode);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.BMessageCode);
                    break;
                case "type":
                    if (searchInput.sortFieldIsAsc == false)
                        quiryResult = quiryResult.OrderByDescending(t => t.Type);
                    else
                        quiryResult = quiryResult.OrderBy(t => t.Type);
                    break;
                default:
                    quiryResult = quiryResult.OrderByDescending(t => t.Id);
                    break;
            }


            int row = searchInput.skip;

            return new GridResultVM<ErrorMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    userFullname = t.UserId > 0 && !string.IsNullOrEmpty((t.User.Firstname + " " + t.User.Lastname).Trim()) ? t.User.Firstname + " " + t.User.Lastname : t.UserId > 0 ? t.User.Username : "",
                    t.Ip1,
                    t.Ip2,
                    t.Ip3,
                    t.Ip4,
                    t.CreateDate,
                    t.Message,
                    t.LineNumber,
                    t.FileName,
                    t.BMessageCode,
                    t.Type
                })
                .ToList()
                .Select(t => new ErrorMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    userFullname = t.userFullname,
                    ip = t.Ip1 + "." + t.Ip2 + "." + t.Ip3 + "." + t.Ip4,
                    createDate = t.CreateDate.ToFaDate() + " " + t.CreateDate.ToString("HH:mm"),
                    description = t.Message.Replace(Environment.NewLine, "<br />"),
                    lineNumber = t.LineNumber.Replace(Environment.NewLine, "<br />"),
                    fileName = t.FileName.Replace(Environment.NewLine, "<br />"),
                    bMessageCode = (t.BMessageCode != null ? ((int)t.BMessageCode.Value) : -1).ToString(),
                    type = (t.Type != null ? ((int)t.Type.Value) : -1).ToString(),
                })
                .ToList()
            };
        }
    }
}
