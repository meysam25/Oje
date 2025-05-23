﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("GlobalInqueryInputParameters")]
    public class GlobalInqueryInputParameter
    {
        public GlobalInqueryInputParameter()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public long GlobalInputInqueryId { get; set; }
        [ForeignKey("GlobalInputInqueryId"), InverseProperty("GlobalInqueryInputParameters")]
        public GlobalInputInquery GlobalInputInquery { get; set; }
        [Required, MaxLength(200)]
        public string Key { get; set; }
        [Required, MaxLength(200)]
        public string Value { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string ShowInDetailes { get; set; }
    }
}
