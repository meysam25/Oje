using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("DashboardSectionCategories")]
    public class DashboardSectionCategory
    {
        public DashboardSectionCategory()
        {
            DashboardSections = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MinLength(200)]
        public string Css { get; set; }
        public DashboardSectionCategoryType? Type { get; set; }
        public int? Order { get; set; }

        [InverseProperty("DashboardSectionCategory")]
        public List<DashboardSection> DashboardSections { get; set; }
    }
}
