﻿using System;
using System.Collections.Generic;
using System.Text;
using AdvancedDevSample.Domain.Entities;
namespace AdvancedDevSample.Domain.Interfaces
{
    public interface IProductRepository
    {
        void Add(Product product);
        void Save(Product product);
        Product GetById(Guid id);
        IEnumerable<Product> GetAll();
        void Delete(Guid id);
        bool Exists(Guid id);
    }
}
