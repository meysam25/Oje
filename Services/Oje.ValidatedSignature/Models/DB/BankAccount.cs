﻿using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("BankAccounts")]
    public class BankAccount: SignatureEntity, IEntityWithSiteSettingId
    {
        public BankAccount()
        {
            BankAccountSizpaies = new();
            BankAccountFactors = new();
            BankAccountSadads = new();
            BankAccountSeps = new();
        }

        [Key]
        public int Id { get; set; }
        public int BankId { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public long CardNo { get; set; }
        [Required, MaxLength(32)]
        public string ShabaNo { get; set; }
        public long HesabNo { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("BankAccounts")]
        public User User { get; set; }
        public bool IsForPayment { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("BankAccounts")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("BankAccount")]
        public List<BankAccountSizpay> BankAccountSizpaies { get; set; }
        [InverseProperty("BankAccount")]
        public List<BankAccountFactor> BankAccountFactors { get; set; }
        [InverseProperty("BankAccount")]
        public List<BankAccountSadad> BankAccountSadads { get; set; }
        [InverseProperty("BankAccount")]
        public List<BankAccountSep> BankAccountSeps { get; set; }
    }
}
