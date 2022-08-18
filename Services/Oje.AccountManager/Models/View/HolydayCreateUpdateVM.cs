namespace Oje.AccountService.Models.View
{
    public class HolydayCreateUpdateVM
    {
        public long? id { get; set; }
        public string targetDate { get; set; }
        public bool? isActive { get; set; }
        public string description { get; set; }
    }
}
