using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.DB;
using Oje.Section.Tender.Services.EContext;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class TenderFilledFormKeyService: ITenderFilledFormKeyService
    {

        readonly TenderDBContext db = null;

        public TenderFilledFormKeyService(TenderDBContext db)
        {
            this.db = db;
        }

        public long CreateIfNeeded(string name)
        {
            if(!db.TenderFilledFormKeys.Any(t => t.Key == name))
            {
                TenderFilledFormKey newItem = new TenderFilledFormKey() { Key = name };
                db.Entry(newItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();

                return newItem.Id;
            }

            return db.TenderFilledFormKeys.Where(t => t.Key == name).Select(t => t.Id).FirstOrDefault();
        }
    }
}
