using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bcc.EntityApi;
using Microsoft.EntityFrameworkCore;

namespace Bcc.EntityApi
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions options) : base(options)
        {
        }

        // Sets the CreatedAt and UpdatedAt timestamps
        public void UpdateTimestamps()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }
        }

        public override int SaveChanges()
        {
            this.UpdateTimestamps();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.UpdateTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            this.UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.UpdateTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}