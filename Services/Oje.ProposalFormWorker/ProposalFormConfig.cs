using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Infrastructure;
using Oje.ProposalFormWorker.Interfaces;
using Oje.ProposalFormWorker.Services;
using Oje.ProposalFormWorker.Services.EContext;
using Oje.Sms;

namespace Oje.ProposalFormWorker
{
    public static class ProposalFormConfig
    {
        public static void ConfigForWorker(IServiceCollection services)
        {
            SmsConfig.ConfigForWorker(services);

            services.AddDbContext<ProposalFormWorkerDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)), ServiceLifetime.Singleton);

            services.AddSingleton<IProposalFormReminderService, ProposalFormReminderService>();
        }
    }
}
