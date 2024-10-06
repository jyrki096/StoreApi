using Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.ModelsDto
{
    public class OrderHeaderUpdateDto
    {
        [Required]
        public int OrderHeaderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public OrderStatus Status { get; set; }
    }
}