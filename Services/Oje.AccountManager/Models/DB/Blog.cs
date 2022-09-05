using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("Blogs")]
    public class Blog
    {
        [Key]
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime PublisheDate { get; set; }
        [MaxLength(200)]
        public string ImageUrl600 { get; set; }
        [MaxLength(200)]
        public string ImageUrl200 { get; set; }
        public int SiteSettingId { get; set; }
    }
}
