using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.ProposalFormBaseData.Models.DB
{
    [Table("ProposalForms")]
    public class ProposalForm
    {
        public ProposalForm()
        {
            ProposalFormRequiredDocumentTypes = new();
            PaymentMethods = new();
            ProposalFormPostPrices = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public int ProposalFormCategoryId { get; set; }
        [ForeignKey("ProposalFormCategoryId")]
        [InverseProperty("ProposalForms")]
        public ProposalFormCategory ProposalFormCategory { get; set; }
        [Required]
        public string JsonConfig { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        [InverseProperty("CreateUserProposalForms")]
        public User CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        [InverseProperty("UpdateUserProposalForms")]
        public User UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        [MaxLength(100)]
        public string RulesFile { get; set; }
        public ProposalFormType? Type { get; set; }
        public int? SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId")]
        [InverseProperty("ProposalForms")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("ProposalForm")]
        public List<ProposalFormRequiredDocumentType> ProposalFormRequiredDocumentTypes { get; set; }
        [InverseProperty("ProposalForm")]
        public List<PaymentMethod> PaymentMethods { get; set; }
        [InverseProperty("ProposalForm")]
        public List<ProposalFormPostPrice> ProposalFormPostPrices { get; set; }
    }
}
