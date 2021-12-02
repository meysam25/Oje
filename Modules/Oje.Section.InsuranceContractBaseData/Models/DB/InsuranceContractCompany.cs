using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractCompanies")]
    public class InsuranceContractCompany
    {
        public InsuranceContractCompany()
        {
            InsuranceContracts = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string ShomareSabt { get; set; }
        [MaxLength(50)]
        public string CodeEghtesadi { get; set; }
        [MaxLength(50)]
        public string ShenaseMeli { get; set; }
        [MaxLength(4000)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(100)]
        public string RabeteSazmaniName { get; set; }
        [MaxLength(100)]
        public string ModirAmelName { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        [InverseProperty("CreateUserInsuranceContractCompanies")]
        public User CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        [InverseProperty("UpdateUserInsuranceContractCompanies")]
        public User UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("InsuranceContractCompany")]
        public List<InsuranceContract> InsuranceContracts { get; set; }

    }
}
