using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces.Reports
{
    public interface IProposalFilledFormReportService
    {
        object GetPaymentChartReport(int? siteSettingId, long? userId);
        object GetStatusChartReport(int? siteSettingId, long? userId);
        object GetProposalFormChartReport(int? siteSettingId, long? userId);
        object GetProposalFormAgentsChartReport(int? siteSettingId, long? userId);
        object GetProposalFormCompanyChartReport(int? siteSettingId, long? userId);
        object GetProposalFormTimeChartReport(int? siteSettingId, long? userId, DateTime beginTime);
    }
}
