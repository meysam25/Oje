using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.SalesNetworkBaseData.Models.DB
{
    [Table("Users")]
    public class User : EntityWithParent<User>
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
        public long? ParentId { get; set; }
        [ForeignKey("ParentId"), InverseProperty("Childs")]
        public User Parent { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("Parent")]
        public List<User> Childs { get; set; }
        [InverseProperty("CreateUser")]
        public List<SalesNetwork> CreateUserSalesNetworks { get; set; }
        [InverseProperty("UpdateUser")]
        public List<SalesNetwork> UpdateUserSalesNetworks { get; set; }
        [InverseProperty("User")]
        public List<SalesNetworkMarketer> SalesNetworkMarketers { get; set; }
    }
}
