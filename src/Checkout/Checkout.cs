using Checkout.Models;

namespace Checkout
{
    public class Checkout : ICheckout
    {
        private readonly List<string> _scannedItems = new List<string>();
        private readonly List<SKU> _productList;
        private readonly List<Discount> _discounts = Enumerable.Empty<Discount>().ToList();
        
        public Checkout(IEnumerable<SKU> productList, IEnumerable<Discount> discounts)
        {
            _productList = productList.ToList();
            _discounts = discounts.ToList();
        }

        public Checkout(IEnumerable<SKU> productList)
        {
            _productList = productList.ToList();
        }

    public int GetTotalPrice()
        {
            if (_scannedItems.Count == 0)
                return 0;

            return CalculatePrice();
        }

        public void Scan(string item)
        {
            if (!_productList.Any(x => x.ProductName == item))
            {
                throw new ArgumentException($"Product {item} is not a valid product.", nameof(item));
            };

            _scannedItems.Add(item);
        }

        // TODO: Refactor - Move to a calculator class and inject an instance of ICalculator into the Checkout constructor
        private int CalculatePrice()
        {
            var price = 0;

            var productQuantities = _productList.Select(x => (SKU: x, Quantity: _scannedItems.Count(y => x.ProductName == y))).Where(x => x.Quantity > 0);

            foreach (var item in productQuantities)
            {
                var productPrice = 0;

                var productQuantity = item.Quantity;

                var discount = _discounts.FirstOrDefault(x => x.Product == item.SKU.ProductName);
                if (discount is not null)
                {
                    var baseRateQuantity = item.Quantity % discount.DiscountUnit;
                    var discountQuantity = item.Quantity - baseRateQuantity;
                    var discountedPrice = (discountQuantity / discount.DiscountUnit) * discount.DiscountPrice;

                    productQuantity -= discountQuantity;
                    productPrice += discountedPrice;
                }

                productPrice += (item.SKU.UnitPrice * productQuantity);

                price += productPrice;
            }

            return price;
        }
    }
}
