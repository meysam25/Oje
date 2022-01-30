using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.DB
{
    [Table("BlockClientConfigs")]
    public class BlockClientConfig
    {
        [Key]
        public int Id { get; set; }
        public BlockClientConfigType Type { get; set; }
        public int MaxSoftware { get; set; }
        public int MaxSuccessSoftware { get; set; }
        public int MaxFirewall { get; set; }
        public int MaxSuccessFirewall { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }

    }
}
