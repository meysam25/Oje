using System.ComponentModel.DataAnnotations;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class PaymentMethodFileMainGridResult
    {
        [Display(Name = "ردیف")]
        public int? row { get; set; }
        [Display(Name = "شناسه")]
        public int? id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "شرایط پرداخت")]
        public string payId { get; set; }
        [Display(Name ="اجباری")]
        public string isRequred { get; set; }
    }
}
