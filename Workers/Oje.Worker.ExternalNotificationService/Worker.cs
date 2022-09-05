using Microsoft.Data.SqlClient;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;

namespace Oje.Worker.ExternalNotificationService
{
    public class Worker : BackgroundService
    {
        public bool isRegister { get; set; }
        public bool checkNotification { get; set; }
        public long timePass { get; set; }

        readonly IPushNotificationService PushNotificationService = null;

        public Worker(IPushNotificationService PushNotificationService)
        {
            this.PushNotificationService = PushNotificationService;
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
                if (checkNotification == true)
                {
                    checkNotification = false;
                    await PushNotificationService.SendWebNotifications();
                }
                timePass += 1000;
                if (timePass % 50000 == 0)
                    await PushNotificationService.SendWebNotifications();

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

                string commandText = "SELECT [UserId] FROM dbo.UserNotifications where LastTryDate is null";

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
                checkNotification = true;
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