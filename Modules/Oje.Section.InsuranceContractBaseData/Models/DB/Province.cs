using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("Provinces")]
    public class Province
    {
        public Province()
        {
            InsuranceContractUsers = new();
            BirthCertificateIssuingPlaceInsuranceContractUsers = new();
            Cities = new();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }


        [InverseProperty("BirthCertificateIssuingPlaceProvince")]
        public List<InsuranceContractUser> BirthCertificateIssuingPlaceInsuranceContractUsers { get; set; }
        [InverseProperty("Province")]
        public List<InsuranceContractUser> InsuranceContractUsers { get; set; }
        [InverseProperty("Province")]
        public List<City> Cities { get; set; }
    }
}
