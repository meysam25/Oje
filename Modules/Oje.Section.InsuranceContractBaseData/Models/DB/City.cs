using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("Cities")]
    public class City
    {
        public City()
        {
            InsuranceContractUsers = new();
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProvinceId { get; set; }
        [ForeignKey("ProvinceId")]
        [InverseProperty("Cities")]
        public Province Province { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("City")]
        public List<InsuranceContractUser> InsuranceContractUsers { get; set; }
    }
}
