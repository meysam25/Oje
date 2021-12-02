using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.SalesNetworkBaseData.Models.DB
{
    [Table("SalesNetworkMarketers")]
    public class SalesNetworkMarketer
    {
        public SalesNetworkMarketer()
        {
            Childs = new();
        }

        [Key]
        public long Id { get; set; }
        public int SalesNetworkId { get; set; }
        [ForeignKey("SalesNetworkId")]
        [InverseProperty("SalesNetworkMarketers")]
        public SalesNetwork SalesNetwork { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("SalesNetworkMarketers")]
        public User User { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [InverseProperty("Childs")]
        public SalesNetworkMarketer Parent { get; set; }
        public DateTime CreateDate { get; set; }

        [InverseProperty("Parent")]
        public List<SalesNetworkMarketer> Childs { get; set; }
    }
}
