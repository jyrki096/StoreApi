using System.ComponentModel.DataAnnotations.Schema;


namespace Api.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("CartId")]
        public int ShoppingCartId { get; set; }
        public ShoppingCart Cart { get; set; }
        public Product Product { get; set; }
    }
}