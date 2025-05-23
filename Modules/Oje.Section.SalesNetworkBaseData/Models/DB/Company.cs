﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.SalesNetworkBaseData.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            SalesNetworkCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("Company")]
        public List<SalesNetworkCompany> SalesNetworkCompanies { get; set; }

    }
}
