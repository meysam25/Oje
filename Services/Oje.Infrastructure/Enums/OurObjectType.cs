using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum OurObjectType : byte
    {
        [Display(Name = "مشتریان ما")]
        OurCustomers = 1,
        [Display(Name = "شرکت های طرف قرارداد")]
        OurContractCompanies = 2
    }
}
