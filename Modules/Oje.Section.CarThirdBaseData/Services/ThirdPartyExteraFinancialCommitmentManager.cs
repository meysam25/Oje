using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarThirdBaseData.Interfaces;
using Oje.Section.CarThirdBaseData.Models.View;
using Oje.Section.CarThirdBaseData.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Oje.Section.CarThirdBaseData.Services.EContext;

namespace Oje.Section.CarThirdBaseData.Services
{
    public class ThirdPartyExteraFinancialCommitmentManager : IThirdPartyExteraFinancialCommitmentManager
    {
        readonly CarThirdBaseDataDBContext db = null;
        public ThirdPartyExteraFinancialCommitmentManager(CarThirdBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateThirdPartyExteraFinancialCommitmentVM input)
        {
            createValidation(input);

            ThirdPartyExteraFinancialCommitment newItem = new ThirdPartyExteraFinancialCommitment()
            {
                CarSpecificationId = input.carSpecId.Value,
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Rate = input.rate.Value,
                ThirdPartyRequiredFinancialCommitmentId = input.thirdPartyRequiredFinancialCommitmentId.Value,
                Title = input.title,
                Year = input.year
            };

            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            if (input.cIds != null)
                foreach (int cid in input.cIds)
                    db.Entry(new ThirdPartyExteraFinancialCommitmentCom() { CompanyId = cid, ThirdPartyExteraFinancialCommitmentId = newItem.Id }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createValidation(CreateUpdateThirdPartyExteraFinancialCommitmentVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (input.carSpecId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Select_CarSpecification);
            if (input.cIds == null || input.cIds.Count == 0)
                throw BException.GenerateNewException(BMessages.Please_Select_Company);
            if (input.thirdPartyRequiredFinancialCommitmentId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_ThirdPartyRequired_Finanical_Commitment);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.year.ToIntReturnZiro() < 1000 || input.year > 2000)
                throw BException.GenerateNewException(BMessages.Please_Enter_Year);
            if (input.rate == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Rate);
            if (input.rate <= 0)
                throw BException.GenerateNewException(BMessages.Invalid_Rate);
        }

        public ApiResult Delete(int? id)
        {
            ThirdPartyExteraFinancialCommitment foundItem = db.ThirdPartyExteraFinancialCommitments.Where(t => t.Id == id).Include(t => t.ThirdPartyExteraFinancialCommitmentComs).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);
            if (foundItem.ThirdPartyExteraFinancialCommitmentComs != null)
                foreach (var item in foundItem.ThirdPartyExteraFinancialCommitmentComs)
                    db.Entry(foundItem).State = EntityState.Deleted;

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateThirdPartyExteraFinancialCommitmentVM GetById(int? id)
        {
            return db.ThirdPartyExteraFinancialCommitments.Where(t => t.Id == id).Select(t => new CreateUpdateThirdPartyExteraFinancialCommitmentVM
            {
                id = t.Id,
                carSpecId = t.CarSpecificationId,
                carSpecId_Title = t.CarSpecification.Title,
                cIds = t.ThirdPartyExteraFinancialCommitmentComs.Select(tt => tt.CompanyId).ToList(),
                isActive = t.IsActive,
                rate = t.Rate,
                title = t.Title,
                year = t.Year,
                thirdPartyRequiredFinancialCommitmentId = t.ThirdPartyRequiredFinancialCommitmentId,
                thirdPartyRequiredFinancialCommitmentId_Title = t.ThirdPartyRequiredFinancialCommitment.Title
            }).FirstOrDefault();
        }

        public GridResultVM<ThirdPartyExteraFinancialCommitmentMainGridResultVM> GetList(ThirdPartyExteraFinancialCommitmentMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new ThirdPartyExteraFinancialCommitmentMainGrid();

            var qureResult = db.ThirdPartyExteraFinancialCommitments.AsQueryable();

            if (searchInput.company.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.ThirdPartyExteraFinancialCommitmentComs.Any(tt => tt.CompanyId == searchInput.company));
            if (!string.IsNullOrEmpty(searchInput.specTitle))
                qureResult = qureResult.Where(t => t.CarSpecification.Title.Contains(searchInput.specTitle));
            if (!string.IsNullOrEmpty(searchInput.commitmentTitle))
                qureResult = qureResult.Where(t => t.ThirdPartyRequiredFinancialCommitment.Title.Contains(searchInput.commitmentTitle));
            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.rate != null && searchInput.rate > 0)
                qureResult = qureResult.Where(t => t.Rate == searchInput.rate);
            if (searchInput.year.ToIntReturnZiro() > 1000 && searchInput.year < 2000)
                qureResult = qureResult.Where(t => t.Year == searchInput.year);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<ThirdPartyExteraFinancialCommitmentMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    company = t.ThirdPartyExteraFinancialCommitmentComs.Select(tt => tt.Company.Title).ToList(),
                    specTitle = t.CarSpecification.Title,
                    commitmentTitle = t.ThirdPartyRequiredFinancialCommitment.Title,
                    title = t.Title,
                    rate = t.Rate,
                    year = t.Year,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new ThirdPartyExteraFinancialCommitmentMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    commitmentTitle = t.commitmentTitle,
                    company = string.Join(",", t.company),
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name,
                    rate = t.rate,
                    specTitle = t.specTitle,
                    title = t.title,
                    year = t.year
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateThirdPartyExteraFinancialCommitmentVM input)
        {
            createValidation(input);

            ThirdPartyExteraFinancialCommitment foundItem = db.ThirdPartyExteraFinancialCommitments.Where(t => t.Id == input.id).Include(t => t.ThirdPartyExteraFinancialCommitmentComs).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);
            if (foundItem.ThirdPartyExteraFinancialCommitmentComs != null)
                foreach (var item in foundItem.ThirdPartyExteraFinancialCommitmentComs)
                    db.Entry(item).State = EntityState.Deleted;

            foundItem.CarSpecificationId = input.carSpecId.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.Rate = input.rate.Value;
            foundItem.ThirdPartyRequiredFinancialCommitmentId = input.thirdPartyRequiredFinancialCommitmentId.Value;
            foundItem.Title = input.title;
            foundItem.Year = input.year;

            if (input.cIds != null)
                foreach (int cid in input.cIds)
                    db.Entry(new ThirdPartyExteraFinancialCommitmentCom() { CompanyId = cid, ThirdPartyExteraFinancialCommitmentId = foundItem.Id }).State = EntityState.Added;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public ApiResult CreateFromExcel(GlobalExcelFile input)
        {
            string resultText = "";

            var excelFile = input?.excelFile;

            if (excelFile == null || excelFile.Length == 0)
                return ApiResult.GenerateNewResult(false, BMessages.Please_Select_File);

            List<CreateUpdateThirdPartyExteraFinancialCommitmentVM> models = ExportToExcel.ConvertToModel<CreateUpdateThirdPartyExteraFinancialCommitmentVM>(input?.excelFile);
            if (models != null && models.Count > 0)
            {
                for (var i = 0; i < models.Count; i++)
                {
                    var model = models[i];
                    try
                    {
                        Create(model);

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
            {
                return ApiResult.GenerateNewResult(false, BMessages.No_Row_Detected);
            }

            return ApiResult.GenerateNewResult(
                    true, 
                    (string.IsNullOrEmpty(resultText) ? BMessages.Operation_Was_Successfull : BMessages.Some_Operation_Was_Successfull), 
                    resultText, 
                    string.IsNullOrEmpty(resultText) ? null : "reportResult.txt"
                );
        }
    }
}
