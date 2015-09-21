using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Domain
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();
        System.Threading.Tasks.Task<int> SaveChangesAsync();
    }
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
  
}
