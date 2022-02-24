using Oje.Infrastructure.Enums;
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
    [Table("InsuranceContractUsers")]
    public class InsuranceContractUser: EntityWithCreateUser<User,long>
    {
        public InsuranceContractUser()
        {
            Childs = new();
        }
        [Key]
        public long Id { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [InverseProperty("Childs")]
        public InsuranceContractUser Parent { get; set; }
        [InverseProperty("Parent")]
        public List<InsuranceContractUser> Childs { get; set; }
        public int InsuranceContractId { get; set; }
        [ForeignKey("InsuranceContractId")]
        [InverseProperty("InsuranceContractUsers")]
        public InsuranceContract InsuranceContract { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("InsuranceContractUsers")]
        public User User { get; set; }
        public InsuranceContractUserFamilyRelation FamilyRelation { get; set; }
        public InsuranceContractUserStatus Status { get; set; }
        [MaxLength(200)]
        public string KartMeliFileUrl { get; set; }
        [MaxLength(200)]
        public string ShenasnamePage1FileUrl { get; set; }
        [MaxLength(200)]
        public string ShenasnamePage2FileUrl { get; set; }
        [MaxLength(200)]
        public string BimeFileUrl { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        [InverseProperty("CreateUserInsuranceContractUsers")]
        public User CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        [InverseProperty("UpdateUserInsuranceContractUsers")]
        public User UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool HasConfilictWithUser { get; set; }
        public int SiteSettingId { get; set; }

    }
}
