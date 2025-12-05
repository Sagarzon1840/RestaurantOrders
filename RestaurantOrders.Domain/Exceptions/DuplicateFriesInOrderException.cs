namespace RestaurantOrders.Domain.Exceptions
{
    /// <summary>
    /// Thrown when attempting to add duplicate fries to an order.
    /// </summary>
    public class DuplicateFriesInOrderException : DomainValidationException
    {
        public DuplicateFriesInOrderException() 
            : base("An order can only contain one order of fries. Please remove the existing fries before adding more.")
        {
        }
    }
}
