using System.ComponentModel.DataAnnotations;

namespace Api.ModelsDto
{
    public class UpdatedProductDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string SpecialTag { get; set; }
        public string Category  { get; set; }
        [Required]
        [Range(1d, 99999d)]
        public decimal Price { get; set; }
        [Required]
        public string Image { get; set; }
    }
}