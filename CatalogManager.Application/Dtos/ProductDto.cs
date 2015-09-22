using CatalogManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Application
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PriceAmount { get; set; }
        public string PriceCurrency { get; set; }
        public int ParentId { get; set; }


    }
}
