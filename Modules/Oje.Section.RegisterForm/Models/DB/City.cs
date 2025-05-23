﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("Cities")]
    public class City
    {
        public City()
        {
            Users = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int ProvinceId { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("City")]
        public List<User> Users { get; set; }
    }
}
