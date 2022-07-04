using Microsoft.EntityFrameworkCore;
using System;

namespace Oje.Infrastructure.Services
{
    public class MyBaseDbContext: DbContext
    {
        public MyBaseDbContext(DbContextOptions options) : base(options)
        {

        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (ex.Entries != null)
                    foreach (var entry in ex.Entries)
                        this.Entry(entry.Entity).State = EntityState.Detached;
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
