namespace ShoppingBasket;

/// <summary>Prices a quantity of a single item. Implement this to add a new offer.</summary>
public interface IPricingRule
{
    /// <summary>Total price in pence for <paramref name="quantity"/> units of the item.</summary>
    int Price(int quantity);
}
