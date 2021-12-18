using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.FireBaseData.Interfaces;
using Oje.Section.FireBaseData.Models.DB;
using Oje.Section.FireBaseData.Models.View;
using Oje.Section.FireBaseData.Services.EContext;
using System.Linq;

namespace Oje.Section.FireBaseData.Services
{
    public class FireInsuranceBuildingAgeService: IFireInsuranceBuildingAgeService
    {
        readonly FireBaseDataDBContext db = null;
        public FireInsuranceBuildingAgeService(FireBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateFireInsuranceBuildingAgeVM input)
        {
            createUpdateValidation(input);

            db.Entry(new FireInsuranceBuildingAge() 
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Order = input.order.ToIntReturnZiro(),
                Percent = input.percent.ToIntReturnZiro(),
                Title = input.title,
                Year = input.year.ToIntReturnZiro()
            }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(CreateUpdateFireInsuranceBuildingAgeVM input)
        {
            if(input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 50)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_50_chars);
            if (input.year.ToIntReturnZiro() < 0)
                throw BException.GenerateNewException(BMessages.Year_Is_Invalid);
            if (input.percent.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Percent);

        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.FireInsuranceBuildingAges.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateFireInsuranceBuildingAgeVM GetById(int? id)
        {
            return db.FireInsuranceBuildingAges.Where(t => t.Id == id).Select(t => new CreateUpdateFireInsuranceBuildingAgeVM
            {
                id = t.Id,
                isActive = t.IsActive,
                order = t.Order,
                percent = t.Percent,
                title = t.Title,
                year = t.Year,
            }).FirstOrDefault();
        }

        public GridResultVM<FireInsuranceBuildingAgeMainGridResultVM> GetList(FireInsuranceBuildingAgeMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new FireInsuranceBuildingAgeMainGrid();

            var qureResult = db.FireInsuranceBuildingAges.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.percent.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Percent == searchInput.percent);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<FireInsuranceBuildingAgeMainGridResultVM>() 
            {
                total = qureResult.Count(),
                data = qureResult.OrderBy(t => t.Order).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new 
                { 
                    id = t.Id,
                    title = t.Title,
                    percent = t.Percent,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new FireInsuranceBuildingAgeMainGridResultVM 
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    percent = t.percent,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateFireInsuranceBuildingAgeVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.FireInsuranceBuildingAges.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Title = input.title;
            foundItem.Year = input.year.ToIntReturnZiro();
            foundItem.Percent = input.percent.ToIntReturnZiro();
            foundItem.Order = input.order.ToIntReturnZiro();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
