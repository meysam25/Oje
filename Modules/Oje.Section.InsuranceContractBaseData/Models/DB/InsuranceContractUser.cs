using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractUsers")]
    public class InsuranceContractUser : EntityWithCreateUser<User, long>
    {
        public InsuranceContractUser()
        {
            Childs = new();
            InsuranceContractProposalFilledFormUsers = new();
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
        public long? UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("InsuranceContractUsers")]
        public User User { get; set; }
        public InsuranceContractUserFamilyRelation FamilyRelation { get; set; }
        public InsuranceContractUserStatus Status { get; set; }
        [MaxLength(50)]
        public string InsuranceECode { get; set; }
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
        public Custody? Custody { get; set; }
        public int SiteSettingId { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string FatherName { get; set; }
        public DateTime? BirthDate { get; set; }
        [MaxLength(20)]
        public string ShenasnameNo { get; set; }
        [Required, MaxLength(11)]
        public string NationalCode { get; set; }
        public MarrageStatus? MarrageStatus { get; set; }
        public int? InsuranceContractUserBaseInsuranceId { get; set; }
        [ForeignKey("InsuranceContractUserBaseInsuranceId"), InverseProperty("InsuranceContractUsers")]
        public InsuranceContractUserBaseInsurance InsuranceContractUserBaseInsurance { get; set; }
        public int? InsuranceContractUserSubCategoryId { get; set; }
        [ForeignKey("InsuranceContractUserSubCategoryId"), InverseProperty("InsuranceContractUsers")]
        public InsuranceContractUserSubCategory InsuranceContractUserSubCategory { get; set; }
        [MaxLength(20)]
        public string InsuranceMiniBookNumber { get; set; }
        [MaxLength(14)]
        public string Mobile { get; set; }
        public Gender? Gender { get; set; }

        [InverseProperty("InsuranceContractUser")]
        public List<InsuranceContractProposalFilledFormUser> InsuranceContractProposalFilledFormUsers { get; set; }
    }
}
