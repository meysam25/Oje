using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Services.EContext;
using System.Linq;

namespace Oje.Section.GlobalForms.Services
{
    public class GeneralFormJsonService : IGeneralFormJsonService
    {
        readonly GeneralFormDBContext db = null;
        public GeneralFormJsonService
            (
                GeneralFormDBContext db
            )
        {
            this.db = db;
        }

        public void Create(long generalFilledFormId, string jsonConfig)
        {
            var tempHash = jsonConfig.GetSha1();
            var curHash = tempHash.GetHashCode32();

            var foundId = db.GeneralFormCacheJsons.Where(t => t.HashCode == curHash).Select(t => t.Id).FirstOrDefault();
            if (foundId <= 0)
            {
                var newCacheJson = new GeneralFormCacheJson()
                {
                    HashCode = curHash,
                    JsonConfig = jsonConfig
                };

                db.Entry(newCacheJson).State = EntityState.Added;
                db.SaveChanges();

                foundId = newCacheJson.Id;
            }

            db.Entry(new GeneralFormJson()
            {
                GeneralFilledFormId = generalFilledFormId,
                GeneralFormCacheJsonId = foundId
            }).State = EntityState.Added;
            db.SaveChanges();
        }

        public GeneralFormCacheJson GetCacheBy(long generalFilledFormId)
        {
            return db.GeneralFormJsons.Where(t => t.GeneralFilledFormId == generalFilledFormId).Select(t => t.GeneralFormCacheJson).AsNoTracking().FirstOrDefault();
        }
    }
}
