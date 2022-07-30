using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserRegisterFormDiscountCodes")]
    public class UserRegisterFormDiscountCode
    {
        public UserRegisterFormDiscountCode()
        {
            UserRegisterFormDiscountCodeUses = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public int UserRegisterFormId { get; set; }
        [ForeignKey("UserRegisterFormId"), InverseProperty("UserRegisterFormDiscountCodes")]
        public UserRegisterForm UserRegisterForm { get; set; }
        [Required, MaxLength(30)]
        public string Code { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long? Price { get; set; }
        public int? Percent { get; set; }
        public int MaxUse { get; set; }
        public long MaxPrice { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("UserRegisterFormDiscountCode")]
        public List<UserRegisterFormDiscountCodeUse> UserRegisterFormDiscountCodeUses { get; set; }
    }
}
