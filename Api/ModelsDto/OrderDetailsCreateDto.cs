using System.ComponentModel.DataAnnotations;

namespace Api.ModelsDto
{
    public class OrderDetailsCreateDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}