using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("Users")]
    public class User : EntityWithParent<User>
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
            Childs = new();
            InsuranceContractProposalFilledForms = new();
            InsuranceContractProposalFilledFormStatusLogs = new();
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
        [MaxLength(40)]
        public string BankShaba { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsActive { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId"), InverseProperty("Childs")]
        public User Parent { get; set; }
        public Gender? Gender { get; set; }
        public int? BankId { get; set; }
        public DateTime? HireDate { get; set; }
        [MaxLength(50)]
        public string Tell { get; set; }
        [MaxLength(20)]
        public string AccountCardNo { get; set; }


        [InverseProperty("Parent")]
        public List<User> Childs { get; set; }
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
        [InverseProperty("CreateUser")]
        public List<InsuranceContractProposalFilledForm> InsuranceContractProposalFilledForms { get; set; }
        [InverseProperty("User")]
        public List<InsuranceContractProposalFilledFormStatusLog> InsuranceContractProposalFilledFormStatusLogs { get; set; }

    }
}
