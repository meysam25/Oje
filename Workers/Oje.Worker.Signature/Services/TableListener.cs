using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.ValidatedSignature.Services.EContext;
using Oje.Worker.Signature.Interfaces;
using Oje.Worker.Signature.Models;
using TableDependency.SqlClient;

namespace Oje.Worker.Signature.Services
{
    public class TableListener<T> : ITableListener where T : class, ISignatureEntity, new()
    {
        SqlTableDependency<T> dep = null;
        DbSet<T> dbSet = null;
        IErrorService ErrorService = null;
        readonly ValidatedSignatureDBContext db = null;
        List<TableListenerItemId> targetIds = new List<TableListenerItemId>();

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
                        var idValue = foundIdProps.GetValue(entity);
                        if (idValue != null)
                        {
                            if (targetIds == null)
                                lock (this)
                                    if (targetIds == null)
                                        targetIds = new List<TableListenerItemId>();

                            if (!targetIds.Any(t => t == idValue))
                                lock (targetIds)
                                    if (!targetIds.Any(t => t == idValue))
                                        targetIds.Add(new TableListenerItemId() { Id = idValue });
                        }

                    }
                    else
                    {
                        if (!entity.IsSignature())
                        {
                            string message = entity.GetTableAttributeName() + " signiture is not valid" + Environment.NewLine;
                            message += entity.GetSignatureChanges();
                            ErrorService.Create(null, "GBS", null, null, message, new Infrastructure.Models.IpSections() { Ip1 = 127, Ip2 = 0, Ip3 = 0, Ip4 = 1 }, "no line", "no file", "/", "/");
                        }
                    }
                }
            }
        }

        public void Run()
        {
            if (targetIds != null && targetIds.Count > 0)
            {
                var newTargetIds = targetIds.Where(t => t.CreateDate.AddSeconds(5) <= DateTime.Now).ToList();
                foreach (var idValue in newTargetIds)
                {
                    var foundItem = dbSet.Find(idValue.Id);
                    if (foundItem != null)
                    {
                        db.Entry(foundItem).State = EntityState.Detached;
                        if (!foundItem.IsSignature())
                        {
                            string message = foundItem.GetTableAttributeName() + " signiture is not valid" + Environment.NewLine;
                            message += foundItem.GetSignatureChanges();
                            ErrorService.Create(null, "GBS", null, null, message, new Infrastructure.Models.IpSections() { Ip1 = 127, Ip2 = 0, Ip3 = 0, Ip4 = 1 }, "no line", "no file", "/", "/");
                        }
                    }
                }
                lock (targetIds)
                {
                    targetIds = targetIds.Where(t => !newTargetIds.Contains(t)).ToList();
                }
            }
        }
    }
}
