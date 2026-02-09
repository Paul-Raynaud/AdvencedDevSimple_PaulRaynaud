using System.ComponentModel.DataAnnotations;

namespace AdvancedDevSample.Application.DTOs
{
    public class CreateProductRequest
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0")]
        public decimal Price { get; set; }
    }
}
