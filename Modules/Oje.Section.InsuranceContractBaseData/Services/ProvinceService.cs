using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Services.EContext;
using System.Linq;

namespace Oje.Section.InsuranceContractBaseData.Services
{
    public class ProvinceService: IProvinceService
    {
        readonly InsuranceContractBaseDataDBContext db = null;
        public ProvinceService
            (
                InsuranceContractBaseDataDBContext db
            ) 
        {
            this.db = db;
        }

        public string GetIdByTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
                title = title.Trim();

            return db.Provinces.Where(t => t.Title == title).Select(t => t.Id).FirstOrDefault() + "";
        }
    }
}
