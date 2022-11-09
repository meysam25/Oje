using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.GlobalForms.Models.DB
{
    [Table("GeneralForms")]
    public class GeneralForm : IEntityWithSiteSettingIdNullable
    {
        public GeneralForm()
        {
            GeneralFormStatuses = new();
            GeneralFormRequiredDocuments = new();
            GeneralFilledForms = new();
            GeneralFormStatusGridColumns = new();
            GeneralFormStatusRoles = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string JsonConfig { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
        [Required, MaxLength(4000)]
        public string TermTemplate { get; set; }
        [MaxLength(200)]
        public string RulesFile { get; set; }
        [MaxLength(200)]
        public string ContractFile { get; set; }
        public int? SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("GeneralForms")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("GeneralForm")]
        public List<GeneralFormStatus> GeneralFormStatuses { get; set; }
        [InverseProperty("GeneralForm")]
        public List<GeneralFormRequiredDocument> GeneralFormRequiredDocuments { get; set; }
        [InverseProperty("GeneralForm")]
        public List<GeneralFilledForm> GeneralFilledForms { get; set; }
        [InverseProperty("GeneralForm")]
        public List<GeneralFormStatusGridColumn> GeneralFormStatusGridColumns { get; set; }
        [InverseProperty("GeneralForm")]
        public List<GeneralFormStatusRole> GeneralFormStatusRoles { get; set; }
    }
}
