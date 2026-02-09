using System;
using Xunit;
using AdvancedDevSample.Domain.ValueObjects;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Test.Domain.ValueObjects
{
    /// <summary>
    /// Tests unitaires pour le Value Object Price
    /// </summary>
    public class PriceTests
    {
        [Theory]
        [InlineData(0.01)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999.99)]
        public void Price_WhenCreatedWithPositiveValue_ShouldSucceed(decimal value)
        {
            // Act
            var price = new Price(value);

            // Assert
            Assert.Equal(value, price.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-0.01)]
        [InlineData(-100)]
        public void Price_WhenCreatedWithNonPositiveValue_ShouldThrowDomainException(decimal value)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new Price(value));
            Assert.Equal("Un prix doit être strictement positif.", exception.Message);
        }

        [Fact]
        public void Price_ToString_ShouldReturnFormattedValue()
        {
            // Arrange
            var price = new Price(123.45m);

            // Act
            var result = price.ToString();

            // Assert
            Assert.Equal("123.45", result);
        }

        [Fact]
        public void Price_TwoPricesWithSameValue_ShouldBeEqual()
        {
            // Arrange
            var price1 = new Price(100m);
            var price2 = new Price(100m);

            // Act & Assert
            Assert.Equal(price1, price2);
        }

        [Fact]
        public void Price_TwoPricesWithDifferentValues_ShouldNotBeEqual()
        {
            // Arrange
            var price1 = new Price(100m);
            var price2 = new Price(200m);

            // Act & Assert
            Assert.NotEqual(price1, price2);
        }
    }
}
