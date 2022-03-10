using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormRequiredDocumentTypeMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string formTitle { get; set; }
        public bool? isActive { get; set; }
    }
}
