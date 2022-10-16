using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.BaseData.Models.DB
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            SiteSettings = new List<SiteSetting>();
            CreateUserSiteSettings = new List<SiteSetting>();
            UpdateUserSiteSettings = new List<SiteSetting>();
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
        public int? SiteSettingId { get; set; }

        [InverseProperty("User")]
        public List<SiteSetting> SiteSettings { get; set; }
        [InverseProperty("CreateUser")]
        public List<SiteSetting> CreateUserSiteSettings { get; set; }
        [InverseProperty("UpdateUser")]
        public List<SiteSetting> UpdateUserSiteSettings { get; set; }
    }
}
