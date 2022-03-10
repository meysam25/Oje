
using Microsoft.Data.SqlClient;
using Oje.Infrastructure;
using Oje.Sms;
using Oje.Sms.Interfaces;

namespace Oje.Worker.Sms
{
    public class Worker : BackgroundService
    {
        public bool isRegister { get; set; }
        public bool checkSms { get; set; }
        public long timePass { get; set; }

        readonly ISmsSendingQueueService SmsSendingQueueService = null;
        public Worker(ISmsSendingQueueService SmsSendingQueueService)
        {
            this.SmsSendingQueueService = SmsSendingQueueService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (isRegister == false)
                    registerSelect();
                if (checkSms == true)
                {
                    checkSms = false;
                    await SmsSendingQueueService.SendSms();
                }
                timePass += 1000;
                if (timePass % 60000 == 0)
                    await SmsSendingQueueService.SendSms();

                await Task.Delay(1000, stoppingToken);
            }
        }

        public void registerSelect()
        {
            isRegister = true;
            using (SqlConnection conn = new SqlConnection(GetDbConnection()))
            {
                conn.Open();

                SqlDependency.Start(GetDbConnection());

                string commandText = "SELECT [Id],[MobileNumber] ,[Subject] ,[Body] FROM dbo.SmsSendingQueues where LastTryDate is null";

                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    SqlDependency dependency = new SqlDependency(cmd);

                    dependency.OnChange += new OnChangeEventHandler(OnDependencyChange);

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info == SqlNotificationInfo.Insert)
                checkSms = true;
            SqlDependency temp = sender as SqlDependency;
            if (temp != null)
                temp.OnChange -= new OnChangeEventHandler(OnDependencyChange);

            registerSelect();
        }

        private string GetDbConnection()
        {
            return GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}