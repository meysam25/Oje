using System;

namespace Oje.Sanab.Models.View
{
    public class UserResultVM
    {
        //وضعیت انطباق کد ملی و تاریخ ‏تولد
        public bool? NationalIDandBirthday { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsAlive { get; set; }
        //وضعیت انطباق کد ملی+ شماره موبایل
        public bool? NationalIDandMobileNo { get; set;  }
    }
}
