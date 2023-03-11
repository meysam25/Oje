using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("TenderFilledFormValidCompanies")]
    public class TenderFilledFormValidCompany : SignatureEntity
    {
        public long TenderFilledFormId { get; set; }
        [ForeignKey("TenderFilledFormId"), InverseProperty("TenderFilledFormValidCompanies")]
        public TenderFilledForm TenderFilledForm { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("TenderFilledFormValidCompanies")]
        public Company Company { get; set; }
    }
}
