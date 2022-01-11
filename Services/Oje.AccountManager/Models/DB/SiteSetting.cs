using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            Roles = new List<Role>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string WebsiteUrl { get; set; }
        [Required]
        [MaxLength(100)]
        public string PanelUrl { get; set; }
        public long? UserId { get; set; }

        [InverseProperty("SiteSetting")]
        public List<Role> Roles { get; set; }
    }
}
