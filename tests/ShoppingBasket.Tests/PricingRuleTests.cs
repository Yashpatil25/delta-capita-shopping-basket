namespace ShoppingBasket.Tests;

public class PricingRuleTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 35)]
    [InlineData(3, 105)]
    public void Unit_price_rule_charges_every_unit(int quantity, int expected) =>
        Assert.Equal(expected, new UnitPriceRule(35).Price(quantity));

    [Theory]
    [InlineData(0, 0)]
    [InlineData(4, 40)] // buy 5 pay 3: four units are below the offer threshold
    [InlineData(5, 30)]
    [InlineData(7, 50)]
    [InlineData(10, 60)]
    public void Multi_buy_rule_charges_paid_units_per_group_plus_remainder(int quantity, int expected) =>
        Assert.Equal(expected, new MultiBuyRule(10, buy: 5, payFor: 3).Price(quantity));

    [Theory]
    [InlineData(-1, 2, 1)] // negative price
    [InlineData(10, 2, 0)] // pays for nothing
    [InlineData(10, 2, 2)] // not an offer
    [InlineData(10, 2, 3)] // costs more than no offer
    public void Multi_buy_rule_rejects_invalid_offers(int unitPrice, int buy, int payFor) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new MultiBuyRule(unitPrice, buy, payFor));

    [Fact]
    public void Rules_reject_negative_prices_and_quantities()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new UnitPriceRule(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new UnitPriceRule(35).Price(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => MultiBuyRule.ThreeForTwo(15).Price(-1));
    }

    [Fact]
    public void Price_overflow_throws_instead_of_wrapping() =>
        Assert.Throws<OverflowException>(() => new UnitPriceRule(35).Price(int.MaxValue));
}
