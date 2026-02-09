using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Domain.ValueObjects
{
    /// <summary>
    /// Value Object représentant un prix strictement positif.
    /// </summary>
    public readonly record struct Price
    {
        public decimal Value { get; init; }

        public Price(decimal value)
        {
            if (value <= 0m)
            {
                throw new DomainException("Un prix doit être strictement positif.");
            }

            Value = value;
        }

        public override string ToString() => Value.ToString("F2");
    }
}