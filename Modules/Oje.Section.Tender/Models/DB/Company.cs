using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            TenderFilledFormValidCompanies = new();
            TenderFilledFormPrices = new();
            UserCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string Pic64 { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("Company")]
        public List<TenderFilledFormValidCompany> TenderFilledFormValidCompanies { get; set; }
        [InverseProperty("Company")]
        public List<TenderFilledFormPrice> TenderFilledFormPrices { get; set; }
        [InverseProperty("Company")]
        public List<UserCompany> UserCompanies { get; set; }
    }
}
