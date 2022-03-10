using Microsoft.AspNetCore.Http;
using Oje.Infrastructure.Models.PageForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IUserFilledRegisterFormValueService
    {
        void CreateByJsonConfig(PageForm jsonOpject, long formId, IFormCollection requestForm);
    }
}
