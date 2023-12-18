using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Retail.Repository.Entity;
using System.Linq.Expressions;

namespace Retail.Repository
{
    public class GenericRepository<TEntity> where TEntity : EntityBase 
    {
        protected DbContext _dataContext;
        protected DbSet<TEntity> _dataTable;

        public GenericRepository(DbContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException("dataContext");
            _dataTable = _dataContext.Set<TEntity>();
        }

        public virtual void Create(params TEntity[] items)
        {
            foreach (TEntity item in items)
            {
                _dataContext.Add(item);
            }
        }

        public virtual void Update(params TEntity[] items)
        {
            foreach (TEntity entity in items)
            {
                _dataContext.Update(entity);
                //_dataContext.Set<TEntity>().Attach(entity);  
                //_dataContext.Entry<TEntity>(entity).State = EntityState.Modified;
            }
        }

        public virtual void Delete(params int[] ids)
        {
            foreach (int id in ids)
            {
                var item = GetById(id);

                _dataTable.Remove(item);
            }
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dataTable.ToList();
        }

        public virtual TEntity GetById(int id)
        {
            return _dataTable.SingleOrDefault(e => e.Id == id);
        }

        public virtual IEnumerable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            return _dataTable.Where(predicate).ToList();
        }

        public virtual IEnumerable<TEntity> GetAllInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties)
        {
            return GetAllIncluding(includeProperties).ToList();
        }

        public virtual IEnumerable<TEntity> SearchForInclude(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            return query.Where(predicate).ToList();
        }

        private IQueryable<TEntity> GetAllIncluding(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties)
        {
            IQueryable<TEntity> queryable = _dataTable;
            queryable = includeProperties(queryable);
            return queryable;
        }

    }
}
