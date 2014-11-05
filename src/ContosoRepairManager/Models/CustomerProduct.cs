using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoRepairManager.Models {
    public class CustomerProduct {
        [Key]
        public string CustomerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public List<Models.Product> Products { get; set; }

    }
}