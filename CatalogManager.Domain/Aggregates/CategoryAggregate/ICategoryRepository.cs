using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Domain.Aggregates
{
    public interface ICategoryRepository
    {
        ICatalogItem GetCatalog();

        void Delete(ICatalogItem catalogItem);

        void Save(ICatalogItem catalogItem);

        void Update(ICatalogItem updatedItem);

        Category GetById(int parentId);

        Category GetParent(int childId);

        Product GetProductById(int childId);

        IEnumerable<ICatalogItem> GetProducts(int parentId);

        bool HasProducts(ICatalogItem catalogItem);
    }
}
