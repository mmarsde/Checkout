using FluentAssertions;

namespace Checkout.Tests
{
    public class CheckoutTests
    {
        [Fact]
        public void GetTotalPrice_NoItems_Returns_Zero()
        {
            // Arrange
            var sut = new Checkout();

            // Act
            var price = sut.GetTotalPrice();

            // Assert
            price.Should().Be(0);
        }

        [Fact]
        public void GetTotalPrice_ProductA_Returns_50()
        {
            // Arrange
            var sut = new Checkout();
            sut.Scan("A");

            // Act
            var price = sut.GetTotalPrice();

            // Assert
            price.Should().Be(50);
        }
    }
}