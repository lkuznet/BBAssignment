using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogManager.Application.Catalog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CatalogManager.ApplicationTests.RepositoryDoubles;
using CatalogManager.Domain.Services;
using CatalogManager.Domain.Aggregates;

namespace CatalogManager.Application.Catalog.Tests
{
    [TestClass()]
    public class CatalogApiTests
    {
        private CatalogApi _catalogApi;
        ICategoryRepository _catRepos;

        [TestInitialize]
        public void Initialization()
        {
            _catRepos = new FakeCategoryRepository();
            var catalogService = new CatalogService(_catRepos);
            _catalogApi = new CatalogApi(_catRepos, catalogService);
        }

        [TestMethod()]
        public void GetCatalogTest()
        {
            var result = _catalogApi.GetCatalog(2);

            Assert.AreNotEqual(result, string.Empty);
            Assert.IsTrue(result.Contains("Kitchen tables"));
            Assert.IsTrue(result.Contains("$650.99"));

        }

        [TestMethod()]
        public void AddCategoryTest()
        {
            var result = _catalogApi.AddCategory(1, new CategoryDto { Name = "Books" });
            var item = _catRepos.GetCatalog().Find("Books");

            Assert.IsTrue(result);
            Assert.AreEqual(item.Name, "Books");
        }

        [TestMethod()]
        public void AddProductTest()
        {
            var result = _catalogApi.AddProduct(5, new ProductDto { Name = "Round Table", Description = "Nice round table", PriceCurrency = Domain.Currency.CAD.ToString(), PriceAmount = 420.00M });
            var item = _catRepos.GetCatalog().Find("Round Table");

            Assert.IsTrue(result);
            Assert.AreEqual(item.Name, "Round Table");
        }

    }
}
