using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.RegisterForm.Services
{
    public class ProvincService: IProvincService
    {
        readonly RegisterFormDBContext db = null;
        public ProvincService(RegisterFormDBContext db)
        {
            this.db = db;
        }

        public Provinc GetById(int id)
        {
            return db.Provincs.Where(t => t.IsActive == true && t.Id == id).FirstOrDefault();
        }
    }
}
