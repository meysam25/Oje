using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Models;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Services.EContext;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class TenderFilledFormPFService: ITenderFilledFormPFService
    {
        readonly TenderDBContext db = null;

        public TenderFilledFormPFService
            (
                TenderDBContext db
            )
        {
            this.db = db;
        }

        public void Create(long tenderFilledFormId, int tenderProposalFormJsonConfigId)
        {
            db.Entry(new TenderFilledFormPF() 
            { 
                TenderFilledFormId = tenderFilledFormId, TenderProposalFormJsonConfigId = tenderProposalFormJsonConfigId 
            }).State = EntityState.Added;
            db.SaveChanges();
        }

        public object GetListForWeb(GlobalGridParentLong searchInput, long? tenderFilledFormId, long? loginUserId, int? siteSettingId)
        {
            searchInput = searchInput ?? new GlobalGridParentLong();

            var quiryResult = db.TenderFilledFormPFs
                .Where(t => t.TenderFilledFormId == tenderFilledFormId && t.TenderFilledForm.SiteSettingId == siteSettingId && (loginUserId == null || t.TenderFilledForm.UserId == loginUserId));

            int row = searchInput.skip;

            return new 
            {
                total = quiryResult.Count(),
                data = quiryResult
                .OrderByDescending(t => t.TenderFilledFormId)
                .ThenByDescending(t => t.TenderProposalFormJsonConfigId)
                .Select(t => new 
                { 
                    fid = t.TenderFilledFormId,
                    cId = t.TenderProposalFormJsonConfigId,
                    insurance = t.TenderProposalFormJsonConfig.ProposalForm.Title
                })
                .ToList()
                .Select(t => new 
                { 
                    row = ++row,
                    t.insurance,
                    t.fid,
                    t.cId,
                    id = t.fid + "_" + t.cId
                })
                .ToList()
            };
        }
    }
}
