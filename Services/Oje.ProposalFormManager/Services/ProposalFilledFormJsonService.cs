using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class ProposalFilledFormJsonService: IProposalFilledFormJsonService
    {
        readonly ProposalFormDBContext db = null;
        public ProposalFilledFormJsonService(
                ProposalFormDBContext db
            )
        {
            this.db = db;
        }

        public void Create(long proposalFilledFormId, string jsonConfig)
        {
            var tempHash = jsonConfig.GetSha1();
            var curHash = tempHash.GetHashCode32();

            var foundId = db.ProposalFilledFormCacheJsons.Where(t => t.HashCode == curHash).Select(t => t.Id).FirstOrDefault();
            if(foundId <= 0)
            {
                var newCacheJson = new ProposalFilledFormCacheJson() 
                {
                    HashCode = curHash,
                    JsonConfig = jsonConfig
                };

                db.Entry(newCacheJson).State = EntityState.Added;
                db.SaveChanges();

                newCacheJson.FilledSignature();
                foundId = newCacheJson.Id;
                db.SaveChanges();
            }

            var newJsonItem = new ProposalFilledFormJson()
            {
                ProposalFilledFormId = proposalFilledFormId,
                ProposalFilledFormCacheJsonId = foundId
            };
            newJsonItem.FilledSignature();

            db.Entry(newJsonItem).State = EntityState.Added;
            db.SaveChanges();
        }

        public ProposalFilledFormJson GetBy(long proposalFilledFormId)
        {
            return db.ProposalFilledFormJsons.Where(t => t.ProposalFilledFormId == proposalFilledFormId).AsNoTracking().FirstOrDefault();
        }

        public ProposalFilledFormCacheJson GetCacheBy(long id)
        {
            return db.ProposalFilledFormJsons.Where(t => t.ProposalFilledFormId == id).Select(t => t.ProposalFilledFormCacheJson).AsNoTracking().FirstOrDefault();
        }
    }
}
