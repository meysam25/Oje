using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Models.DB
{
    [Table("GlobalInputInqueries")]
    public class GlobalInputInquery
    {
        public GlobalInputInquery()
        {
            GlobalInqueries = new();
            GlobalInqueryInputParameters = new();
        }

        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int SiteSettingId { get; set; }


        [InverseProperty("GlobalInputInquery")]
        public List<GlobalInquery> GlobalInqueries { get; set; }
        [InverseProperty("GlobalInputInquery")]
        public List<GlobalInqueryInputParameter> GlobalInqueryInputParameters { get; set; }
    }
}
