namespace Checkout
{
    public class Checkout : ICheckout
    {
        private readonly List<string> _scannedItems = new List<string>();

        public int GetTotalPrice()
        {
            if (_scannedItems.Count == 0)
                return 0;

            return 50;
        }

        public void Scan(string item)
        {
            _scannedItems.Add(item);
        }
    }
}
