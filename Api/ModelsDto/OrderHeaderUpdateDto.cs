using System.ComponentModel.DataAnnotations;
using Api.Models;

namespace Api.ModelsDto
{
    public class OrderHeaderUpdateDto
    {
        [Required]
        public int OrderHeaderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Status { get; set; }
    }
}