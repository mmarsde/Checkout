using Checkout.Models;
using FluentAssertions;

namespace Checkout.Tests
{
    public class CheckoutTests
    {
        private readonly List<SKU> _productList = new List<SKU>
        {
            new() { ProductName = "A", UnitPrice = 50 },
            new() { ProductName = "B", UnitPrice = 30 },
            new() { ProductName = "C", UnitPrice = 20 },
            new() { ProductName = "D", UnitPrice = 15 },
        };

        [Fact]
        public void GetTotalPrice_NoItems_Returns_Zero()
        {
            // Arrange
            var sut = new Checkout(_productList);

            // Act
            var price = sut.GetTotalPrice();

            // Assert
            price.Should().Be(0);
        }

        [Fact]
        public void GetTotalPrice_ProductA_Returns_50()
        {
            // Arrange
            var sut = new Checkout(_productList);
            sut.Scan("A");

            // Act
            var price = sut.GetTotalPrice();

            // Assert
            price.Should().Be(50);
        }

        [Fact]
        public void GetTotalPrice_ProductsA_And_B_Returns_70()
        {
            // Arrange
            var sut = new Checkout(_productList);
            sut.Scan("A");
            sut.Scan("B");

            // Act
            var price = sut.GetTotalPrice();

            // Assert
            price.Should().Be(80);
        }
    }
}