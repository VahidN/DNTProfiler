using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DNTProfiler.TestEFContext.Domain;

namespace DNTProfiler.TestEFContext.DataLayer
{
    public class SampleContext : DbContext, IUnitOfWork
    {
        public DbSet<Category> Categories { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<User> Users { set; get; }

        public SampleContext()
        {
            //this.Database.Log = data => System.Diagnostics.Debug.WriteLine(data);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public int SaveAllChanges()
        {
            return base.SaveChanges();
        }

        public IEnumerable<TEntity> AddThisRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            return ((DbSet<TEntity>)this.Set<TEntity>()).AddRange(entities);
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public IList<T> GetRows<T>(string sql, params object[] parameters)
        {
            return Database.SqlQuery<T>(sql, parameters).ToList();
        }
    }
}