using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractValidUserForFullDebits")]
    public class InsuranceContractValidUserForFullDebit: EntityWithCreateUser<User, long>
    {
        [Key]
        public long Id { get; set; }
        public int InsuranceContractId { get; set; }
        [ForeignKey("InsuranceContractId")]
        [InverseProperty("InsuranceContractValidUserForFullDebits")]
        public InsuranceContract InsuranceContract { get; set; }
        [Required]
        [MaxLength(14)]
        public string Mobile { get; set; }
        [Required]
        [MaxLength(12)]
        public string NationalCode { get; set; }
        public int CountUse { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        [InverseProperty("CreateUserInsuranceContractValidUserForFullDebits")]
        public User CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        [InverseProperty("UpdateUserInsuranceContractValidUserForFullDebits")]
        public User UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
