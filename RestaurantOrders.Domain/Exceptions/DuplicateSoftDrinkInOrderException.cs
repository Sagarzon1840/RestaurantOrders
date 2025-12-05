namespace RestaurantOrders.Domain.Exceptions
{
    /// <summary>
    /// Thrown when attempting to add a duplicate soft drink to an order.
    /// </summary>
    public class DuplicateSoftDrinkInOrderException : DomainValidationException
    {
        public DuplicateSoftDrinkInOrderException() 
            : base("An order can only contain one soft drink. Please remove the existing soft drink before adding another.")
        {
        }
    }
}
