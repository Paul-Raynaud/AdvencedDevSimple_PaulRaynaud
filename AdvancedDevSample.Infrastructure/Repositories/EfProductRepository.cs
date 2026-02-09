﻿using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Interfaces;


namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        // Stockage en mémoire pour simulation
        private static readonly Dictionary<Guid, Product> _products = new();

        public void Add(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            
            if (_products.ContainsKey(product.Id))
                throw new InvalidOperationException($"Un produit avec l'ID {product.Id} existe déjà.");
            
            _products[product.Id] = product;
        }

        public Product GetById(Guid id)
        {
            _products.TryGetValue(id, out var product);
            return product;
        }

        public IEnumerable<Product> GetAll()
        {
            return _products.Values.ToList();
        }

        public void Save(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            
            _products[product.Id] = product;
            Console.WriteLine($"Produit avec ID {product.Id} sauvegardé avec le prix {product.Price}.");
        }

        public void Delete(Guid id)
        {
            _products.Remove(id);
        }

        public bool Exists(Guid id)
        {
            return _products.ContainsKey(id);
        }
    }
}
