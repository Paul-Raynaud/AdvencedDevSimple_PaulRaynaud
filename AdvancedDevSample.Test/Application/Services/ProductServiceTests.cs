using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Test.Application.Services
{
    /// <summary>
    /// Tests unitaires pour ProductService
    /// </summary>
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockRepository.Object);
        }

        [Fact]
        public void CreateProduct_WithValidRequest_ShouldReturnProductResponse()
        {
            // Arrange
            var request = new CreateProductRequest { Price = 100m };
            _mockRepository.Setup(r => r.Add(It.IsAny<Product>()));

            // Act
            var result = _productService.CreateProduct(request);

            // Assert
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(100m, result.Price);
            Assert.True(result.IsActive);
            _mockRepository.Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void CreateProduct_WithInvalidPrice_ShouldThrowDomainException()
        {
            // Arrange
            var request = new CreateProductRequest { Price = -10m };

            // Act & Assert
            Assert.Throws<DomainException>(() => _productService.CreateProduct(request));
        }

        [Fact]
        public void GetProduct_WithExistingId_ShouldReturnProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product(productId, new Price(100m));
            _mockRepository.Setup(r => r.GetById(productId)).Returns(product);

            // Act
            var result = _productService.GetProduct(productId);

            // Assert
            Assert.Equal(productId, result.Id);
            Assert.Equal(100m, result.Price);
        }

        [Fact]
        public void GetProduct_WithNonExistingId_ShouldThrowApplicationServiceException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetById(productId)).Returns((Product)null);

            // Act & Assert
            var exception = Assert.Throws<ApplicationServiceException>(() => _productService.GetProduct(productId));
            Assert.Equal("Produit non trouvé.", exception.Message);
        }

        [Fact]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product(new Price(100m)),
                new Product(new Price(200m)),
                new Product(new Price(300m))
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(products);

            // Act
            var results = _productService.GetAllProducts().ToList();

            // Assert
            Assert.Equal(3, results.Count);
            Assert.Equal(100m, results[0].Price);
            Assert.Equal(200m, results[1].Price);
            Assert.Equal(300m, results[2].Price);
        }

        [Fact]
        public void UpdateProduct_WithExistingId_ShouldUpdateAndReturnProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product(productId, new Price(100m));
            var request = new UpdateProductRequest { Price = 150m, IsActive = false };
            
            _mockRepository.Setup(r => r.GetById(productId)).Returns(product);
            _mockRepository.Setup(r => r.Save(It.IsAny<Product>()));

            // Act
            var result = _productService.UpdateProduct(productId, request);

            // Assert
            Assert.Equal(productId, result.Id);
            Assert.Equal(150m, result.Price);
            Assert.False(result.IsActive);
            _mockRepository.Verify(r => r.Save(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void UpdateProduct_WithNonExistingId_ShouldThrowApplicationServiceException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var request = new UpdateProductRequest { Price = 150m };
            _mockRepository.Setup(r => r.GetById(productId)).Returns((Product)null);

            // Act & Assert
            Assert.Throws<ApplicationServiceException>(() => _productService.UpdateProduct(productId, request));
        }

        [Fact]
        public void UpdateProduct_WithInactiveProduct_ShouldThrowDomainException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product(productId, new Price(100m));
            product.Deactivate();
            var request = new UpdateProductRequest { Price = 150m };
            
            _mockRepository.Setup(r => r.GetById(productId)).Returns(product);

            // Act & Assert
            Assert.Throws<DomainException>(() => _productService.UpdateProduct(productId, request));
        }

        [Fact]
        public void DeleteProduct_WithExistingId_ShouldDeleteProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockRepository.Setup(r => r.Exists(productId)).Returns(true);
            _mockRepository.Setup(r => r.Delete(productId));

            // Act
            _productService.DeleteProduct(productId);

            // Assert
            _mockRepository.Verify(r => r.Delete(productId), Times.Once);
        }

        [Fact]
        public void DeleteProduct_WithNonExistingId_ShouldThrowApplicationServiceException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockRepository.Setup(r => r.Exists(productId)).Returns(false);

            // Act & Assert
            var exception = Assert.Throws<ApplicationServiceException>(() => _productService.DeleteProduct(productId));
            Assert.Equal("Produit non trouvé.", exception.Message);
        }

        [Fact]
        public void ChangePrice_WithValidData_ShouldUpdatePrice()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product(productId, new Price(100m));
            var request = new ChangePriceRequest { NewPrice = 200m };
            
            _mockRepository.Setup(r => r.GetById(productId)).Returns(product);
            _mockRepository.Setup(r => r.Save(It.IsAny<Product>()));

            // Act
            _productService.ChangePrice(productId, request);

            // Assert
            _mockRepository.Verify(r => r.Save(It.Is<Product>(p => p.Price.Value == 200m)), Times.Once);
        }
    }
}
