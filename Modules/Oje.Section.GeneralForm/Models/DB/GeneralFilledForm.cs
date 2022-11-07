using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("GeneralFilledForms")]
    public class GeneralFilledForm
    {
        public GeneralFilledForm()
        {
            GeneralFormJsons = new();
            GeneralFilledFormValues = new();
        }

        [Key]
        public long Id { get; set; }
        public long GeneralFormId { get; set; }
        [ForeignKey("GeneralFormId"), InverseProperty("GeneralFilledForms")]
        public GeneralForm GeneralForm { get; set; }
        public DateTime CreateDate { get; set; }
        public long GeneralFormStatusId { get; set; }
        [ForeignKey("GeneralFormStatusId"), InverseProperty("GeneralFilledForms")]
        public GeneralFormStatus GeneralFormStatus { get; set; }
        public bool IsDelete { get; set; }
        public long? Price { get; set; }
        public string PaymentTraceCode { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId"), InverseProperty("GeneralFilledForms")]
        public User CreateUser { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("GeneralFilledForms")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("GeneralFilledForm")]
        public List<GeneralFormJson> GeneralFormJsons { get; set; }
        [InverseProperty("GeneralFilledForm")]
        public List<GeneralFilledFormValue> GeneralFilledFormValues { get; set; }
    }
}
