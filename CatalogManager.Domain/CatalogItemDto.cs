using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatalogManager.Domain
{
    public class CatalogItemDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; }

        public string Name { get; set; }
        public int Indent { get; set; }
        public string Path { get; set; }

    }
}
