using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountSizpayMainGridResultVM
    {
        [Display(Name = "ردیف")]
        public int row { get; set; }
        [Display(Name = "شناسه")]
        public int id { get; set; }
        [Display(Name = "عنوان")]
        public string title { get; set; }
        [Display(Name = "شناسه ترمینال")]
        public string terminalId { get; set; }
        [Display(Name = "شناسه پذیرنده")]
        public string merchandId { get; set; }
        [Display(Name = "شماره شبا")]
        public string shbaNo { get; set; }
    }
}
