namespace Checkout.Models
{
    public class Discount
    {
        public string Product { get; set; }
        public int PurchaseQuantity { get; set; }
        public int DiscountPrice { get; set; }
    }
}
