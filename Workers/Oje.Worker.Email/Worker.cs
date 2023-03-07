using Microsoft.Data.SqlClient;
using Oje.EmailService;
using Oje.EmailService.Interfaces;
using Oje.Infrastructure;

namespace Oje.Worker.Email
{
    public class Worker : BackgroundService
    {
        public bool isRegister { get; set; }
        public bool checkEmail { get; set; }
        public long timePass { get; set; }

        readonly IEmailSendingQueueService EmailSendingQueueService = null;

        public Worker(IEmailSendingQueueService EmailSendingQueueService)
        {
            this.EmailSendingQueueService = EmailSendingQueueService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SqlDependency.Start(GetDbConnection());

            while (!stoppingToken.IsCancellationRequested)
            {
                if (isRegister == false)
                    registerSelect();
                if (checkEmail == true)
                {
                    checkEmail = false;
                    await EmailSendingQueueService.SendEmail();
                }
                timePass += 1000;
                if (timePass % 60000 == 0)
                    await EmailSendingQueueService.SendEmail();

                await Task.Delay(1000, stoppingToken);
            }

            SqlDependency.Stop(GetDbConnection());
        }

        public void registerSelect()
        {
            isRegister = true;
            using (SqlConnection conn = new SqlConnection(GetDbConnection()))
            {
                conn.Open();


                string commandText = "SELECT [Id],[Email] ,[Subject] ,[Body] FROM dbo.EmailSendingQueues where LastTryDate is null and BMessageCode is null";

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
                checkEmail = true;
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