using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Models.DB
{
    [Table("Provinces")]
    public class Province
    {
        public Province()
        {
            Cities = new();
            Users = new();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("Province")]
        public List<City> Cities { get; set; }
        [InverseProperty("Province")]
        public List<User> Users { get; set; }
    }
}
