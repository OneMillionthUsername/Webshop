using System;
using System.Collections.Generic;
using System.Text;

using FluentAssertions;
using Webshop.Models;
using Webshop.Services;

namespace Tests_BL_Backend.Services
{
    public class PricingServiceTests
    {
        private readonly PricingService _service;

        public PricingServiceTests()
        {
            _service = new PricingService();
        }

        // -------------------------
        // CalculateDiscount – Null/Leer/Whitespace
        // -------------------------

        /// <summary>
        /// Tests that CalculateDiscount returns 0 when couponCode is null, empty, or whitespace.
        /// </summary>
        /// <param name="couponCode">The coupon code to test (null, empty, or whitespace).</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        [InlineData("  \t  \n  ")]
        public void CalculateDiscount_NullEmptyOrWhitespaceCouponCode_ReturnsZero(string? couponCode)
        {
            // Arrange
            var product = new Product { BasePrice = 100m };

            // Act
            var result = _service.CalculateDiscount(product, couponCode!);

            // Assert
            Assert.Equal(0m, result);
        }

        // -------------------------
        // CalculateDiscount – Gültige Gutscheincodes
        // -------------------------

        /// <summary>
        /// Tests that CalculateDiscount correctly calculates discount for valid coupon codes (case-insensitive).
        /// </summary>
        /// <param name="couponCode">The coupon code to test.</param>
        /// <param name="basePrice">The base price of the product.</param>
        /// <param name="expectedDiscount">The expected discount amount.</param>
        [Theory]
        [InlineData("SUMMER10", 100, 10)]
        [InlineData("summer10", 100, 10)]
        [InlineData("Summer10", 200, 20)]
        [InlineData("SuMmEr10", 50, 5)]
        [InlineData("WELCOME20", 100, 20)]
        [InlineData("welcome20", 100, 20)]
        [InlineData("Welcome20", 200, 40)]
        [InlineData("WeLcOmE20", 50, 10)]
        [InlineData("VIP30", 100, 30)]
        [InlineData("vip30", 100, 30)]
        [InlineData("Vip30", 200, 60)]
        [InlineData("ViP30", 1000, 300)]
        public void CalculateDiscount_ValidCouponCode_ReturnsCorrectDiscount(string couponCode, decimal basePrice, decimal expectedDiscount)
        {
            // Arrange
            var product = new Product { BasePrice = basePrice };

            // Act
            var result = _service.CalculateDiscount(product, couponCode);

            // Assert
            Assert.Equal(expectedDiscount, result);
        }

        // -------------------------
        // CalculateDiscount – Ungültige Gutscheincodes
        // -------------------------

        /// <summary>
        /// Tests that CalculateDiscount returns 0 for unknown or invalid coupon codes.
        /// </summary>
        /// <param name="couponCode">The invalid coupon code to test.</param>
        [Theory]
        [InlineData("INVALID")]
        [InlineData("XYZ")]
        [InlineData("SUMMER")]
        [InlineData("SUMMER10X")]
        [InlineData("XSUMMER10")]
        [InlineData("WELCOME")]
        [InlineData("VIP")]
        [InlineData("VIP40")]
        [InlineData("ABC123")]
        [InlineData("!@#$%")]
        [InlineData("123456")]
        public void CalculateDiscount_InvalidCouponCode_ReturnsZero(string couponCode)
        {
            // Arrange
            var product = new Product { BasePrice = 100m };

            // Act
            var result = _service.CalculateDiscount(product, couponCode);

            // Assert
            Assert.Equal(0m, result);
        }

        // -------------------------
        // CalculateDiscount – Grenzwerte BasePrice
        // -------------------------

        /// <summary>
        /// Tests that CalculateDiscount correctly handles edge case BasePrice values.
        /// </summary>
        /// <param name="basePrice">The base price to test.</param>
        /// <param name="couponCode">The coupon code to use.</param>
        /// <param name="expectedDiscount">The expected discount amount.</param>
        [Theory]
        [InlineData(0, "SUMMER10", 0)]
        [InlineData(1, "SUMMER10", 0.10)]
        [InlineData(0.01, "WELCOME20", 0.002)]
        [InlineData(999999.99, "VIP30", 299999.997)]
        [InlineData(10000, "SUMMER10", 1000)]
        public void CalculateDiscount_EdgeCaseBasePrices_ReturnsCorrectDiscount(decimal basePrice, string couponCode, decimal expectedDiscount)
        {
            // Arrange
            var product = new Product { BasePrice = basePrice };

            // Act
            var result = _service.CalculateDiscount(product, couponCode);

            // Assert
            Assert.Equal(expectedDiscount, result);
        }

        /// <summary>
        /// Tests that CalculateDiscount handles negative BasePrice values (edge case scenario).
        /// Negative prices may represent credits or refunds in some business contexts.
        /// </summary>
        [Fact]
        public void CalculateDiscount_NegativeBasePrice_ReturnsNegativeDiscount()
        {
            // Arrange
            var product = new Product { BasePrice = -100m };

            // Act
            var result = _service.CalculateDiscount(product, "SUMMER10");

            // Assert
            Assert.Equal(-10m, result);
        }

        /// <summary>
        /// Tests that CalculateDiscount handles very long coupon codes correctly.
        /// </summary>
        [Fact]
        public void CalculateDiscount_VeryLongCouponCode_ReturnsZero()
        {
            // Arrange
            var product = new Product { BasePrice = 100m };
            var longCouponCode = new string('A', 10000);

            // Act
            var result = _service.CalculateDiscount(product, longCouponCode);

            // Assert
            Assert.Equal(0m, result);
        }

        // -------------------------
        // CalculateDiscount – Sonderzeichen
        // -------------------------

        /// <summary>
        /// Tests that CalculateDiscount handles coupon codes with special characters correctly.
        /// </summary>
        /// <param name="couponCode">The coupon code with special characters.</param>
        [Theory]
        [InlineData("SUMMER-10")]
        [InlineData("SUMMER_10")]
        [InlineData("SUMMER 10")]
        [InlineData("SUMMER10!")]
        [InlineData("@SUMMER10")]
        [InlineData("SUMMER10\n")]
        [InlineData("SUMMER10\t")]
        public void CalculateDiscount_CouponCodeWithSpecialCharacters_ReturnsZero(string couponCode)
        {
            // Arrange
            var product = new Product { BasePrice = 100m };

            // Act
            var result = _service.CalculateDiscount(product, couponCode);

            // Assert
            Assert.Equal(0m, result);
        }

        // -------------------------
        // CalculateDiscount – BasePrice = 0
        // -------------------------

        /// <summary>
        /// Tests that CalculateDiscount returns 0 when BasePrice is 0 regardless of valid coupon code.
        /// </summary>
        /// <param name="couponCode">The valid coupon code to test.</param>
        [Theory]
        [InlineData("SUMMER10")]
        [InlineData("WELCOME20")]
        [InlineData("VIP30")]
        public void CalculateDiscount_ZeroBasePrice_ReturnsZero(string couponCode)
        {
            // Arrange
            var product = new Product { BasePrice = 0m };

            // Act
            var result = _service.CalculateDiscount(product, couponCode);

            // Assert
            Assert.Equal(0m, result);
        }

        // -------------------------
        // CalculateDiscount – Prozentsatz-Verifikation
        // -------------------------

        /// <summary>
        /// Tests that CalculateDiscount calculates correct percentage for all valid coupon codes.
        /// Verifies the exact percentage calculation for each known coupon.
        /// </summary>
        /// <param name="couponCode">The coupon code to test.</param>
        /// <param name="expectedPercentage">The expected discount percentage (as decimal, e.g., 0.10 for 10%).</param>
        [Theory]
        [InlineData("SUMMER10", 0.10)]
        [InlineData("WELCOME20", 0.20)]
        [InlineData("VIP30", 0.30)]
        public void CalculateDiscount_AllValidCoupons_CalculatesCorrectPercentage(string couponCode, decimal expectedPercentage)
        {
            // Arrange
            var basePrice = 123.45m;
            var product = new Product { BasePrice = basePrice };
            var expectedDiscount = basePrice * expectedPercentage;

            // Act
            var result = _service.CalculateDiscount(product, couponCode);

            // Assert
            Assert.Equal(expectedDiscount, result);
        }
    }
}