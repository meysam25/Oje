using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sms.Models.DB
{
    [Table("BlockLoginUsers")]
    public class BlockLoginUser: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
