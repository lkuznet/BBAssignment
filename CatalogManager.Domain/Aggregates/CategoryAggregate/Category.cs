using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CatalogManager.Domain.Aggregates
{
    public class Category : CatalogItem 
    {
        public Category(string name)
            : base(name)
        {
            Validate(name);
        }

        private static void Validate(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length > 25)
                throw new ArgumentException("Name");
        }

        public void Update(string name)
        {
            Validate(name);
            Name = name;
        }

        public override bool AddChild(ICatalogItem item)
        {
            if (Children.Any(c => c.Name == item.Name))
            {
                return false;
            }

            item.Parent = this;
            item.Path = this.Path + "/" + Name;
            Children.Add(item);

            return true;
        }
        public override bool RemoveChild(ICatalogItem item)
        {
            var itemToDelete = Children.SingleOrDefault(x => x.Name == item.Name);
            if (Children.Remove(itemToDelete))
            {
                return true;
            }
            return false;
        }
        public override bool CanUpdateChild(ICatalogItem item)
        {
            ICatalogItem existingItem = Children.SingleOrDefault(c => c.Id == item.Id);
            if (existingItem == null)
            {
                return false;
            }
            if (Children.Any(c => c.Name == item.Name && c.Id != item.Id))
            {
                return false;
            }
            return true;
        }
        public override string Display(int indent)
        {
            string result = new String(' ', indent) + string.Format("-{0} ({1})", Name, ProductCount) + Environment.NewLine;

            foreach (var child in Children)
            {
                result += child.Display(indent + 2);
            }
            return result;
        }
        public override List<CatalogItemDto> DisplayTree(int indent)
        {
            int parentId = this.Parent != null ? this.Parent.Id : 0;
            List<CatalogItemDto> result = new List<CatalogItemDto> {new CatalogItemDto{ Id = this.Id, Name = new String('_', indent) + '-' + this.Name + " (" + this.ProductCount + ")",
                ParentId = parentId, Path = this.Path, Indent = indent} };
            Debug.WriteLine(new String('-', indent) + string.Format("-{0} ({1})", Name, ProductCount));

            foreach (var child in Children)
            {
                result.AddRange(child.DisplayTree(indent + 2));
            }
            return result;
        }
        public override ICatalogItem Find(int id)
        {
            if (Id == id) return this;
            foreach (var child in Children)
            {
                if (child.Id == Id)
                {
                    return child;
                }

                var result = child.Find(id);
                if (result != null)
                    return result;
            }
            return null;
        }
        public override ICatalogItem Find(string name)
        {
            foreach (var child in Children)
            {
                if (child.Name == name)
                {
                    return child;
                }

                var result = child.Find(name);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
