using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.DB
{
    [Table("Sections")]
    public class Section
    {
        public Section()
        {
            Controllers = new List<Controller>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        [MaxLength(30)]
        public string Icon { get; set; }

        [InverseProperty("Section")]
        public List<Controller> Controllers { get; set; }
    }
}
