using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("SiteSettings")]
    public  class SiteSetting
    {
        public SiteSetting()
        {
            ProposalFormReminders = new();
            ProposalFilledForms = new();
            ProposalFilledFormSiteSettings = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<ProposalFormReminder> ProposalFormReminders { get; set; }
        [InverseProperty("SiteSetting")]
        public List<ProposalFilledForm> ProposalFilledForms { get; set; }
        [InverseProperty("SiteSetting")]
        public List<ProposalFilledFormSiteSetting> ProposalFilledFormSiteSettings { get; set; }
    }
}
