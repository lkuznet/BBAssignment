using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CatalogManager.Domain.Aggregates
{
    public class Product : CatalogItem
    {
        public string Description { get; set; }
        public Money Price { get; set; }
        public Product(string name, string description, Money price)
            : base(name)
        {
            Validate(name, description);
            Description = description;
            Price = price;
        }
        public Product(string name, string description, Money price, int id)
            : this(name, description, price)
        {
            Id = id;
        }
        public Product(string name, int id) : base(name)
        {
            Id = id;
        }
        private static void Validate(string name, string description)
        {
            if (string.IsNullOrEmpty(name) || name.Length > 50)
                throw new ArgumentException("Name");
            if (string.IsNullOrEmpty(description) || description.Length > 250)
                throw new ArgumentException("Description");
        }
         public void Update(string name, string description, Money price)
        {
            Validate(name, description);
            Name = name;
            Description = description;
            Price = price;
        }
 
        public override bool AddChild(ICatalogItem item)
        {
            throw new ApplicationException("Product cannot have children.");
        }
        public override bool RemoveChild(ICatalogItem item)
        {
            throw new ApplicationException("Product does not have children.");
        }
        public override bool CanUpdateChild(ICatalogItem updatedItem)
        {
            throw new ApplicationException("Product cannot have children.");
        }
        public override string Display(int indent)
        {
            return new String(' ', indent) + "-" + Name + " " + Price + Environment.NewLine;
        }
        public override List<CatalogItemDto> DisplayTree(int indent)
        {
            List<CatalogItemDto> result = new List<CatalogItemDto> {new CatalogItemDto{ Id = this.Id, Name = new String('_', indent) + '-' + this.Name + " (" + this.ProductCount + ")",
                ParentId = this.Parent.Id, Path = this.Path, Indent = indent} };
            Debug.WriteLine(new String('-', indent) + string.Format("-{0} ({1})", Name, ProductCount));
            return result;
        }
        public override ICatalogItem Find(int id)
        {
            return null;
        }
        public override ICatalogItem Find(string name)
        {
            return null;
        }
   
    }
}
