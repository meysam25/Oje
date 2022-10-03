using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.ProposalFormBaseData.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            ProposalForms = new();
            ProposalFormRequiredDocumentTypes = new();
            AgentReffers = new();
            PaymentMethods = new();
            ProposalFormPostPrices = new();
            ProposalFormPrintDescrptions = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string WebsiteUrl { get; set; }
        [Required]
        [MaxLength(100)]
        public string PanelUrl { get; set; }
        public long UserId { get; set; }
        [InverseProperty("SiteSettings")]
        [ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime CreateDate { get; set; }
        public long CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        [InverseProperty("CreateUserSiteSettings")]
        public User CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        [InverseProperty("UpdateUserSiteSettings")]
        public User UpdateUser { get; set; }
        public bool IsHttps { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("SiteSetting")]
        public List<ProposalForm> ProposalForms { get; set; }
        [InverseProperty("SiteSetting")]
        public List<ProposalFormRequiredDocumentType> ProposalFormRequiredDocumentTypes { get; set; }
        [InverseProperty("SiteSetting")]
        public List<AgentReffer> AgentReffers { get; set; }
        [InverseProperty("SiteSetting")]
        public List<PaymentMethod> PaymentMethods { get; set; }
        [InverseProperty("SiteSetting")]
        public List<ProposalFormPostPrice> ProposalFormPostPrices { get; set; }
        [InverseProperty("SiteSetting")]
        public List<ProposalFormPrintDescrption> ProposalFormPrintDescrptions { get; set; }
    }
}
