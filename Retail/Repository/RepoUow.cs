using Retail.Repository.Entity;

namespace Retail.Repository
{

    public class RepoUow : IDisposable
    {
        private readonly RetailDbContext _dataContext;

        public RepoUow(RetailDbContext context)
        {
            _dataContext = context;
        }

        public GenericRepository<ProductEntity> Product => new GenericRepository<ProductEntity>(_dataContext);

        public void Save()
        {
            _dataContext.SaveChanges();
        }


        private protected bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _dataContext.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
