using Oje.Section.RegisterForm.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IProvincService
    {
        Provinc GetById(int id);
    }
}
