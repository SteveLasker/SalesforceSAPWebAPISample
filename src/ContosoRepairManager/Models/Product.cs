using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoRepairManager.Models {
    public class Product {
        [Key]
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string SupplierName { get; set; }
        [Url]
        public string ProductPicUrl { get; set; }
        public string ProductFamily { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime WarrantyEndDate { get; set; }
    }
}