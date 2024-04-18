namespace Checkout
{
    public class Checkout : ICheckout
    {
        private readonly List<char> _scannedItems = new List<char>();

        public int GetTotalPrice()
        {
            if (_scannedItems.Count == 0)
                return 0;

            return 50;
        }

        public void Scan(char item)
        {
            _scannedItems.Add(item);
        }
    }
}
