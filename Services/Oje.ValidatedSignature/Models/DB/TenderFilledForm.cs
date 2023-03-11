using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("TenderFilledForms")]
    public class TenderFilledForm : SignatureEntity, IEntityWithSiteSettingId
    {
        public TenderFilledForm()
        {
            TenderFilledFormsValues = new();
            TenderFilledFormPFs = new();
            TenderFilledFormValidCompanies = new();
            TenderFilledFormPrices = new();
            TenderFilledFormIssues = new();
            TenderFilledFormJsons = new();
        }

        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? AvalibleDate { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? PriceExpireDate { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("TenderFilledForms")]
        public User User { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public bool? IsPublished { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("TenderFilledForms")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("TenderFilledForm")]
        public List<TenderFilledFormsValue> TenderFilledFormsValues { get; set; }
        [InverseProperty("TenderFilledForm")]
        public List<TenderFilledFormJson> TenderFilledFormJsons { get; set; }
        [InverseProperty("TenderFilledForm")]
        public List<TenderFilledFormPF> TenderFilledFormPFs { get; set; }
        [InverseProperty("TenderFilledForm")]
        public List<TenderFilledFormValidCompany> TenderFilledFormValidCompanies { get; set; }
        [InverseProperty("TenderFilledForm")]
        public List<TenderFilledFormPrice> TenderFilledFormPrices { get; set; }
        [InverseProperty("TenderFilledForm")]
        public List<TenderFilledFormIssue> TenderFilledFormIssues { get; set; }

    }
}
