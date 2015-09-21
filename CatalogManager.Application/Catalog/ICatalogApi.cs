using CatalogManager.Domain;
using CatalogManager.Domain.Aggregates;
using System;
using System.Collections.Generic;
namespace CatalogManager.Application.Catalog
{
    public interface ICatalogApi
    {
        bool AddCategory(int parentId, CategoryDto categoryDto);
        bool AddProduct(int parentId, ProductDto productDto);
        string GetCatalog(int indent);
        System.Collections.Generic.List<CatalogItemDto> GetCatalogTree(int indent);
        CatalogManager.Application.ProductDto GetProduct(int id);
        IEnumerable<ICatalogItem> GetProducts(int parentId);
        bool RemoveCategory(int childId);
        bool RemoveProduct(int parentId, int id);
        bool UpdateCategory(int childId, CategoryDto categoryDto);
        bool UpdateProduct(int parentId, int childId, ProductDto productDto);
    }
}
