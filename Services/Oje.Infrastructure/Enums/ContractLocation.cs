using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum ContractLocation: byte
    {
        [Display(Name = "محل من")]
        MyLocation = 1,
        [Display(Name = "شرکت")]
        CompanyLocation = 2
    }
}
