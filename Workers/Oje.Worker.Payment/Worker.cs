using Oje.PaymentService.Interfaces;

namespace Oje.Worker.Payment
{
    public class Worker : BackgroundService
    {
        public long timePass { get; set; }

        readonly ITiTecService TiTecService = null;

        public Worker
            (
                ITiTecService TiTecService
            )
        {
            this.TiTecService = TiTecService;
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
                    await TiTecService.InquiryFactorForWorkerAsync();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}