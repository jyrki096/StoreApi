using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string SpecialTag { get; set; }
        public string Category  { get; set; }
        [Range(1d, 99999d)]
        public decimal Price { get; set; }
        public string Image { get; set; }
        public CartItem CartItem { get; set; }
    }
}