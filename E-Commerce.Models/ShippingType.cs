using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class ShippingType
    {
        [Key]
        public int Id { get; set; }

        [Required]  
        [Display(Name = "Shipping")]
        public string Name { get; set; }
    }
}
