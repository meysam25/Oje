using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.DB
{
    [Table("Provinces")]
    public class Province
    {
        public Province()
        {
            Cities = new List<City>();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("Province")]
        public List<City> Cities { get; set; }
    }
}
