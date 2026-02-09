using System;

namespace AdvancedDevSample.Application.DTOs
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
