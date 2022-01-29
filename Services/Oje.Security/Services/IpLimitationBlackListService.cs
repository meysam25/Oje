using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Oje.Security.Interfaces;
using Oje.Security.Services.EContext;
using Oje.Security.Models.View;
using Oje.Security.Models.DB;

namespace Oje.Security.Services
{
    public class IpLimitationBlackListService: IIpLimitationBlackListService
    {
        readonly SecurityDBContext db = null;
        public IpLimitationBlackListService(SecurityDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateIpLimitationBlackListVM input)
        {
            CreateValidation(input);

            var ipParts = input.ip.ToIp();

            db.Entry(new IpLimitationBlackList
            {
                CreateDate = DateTime.Now,
                Ip1 = ipParts.Ip1,
                Ip2 = ipParts.Ip2,
                Ip3 = ipParts.Ip3,
                Ip4 = ipParts.Ip4,
                IsActive = input.isActive.ToBooleanReturnFalse()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateIpLimitationBlackListVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.ip))
                throw BException.GenerateNewException(BMessages.Please_Enter_IP);
            if (input.ip.ToIp() == null)
                throw BException.GenerateNewException(BMessages.Ip_Format_Is_Not_Valid);
            var allIpPart = input.ip.ToIp();
            if (db.IpLimitationBlackLists.Any(t => t.Id != input.id && t.Ip1 == allIpPart.Ip1 && t.Ip2 == allIpPart.Ip2 && t.Ip3 == allIpPart.Ip3 && t.Ip4 == allIpPart.Ip4))
                throw BException.GenerateNewException(BMessages.Dublicate_IP);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.IpLimitationBlackLists.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateIpLimitationBlackListVM GetById(int? id)
        {
            return db.IpLimitationBlackLists.Where(t => t.Id == id).AsNoTracking().Select(t => new CreateUpdateIpLimitationBlackListVM
            {
                id = t.Id,
                ip = t.Ip1 + "." + t.Ip2 + "." + t.Ip3 + "." + t.Ip4,
                isActive = t.IsActive
            }).FirstOrDefault();
        }

        public GridResultVM<IpLimitationBlackListMainGridResultVM> GetList(IpLimitationBlackListMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new IpLimitationBlackListMainGrid();

            var qureResult = db.IpLimitationBlackLists.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.ip) && searchInput.ip.ToIp() != null)
            {
                var allIpParts = searchInput.ip.ToIp();
                qureResult = qureResult.Where(t => t.Ip1 == allIpParts.Ip1 && t.Ip2 == allIpParts.Ip2 && t.Ip3 == allIpParts.Ip3 && t.Ip4 == allIpParts.Ip4);
            }
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            var row = searchInput.skip;

            return new GridResultVM<IpLimitationBlackListMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take).AsNoTracking()
                .Select(t => new
                {
                    id = t.Id,
                    ip = t.Ip1 + "." + t.Ip2 + "." + t.Ip3 + "." + t.Ip4,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new IpLimitationBlackListMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    ip = t.ip,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateIpLimitationBlackListVM input)
        {
            CreateValidation(input);

            var ipParts = input.ip.ToIp();
            var foundItem = db.IpLimitationBlackLists.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Ip1 = ipParts.Ip1;
            foundItem.Ip2 = ipParts.Ip2;
            foundItem.Ip3 = ipParts.Ip3;
            foundItem.Ip4 = ipParts.Ip4;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
