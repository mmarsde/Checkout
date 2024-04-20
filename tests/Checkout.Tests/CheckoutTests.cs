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

        private readonly List<Discount> _discounts = new List<Discount>
        {
            new() { Product = "B", PurchaseQuantity = 2, DiscountPrice = 45 }
        };

        [Fact]
        public void GetTotalPrice_NoItems_ReturnsZero()
        {
            // Arrange
            var sut = new Checkout(_productList);

            // Act
            var price = sut.GetTotalPrice();

            // Assert
            price.Should().Be(0);
        }

        [Fact]
        public void GetTotalPrice_ProductA_Returns50()
        {
            // Arrange
            var expected = 50;
            var sut = new Checkout(_productList);
            sut.Scan("A");

            // Act
            var price = sut.GetTotalPrice();

            // Assert
            price.Should().Be(expected);
        }

        [Fact]
        public void GetTotalPrice_ProductsA_And_B_Returns70()
        {
            // Arrange
            var expected = 80;
            var sut = new Checkout(_productList);
            sut.Scan("A");
            sut.Scan("B");

            // Act
            var price = sut.GetTotalPrice();

            // Assert
            price.Should().Be(expected);
        }

        [Fact]
        public void GetTotalPrice_AllProductsScanned_ReturnsExpected()
        {
            // Arrange
            var expected = 115;
            var sut = new Checkout(_productList);
            sut.Scan("A");
            sut.Scan("B");
            sut.Scan("C");
            sut.Scan("D");

            // Act
            var price = sut.GetTotalPrice();

            // Assert
            price.Should().Be(expected);
        }

        [Fact]
        public void GetTotalPrice_ProductsBA_AndB_ReturnsDiscountedPrice()
        {
            // Arrange
            var expected = 95;
            var sut = new Checkout(_productList, _discounts);
            sut.Scan("B");
            sut.Scan("A");
            sut.Scan("B");

            // Act
            var price = sut.GetTotalPrice();

            // Assert
            price.Should().Be(expected);
        }

        [Fact]
        public void Scan_InvalidProduct_ThrowsArgumentException()
        {
            // Arrange
            var sut = new Checkout(_productList);

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => sut.Scan("Z"));
        }
    }
}