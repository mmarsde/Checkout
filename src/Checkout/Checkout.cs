using Checkout.Models;

namespace Checkout
{
    public class Checkout : ICheckout
    {
        private readonly List<string> _scannedItems = new List<string>();
        private readonly List<SKU> _productList;

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
            _scannedItems.Add(item);
        }

        private int CalculatePrice()
        {
            var price = 0;

            foreach(var item in _scannedItems)
            {
                if (item == "A")
                    price += 50;
                else if (item == "B")
                    price += 30;
            }

            return price;
        }
    }
}
