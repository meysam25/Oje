using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class TenderFilledFormJsonService : ITenderFilledFormJsonService
    {
        readonly TenderDBContext db = null;

        public TenderFilledFormJsonService(TenderDBContext db)
        {
            this.db = db;
        }

        public long Create(long tenderFilledFormId, string tempJsonFile, int? tenderProposalFormJsonConfigId, bool? isConsultation = null)
        {
            if (isConsultation == true)
            {
                var foundItem = db.TenderFilledFormJsons
                   .Where(t => t.TenderFilledFormId == tenderFilledFormId && t.TenderProposalFormJsonConfigId == tenderProposalFormJsonConfigId && t.IsConsultation == true)
                   .FirstOrDefault();
                if (foundItem != null)
                {
                    if (!foundItem.IsSignature())
                        throw BException.GenerateNewException(BMessages.Can_Not_Be_Edited);

                    db.Entry(foundItem).State = EntityState.Deleted;
                }
            } 
            
            var newItem = new TenderFilledFormJson()
            {
                JsonConfig = tempJsonFile,
                TenderFilledFormId = tenderFilledFormId,
                TenderProposalFormJsonConfigId = tenderProposalFormJsonConfigId,
                IsConsultation = isConsultation
            };
            db.Entry(newItem).State = EntityState.Added;
            db.SaveChanges();

            newItem.FilledSignature();
            db.SaveChanges();

            return newItem.Id;
        }

        public List<TenderFilledFormJson> GetBy(long tenderFilledFormId)
        {
            return db.TenderFilledFormJsons
                .Where(t => t.TenderFilledFormId == tenderFilledFormId)
                .OrderBy(t => t.IsConsultation)
                .ThenByDescending(t => t.Id)
                .AsNoTracking()
                .ToList();
        }
    }
}
