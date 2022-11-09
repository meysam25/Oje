using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("GeneralFilledFormStatuses")]
    public class GeneralFilledFormStatus
    {
        [Key]
        public long Id { get; set; }
        public long GeneralFormStatusId { get; set; }
        [ForeignKey("GeneralFormStatusId"), InverseProperty("GeneralFilledFormStatuses")]
        public GeneralFormStatus GeneralFormStatus { get; set; }
        public long GeneralFilledFormId { get; set; }
        [ForeignKey("GeneralFilledFormId"), InverseProperty("GeneralFilledFormStatuses")]
        public GeneralFilledForm GeneralFilledForm { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("GeneralFilledFormStatuses")]
        public User User { get; set; }
        public DateTime CreateDate { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
    }
}
