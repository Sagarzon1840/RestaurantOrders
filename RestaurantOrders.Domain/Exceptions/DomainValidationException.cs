namespace RestaurantOrders.Domain.Exceptions
{
    /// <summary>
    /// Base exception for domain validation errors.
    /// </summary>
    public class DomainValidationException : Exception
    {
        public DomainValidationException(string message) : base(message)
        {
        }

        public DomainValidationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
