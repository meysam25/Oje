using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("Holydays")]
    public class Holyday
    {
        [Key]
        public long Id { get; set; }
        public DateTime TargetDate { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
    }
}
