using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Models.View
{
    public class UserRegisterFormCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString jConfig { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString termT { get; set; }
        public long? userId { get; set; }
        public string userId_Title { get; set; }
        public string description { get; set; }
        public IFormFile rules { get; set; }
        public IFormFile secoundFile { get; set; }
        public bool? isActive { get; set; }
    }
}
