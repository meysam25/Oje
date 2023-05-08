using Oje.Infrastructure;
using Oje.ProposalFormWorker.Interfaces;

namespace Oje.Worker.ProposalFormReminder
{
    public class Worker : BackgroundService
    {
        public long timePass { get; set; }

        readonly IProposalFormReminderService ProposalFormReminderService = null;
        public Worker(IProposalFormReminderService ProposalFormReminderService)
        {
            this.ProposalFormReminderService = ProposalFormReminderService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                timePass += 1000;
                if (timePass % 60000 == 0)
                    ProposalFormReminderService.Notify(GlobalConfig.Configuration.GetSection("remindingDays").Get<List<int>>());

                await Task.Delay(1000, stoppingToken);
            }

        }
    }
}