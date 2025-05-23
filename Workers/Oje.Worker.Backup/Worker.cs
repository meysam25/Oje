using Oje.BackupService.Interfaces;
using Oje.Infrastructure.Enums;

namespace Oje.Worker.Backup
{
    public class Worker : BackgroundService
    {
        public long timePass { get; set; }

        readonly IGoogleBackupService GoogleBackupService = null;
        readonly IGoogleBackupArchiveLogService GoogleBackupArchiveLogService = null;

        public Worker(
                IGoogleBackupService GoogleBackupService,
                IGoogleBackupArchiveLogService GoogleBackupArchiveLogService
            )
        {
            this.GoogleBackupService = GoogleBackupService;
            this.GoogleBackupArchiveLogService = GoogleBackupArchiveLogService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await GoogleBackupService.CheckTimeAndCreateBackup(true);

            }
            catch (Exception ex)
            {
                GoogleBackupArchiveLogService.Create(ex.Message, GoogleBackupArchiveLogType.UnknownSection);
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                timePass += 10000;
                try
                {
                    if (timePass % 3510000 == 0)
                        await GoogleBackupService.CheckTimeAndCreateBackup();
                }
                catch (Exception ex)
                {
                    GoogleBackupArchiveLogService.Create(ex.Message, GoogleBackupArchiveLogType.UnknownSection);
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}