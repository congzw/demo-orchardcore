using System;

namespace NbSites.Base.Data.Products
{
    public class ProductVersion
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string BuildVersion { get; set; }
        public string FriendlyVersion { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreateAt { get; set; }
    }
}