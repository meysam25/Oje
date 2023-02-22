using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Tender.Models.DB
{
    [Table("Users")]
    public class User: EntityWithParent<User>
    {
        public User()
        {
            TenderFilledForms = new();
            TenderFilledFormPrices = new();
            Childs = new();
            UserCompanies = new();
            TenderFilledFormIssues = new();
            TenderFilledFormsValues = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; }
        [MaxLength(59), Required]
        public string Firstname { get; set; }
        [Required, MaxLength(50)]
        public string Lastname { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId"), InverseProperty("Childs")]
        public User Parent { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public long? AgentCode { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("User")]
        public List<TenderFilledForm> TenderFilledForms { get; set; }
        [InverseProperty("User")]
        public List<TenderFilledFormPrice> TenderFilledFormPrices { get; set; }
        [InverseProperty("Parent")]
        public List<User> Childs { get; set; }
        [InverseProperty("User")]
        public List<UserCompany> UserCompanies { get; set; }
        [InverseProperty("User")]
        public List<TenderFilledFormIssue> TenderFilledFormIssues { get; set; }
        [InverseProperty("User")]
        public List<TenderFilledFormsValue> TenderFilledFormsValues { get; set; }
    }
}
