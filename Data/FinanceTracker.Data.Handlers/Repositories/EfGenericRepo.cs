namespace FinanceTracker.Data.Handlers.Repositories
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class EfGenericRepo<T> : IRepository<T> where T : class
    {

        public EfGenericRepo(DbContext context)
        {
            this.DbContext = context;
            this.DbSet = this.DbContext.Set<T>();
        }

        protected DbContext DbContext { get; }

        protected IDbSet<T> DbSet { get; }

        public void Dispose()
        {
            this.DbContext.Dispose();
        }

        public virtual IQueryable<T> All()
        {
            return this.DbSet.AsQueryable();
        }

        public virtual T GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        public virtual IRepository<T> Add(T entity)
        {
            var entry = this.DbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.DbSet.Add(entity);
            }

            return this;
        }

        public virtual IRepository<T> Update(T entity)
        {
            var entry = this.DbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;

            return this;
        }

        public IRepository<T> Delete(T entity)
        {
            var entry = this.DbContext.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }

            return this;
        }

        public IRepository<T> Delete(object id)
        {
            T entity = this.DbSet.Find(id);
            this.DbSet.Remove(entity);

            return this;
        }

        public T Attach(T entity)
        {
            return this.DbSet.Attach(entity);
        }

        public void Detach(T entity)
        {
            var entry = this.DbContext.Entry(entity);
            entry.State = EntityState.Detached;
        }

        public int SaveChanges()
        {
            return this.DbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.DbContext.SaveChangesAsync();
        }
    }
}