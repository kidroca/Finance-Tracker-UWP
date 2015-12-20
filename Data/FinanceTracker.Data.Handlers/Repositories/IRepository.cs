namespace FinanceTracker.Data.Handlers.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> All();

        T GetById(object id);

        IRepository<T> Add(T entity);

        IRepository<T> Update(T entity);

        IRepository<T> Delete(T entity);

        IRepository<T> Delete(object id);

        T Attach(T entity);

        void Detach(T entity);

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}