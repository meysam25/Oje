using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.DB
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            CreatedGlobalDiscounts = new ();
            UpdatedGlobalDiscounts = new ();
            CreateUserInquiryCompanyLimits = new();
            UpdateUserInquiryCompanyLimits = new();
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

        [InverseProperty("CreateUser")]
        public List<GlobalDiscount> CreatedGlobalDiscounts { get; set; }
        [InverseProperty("UpdateUser")]
        public List<GlobalDiscount> UpdatedGlobalDiscounts { get; set; }
        [InverseProperty("CreateUser")]
        public List<InquiryCompanyLimit> CreateUserInquiryCompanyLimits { get; set; }
        [InverseProperty("UpdateUser")]
        public List<InquiryCompanyLimit> UpdateUserInquiryCompanyLimits { get; set; }

    }
}
