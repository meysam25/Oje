using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.SalesNetworkBaseData.Models.DB
{
    [Table("SalesNetworks")]
    public class SalesNetwork: EntityWithCreateUser<User, long>
    {
        public SalesNetwork()
        {
            SalesNetworkCompanies = new ();
            SalesNetworkProposalForms = new();
            SalesNetworkMarketers = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public SalesNetworkType Type { get; set; }
        public PersonType CalceType { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        [InverseProperty("CreateUserSalesNetworks")]
        public User CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        [InverseProperty("UpdateUserSalesNetworks")]
        public User UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("SalesNetwork")]
        public List<SalesNetworkCompany> SalesNetworkCompanies { get; set; }
        [InverseProperty("SalesNetwork")]
        public List<SalesNetworkProposalForm> SalesNetworkProposalForms { get; set; }
        [InverseProperty("SalesNetwork")]
        public List<SalesNetworkMarketer> SalesNetworkMarketers { get; set; }
    }
}
