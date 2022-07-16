
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Producer { get; set; }

        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        [Display(Name = "List price")]
        public double ListPrice { get; set; }

        [Required]
        [Display(Name = "Price for 100+ items")]
        public double ListPrice100 { get; set; }

        [ValidateNever]
        public string Image { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        
        [Required]
        [Display(Name="Delivery")]
        public int ShippingTypeId { get; set; }

        [ForeignKey("ShippingTypeId")]
        [ValidateNever]
        public ShippingType ShippingType { get; set; }









    }
}
