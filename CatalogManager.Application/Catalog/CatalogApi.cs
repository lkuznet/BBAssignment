using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogManager.Domain.Aggregates;
using CatalogManager.Domain;
using CatalogManager.Domain.Services;
using CatalogManager.Persistence.Repositories;
using System.Data.SqlClient;

namespace CatalogManager.Application.Catalog
{
    public class CatalogApi : ICatalogApi
    {
        private ICategoryRepository _categoryRepos;
        private ICatalogService _catalogService;

        //public CatalogApi(SqlConnection sqlConnection)
        //{
        //    _sqlConnection = sqlConnection;
        //    _categoryRepos = new CategoryRepository(_sqlConnection);
        //    _catalogService = new CatalogService(_categoryRepos);
        //}
        public CatalogApi(ICategoryRepository categoryRepos, ICatalogService catalogService)
        {
            _categoryRepos = categoryRepos;
            _catalogService = catalogService;
        }
        // public CatalogApi(ICategoryRepository categoryRepos, ICatalogService catalogService, SqlConnection sqlConnection)
        //{
        //    _sqlConnection = sqlConnection;
        //    _categoryRepos = categoryRepos;
        //    _catalogService = catalogService;
        //}
        public string GetCatalog(int indent)
        {
            return _catalogService.DisplayCatalog(indent);
        }
        public List<CatalogItemDto> GetCatalogTree(int indent)
        {
            return _catalogService.DisplayCategoryTree(indent);
        }
         public IEnumerable<ICatalogItem> GetProducts(int parentId)
        {
            return _categoryRepos.GetProducts(parentId);
        }
   
        public bool AddCategory(int parentId, CategoryDto categoryDto)
        {
            Category parent = _categoryRepos.GetById(parentId);
            Category child = new Category(categoryDto.Name);
            return _catalogService.AddChild(parent, child);
        }
        public bool AddProduct(int parentId, ProductDto productDto)
        {
            Category parent = _categoryRepos.GetById(parentId);
            Product child = new Product(productDto.Name, productDto.Description, new Money(productDto.PriceAmount, 
                (Currency)Enum.Parse(typeof(Currency), productDto.PriceCurrency)));
            return _catalogService.AddChild(parent, child);
        }
        public bool RemoveCategory(int childId)
        {
            string message = string.Empty; //ToDo implement this later
            Category parent = _categoryRepos.GetParent(childId);
            Category child = _categoryRepos.GetById(childId);
            return _catalogService.RemoveChild(parent, child, out message);
        }
        public bool RemoveProduct(int parentId, int id)
        {
            string message = string.Empty; // //ToDo implement this later
            Category parent = _categoryRepos.GetById(parentId);
            Product child = _categoryRepos.GetProductById(id);
            return _catalogService.RemoveChild(parent, child, out message);
        }
        public bool UpdateCategory(int childId, CategoryDto categoryDto)
        {
            Category parent = _categoryRepos.GetParent(childId);
            Category existingItem = _categoryRepos.GetById(childId);
            existingItem.Update(categoryDto.Name);
 
           return _catalogService.UpdateChild(parent, existingItem);
        }
       public bool UpdateProduct(int parentId, int childId, ProductDto productDto)
        {
            Category parent = _categoryRepos.GetById(parentId);
            Product existingItem = _categoryRepos.GetProductById(childId);
           existingItem.Update(productDto.Name, productDto.Description, new Money(productDto.PriceAmount,
                (Currency)Enum.Parse(typeof(Currency), productDto.PriceCurrency)));
  
            return _catalogService.UpdateChild(parent, existingItem);
        }
       public ProductDto GetProduct(int id)
       {
          Product product =  _categoryRepos.GetProductById(id);
           return new ProductDto{Name = product.Name, Description = product.Description, PriceAmount = product.Price.Amount, PriceCurrency= product.Price.Currency.ToString(), Id = product.Id};
       }

    }
}
