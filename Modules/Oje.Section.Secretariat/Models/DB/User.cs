using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;


namespace Oje.Section.Secretariat.Models.DB
{
    [Table("Users")]
    public class User : SignatureEntity
    {
        public User()
        {
            SecretariatUserDigitalSignatures = new();
            SecretariatLetters = new();
            SecretariatLetterUsers = new();
            UserRoles = new();
            BySecretariatLetterUsers = new();
            SecretariatLetterRecives = new();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }
        [MaxLength(50)]
        public string FatherName { get; set; }
        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public long? ParentId { get; set; }

        [MaxLength(12)]
        public string Nationalcode { get; set; }
        [MaxLength(14)]
        public string Mobile { get; set; }
        public bool IsMobileConfirm { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public bool IsEmailConfirm { get; set; }
        [MaxLength(50)]
        public string Tell { get; set; }
        [MaxLength(12)]
        public string PostalCode { get; set; }
        [MaxLength(1000)]
        public string Address { get; set; }
        [MaxLength(100)]
        public string UserPic { get; set; }
        public long? CreateByUserId { get; set; }
        public long? UpdateByUserId { get; set; }
        public long? AgentCode { get; set; }
        [MaxLength(100)]
        public string CompanyTitle { get; set; }
        [MaxLength(40)]
        public string BankShaba { get; set; }
        public DateTime? BirthDate { get; set; }
        [MaxLength(50)]
        public string InsuranceECode { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? CountInvalidPass { get; set; }
        public DateTime? TemproryLockDate { get; set; }
        public decimal? MapLat { get; set; }
        public decimal? MapLon { get; set; }
        public byte? MapZoom { get; set; }
        public NetTopologySuite.Geometries.Point MapLocation { get; set; }
        public DateTime? HireDate { get; set; }
        public Gender? Gender { get; set; }
        [MaxLength(20)]
        public string ShenasnameNo { get; set; }
        public MarrageStatus? MarrageStatus { get; set; }
        public int? BankId { get; set; }
        [MaxLength(50)]
        public string LastSessionFileName { get; set; }
        [MaxLength(50)]
        public string RefferCode { get; set; }
        public PersonType? RealOrLegaPerson { get; set; }
        public DateTime? LicenceExpireDate { get; set; }
        public int? StartHour { get; set; }
        public int? EndHour { get; set; }
        public bool? WorkingHolyday { get; set; }
        public int? SiteSettingId { get; set; }
        public bool? CanSeeOtherSites { get; set; }
        public int? BirthCertificateIssuingPlaceProvinceId { get; set; }
        [MaxLength(20)]
        public string AccountCardNo { get; set; }

        [InverseProperty(nameof(User))]
        public List<SecretariatUserDigitalSignature> SecretariatUserDigitalSignatures { get; set; }
        [InverseProperty(nameof(User))]
        public List<SecretariatLetter> SecretariatLetters { get; set; }
        [InverseProperty(nameof(User))]
        public List<SecretariatLetterUser> SecretariatLetterUsers { get; set; }
        [InverseProperty(nameof(User))]
        public List<UserRole> UserRoles { get; set; }
        [InverseProperty("ByUser")]
        public List<SecretariatLetterUser> BySecretariatLetterUsers { get; set; }
        [InverseProperty(nameof(User))]
        public List<SecretariatLetterRecive> SecretariatLetterRecives { get; set; }
    }
}
