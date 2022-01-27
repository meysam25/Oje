using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IProposalFormReminderService
    {
        ApiResult Create(ReminderCreateVM input, int? siteSettingId, IpSections ipSections);
    }
}
