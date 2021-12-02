using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.DB
{
    [Table("Jobs")]
    public class Job
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int JobDangerLevelId { get; set; }
        [ForeignKey("JobDangerLevelId")]
        [InverseProperty("Jobs")]
        public JobDangerLevel JobDangerLevel { get; set; }
        public bool IsActive { get; set; }
    }
}
