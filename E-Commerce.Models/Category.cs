using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
         
        [Display(Name = "Display Order")]
        public string DisplayOrder { get; set; }

        public DateTime ListingDateTime { get; set; } = DateTime.Now;   
    }
}
