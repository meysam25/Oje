﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Models.DB
{
    [Table("ThirdPartyExteraFinancialCommitments")]
    public class ThirdPartyExteraFinancialCommitment
    {
        public ThirdPartyExteraFinancialCommitment()
        {
            ThirdPartyExteraFinancialCommitmentComs = new List<ThirdPartyExteraFinancialCommitmentCom>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int CarSpecificationId { get; set; }
        [ForeignKey("CarSpecificationId")]
        [InverseProperty("ThirdPartyExteraFinancialCommitments")]
        public CarSpecification CarSpecification { get; set; }
        public int Year { get; set; }
        public int ThirdPartyRequiredFinancialCommitmentId { get; set; }
        [ForeignKey("ThirdPartyRequiredFinancialCommitmentId")]
        [InverseProperty("ThirdPartyExteraFinancialCommitments")]
        public ThirdPartyRequiredFinancialCommitment ThirdPartyRequiredFinancialCommitment { get; set; }
        public decimal Rate { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("ThirdPartyExteraFinancialCommitment")]
        public List<ThirdPartyExteraFinancialCommitmentCom> ThirdPartyExteraFinancialCommitmentComs { get; set; }
    }
}
