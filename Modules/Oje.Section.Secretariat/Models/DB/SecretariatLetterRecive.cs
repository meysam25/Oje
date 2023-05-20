using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Secretariat.Models.DB
{
    [Table("SecretariatLetterRecives")]
    public class SecretariatLetterRecive
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime Date { get; set; }
        [Required, MaxLength(50)]
        public string Number { get; set; }
        [MaxLength(14)]
        public string Mobile { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId)), InverseProperty("SecretariatLetterRecives")]
        public User User { get; set; }
        public int SiteSettingId { get; set; }
    }
}
