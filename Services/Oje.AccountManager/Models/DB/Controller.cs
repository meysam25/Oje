﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("Controllers")]
    public class Controller
    {
        public Controller()
        {
            Actions = new ();
            ControllerCategoryControllers = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Icon { get; set; }
        public int SectionId { get; set; }
        [ForeignKey("SectionId")]
        [InverseProperty("Controllers")]
        public Section Section { get; set; }
        public bool HasFormGenerator { get; set; }
        public int? Order { get; set; }

        [InverseProperty("Controller")]
        public List<Action> Actions { get; set; }
        [InverseProperty("Controller")]
        public List<ControllerCategoryController> ControllerCategoryControllers { get; set; }
    }
}
