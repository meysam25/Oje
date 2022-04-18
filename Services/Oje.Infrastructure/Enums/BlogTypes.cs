using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum BlogTypes
    {
        [Display(Name = "متن")]
        Text = 1,
        [Display(Name = "صدا")]
        Sound = 2,
        [Display(Name = "ویدئو")]
        Video = 3
    }
}
