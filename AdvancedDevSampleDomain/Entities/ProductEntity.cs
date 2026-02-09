using System;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    /// <summary>
    /// Entité de persistence simple pour les produits (pour EF / mapping).
    /// </summary>
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public ProductEntity() { }

        public ProductEntity(Guid id, decimal price, bool isActive)
        {
            Id = id;
            Price = price;
            IsActive = isActive;
        }

        /// <summary>
        /// Convertit l'entité de persistence en objet domaine.
        /// </summary>
        public Product ToDomain()
        {
            var domain = new Product(Id, new Price(Price));
            if (!IsActive) domain.Deactivate();
            return domain;
        }

        /// <summary>
        /// Crée une entité de persistence à partir de l'objet domaine.
        /// </summary>
        public static ProductEntity FromDomain(Product product) =>
            new ProductEntity(product.Id, product.Price.Value, product.IsActive);
    }
}