using CatalogManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataContext _db;

        public UnitOfWork(IDataContext context)
        {
            _db = context;
        }
        public int SaveChanges()
        {
            return _db.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        #region Disposed
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
