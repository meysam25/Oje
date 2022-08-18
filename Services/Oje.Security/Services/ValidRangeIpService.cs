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
    public class ValidRangeIpService : IValidRangeIpService
    {
        readonly SecurityDBContext db = null;
        static List<ValidRangeIp> ValidRangeIps { get; set; }
        static DateTime? fillTime = null;

        public ValidRangeIpService
            (
                SecurityDBContext db
            )
        {
            this.db = db;
        }

        public ApiResult Create(ValidRangeIpCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var fromIp = input.fromIp.ToIp();
            var toIp = input.toIp.ToIp();

            db.Entry(new ValidRangeIp()
            {
                Title = input.title,
                FromIp1 = fromIp.Ip1,
                FromIp2 = fromIp.Ip2,
                FromIp3 = fromIp.Ip3,
                FromIp4 = fromIp.Ip4,
                ToIp1 = toIp.Ip1,
                ToIp2 = toIp.Ip2,
                ToIp3 = toIp.Ip3,
                ToIp4 = toIp.Ip4,
                IsActive = input.isActive.ToBooleanReturnFalse()
            }).State = EntityState.Added;
            db.SaveChanges();

            ValidRangeIps = null;

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(ValidRangeIpCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 200)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_200_chars);
            if (string.IsNullOrEmpty(input.fromIp))
                throw BException.GenerateNewException(BMessages.Please_Enter_IP);
            if (string.IsNullOrEmpty(input.toIp))
                throw BException.GenerateNewException(BMessages.Please_Enter_IP);
            if (input.fromIp.ToIp() == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_IP);
            if (input.toIp.ToIp() == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_IP);

            var fromIp = input.fromIp.ToIp();
            var toIp = input.toIp.ToIp();

            if (db.ValidRangeIps.Any(t => t.Id != input.id && t.FromIp1 == fromIp.Ip1 && t.FromIp2 == fromIp.Ip2 && t.FromIp3 == fromIp.Ip3 && t.FromIp4 == fromIp.Ip4 && t.ToIp1 == toIp.Ip1 && t.ToIp2 == toIp.Ip2 && t.ToIp3 == toIp.Ip3 && t.ToIp4 == toIp.Ip4))
                throw BException.GenerateNewException(BMessages.Dublicate_IP);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.ValidRangeIps.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            ValidRangeIps = null;

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.ValidRangeIps
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    id = t.Id,
                    fromIp = t.FromIp1 + "." + t.FromIp2 + "." + t.FromIp3 + "." + t.FromIp4,
                    toIp = t.ToIp1 + "." + t.ToIp2 + "." + t.ToIp3 + "." + t.ToIp4,
                    isActive = t.IsActive,
                    title = t.Title
                })
                .FirstOrDefault();
        }

        public GridResultVM<ValidRangeIpMainGridResultVM> GetList(ValidRangeIpMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.ValidRangeIps.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);
            if (!string.IsNullOrEmpty(searchInput.fromIp) && searchInput.fromIp.ToIp() != null)
            {
                var targetIp = searchInput.fromIp.ToIp();
                quiryResult = quiryResult.Where(t => t.FromIp1 == targetIp.Ip1 && t.FromIp2 == targetIp.Ip2 && t.FromIp3 == targetIp.Ip3 && t.FromIp4 == targetIp.Ip4);
            }
            if (!string.IsNullOrEmpty(searchInput.toIp) && searchInput.toIp.ToIp() != null)
            {
                var targetIp = searchInput.toIp.ToIp();
                quiryResult = quiryResult.Where(t => t.ToIp1 == targetIp.Ip1 && t.ToIp2 == targetIp.Ip2 && t.ToIp3 == targetIp.Ip3 && t.ToIp4 == targetIp.Ip4);
            }


            int row = searchInput.skip;

            return new GridResultVM<ValidRangeIpMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    t.FromIp1,
                    t.FromIp2,
                    t.FromIp3,
                    t.FromIp4,
                    t.ToIp1,
                    t.ToIp2,
                    t.ToIp3,
                    t.ToIp4,
                    t.IsActive,
                    t.Title
                })
                .ToList()
                .Select(t => new ValidRangeIpMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    fromIp = t.FromIp1 + "." + t.FromIp2 + "." + t.FromIp3 + "." + t.FromIp4,
                    toIp = t.ToIp1 + "." + t.ToIp2 + "." + t.ToIp3 + "." + t.ToIp4,
                    isActive = t.IsActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    title = t.Title
                }).ToList()
            };
        }

        public ApiResult Update(ValidRangeIpCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.ValidRangeIps.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var fromIp = input.fromIp.ToIp();
            var toIp = input.toIp.ToIp();

            foundItem.Title = input.title;
            foundItem.FromIp1 = fromIp.Ip1;
            foundItem.FromIp2 = fromIp.Ip2;
            foundItem.FromIp3 = fromIp.Ip3;
            foundItem.FromIp4 = fromIp.Ip4;
            foundItem.ToIp1 = toIp.Ip1;
            foundItem.ToIp2 = toIp.Ip2;
            foundItem.ToIp3 = toIp.Ip3;
            foundItem.ToIp4 = toIp.Ip4;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            ValidRangeIps = null;

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object CreateFromExcel(GlobalExcelFile input)
        {
            string resultText = "";

            var excelFile = input?.excelFile;

            if (excelFile == null || excelFile.Length == 0)
                return ApiResult.GenerateNewResult(false, BMessages.Please_Select_File);

            List<ValidRangeIpCreateXLS> models = ExportToExcel.ConvertToModel<ValidRangeIpCreateXLS>(input?.excelFile);
            if (models != null && models.Count > 0)
            {
                for (var i = 0; i < models.Count; i++)
                {
                    var model = models[i];
                    try
                    {
                        Create(new ValidRangeIpCreateUpdateVM() { fromIp = model.fromIp, toIp = model.toIp, title = String.IsNullOrEmpty(model.title) ? "unknown" : model.title, isActive = true });

                    }
                    catch (BException be)
                    {
                        resultText += "ردیف " + (i + 1) + " " + be.Message + Environment.NewLine;
                    }
                    catch (Exception)
                    {
                        resultText += "ردیف " + (i + 1) + " " + "خطای نامشخص " + Environment.NewLine;
                    }
                }
            }
            else
                return ApiResult.GenerateNewResult(false, BMessages.No_Row_Detected);

            return ApiResult.GenerateNewResult(
                    true,
                    (string.IsNullOrEmpty(resultText) ? BMessages.Operation_Was_Successfull : BMessages.Some_Operation_Was_Successfull),
                    resultText,
                    string.IsNullOrEmpty(resultText) ? null : "reportResult.txt"
                );
        }

        public List<ValidRangeIp> GetCacheIpRangeList()
        {
            var targetDate = DateTime.Now.AddSeconds(-180);
            if (ValidRangeIps == null || (fillTime != null && targetDate > fillTime))
            {
                ValidRangeIps = db.ValidRangeIps.Where(t => t.IsActive == true).AsNoTracking().ToList();
                fillTime = DateTime.Now;
            }

            return ValidRangeIps;
        }
    }
}
