namespace ShoppingBasket;

/// <summary>"Buy <c>buy</c>, pay for <c>payFor</c>" offer, e.g. buy one get one free is buy 2 pay for 1.</summary>
public sealed class MultiBuyRule : IPricingRule
{
    private readonly int _unitPrice, _buy, _payFor;

    public MultiBuyRule(int unitPrice, int buy, int payFor)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(unitPrice);
        ArgumentOutOfRangeException.ThrowIfLessThan(payFor, 1);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(buy, payFor);
        (_unitPrice, _buy, _payFor) = (unitPrice, buy, payFor);
    }

    public static MultiBuyRule BuyOneGetOneFree(int unitPrice) => new(unitPrice, buy: 2, payFor: 1);

    public static MultiBuyRule ThreeForTwo(int unitPrice) => new(unitPrice, buy: 3, payFor: 2);

    public int Price(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        var paidUnits = quantity / _buy * _payFor + quantity % _buy;
        return paidUnits * _unitPrice;
    }
}
