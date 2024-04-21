using Checkout.Models;

namespace Checkout.Services
{
    public class CheckoutService : ICheckout
    {
        private readonly List<string> _scannedItems = new List<string>();
        private readonly List<SKU> _productList;
        private readonly List<Discount> _discounts;

        public CheckoutService(IEnumerable<SKU> productList)
            : this(productList, Enumerable.Empty<Discount>())
        {
        }

        public CheckoutService(IEnumerable<SKU> productList, IEnumerable<Discount> discounts)
        {
            _productList = productList.ToList();
            _discounts = discounts.ToList();
        }

        public int GetTotalPrice()
        {
            if (_scannedItems.Count == 0)
                return 0;

            var productQuantities = _productList
                .Select(sku => (SKU: sku,
                    Quantity: _scannedItems.Count(scannedItem => scannedItem == sku.ProductName),
                    Discount: _discounts.FirstOrDefault(discount => discount.Product == sku.ProductName)))
                .Where(productGroupings => productGroupings.Quantity > 0);

            return productQuantities
                .Sum(CalculatePrice);
        }

        public void Scan(string item)
        {
            void ScanInternal(string item)
            {
                if (!_productList.Any(sku => sku.ProductName == item))
                {
                    throw new ArgumentException($"Product {item} is not a valid product.", nameof(item));
                };

                _scannedItems.Add(item);
            }

            var items = item.ToCharArray().Select(x => x.ToString()).ToList();
            items.ForEach(ScanInternal);
        }

        private static int CalculatePrice((SKU SKU, int Quantity, Discount Discount) productQuantitiesAndDiscounts)
        {
            var productPrice = 0;
            var productQuantity = productQuantitiesAndDiscounts.Quantity;

            if (productQuantitiesAndDiscounts.Discount is not null)
            {
                (int discountPrice, int discountQuantity) = CalculateDiscountPrice(productQuantitiesAndDiscounts.Quantity, productQuantitiesAndDiscounts.Discount);

                productPrice += discountPrice;
                productQuantity -= discountQuantity;
            }

            productPrice += CalculateBasePrice(productQuantity, productQuantitiesAndDiscounts.SKU.UnitPrice);

            return productPrice;
        }

        private static int CalculateBasePrice(int quantity, int unitPrice) => quantity * unitPrice;

        private static (int DiscountPrice, int DiscountQuantity) CalculateDiscountPrice(int scannedQuantity, Discount discount)
        {
            var nonDiscountableQuantity = scannedQuantity % discount.DiscountUnit;
            var discountableQuantity = scannedQuantity - nonDiscountableQuantity;

            var discountGroups = discountableQuantity / discount.DiscountUnit;
            var discountPrice = discountGroups * discount.DiscountPrice;

            return (discountPrice, discountableQuantity);
        }
    }
}