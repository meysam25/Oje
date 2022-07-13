using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            UserRegisterForms = new();
            UserFilledRegisterForms = new();
            UserCompanies = new();
            UserRoles = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; }
        [Required, MaxLength(50)]
        public string Firstname { get; set; }
        [Required, MaxLength(50)]
        public string Lastname { get; set; }
        [Required, MaxLength(200)]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
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
        public long? AgentCode { get; set; }
        [MaxLength(100)]
        public string CompanyTitle { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public decimal? MapLat { get; set; }
        public decimal? MapLon { get; set; }
        public byte? MapZoom { get; set; }
        public NetTopologySuite.Geometries.Point MapLocation { get; set; }
        public DateTime? BirthDate { get; set; }
        [MaxLength(20)]
        public string BankAccount { get; set; }
        [MaxLength(40)]
        public string BankShaba { get; set; }
        public int? SiteSettingId { get; set; }


        [InverseProperty("PaymentUser")]
        public List<UserRegisterForm> UserRegisterForms { get; set; }
        [InverseProperty("User")]
        public List<UserFilledRegisterForm> UserFilledRegisterForms { get; set; }
        [InverseProperty("User")]
        public List<UserCompany> UserCompanies { get; set; }
        [InverseProperty("User")]
        public List<UserRole> UserRoles { get; set; }
    }
}
