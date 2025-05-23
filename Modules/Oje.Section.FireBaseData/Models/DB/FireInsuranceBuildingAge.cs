﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.DB
{
    [Table("FireInsuranceBuildingAges")]
    public class FireInsuranceBuildingAge
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public int Order { get; set; }
        public int Year { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
    }
}
