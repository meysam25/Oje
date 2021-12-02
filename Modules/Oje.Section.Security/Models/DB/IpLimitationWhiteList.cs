using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Security.Models.DB
{
    [Table("IpLimitationWhiteLists")]
    public class IpLimitationWhiteList
    {
        [Key]
        public int Id { get; set; }
        public int Ip1 { get; set; }
        public int Ip2 { get; set; }
        public int Ip3 { get; set; }
        public int Ip4 { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
    }
}
