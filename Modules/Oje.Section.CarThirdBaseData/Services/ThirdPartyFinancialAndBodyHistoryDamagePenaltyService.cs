﻿using Oje.Infrastructure.Enums;
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
    public class ThirdPartyFinancialAndBodyHistoryDamagePenaltyService : IThirdPartyFinancialAndBodyHistoryDamagePenaltyService
    {
        readonly CarThirdBaseDataDBContext db = null;
        public ThirdPartyFinancialAndBodyHistoryDamagePenaltyService(CarThirdBaseDataDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(CreateUpdateThirdPartyFinancialAndBodyHistoryDamagePenaltyVM input)
        {
            CreateValidation(input);

            db.Entry(new ThirdPartyFinancialAndBodyHistoryDamagePenalty()
            {
                IsActive = input.isActive.ToBooleanReturnFalse(),
                Percent = input.percent.Value,
                Title = input.title,
                IsFinancial = input.isFinancial.ToBooleanReturnFalse(),
                Count = input.count
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void CreateValidation(CreateUpdateThirdPartyFinancialAndBodyHistoryDamagePenaltyVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.title))
                throw BException.GenerateNewException(BMessages.Please_Enter_Title);
            if (input.title.Length > 100)
                throw BException.GenerateNewException(BMessages.Title_Can_Not_Be_More_Then_100_chars);
            if (input.percent == null)
                throw BException.GenerateNewException(BMessages.Please_Enter_Percent);
            if (input.percent <= 0 || input.percent > 100)
                throw BException.GenerateNewException(BMessages.Invalid_Percent);
            if (db.ThirdPartyFinancialAndBodyHistoryDamagePenalties.Any(tt => tt.Title == input.title && tt.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Item);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.ThirdPartyFinancialAndBodyHistoryDamagePenalties.Where(t => t.Id == id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public CreateUpdateThirdPartyFinancialAndBodyHistoryDamagePenaltyVM GetById(int? id)
        {
            return db.ThirdPartyFinancialAndBodyHistoryDamagePenalties
                .Where(t => t.Id == id)
                .Select(t => new CreateUpdateThirdPartyFinancialAndBodyHistoryDamagePenaltyVM
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    percent = t.Percent,
                    isFinancial = t.IsFinancial == null ? false : t.IsFinancial.Value,
                    count = t.Count
                }).FirstOrDefault();
        }

        public GridResultVM<ThirdPartyFinancialAndBodyHistoryDamagePenaltyMainGridResultVM> GetList(ThirdPartyFinancialAndBodyHistoryDamagePenaltyMainGrid searchInput)
        {
            if (searchInput == null)
                searchInput = new ThirdPartyFinancialAndBodyHistoryDamagePenaltyMainGrid();

            var qureResult = db.ThirdPartyFinancialAndBodyHistoryDamagePenalties.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.title))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.title));
            if (searchInput.percent.ToIntReturnZiro() > 0)
                qureResult = qureResult.Where(t => t.Percent == searchInput.percent);
            if (searchInput.isActive != null)
                qureResult = qureResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<ThirdPartyFinancialAndBodyHistoryDamagePenaltyMainGridResultVM>()
            {
                total = qureResult.Count(),
                data = qureResult.OrderByDescending(t => t.Id).Skip(searchInput.skip).Take(searchInput.take)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    isActive = t.IsActive,
                    percent = t.Percent
                })
                .ToList()
                .Select(t => new ThirdPartyFinancialAndBodyHistoryDamagePenaltyMainGridResultVM
                {
                    row = ++row,
                    id = t.id,
                    title = t.title,
                    percent = t.percent,
                    isActive = t.isActive == true ? BMessages.Active.GetAttribute<DisplayAttribute>()?.Name : BMessages.InActive.GetAttribute<DisplayAttribute>()?.Name
                })
                .ToList()
            };
        }

        public ApiResult Update(CreateUpdateThirdPartyFinancialAndBodyHistoryDamagePenaltyVM input)
        {
            CreateValidation(input);

            var foundItem = db.ThirdPartyFinancialAndBodyHistoryDamagePenalties.Where(t => t.Id == input.id).FirstOrDefault();
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found, ApiResultErrorCode.NotFound);

            foundItem.Title = input.title;
            foundItem.Percent = input.percent.Value;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();
            foundItem.IsFinancial = input.isFinancial.ToBooleanReturnFalse();
            foundItem.Count = input.count;

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
