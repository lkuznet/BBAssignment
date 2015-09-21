using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogManager.Domain.Aggregates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
namespace CatalogManager.Domain.Aggregates.Tests
{
    [TestClass()]
    public class CatalogItemTests
    {
        [TestMethod()]
        public void CatalogItemTest()
        {
            Category catalog = new Category("Catalog");
            Category furniture = new Category("Furniture");
            Category electronics = new Category("Electronics");
            Category sofas = new Category("Sofas");
            furniture.AddChild(sofas);
            Category tables = new Category("Tables");
            furniture.AddChild(tables);

            Category tvs = new Category("Tvs");
            electronics.AddChild(tvs);
            Category computers = new Category("Computers");
            electronics.AddChild(computers);

            computers.AddChild(new Product("LapTop", "Descr", new Money(650.99M, Currency.CAD)));
            computers.AddChild(new Product("Tablet", "Descr", new Money(730.50M, Currency.CAD)));

            tables.AddChild(new Category("Kitchen tables"));
            catalog.AddChild(furniture);
            catalog.AddChild(electronics);

            string result = catalog.Display(2);

            Debug.Write(result);
            Assert.AreNotEqual(result, string.Empty);
            Assert.IsTrue(result.Contains("Kitchen tables"));
            Assert.IsTrue(result.Contains("$650.99"));
        }

        [TestMethod()]
        public void FindTest()
        {
            Category catalog = new Category("Catalog");
            Category furniture = new Category("Furniture");
            Category electronics = new Category("Electronics");
            Category sofas = new Category("Sofas");
            furniture.AddChild(sofas);
            Category tables = new Category("Tables");
            furniture.AddChild(tables);

            Category tvs = new Category("Tvs");
            electronics.AddChild(tvs);
            Category computers = new Category("Computers");
            electronics.AddChild(computers);

            computers.AddChild(new Product("LapTop", "Descr", new Money(650.99M, Currency.CAD)));
            computers.AddChild(new Product("Tablet", "Descr", new Money(730.50M, Currency.CAD)));

            tables.AddChild(new Category("Kitchen tables"));
            catalog.AddChild(furniture);
            catalog.AddChild(electronics);
            tvs.AddChild(new Product("Sony 40-50", "Descr", new Money(1980.99M, Currency.CAD)));


            var result = catalog.Find("Computers");
            result.AddChild(new Product("Desctop", "Descr", new Money(1380.99M, Currency.CAD)));

            Assert.AreEqual(result.Name, "Computers");
        }

    }
}
