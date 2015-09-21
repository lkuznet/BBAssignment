using System.Collections.Generic;

namespace CatalogManager.Domain.Aggregates
{
    public abstract class CatalogItem : ICatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICatalogItem Parent { get; set; }
        public int ProductCount { get; set; }
        public string Path { get; set; }

        public List<ICatalogItem> Children { get; private set; }

        public CatalogItem(string name)
        {
            //          Id = Guid.NewGuid();
            this.Name = name;
            Children = new List<ICatalogItem>();
        }
        public abstract bool AddChild(ICatalogItem item);
        public abstract string Display(int indent);
        public abstract List<CatalogItemDto> DisplayTree(int indent);
        public abstract ICatalogItem Find(int id);
        public abstract ICatalogItem Find(string name);
        public abstract bool RemoveChild(ICatalogItem item);
        public abstract bool CanUpdateChild(ICatalogItem updatedItem);

    }
}
