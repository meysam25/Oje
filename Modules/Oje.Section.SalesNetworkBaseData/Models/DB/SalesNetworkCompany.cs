using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.SalesNetworkBaseData.Models.DB
{
    [Table("SalesNetworkCompanies")]
    public class SalesNetworkCompany
    {
        [Key]
        public long Id { get; set; }
        public int SalesNetworkId { get; set; }
        [ForeignKey("SalesNetworkId")]
        [InverseProperty("SalesNetworkCompanies")]
        public SalesNetwork SalesNetwork { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("SalesNetworkCompanies")]
        public Company Company { get; set; }
    }
}
