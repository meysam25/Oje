using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Services.EContext;
using System.Linq;

namespace Oje.Section.RegisterForm.Services
{
    public class AgentRefferService : IAgentRefferService
    {
        readonly RegisterFormDBContext db = null;
        public AgentRefferService
            (
                RegisterFormDBContext db
            )
        {
            this.db = db;
        }

        public string GetRefferCode(int? siteSettingId, int? companyId)
        {
            return db.AgentReffers.Where(t => t.SiteSettingId == siteSettingId && t.CompanyId == companyId).Select(t => t.Code).FirstOrDefault();
        }
    }
}
