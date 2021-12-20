using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Enums
{
    public enum InsuranceContractUserFamilyRelation
    {
        [Display(Name = "اصلی")]
        Self = 1,
        [Display(Name = "همسر")]
        Spouse = 2,
        [Display(Name = "فرزند پسر")]
        ChildBoy = 3,
        [Display(Name = "فرزند دختر")]
        ChildGirl = 4,
        [Display(Name = "پدر")]
        Father = 5,
        [Display(Name = "مادر")]
        Mother = 6,
        [Display(Name = "نوه")]
        Grandchild = 7,
    }
}
