namespace ShoppingBasket;

/// <summary>The prices and offers given in the specification. Prices are in pence.</summary>
public static class Catalogue
{
    public static IReadOnlyDictionary<string, IPricingRule> Standard() => new Dictionary<string, IPricingRule>
    {
        ["Apple"] = new UnitPriceRule(35),
        ["Banana"] = new UnitPriceRule(20),
        ["Melon"] = MultiBuyRule.BuyOneGetOneFree(50),
        ["Lime"] = MultiBuyRule.ThreeForTwo(15),
    };
}
