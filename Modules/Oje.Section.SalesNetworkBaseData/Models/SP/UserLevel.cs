using System.ComponentModel.DataAnnotations;

namespace Oje.Section.SalesNetworkBaseData.Models.SP
{
    public class UserLevel
    {
        [Key]
        public long id { get; set; }
        public string text { get; set; }
    }
}
