using System.Collections.Generic;

namespace NbSites.Base.Data.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ProductVersion> ProductVersions { get; set; }
    }
}
