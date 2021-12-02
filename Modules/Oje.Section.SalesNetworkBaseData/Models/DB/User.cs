using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.SalesNetworkBaseData.Models.DB
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            CreateUserSalesNetworks = new();
            UpdateUserSalesNetworks = new();
            SalesNetworkMarketers = new();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }

        [InverseProperty("CreateUser")]
        public List<SalesNetwork> CreateUserSalesNetworks { get; set; }
        [InverseProperty("UpdateUser")]
        public List<SalesNetwork> UpdateUserSalesNetworks { get; set; }
        [InverseProperty("User")]
        public List<SalesNetworkMarketer> SalesNetworkMarketers { get; set; }
    }
}
