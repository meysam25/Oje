using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum BlogLastLikeAndViewType: byte
    {
        [Display(Name = "مشاهده")]
        View = 1,
        [Display(Name = "پسندید")]
        Like = 2
    }
}
