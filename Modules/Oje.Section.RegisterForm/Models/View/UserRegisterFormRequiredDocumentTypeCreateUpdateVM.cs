using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormRequiredDocumentTypeCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? userRegisterFormId { get; set; }
        public bool? isActive { get; set; }
    }
}
