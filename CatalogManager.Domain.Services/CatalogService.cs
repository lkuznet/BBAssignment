using CatalogManager.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CatalogManager.Domain.Services
{
    public class CatalogService : ICatalogService
    {
        private ICategoryRepository _categoryRepos;
        private ICatalogItem _catalog;
        public CatalogService(ICategoryRepository categoryRepos)
        {
            //ToiDo:
            //if (!Thread.CurrentPrincipal.Identity.IsAuthenticated || !Thread.CurrentPrincipal.IsInRole("CatalogManager"))
            //    throw new AccessViolationException("User must be in CatalogManager role to use this functionality.");

            _categoryRepos = categoryRepos;
        }
        private CatalogService()
        {
        }
        public string DisplayCatalog(int indent)
        {
            _catalog = _categoryRepos.GetCatalog();
            string result = _catalog.Display(indent);
            return result;
        }
        public List<CatalogItemDto> DisplayCategoryTree(int indent)
        {
            _catalog = _categoryRepos.GetCatalog();

            var result = _catalog.DisplayTree(indent);
            return result;
        }
        public bool AddChild(Category category, ICatalogItem catalogItem)
        {
            if (!category.AddChild(catalogItem))
            {
                return false;
            }
            _categoryRepos.Save(catalogItem);
            return true;
        }
        public bool RemoveChild(Category category, ICatalogItem catalogItem, out string message)
        {
            message = string.Empty;
            if (catalogItem is Category)
            {
                bool hasProducts = _categoryRepos.HasProducts(catalogItem);
                if (hasProducts)
                {
                    message = "Cannot remove, this category branch has products.";
                    return false;
                }
            }

            if (!category.RemoveChild(catalogItem))
            {
                return false;
            }
            _categoryRepos.Delete(catalogItem);
            return true;
        }
        public bool UpdateChild(Category parentCategory, ICatalogItem item)
        {
            if (!parentCategory.CanUpdateChild(item))
            {
                return false;
            }

            _categoryRepos.Update(item);
            return true;
        }

    }
}
