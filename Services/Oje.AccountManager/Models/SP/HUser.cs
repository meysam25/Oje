using System.ComponentModel.DataAnnotations;

namespace Oje.AccountService.Models.SP
{
    public class HUser
    {
        [Key]
        public long id { get; set; }
        public string fistname { get; set; }
        public string lastname { get; set; }
        public long? parentid { get; set; }
        public int lv { get; set; }
        public string role { get; set; }
    }
}
