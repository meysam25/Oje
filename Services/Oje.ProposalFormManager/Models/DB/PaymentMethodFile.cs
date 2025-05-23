﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("PaymentMethodFiles")]
    public class PaymentMethodFile
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public int PaymentMethodId { get; set; }
        [ForeignKey("PaymentMethodId"), InverseProperty("PaymentMethodFiles")]
        public PaymentMethod PaymentMethod { get; set; }
        [MaxLength(200)]
        public string DownloadImageUrl { get; set; }
        public bool IsRequired { get; set; }

    }
}
