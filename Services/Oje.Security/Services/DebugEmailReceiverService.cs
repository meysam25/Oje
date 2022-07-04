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
    public class DebugEmailReceiverService : IDebugEmailReceiverService
    {
        readonly SecurityDBContext db = null;
        public DebugEmailReceiverService(SecurityDBContext db)
        {
            this.db = db;
        }

        public ApiResult Create(DebugEmailReceiverCreateUpdateVM input)
        {
            createUpdateValidation(input);

            db.Entry(new DebugEmailReceiver()
            {
                Email = input.email,
                IsActive = input.isActive.ToBooleanReturnFalse()
            }).State = EntityState.Added;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        private void createUpdateValidation(DebugEmailReceiverCreateUpdateVM input)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.email))
                throw BException.GenerateNewException(BMessages.Please_Enter_Email);
            if (!input.email.IsValidEmail())
                throw BException.GenerateNewException(BMessages.Invalid_Email);
            if (db.DebugEmailReceivers.Any(t => t.Email == input.email && t.Id != input.id))
                throw BException.GenerateNewException(BMessages.Dublicate_Email);
        }

        public ApiResult Delete(int? id)
        {
            var foundItem = db.DebugEmailReceivers.Where(t => t.Id == id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            db.Entry(foundItem).State = EntityState.Deleted;
            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }

        public object GetById(int? id)
        {
            return db.DebugEmailReceivers.Where(t => t.Id == id).Select(t => new
            {
                id = t.Id,
                email = t.Email,
                isActive = t.IsActive
            }).FirstOrDefault();
        }

        public GridResultVM<DebugEmailReceiverMainGridResultVM> GetList(DebugEmailReceiverMainGrid searchInput)
        {
            searchInput = searchInput ?? new DebugEmailReceiverMainGrid();

            var quiryResult = db.DebugEmailReceivers.AsQueryable();

            if (!string.IsNullOrEmpty(searchInput.email))
                quiryResult = quiryResult.Where(t => t.Email.Contains(searchInput.email));
            if (searchInput.isActive != null)
                quiryResult = quiryResult.Where(t => t.IsActive == searchInput.isActive);

            int row = searchInput.skip;

            return new GridResultVM<DebugEmailReceiverMainGridResultVM>()
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.Id)
                .Skip(searchInput.skip)
                .Take(searchInput.take)
                .Select(t => new 
                {
                    id = t.Id,
                    email = t.Email,
                    isActive = t.IsActive
                })
                .ToList()
                .Select(t => new DebugEmailReceiverMainGridResultVM 
                { 
                    row = ++row,
                    id = t.id,
                    email = t.email,
                    isActive = t.isActive == true ? BMessages.Active.GetEnumDisplayName() : BMessages.InActive.GetEnumDisplayName()
                })
                .ToList()
            };
        }

        public ApiResult Update(DebugEmailReceiverCreateUpdateVM input)
        {
            createUpdateValidation(input);

            var foundItem = db.DebugEmailReceivers.Where(t => t.Id == input.id).FirstOrDefault();

            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            foundItem.Email = input.email;
            foundItem.IsActive = input.isActive.ToBooleanReturnFalse();

            db.SaveChanges();

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull);
        }
    }
}
