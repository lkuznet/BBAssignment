using CatalogManager.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Domain.Services
{
    public interface ICatalogService
    {
        string DisplayCatalog(int indent);
        List<CatalogItemDto> DisplayCategoryTree(int indent);
        bool AddChild(Category category, ICatalogItem catalogItem);
        bool RemoveChild(Category category, ICatalogItem catalogItem, out string message);
        bool UpdateChild(Category parentCategory, ICatalogItem item);
    }
}
