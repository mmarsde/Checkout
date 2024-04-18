namespace Checkout
{
    public interface ICheckout
    {
        void Scan(char item);
        int GetTotalPrice();
    }
}
