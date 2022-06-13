using Microsoft.EntityFrameworkCore;
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

        public void Create(long tenderFilledFormId, string tempJsonFile, int? tenderProposalFormJsonConfigId)
        {
            db.Entry(new TenderFilledFormJson()
            {
                JsonConfig = tempJsonFile,
                TenderFilledFormId = tenderFilledFormId,
                TenderProposalFormJsonConfigId = tenderProposalFormJsonConfigId
            }).State = EntityState.Added;
            db.SaveChanges();
        }

        public List<TenderFilledFormJson> GetBy(long tenderFilledFormId)
        {
            return db.TenderFilledFormJsons.OrderByDescending(t => t.Id).Where(t => t.TenderFilledFormId == tenderFilledFormId).ToList();
        }
    }
}
