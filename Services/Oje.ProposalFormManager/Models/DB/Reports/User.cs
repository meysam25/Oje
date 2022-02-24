using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB.Reports
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            ProposalFilledFormUsers = new();
            FromUserProposalFilledFormUsers = new();
            UserRoles = new();
            Childs = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; }
        [Required, MaxLength(50)]
        public string Firstname { get; set; }
        [Required, MaxLength(50)]
        public string Lastname { get; set; }
        public int? SiteSettingId { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId"), InverseProperty("Childs")]
        public User Parent { get; set; }


        [InverseProperty("Parent")]
        public List<User> Childs { get; set; }
        [InverseProperty("User")]
        public List<ProposalFilledFormUser> ProposalFilledFormUsers { get; set; }
        [InverseProperty("FromUser")]
        public List<ProposalFilledFormUser> FromUserProposalFilledFormUsers { get; set; }
        [InverseProperty("User")]
        public List<UserRole> UserRoles { get; set; }
    }
}
