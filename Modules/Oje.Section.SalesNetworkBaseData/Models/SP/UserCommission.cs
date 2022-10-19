using Oje.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Oje.Section.SalesNetworkBaseData.Models.SP
{
    public class UserCommission
    {
        [Key]
        public long UserId { get; set; }
        public long? parentid { get; set; }
        public PersonType realOrLegal { get; set; }
        public string fistname { get; set; }
        public string lastname { get; set; }
        public int lv { get; set; }
        public long? commission { get; set; }
        public long? saleSum { get; set; }
        public string role { get; set; }
    }
}
