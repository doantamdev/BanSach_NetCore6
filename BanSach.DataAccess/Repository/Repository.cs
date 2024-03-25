using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BanSach.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> DbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.DbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            DbSet.Add(entity);
        }
        // include category,covertype
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (filter!=null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(','))
                {
                    query = query.Include(item);
                }

            }
            return query.ToList();
        }

        public IEnumerable<T> GetAllByNameAsending(Expression<Func<T, string>> orderBy, bool ascendingOrder = true, Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(','))
                {
                    query = query.Include(item);
                }
            }

            if (ascendingOrder)
            {
                query = query.OrderBy(orderBy);
            }
            else
            {
                query = query.OrderByDescending(orderBy);
            }

            return query.ToList();
        }

        public IEnumerable<T> GetAllByNameDesending(Expression<Func<T, string>> orderBy, bool desendingOrder = true, Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(','))
                {
                    query = query.Include(item);
                }
            }

            if (desendingOrder)
            {
                query = query.OrderByDescending(orderBy);
            }

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = DbSet;
            }
            else
            {
                query=DbSet.AsNoTracking();
            }


            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(','))
                {
                    query = query.Include(item);
                }

            }
            query = query.Where(filter);
            return query.FirstOrDefault();
        }
        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }
    }
}
