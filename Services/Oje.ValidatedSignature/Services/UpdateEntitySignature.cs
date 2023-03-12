using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Interfac;
using Oje.ValidatedSignature.Services.EContext;

namespace Oje.ValidatedSignature.Services
{
    public class UpdateEntitySignature<T> where T : class, ISignatureEntity, new()
    {
        readonly DbSet<T> dbSet = null;
        readonly ValidatedSignatureDBContext db = null;
        public UpdateEntitySignature
            (
                ValidatedSignatureDBContext db
            )
        {
            dbSet = db.Set<T>();
            this.db = db;
        }

        public void Update()
        {
            var allItems = dbSet.ToList();
            foreach (var item in allItems)
            {
                item.FilledSignature();
            }
            db.SaveChanges();
        }
    }
}
