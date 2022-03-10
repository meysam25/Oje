using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormRequiredDocumentCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? typeId { get; set; }
        public bool? isRequird { get; set; }
        public bool? isActive { get; set; }
        public IFormFile downloadFile { get; set; }
    }
}
