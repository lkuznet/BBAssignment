using CatalogManager.Domain;
using CatalogManager.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.ApplicationTests.RepositoryDoubles
{
    public class FakeCategoryRepository : ICategoryRepository
    {
        Category _catalog;
        public FakeCategoryRepository()
        {
            _catalog = new Category("Catalog"); _catalog.Id = 1;
            Category furniture = new Category("Furniture"); furniture.Id = 2;
            Category electronics = new Category("Electronics"); electronics.Id = 3;
            Category sofas = new Category("Sofas"); sofas.Id = 4;
            furniture.AddChild(sofas);
            Category tables = new Category("Tables"); tables.Id = 5;
            furniture.AddChild(tables);

            Category tvs = new Category("Tvs");
            electronics.AddChild(tvs);
            Category computers = new Category("Computers");
            electronics.AddChild(computers);

            computers.AddChild(new Product("LapTop", "Descr", new Money(650.99M, Currency.CAD)));
            computers.AddChild(new Product("Tablet", "Descr", new Money(730.50M, Currency.CAD)));

            tables.AddChild(new Category("Kitchen tables"));
            _catalog.AddChild(furniture);
            _catalog.AddChild(electronics);
        }

        public ICatalogItem GetCatalog()
        {
            return _catalog;
        }

        public void Delete(ICatalogItem catalogItem)
        {
        }

        public void Save(ICatalogItem catalogItem)
        {
        }

        public void Update(ICatalogItem updatedItem)
        {
        }

        public Category GetById(int parentId)
        {
            return (Category)_catalog.Find(parentId);
        }
        public Product GetProductById(int parentId)
        {
            return (Product)_catalog.Find(parentId);
        }

        public IEnumerable<ICatalogItem> GetChildren(string parentName)
        {
            var parent = _catalog.Find(parentName);
            return parent.Children;
        }
        public Category GetParent(int childId)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, ICatalogItem item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICatalogItem> GetProducts(int parentId)
        {
            throw new NotImplementedException();
        }

        public bool HasProducts(ICatalogItem catalogItem)
        {
            throw new NotImplementedException();
        }
    }
}
