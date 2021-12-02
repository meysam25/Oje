using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            UserCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("Company")]
        public List<UserCompany> UserCompanies { get; set; }
    }
}
