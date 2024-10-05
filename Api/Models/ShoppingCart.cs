using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItem> CartItems { get; set; }

        [NotMapped]
        public decimal TotalPrice => CartItems.Sum(x => x.Product.Price * x.Quantity);
        [NotMapped]
        public int TotalAmount => CartItems.Sum(x => x.Quantity);
    }
}
