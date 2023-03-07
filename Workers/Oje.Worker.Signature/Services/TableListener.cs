using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.ValidatedSignature.Services.EContext;
using Oje.Worker.Signature.Interfaces;
using TableDependency.SqlClient;

namespace Oje.Worker.Signature.Services
{
    public class TableListener<T> : ITableListener where T : class, ISignatureEntity, new()
    {
        SqlTableDependency<T> dep = null;
        DbSet<T> dbSet = null;
        IErrorService ErrorService = null;
        readonly ValidatedSignatureDBContext db = null;

        public TableListener
            (
                ValidatedSignatureDBContext db,
                IErrorService ErrorService
            )
        {
            dbSet = db.Set<T>();
            this.ErrorService = ErrorService;
            this.db = db;
        }
        public ITableListener Start()
        {
            dep = new SqlTableDependency<T>(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], new T().GetTableAttributeName());
            dep.OnChanged += Dep_OnChanged;
            dep.Start();

            return this;
        }

        public void Stop()
        {
            dep.OnChanged -= Dep_OnChanged;
            dep.Stop();
            dep.Dispose();
            dep = null;
        }

        Thread prevTr = null;
        private void Dep_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<T> e)
        {
            if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Update ||
                e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Insert)
            {
                var entity = e.Entity;
                if (entity != null)
                {
                    var allProps = entity.GetType().GetProperties();
                    var foundIdProps = allProps.Where(t => t.Name == "Id").FirstOrDefault();
                    if (foundIdProps != null)
                    {
                        if (prevTr != null && prevTr.IsAlive)
                        {
                            prevTr.Interrupt();
                            prevTr.Join();
                        }
                        prevTr = new Thread(() =>
                        {
                            try
                            {
                                var idValue = foundIdProps.GetValue(entity);
                                Thread.Sleep(3000);
                                var foundItem = dbSet.Find(idValue);
                                if (foundItem != null)
                                {
                                    db.Entry(foundItem).State = EntityState.Detached;
                                    if (!foundItem.IsSignature())
                                    {
                                        string message = entity.GetTableAttributeName() + " signiture is not valid" + Environment.NewLine;
                                        message += entity.GetSignatureChanges();
                                        message += JsonConvert.SerializeObject(entity);
                                        ErrorService.Create(null, "GBS", null, null, message, new Infrastructure.Models.IpSections() { Ip1 = 127, Ip2 = 0, Ip3 = 0, Ip4 = 1 }, "no line", "no file", "/", "/");
                                    }
                                }
                            }
                            catch
                            {

                            }

                        });
                        prevTr.Start();

                    }
                    else
                    {
                        if (!entity.IsSignature())
                        {
                            string message = entity.GetTableAttributeName() + " signiture is not valid" + Environment.NewLine;
                            message += entity.GetSignatureChanges();
                            message += JsonConvert.SerializeObject(entity);
                            ErrorService.Create(null, "GBS", null, null, message, new Infrastructure.Models.IpSections() { Ip1 = 127, Ip2 = 0, Ip3 = 0, Ip4 = 1 }, "no line", "no file", "/", "/");
                        }
                    }
                }
            }
        }
    }
}
