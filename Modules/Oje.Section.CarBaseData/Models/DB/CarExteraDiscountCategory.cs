﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.DB
{
    [Table("CarExteraDiscountCategories")]
    public class CarExteraDiscountCategory
    {
        public CarExteraDiscountCategory()
        {
            CarExteraDiscounts = new List<CarExteraDiscount>();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int Order { get; set; }

        [InverseProperty("CarExteraDiscountCategory")]
        public List<CarExteraDiscount> CarExteraDiscounts { get; set; }
    }
}
