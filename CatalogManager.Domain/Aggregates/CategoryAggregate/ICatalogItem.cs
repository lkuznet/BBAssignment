using System.Collections.Generic;
namespace CatalogManager.Domain.Aggregates
{
    public interface ICatalogItem
    {
        int Id { get; set; }
        string Name { get; set; }
        ICatalogItem Parent { get; set; }
        int ProductCount { get; set; }
        string Path { get; set; }
        List<ICatalogItem> Children { get; }
        bool AddChild(ICatalogItem item);
        string Display(int indent);
        List<CatalogItemDto> DisplayTree(int indent);

        ICatalogItem Find(int id);
        ICatalogItem Find(string name);
        bool RemoveChild(ICatalogItem item);
        bool CanUpdateChild(ICatalogItem updatedItem);

    }
}
