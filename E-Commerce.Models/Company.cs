using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]  
        public string Name { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        [Display(Name = "Postal code")]
        public string? PostalCode { get; set; }

        [Display(Name = "Phone number")]
        public string? PhoneNumber { get; set; }
    }
}
