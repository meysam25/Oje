using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarThirdBaseData.Interfaces;
using Oje.Section.CarThirdBaseData.Models.View;
using Oje.Section.CarThirdBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.CarThirdBaseData.Services.EContext;

namespace Oje.Section.CarThirdBaseData.Services
{
    public class ThirdPartyLifeCommitmentService: IThirdPartyLifeCommitmentService
    {
        readonly CarThirdBaseDataDBContext db = null;
        public ThirdPartyLifeCommitmentService(CarThirdBaseDataDBContext db)
        {
            this.db = db;
        }
        public ApiResult Create(CreateUpdateThirdPartyLifeCommitmentVM input)
        {
            createValidation(input);

            db.Entry(new ThirdPartyLifeCommitment()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Price = input.price.Value,
                Title = input.title,
                Year = input.year.Value
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateThirdPartyLifeCommitmentVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.year.ToIntReturnZiro() <= 1000 || input.year.ToIntReturnZiro() > 2000)
                throw BException.GenerateNewException(BMessages.Please_Enter_Year);
            if (input.price.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Enter_Price);
            if (db.ThirdPartyLifeCommitments.Any(t => t.Year == input.year && t.Title == input.title && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {

            var foundItem = db.ThirdPartyLifeCommitments.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateThirdPartyLifeCommitmentVM GetById(int? id)
        {
            return db.ThirdPartyLifeCommitments
                .Where(t => t.Id == id)
                .Select(t => new CreateUpdateThirdPartyLifeCommitmentVM
                {
                    id = t.Id,
                    isActive = t.IsActive,
                    price = t.Price,
                    title = t.Title,
                    year = t.Year
                })
                .FirstOrDefault();
        }

        public GridResultVM<ThirdPartyLifeCommitmentMainGridResultVM> GetList(ThirdPartyLifeCommitmentMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new ThirdPartyLifeCommitmentMainGrid();

            var qureResult = db.ThirdPartyLifeCommitments.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.year.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Year == searchInput.year);
            if (searchInput.price.ToLongReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Price == searchInput.price);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive);

            int row = searchInput.skip;

            return new GridResultVM<ThirdPartyLifeCommitmentMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    year = t.Year,
                    price = t.Price,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new ThirdPartyLifeCommitmentMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    price = t.price.ToString("###,###"),
                    title = t.title,
                    year = t.year
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateThirdPartyLifeCommitmentVM input)
        {
            createValidation(input);

            var foundItem = db.ThirdPartyLifeCommitments.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Price = input.price.Value;
            foundItem.Title = input.title;
            foundItem.Year = input.year.Value;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
