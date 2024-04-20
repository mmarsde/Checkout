using Checkout.Models;
using System.ComponentModel;

namespace Checkout
{
    public class Checkout : ICheckout
    {
        private readonly List<string> _scannedItems = new List<string>();
        private readonly List<SKU> _productList;
        private readonly List<Discount> _discounts;
        
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
            }

            _scannedItems.Add(item);
        }

        private int CalculatePrice()
        {
            var price = 0;

            var productQuantities = _productList.Select(x => (SKU: x, Quantity: _scannedItems.Count(y => x.ProductName == y)));

            foreach(var item in productQuantities)
            {
                price += (item.SKU.UnitPrice * item.Quantity); 
            }

            return price;
        }
    }
}
