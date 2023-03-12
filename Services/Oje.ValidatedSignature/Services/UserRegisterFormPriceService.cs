using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.View;
using Oje.ValidatedSignature.Services.EContext;

namespace Oje.ValidatedSignature.Services
{
    public class UserRegisterFormPriceService : IUserRegisterFormPriceService
    {
        readonly ValidatedSignatureDBContext db = null;
        public UserRegisterFormPriceService
            (
                ValidatedSignatureDBContext db
            )
        {
            this.db = db;
        }

        public object GetBy(long? id)
        {
            string result = "";
            var foundItem = db.UserRegisterFormPrices
                .FirstOrDefault(t => t.Id == id);

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            if (!foundItem.IsSignature())
            {
                result += "change has been found in UserRegisterFormPrices" + Environment.NewLine;
                result += foundItem.GetSignatureChanges();
            }


            return new { description = string.IsNullOrEmpty(result) ? BMessages.Its_All_Valid.GetEnumDisplayName() : result };
        }

        public GridResultVM<UserRegisterFormPriceMainGridResultVM> GetList(UserRegisterFormPriceMainGrid searchInput)
        {
            searchInput = searchInput ?? new();

            var quiryResult = db.UserRegisterFormPrices.AsQueryable();

            if (searchInput.id.ToIntReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Id == searchInput.id);
            if (!string.IsNullOrEmpty(searchInput.title))
                quiryResult = quiryResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.price.ToLongReturnZiro() > 0)
                quiryResult = quiryResult.Where(t => t.Price == searchInput.price);
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);

            bool hasSort = false;

            if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.Id);
            }
            else if (searchInput.sortField == "id" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.Id);
            } 
            else if (searchInput.sortField == "title" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.Title);
            }
            else if (searchInput.sortField == "title" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.Title);
            }
            else if (searchInput.sortField == "price" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.Price);
            }
            else if (searchInput.sortField == "price" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.Price);
            }
            else if (searchInput.sortField == "isActive" && searchInput.sortFieldIsAsc == true)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderBy(t => t.IsActive);
            }
            else if (searchInput.sortField == "isActive" && searchInput.sortFieldIsAsc == false)
            {
                hasSort = true;
                quiryResult = quiryResult.OrderByDescending(t => t.IsActive);
            }

            if (!hasSort)
                quiryResult = quiryResult.OrderByDescending(t => t.Id);

            int row = searchInput.skip;

            return new GridResultVM<UserRegisterFormPriceMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .ToList()
                .Select(t => new UserRegisterFormPriceMainGridResultVM 
                {
                    id = t.Id,
                    isActive = t.IsActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName(),
                    price = t.Price.ToString("###,###"),
                    row = ++row,
                    title = t.Title,
                    isValid = t.IsSignature() ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName()
                })
                .ToList()
            };
        }
    }
}
