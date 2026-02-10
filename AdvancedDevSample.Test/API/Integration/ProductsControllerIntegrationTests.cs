using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using AdvancedDevSample.Application.DTOs;

namespace AdvancedDevSample.Test.API.Integration
{
    /// <summary>
    /// Tests d'intégration pour l'API Products
    /// </summary>
    public class ProductsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductsControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateProduct_WithValidData_ShouldReturn201()
        {
            // Arrange
            var request = new CreateProductRequest { Price = 100m };

            // Act
            var response = await _client.PostAsJsonAsync("/api/products", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var product = await response.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(product);
            Assert.Equal(100m, product.Price);
            Assert.True(product.IsActive);
        }

        [Fact]
        public async Task CreateProduct_WithInvalidPrice_ShouldReturn400()
        {
            // Arrange
            var request = new CreateProductRequest { Price = -10m };

            // Act
            var response = await _client.PostAsJsonAsync("/api/products", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetProduct_WithExistingId_ShouldReturn200()
        {
            // Arrange - Créer d'abord un produit
            var createRequest = new CreateProductRequest { Price = 150m };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(createdProduct);

            // Act
            var response = await _client.GetAsync($"/api/products/{createdProduct.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var product = await response.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(product);
            Assert.Equal(createdProduct.Id, product.Id);
            Assert.Equal(150m, product.Price);
        }

        [Fact]
        public async Task GetProduct_WithNonExistingId_ShouldReturn404()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/api/products/{nonExistingId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturn200WithList()
        {
            // Arrange - Créer quelques produits
            await _client.PostAsJsonAsync("/api/products", new CreateProductRequest { Price = 100m });
            await _client.PostAsJsonAsync("/api/products", new CreateProductRequest { Price = 200m });

            // Act
            var response = await _client.GetAsync("/api/products");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var products = await response.Content.ReadFromJsonAsync<ProductResponse[]>();
            Assert.NotNull(products);
            Assert.NotEmpty(products);
        }

        [Fact]
        public async Task UpdateProduct_WithValidData_ShouldReturn200()
        {
            // Arrange - Créer d'abord un produit
            var createRequest = new CreateProductRequest { Price = 100m };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(createdProduct);

            var updateRequest = new UpdateProductRequest { Price = 250m, IsActive = true };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{createdProduct.Id}", updateRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var updatedProduct = await response.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(updatedProduct);
            Assert.Equal(250m, updatedProduct.Price);
            Assert.True(updatedProduct.IsActive);
        }

        [Fact]
        public async Task UpdateProduct_WithNonExistingId_ShouldReturn404()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            var updateRequest = new UpdateProductRequest { Price = 250m };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{nonExistingId}", updateRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_WithExistingId_ShouldReturn204()
        {
            // Arrange - Créer d'abord un produit
            var createRequest = new CreateProductRequest { Price = 100m };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(createdProduct);

            // Act
            var response = await _client.DeleteAsync($"/api/products/{createdProduct.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Vérifier que le produit n'existe plus
            var getResponse = await _client.GetAsync($"/api/products/{createdProduct.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_WithNonExistingId_ShouldReturn404()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"/api/products/{nonExistingId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ChangePrice_WithValidData_ShouldReturn204()
        {
            // Arrange - Créer d'abord un produit
            var createRequest = new CreateProductRequest { Price = 100m };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(createdProduct);

            var changePriceRequest = new ChangePriceRequest { NewPrice = 300m };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{createdProduct.Id}/price", changePriceRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Vérifier que le prix a été changé
            var getResponse = await _client.GetAsync($"/api/products/{createdProduct.Id}");
            var product = await getResponse.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(product);
            Assert.Equal(300m, product.Price);
        }

        [Fact]
        public async Task ChangePrice_ForInactiveProduct_ShouldReturn400()
        {
            // Arrange - Créer un produit et le désactiver
            var createRequest = new CreateProductRequest { Price = 100m };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(createdProduct);

            var deactivateRequest = new UpdateProductRequest { Price = 100m, IsActive = false };
            await _client.PutAsJsonAsync($"/api/products/{createdProduct.Id}", deactivateRequest);

            var changePriceRequest = new ChangePriceRequest { NewPrice = 300m };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{createdProduct.Id}/price", changePriceRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_WithZeroPrice_ShouldReturn400()
        {
            // Arrange
            var request = new CreateProductRequest { Price = 0m };

            // Act
            var response = await _client.PostAsJsonAsync("/api/products", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_WithInactiveProductAndPriceChange_ShouldReturn400()
        {
            // Arrange - Créer un produit et le désactiver
            var createRequest = new CreateProductRequest { Price = 100m };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(createdProduct);

            var deactivateRequest = new UpdateProductRequest { Price = 100m, IsActive = false };
            await _client.PutAsJsonAsync($"/api/products/{createdProduct.Id}", deactivateRequest);

            // Act - Essayer de changer le prix d'un produit inactif
            var updateRequest = new UpdateProductRequest { Price = 200m, IsActive = false };
            var response = await _client.PutAsJsonAsync($"/api/products/{createdProduct.Id}", updateRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ChangePrice_WithNonExistingId_ShouldReturn404()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            var changePriceRequest = new ChangePriceRequest { NewPrice = 300m };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{nonExistingId}/price", changePriceRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ChangePrice_WithNegativePrice_ShouldReturn400()
        {
            // Arrange
            var createRequest = new CreateProductRequest { Price = 100m };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(createdProduct);

            var changePriceRequest = new ChangePriceRequest { NewPrice = -50m };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{createdProduct.Id}/price", changePriceRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ActivateDeactivatedProduct_ShouldReturn200()
        {
            // Arrange
            var createRequest = new CreateProductRequest { Price = 100m };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(createdProduct);

            // Désactiver
            var deactivateRequest = new UpdateProductRequest { Price = 100m, IsActive = false };
            await _client.PutAsJsonAsync($"/api/products/{createdProduct.Id}", deactivateRequest);

            // Act - Réactiver
            var activateRequest = new UpdateProductRequest { Price = 100m, IsActive = true };
            var response = await _client.PutAsJsonAsync($"/api/products/{createdProduct.Id}", activateRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var reactivatedProduct = await response.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(reactivatedProduct);
            Assert.True(reactivatedProduct.IsActive);
        }
    }
}
