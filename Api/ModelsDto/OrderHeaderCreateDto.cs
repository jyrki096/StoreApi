using System.ComponentModel.DataAnnotations;
using Api.Models;

namespace Api.ModelsDto
{
    public class OrderHeaderCreateDto
    {
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        public string UserId { get; set;}
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; } 
        public int TotalCount { get; set; }
        public IEnumerable<OrderDetailsCreateDto> OrderDetails { get; set; }
    }
}