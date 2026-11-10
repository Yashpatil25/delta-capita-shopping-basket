namespace ShoppingBasket;

/// <summary>No offer: every unit costs the same.</summary>
public sealed class UnitPriceRule(int unitPrice) : IPricingRule
{
    private readonly int _unitPrice = unitPrice >= 0 ? unitPrice : throw new ArgumentOutOfRangeException(nameof(unitPrice));

    public int Price(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        return quantity * _unitPrice;
    }
}
