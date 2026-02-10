﻿﻿﻿using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        // Stockage en mémoire pour simulation
        private static readonly Dictionary<Guid, Product> Products = new();

        public void Add(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);
            
            if (Products.ContainsKey(product.Id))
                throw new InvalidOperationException($"Un produit avec l'ID {product.Id} existe déjà.");
            
            Products[product.Id] = product;
        }

        public Product? GetById(Guid id)
        {
            Products.TryGetValue(id, out var product);
            return product;
        }

        public IEnumerable<Product> GetAll()
        {
            return Products.Values.ToList();
        }

        public void Save(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);
            
            Products[product.Id] = product;
            Console.WriteLine($"Produit avec ID {product.Id} sauvegardé avec le prix {product.Price}.");
        }

        public void Delete(Guid id)
        {
            Products.Remove(id);
        }

        public bool Exists(Guid id)
        {
            return Products.ContainsKey(id);
        }
    }
}
