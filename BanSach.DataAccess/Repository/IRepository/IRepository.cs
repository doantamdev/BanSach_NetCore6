using System.Linq.Expressions;

namespace BanSach.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T -Category

        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        IEnumerable<T> GetAllByNameAsending(Expression<Func<T, string>> orderBy, bool ascendingOrder = true, Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        IEnumerable<T> GetAllByNameDesending(Expression<Func<T, string>> orderBy, bool desendingOrder = true, Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
