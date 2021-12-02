using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum InsuranceContractUserStatus
    {
        [Display(Name ="موقت")]
        Temprory = 1,
        [Display(Name = "دائمی")]
        Premanent = 2
    }
}
