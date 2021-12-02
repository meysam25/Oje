using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.DB
{
    [Table("JobDangerLevels")]
    public class JobDangerLevel
    {
        public JobDangerLevel()
        {
            Jobs = new List<Job>();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }
        public int DangerLevel { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("JobDangerLevel")]
        public List<Job> Jobs { get; set; }
    }
}
