using System.ComponentModel.DataAnnotations;

namespace Oje.Infrastructure.Enums
{
    public enum DashboardSectionType
    {
        [Display(Name = "دکمه میانبور")]
        Shortcut = 1,
        [Display(Name = "محتوی")]
        Content = 2,
        [Display(Name = "کلید تب")]
        TabContent = 3,
        [Display(Name = "تب")]
        Tab = 4,
        [Display(Name = "کلید تب متن دینامیک")]
        TabContentDynamicContent = 5,
        [Display(Name = "کلید تب افوقی")]
        TabContentHorizontal = 6,
    }
}
