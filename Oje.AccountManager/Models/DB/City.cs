using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.DB
{
    [Table("Cities")]
    public class City
    {
        public City()
        {
            Users = new();
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProvinceId { get; set; }
        [ForeignKey("ProvinceId")]
        [InverseProperty("Cities")]
        public Province Province { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("City")]
        public List<User> Users { get; set; }
    }
}
