using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [ValidateNever]
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Count { get; set; }

        public string ApplicationUserId { get; set; }   

        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplictionUser { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
