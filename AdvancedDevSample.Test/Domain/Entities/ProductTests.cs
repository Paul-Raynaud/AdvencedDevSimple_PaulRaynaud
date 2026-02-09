using System;
using Xunit;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.ValueObjects;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Test.Domain.Entities
{
    /// <summary>
    /// Tests unitaires pour l'entité Product
    /// </summary>
    public class ProductTests
    {
        [Fact]
        public void Product_WhenCreatedWithValidPrice_ShouldSucceed()
        {
            // Arrange
            var price = new Price(100m);

            // Act
            var product = new Product(price);

            // Assert
            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.Equal(100m, product.Price.Value);
            Assert.True(product.IsActive);
        }

        [Fact]
        public void Product_WhenCreatedWithId_ShouldUseProvidedId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var price = new Price(50m);

            // Act
            var product = new Product(id, price);

            // Assert
            Assert.Equal(id, product.Id);
        }

        [Fact]
        public void ChangePrice_WhenProductIsActive_ShouldUpdatePrice()
        {
            // Arrange
            var product = new Product(new Price(100m));
            var newPrice = new Price(150m);

            // Act
            product.ChangePrice(newPrice);

            // Assert
            Assert.Equal(150m, product.Price.Value);
        }

        [Fact]
        public void ChangePrice_WhenProductIsInactive_ShouldThrowDomainException()
        {
            // Arrange
            var product = new Product(new Price(100m));
            product.Deactivate();
            var newPrice = new Price(150m);

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => product.ChangePrice(newPrice));
            Assert.Equal("Le produit est inactif.", exception.Message);
        }

        [Fact]
        public void Deactivate_ShouldSetIsActiveToFalse()
        {
            // Arrange
            var product = new Product(new Price(100m));

            // Act
            product.Deactivate();

            // Assert
            Assert.False(product.IsActive);
        }

        [Fact]
        public void Activate_WhenDeactivated_ShouldSetIsActiveToTrue()
        {
            // Arrange
            var product = new Product(new Price(100m));
            product.Deactivate();

            // Act
            product.Activate();

            // Assert
            Assert.True(product.IsActive);
        }
    }
}
