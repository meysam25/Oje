using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("TenderFilledFormValidCompanies")]
    public class TenderFilledFormValidCompany : SignatureEntity
    {
        public long TenderFilledFormId { get; set; }
        [ForeignKey("TenderFilledFormId"), InverseProperty("TenderFilledFormValidCompanies")]
        public TenderFilledForm TenderFilledForm { get; set; }
        public int CompanyId { get; set; }
    }
}
