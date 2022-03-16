using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.DB
{
    [Table("ControllerCategoryControllers")]
    public class ControllerCategoryController
    {
        public int ControllerId { get; set; }
        [ForeignKey("ControllerId"), InverseProperty("ControllerCategoryControllers")]
        public Controller Controller { get; set; }
        public int ControllerCategoryId { get; set; }
        [ForeignKey("ControllerCategoryId"), InverseProperty("ControllerCategoryControllers")]
        public ControllerCategory ControllerCategory { get; set; }
    }
}
