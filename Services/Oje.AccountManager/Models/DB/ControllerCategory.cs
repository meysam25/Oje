using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.DB
{
    [Table("ControllerCategories")]
    public class ControllerCategory
    {
        public ControllerCategory()
        {
            ControllerCategoryControllers = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        [Required, MinLength(100)]
        public string Icon { get; set; }
        public int Order { get; set; }

        [InverseProperty("ControllerCategory")]
        public List<ControllerCategoryController> ControllerCategoryControllers { get; set; }
    }
}
