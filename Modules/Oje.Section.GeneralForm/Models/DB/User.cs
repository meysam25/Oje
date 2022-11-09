using Oje.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("Users")]
    public class User 
    {
        public User()
        {
            GeneralFilledForms = new();
            GeneralFilledFormStatuses = new();
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
        [MaxLength(12)]
        public string Nationalcode { get; set; }
        [MaxLength(14)]
        public string Mobile { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Tell { get; set; }
        [MaxLength(12)]
        public string PostalCode { get; set; }
        [MaxLength(1000)]
        public string Address { get; set; }
        public PersonType? RealOrLegaPerson { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("CreateUser")]
        public List<GeneralFilledForm> GeneralFilledForms { get; set; }
        [InverseProperty("User")]
        public List<GeneralFilledFormStatus> GeneralFilledFormStatuses { get; set; }
    }
}
