﻿using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("Users")]
    public class User: SignatureEntity
    {
        public User()
        {
            ProposalFilledFormUsers = new();
            FromUserProposalFilledFormUsers = new();
            UserRoles = new();
            UserCompanies = new();
            CreateUserProposalFilledFormCompanies = new();
            UpdateUserProposalFilledFormCompanies = new();
            ProposalFilledFormStatusLogs = new();
            ProposalFormReminders = new();
            WalletTransactions = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; }
        [Required, MaxLength(50)]
        public string Firstname { get; set; }
        [Required, MaxLength(50)]
        public string Lastname { get; set; }
        [Required, MinLength(200)]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId"), InverseProperty("Childs")]
        public User Parent { get; set; }
        [MaxLength(12)]
        public string Nationalcode { get; set; }
        [MaxLength(14)]
        public string Mobile { get; set; }
        public bool IsMobileConfirm { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public bool IsEmailConfirm { get; set; }
        [MaxLength(50)]
        public string Tell { get; set; }
        [MaxLength(12)]
        public string PostalCode { get; set; }
        public long? AgentCode { get; set;}
        [MaxLength(1000)]
        public string Address { get; set; }
        public long? CreateByUserId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        [MaxLength(100)]
        public string CompanyTitle { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("Parent")]
        public List<User> Childs { get; set; }
        [InverseProperty("User")]
        public List<ProposalFilledFormUser> ProposalFilledFormUsers { get; set; }
        [InverseProperty("FromUser")]
        public List<ProposalFilledFormUser> FromUserProposalFilledFormUsers { get; set; }
        [InverseProperty("User")]
        public List<UserRole> UserRoles { get; set; }
        [InverseProperty("User")]
        public List<UserCompany> UserCompanies { get; set; }
        [InverseProperty("CreateUser")]
        public List<ProposalFilledFormCompany> CreateUserProposalFilledFormCompanies { get; set; }
        [InverseProperty("UpdateUser")]
        public List<ProposalFilledFormCompany> UpdateUserProposalFilledFormCompanies { get; set; }
        [InverseProperty("User")]
        public List<ProposalFilledFormStatusLog> ProposalFilledFormStatusLogs { get; set; }
        [InverseProperty("LoginUser")]
        public List<ProposalFormReminder> ProposalFormReminders { get; set; }
        [InverseProperty("User")]
        public List<WalletTransaction> WalletTransactions { get; set; }
    }
}
