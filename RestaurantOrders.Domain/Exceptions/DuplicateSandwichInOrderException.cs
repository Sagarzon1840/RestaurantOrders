namespace RestaurantOrders.Domain.Exceptions
{
    /// <summary>
    /// Thrown when attempting to add a duplicate sandwich to an order.
    /// </summary>
    public class DuplicateSandwichInOrderException : DomainValidationException
    {
        public DuplicateSandwichInOrderException() 
            : base("An order can only contain one sandwich. Please remove the existing sandwich before adding a new one.")
        {
        }
    }
}
