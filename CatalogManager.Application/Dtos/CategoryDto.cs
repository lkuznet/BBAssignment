using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatalogManager.Application
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }

    }
 
}
