using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Application.Services
{
    public class ProductService
    {
        private const string ProductNotFoundMessage = "Produit non trouvé.";
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public ProductResponse CreateProduct(CreateProductRequest request)
        {
            var price = new Price(request.Price);
            var product = new Product(price);
            
            _repo.Add(product);
            
            return MapToResponse(product);
        }

        public ProductResponse GetProduct(Guid id)
        {
            var product = _repo.GetById(id)
                ?? throw new ApplicationServiceException(ProductNotFoundMessage);
            
            return MapToResponse(product);
        }

        public IEnumerable<ProductResponse> GetAllProducts()
        {
            return _repo.GetAll().Select(MapToResponse);
        }

        public ProductResponse UpdateProduct(Guid id, UpdateProductRequest request)
        {
            var product = _repo.GetById(id)
                ?? throw new ApplicationServiceException(ProductNotFoundMessage);

            var newPrice = new Price(request.Price);
            product.ChangePrice(newPrice);

            if (request.IsActive.HasValue)
            {
                if (request.IsActive.Value)
                    product.Activate();
                else
                    product.Deactivate();
            }

            _repo.Save(product);
            
            return MapToResponse(product);
        }

        public void DeleteProduct(Guid id)
        {
            if (!_repo.Exists(id))
                throw new ApplicationServiceException(ProductNotFoundMessage);
            
            _repo.Delete(id);
        }

        public void ChangePrice(Guid id, ChangePriceRequest request)
        {
            var product = _repo.GetById(id)
                ?? throw new ApplicationServiceException(ProductNotFoundMessage);
            
            var newPrice = new Price(request.NewPrice);
            product.ChangePrice(newPrice);
            _repo.Save(product);
        }

        private ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Price = product.Price.Value,
                IsActive = product.IsActive
            };
        }
    }
}

