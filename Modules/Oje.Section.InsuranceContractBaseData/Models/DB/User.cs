using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            CreateUserInsuranceContractTypes = new();
            UpdateUserInsuranceContractTypes = new();
            CreateUserInsuranceContractCompanies = new();
            UpdateUserInsuranceContractCompanies = new();
            CreateUserInsuranceContracts = new();
            UpdateUserInsuranceContracts = new();
            CreateUserInsuranceContractValidUserForFullDebits = new();
            UpdateUserInsuranceContractValidUserForFullDebits = new();
            InsuranceContractUsers = new();
            CreateUserInsuranceContractUsers = new();
            UpdateUserInsuranceContractUsers = new();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }
        [MaxLength(50)]
        public string Mobile { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string InsuranceECode { get; set; }
        [MaxLength(12)]
        public string Nationalcode { get; set; }
        [MaxLength(20)]
        public string BankAccount { get; set; }
        [MaxLength(40)]
        public string BankShaba { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsActive { get; set; }




        [InverseProperty("CreateUser")]
        public List<InsuranceContractType> CreateUserInsuranceContractTypes { get; set; }
        [InverseProperty("UpdateUser")]
        public List<InsuranceContractType> UpdateUserInsuranceContractTypes { get; set; }
        [InverseProperty("CreateUser")]
        public List<InsuranceContractCompany> CreateUserInsuranceContractCompanies { get; set; }
        [InverseProperty("UpdateUser")]
        public List<InsuranceContractCompany> UpdateUserInsuranceContractCompanies { get; set; }
        [InverseProperty("CreateUser")]
        public List<InsuranceContract> CreateUserInsuranceContracts { get; set; }
        [InverseProperty("UpdateUser")]
        public List<InsuranceContract> UpdateUserInsuranceContracts { get; set; }
        [InverseProperty("CreateUser")]
        public List<InsuranceContractValidUserForFullDebit> CreateUserInsuranceContractValidUserForFullDebits { get; set; }
        [InverseProperty("UpdateUser")]
        public List<InsuranceContractValidUserForFullDebit> UpdateUserInsuranceContractValidUserForFullDebits { get; set; }
        [InverseProperty("User")]
        public List<InsuranceContractUser> InsuranceContractUsers { get; set; }
        [InverseProperty("CreateUser")]
        public List<InsuranceContractUser> CreateUserInsuranceContractUsers { get; set; }
        [InverseProperty("UpdateUser")]
        public List<InsuranceContractUser> UpdateUserInsuranceContractUsers { get; set; }

    }
}
