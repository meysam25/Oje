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
    public class CityService: ICityService
    {
        readonly RegisterFormDBContext db = null;
        public CityService(RegisterFormDBContext db)
        {
            this.db = db;
        }

        public City GetById(int id)
        {
            return db.Cities.Where(t => t.Id == id && t.IsActive == true).FirstOrDefault();
        }
    }
}
