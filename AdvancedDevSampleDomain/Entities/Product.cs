﻿using System;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Domain.Entities
{
    public class Product
    {
        /// <summary>
        /// Représente un produit vendable.
        /// </summary>
        public Guid Id { get; private set; } // Identité
        public Price Price { get; private set; } // Invariant encapsulé dans Price
        public bool IsActive { get; private set; } // true par défaut

        // Constructeur principal
        public Product(Price price) : this(Guid.NewGuid(), price, true) { }

        // Constructeur avec ID
        public Product(Guid id, Price price) : this(id, price, true) { }

        // Constructeur complet
        public Product(Guid id, Price price, bool isActive)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Price = price; // Price valide par construction
            IsActive = isActive;
        }

        // Constructeur requis par certains ORMs ; protégé pour empêcher l'utilisation publique.
        protected Product()
        {
            IsActive = true;
        }

        public void ChangePrice(Price newPrice)
        {
            // Règle métier : le produit ne doit pas être inactif
            if (!IsActive)
            {
                throw new DomainException("Le produit est inactif.");
            }

            // Invariant déjà garanti par Price
            Price = newPrice;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}

