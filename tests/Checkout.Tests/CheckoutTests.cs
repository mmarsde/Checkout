using Checkout.Models;
using Checkout.Services;
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
            new() { Product = "A", DiscountUnit = 3, DiscountPrice = 130 },
            new() { Product = "B", DiscountUnit = 2, DiscountPrice = 45 }
        };

        [Theory]
        [InlineData(0, "")]
        [InlineData(50, "A")]
        [InlineData(80, "AB")]
        [InlineData(115, "CDBA")]
        [InlineData(100, "AA")]
        public void GetTotalPrice_NoDiscountableProducts_ReturnsExpected(int expectedPrice, string item)
        {
            // Arrange
            var expected = expectedPrice;
            var sut = new CheckoutService(_productList);
            sut.Scan(item);

            // Act
            var price = sut.GetTotalPrice();

            // Assert
            price.Should().Be(expected);
        }

        [Theory]
        [InlineData(130, "AAA")]
        [InlineData(180, "AAAA")]
        [InlineData(230, "AAAAA")]
        [InlineData(260, "AAAAAA")]
        [InlineData(160, "AAAB")]
        [InlineData(175, "AAABB")]
        [InlineData(190, "AAABBD")]
        [InlineData(190, "DABABA")]
        public void GetTotalPrice_ApplyDiscountRate_ReturnsExpected(int expectedPrice, string item)
        {
            // Arrange
            var expected = expectedPrice;
            var sut = new CheckoutService(_productList, _discounts);
            sut.Scan(item);

            // Act
            var price = sut.GetTotalPrice();

            // Assert
            price.Should().Be(expected);
        }

        [Fact]
        public void Scan_InvalidProduct_ThrowsArgumentException()
        {
            // Arrange
            var sut = new CheckoutService(_productList);

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => sut.Scan("AZ"));
        }
    }
}