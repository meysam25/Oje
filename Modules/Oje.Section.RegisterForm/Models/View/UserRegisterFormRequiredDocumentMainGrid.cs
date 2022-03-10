using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormRequiredDocumentMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string typeTitle { get; set; }
        public bool? isActive { get; set; }
        public bool? isRequired { get; set; }
    }
}
