using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;
using Oje.Security.Services.EContext;

namespace Oje.Security.Services
{
    public class InValidRangeIpService : IInValidRangeIpService
    {
        readonly SecurityDBContext db = null;
        readonly IIpapiService IpapiService = null;
        readonly IValidRangeIpService ValidRangeIpService = null;

        public InValidRangeIpService
            (
                SecurityDBContext db,
                IIpapiService IpapiService,
                IValidRangeIpService ValidRangeIpService
            )
        {
            this.db = db;
            this.IpapiService = IpapiService;
            this.ValidRangeIpService = ValidRangeIpService;
        }

        public bool Create(IpSections ip)
        {
            if (ip != null)
            {
                if (!db.InValidRangeIps.Any(t => t.Ip1 == ip.Ip1 && t.Ip2 == ip.Ip2 && t.Ip3 == ip.Ip3 && t.Ip4 == ip.Ip4))
                {
                    try
                    {
                        db.Entry(new InValidRangeIp()
                        {
                            Ip1 = ip.Ip1,
                            Ip2 = ip.Ip2,
                            Ip3 = ip.Ip3,
                            Ip4 = ip.Ip4
                        }).State = EntityState.Added;
                        db.SaveChanges();
                        return true;
                    }
                    catch { }
                }
            }
            return false;
        }

        public GridResultVM<InValidRangeIpMainGridResult> GetList(InValidRangeIpMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.InValidRangeIps.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.ip) && searchInput.ip.ToIp() != null)
            {
                var targetIp = searchInput.ip.ToIp();
                quiryResult = quiryResult.Where(t => t.Ip1 == targetIp.Ip1 && t.Ip2 == targetIp.Ip2 && t.Ip3 == targetIp.Ip3 && t.Ip4 == targetIp.Ip4);
            }
            if (!string.IsNullOrEmpty(searchInput.lastDate) && searchInput.lastDate.ToEnDate() != null)
            {
                var targetDate = searchInput.lastDate.ToEnDate().Value;
                quiryResult = quiryResult.Where(t => t.LastTryDate != null && t.LastTryDate.Value.Year == targetDate.Year && t.LastTryDate.Value.Month == targetDate.Month && t.LastTryDate.Value.Day == targetDate.Day);
            }
            if (searchInput.isSuccess != null)
                quiryResult = quiryResult.Where(t => t.IsSuccess == searchInput.isSuccess);
            if (searchInput.count != null)
                quiryResult = quiryResult.Where(t => t.CountTry == searchInput.count);
            if (!string.IsNullOrEmpty(searchInput.message))
                quiryResult = quiryResult.Where(t => t.LastEmailErrorMessage.Contains(searchInput.message));
            if (searchInput.iB == true)
                quiryResult = quiryResult.Where(t => db.ErrorFirewallManualAdds.Any(tt => tt.Ip4 == t.Ip4 && tt.Ip3 == t.Ip3 && t.Ip2 == tt.Ip2 && tt.Ip1 == t.Ip1));
            if (searchInput.iB == false)
                quiryResult = quiryResult.Where(t => !db.ErrorFirewallManualAdds.Any(tt => tt.Ip4 == t.Ip4 && tt.Ip3 == t.Ip3 && t.Ip2 == tt.Ip2 && tt.Ip1 == t.Ip1));

            var curDate = DateTime.Now;
            var row = searchInput.skip;

            return new GridResultVM<InValidRangeIpMainGridResult>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.LastTryDate == null ? curDate : t.LastTryDate.Value)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new 
                { 
                    t.Ip1,
                    t.Ip2,
                    t.Ip3,
                    t.Ip4,
                    t.LastEmailErrorMessage,
                    t.LastTryDate,
                    t.IsSuccess,
                    t.CountTry,
                    isBlocked = db.ErrorFirewallManualAdds.Any(tt => tt.Ip4 == t.Ip4 && tt.Ip3 == t.Ip3 && t.Ip2 == tt.Ip2 && tt.Ip1 == t.Ip1)
                })
                .ToList()
                .Select(t => new InValidRangeIpMainGridResult 
                { 
                    row = ++row,
                    id = t.Ip1 + "." + t.Ip2 + "." + t.Ip3 + "." + t.Ip4,
                    count = t.CountTry != null ? t.CountTry.ToString() : "0",
                    ip = t.Ip1 + "." + t.Ip2 + "." + t.Ip3 + "." + t.Ip4,
                    isSuccess = t.IsSuccess == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                    lastDate = t.LastTryDate.ToFaDate(),
                    message = t.LastEmailErrorMessage,
                    iB = t.isBlocked == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public async Task ValidateIps()
        {
            var curDT = DateTime.Now.AddSeconds(-55);
            var allItems = db.InValidRangeIps.Where(t => t.LastTryDate == null && (t.CountTry == null || t.CountTry == 0) && t.IsSuccess != true).ToList();
            if (allItems.Count == 0)
                allItems = db.InValidRangeIps.Where(t => t.LastTryDate != null && curDT > t.LastTryDate && (t.CountTry == null || t.CountTry <= 2) && t.IsSuccess != true).ToList();
            foreach (var item in allItems)
            {
                if (item.CountTry == null)
                    item.CountTry = 0;
                item.LastTryDate = DateTime.Now;
            }

            if (allItems.Count == 0)
                return;

            db.SaveChanges();

            foreach (var item in allItems)
            {
                IpDetectionServiceVM resultSms = null;
                try
                {
                    resultSms = await IpapiService.Validate(item.Ip1 + "." + item.Ip2 + "." + item.Ip3 + "." + item.Ip4);
                }
                catch (Exception ex)
                {
                    item.CountTry++;
                    item.IsSuccess = false;
                    item.LastEmailErrorMessage = ex.Message;
                    continue;
                };
                if (resultSms != null && !string.IsNullOrEmpty(resultSms.country_code) && resultSms.country_code.ToLower() == "ir")
                {
                    item.IsSuccess = true;
                    bool isHandle = false;

                    try
                    {
                        var rangeFromResult = await IpapiService.Validate(item.Ip1 + "." + item.Ip2 + "." + item.Ip3 + ".1");
                        var rangeToResult = await IpapiService.Validate(item.Ip1 + "." + item.Ip2 + "." + item.Ip3 + ".255");
                        if (
                            rangeFromResult != null && !string.IsNullOrEmpty(rangeFromResult.country_code) && rangeFromResult.country_code.ToLower() == "ir" &&
                            rangeToResult != null && !string.IsNullOrEmpty(rangeToResult.country_code) && rangeToResult.country_code.ToLower() == "ir"
                          )
                        {
                            try
                            {
                                var rangeFromLevel2Result = await IpapiService.Validate(item.Ip1 + "." + item.Ip2 + ".1.1");
                                var rangeToLevel2Result = await IpapiService.Validate(item.Ip1 + "." + item.Ip2 + ".255.255");
                                if (
                                   rangeFromLevel2Result != null && !string.IsNullOrEmpty(rangeFromLevel2Result.country_code) && rangeFromLevel2Result.country_code.ToLower() == "ir" &&
                                   rangeToLevel2Result != null && !string.IsNullOrEmpty(rangeToLevel2Result.country_code) && rangeToLevel2Result.country_code.ToLower() == "ir"
                                 )
                                {
                                    try
                                    {
                                        ValidRangeIpService.Create(new ValidRangeIpCreateUpdateVM() { fromIp = item.Ip1 + "." + item.Ip2 + ".0.0", toIp = item.Ip1 + "." + item.Ip2 + ".255.255", isActive = true, title = "ساخت به صورت خودکار" + " " + rangeToLevel2Result.city });
                                        isHandle = true;
                                    }
                                    catch
                                    {

                                    }
                                    
                                }
                            }
                            catch { }
                            if (isHandle == false)
                            {
                                try
                                {
                                    ValidRangeIpService.Create(new ValidRangeIpCreateUpdateVM() { fromIp = item.Ip1 + "." + item.Ip2 + "." + item.Ip3 + ".0", toIp = item.Ip1 + "." + item.Ip2 + "." + item.Ip3 + ".255", isActive = true, title = "ساخت به صورت خودکار" + " " + rangeFromResult.city });
                                    isHandle = true;
                                }
                                catch
                                {

                                }
                                
                            }
                        }
                    }
                    catch { }
                    if (isHandle == false)
                    {
                        try
                        {
                            ValidRangeIpService.Create(new ValidRangeIpCreateUpdateVM() { fromIp = item.Ip1 + "." + item.Ip2 + "." + item.Ip3 + "." + item.Ip4, toIp = item.Ip1 + "." + item.Ip2 + "." + item.Ip3 + "." + item.Ip4, isActive = true, title = "ساخت به صورت خودکار" + " " + resultSms.city });
                            isHandle = true;
                        }
                        catch
                        {

                        }
                    }
                }
                else if (resultSms != null && !string.IsNullOrEmpty(resultSms.country_code) && resultSms.country_code.ToLower() != "ir")
                {
                    item.CountTry = 99;
                    item.IsSuccess = false;
                    item.LastEmailErrorMessage = "ای پی کشور " + resultSms.country_name + "(" + resultSms.country_code.ToLower() + ") " + resultSms.city;
                }
                else
                {
                    item.CountTry++;
                    item.IsSuccess = false;
                    item.LastEmailErrorMessage = "علت خطا مشخص نمی باشد";
                }
            }

            db.SaveChanges();
        }
    }
}
