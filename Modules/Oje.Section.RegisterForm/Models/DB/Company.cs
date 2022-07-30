using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            UserCompanies = new();
            UserFilledRegisterFormCompanies = new();
            UserRegisterFormCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        [Required, MaxLength(100)]
        public string Pic { get; set; }

        [InverseProperty("Company")]
        public List<UserCompany> UserCompanies { get; set; }
        [InverseProperty("Company")]
        public List<UserFilledRegisterFormCompany> UserFilledRegisterFormCompanies { get; set; }
        [InverseProperty("Company")]
        public List<UserRegisterFormCompany> UserRegisterFormCompanies { get; set; }
    }
}
