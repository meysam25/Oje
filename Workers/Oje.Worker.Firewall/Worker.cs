
using Microsoft.Data.SqlClient;
using Oje.Infrastructure;
using Oje.Security;
using Oje.Security.Interfaces;

namespace Oje.Worker.Firewall
{
    public class Worker : BackgroundService
    {
        public bool isRegister { get; set; }
        public bool checkIp { get; set; }
        public long timePass { get; set; }
        readonly IBlockFirewallIpService BlockFirewallIpService = null;

        public Worker(IBlockFirewallIpService BlockFirewallIpService)
        {
            this.BlockFirewallIpService = BlockFirewallIpService;
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
                if (checkIp == true)
                {
                    checkIp = false;
                    await BlockFirewallIpService.RegisterInFirewall();
                }
                timePass += 1000;
                if (timePass % 60000 == 0)
                    await BlockFirewallIpService.RegisterInFirewall();

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


                string commandText = "SELECT [Ip1],[Ip2] ,[Ip3] ,[Ip4] FROM dbo.BlockFirewallIps where IsRead is null";

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
                checkIp = true;
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