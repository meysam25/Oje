using Microsoft.Data.SqlClient;
using Oje.Infrastructure;
using Oje.Security.Interfaces;

namespace Oje.Worker.DebugEmail
{
    public class Worker : BackgroundService
    {
        public bool isRegister { get; set; }
        public bool checkEmail { get; set; }
        public long timePass { get; set; }

        readonly IDebugEmailService DebugEmailService = null;

        public Worker(
                IDebugEmailService DebugEmailService
            )
        {
            this.DebugEmailService = DebugEmailService;
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
                if (checkEmail == true)
                {
                    checkEmail = false;
                    await DebugEmailService.SendEmail();
                }
                timePass += 1000;
                if (timePass % 60000 == 0)
                    await DebugEmailService.SendEmail();

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

                string commandText = "SELECT [Id] FROM dbo.EmailSendingQueues where LastTryDate is null";

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