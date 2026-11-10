namespace ShoppingBasket.Tests;

public class CheckoutTests
{
    private static int Total(params string[] items) => new Checkout(Catalogue.Standard()).Total(items);

    private static int Total(string item, int quantity) => Total(Enumerable.Repeat(item, quantity).ToArray());

    [Fact]
    public void Empty_basket_costs_nothing() => Assert.Equal(0, Total());

    [Theory]
    [InlineData("Apple", 35)]
    [InlineData("Banana", 20)]
    [InlineData("Melon", 50)]
    [InlineData("Lime", 15)]
    public void Single_item_costs_its_unit_price(string item, int expected) => Assert.Equal(expected, Total(item));

    [Theory]
    [InlineData(1, 50)]
    [InlineData(2, 50)]
    [InlineData(3, 100)]
    [InlineData(4, 100)]
    public void Melons_are_buy_one_get_one_free(int quantity, int expected) => Assert.Equal(expected, Total("Melon", quantity));

    [Theory]
    [InlineData(1, 15)]
    [InlineData(2, 30)]
    [InlineData(3, 30)]
    [InlineData(4, 45)]
    [InlineData(5, 60)]
    [InlineData(6, 60)]
    public void Limes_are_three_for_the_price_of_two(int quantity, int expected) => Assert.Equal(expected, Total("Lime", quantity));

    [Fact]
    public void Example_from_specification_costs_90p() => Assert.Equal(90, Total("Apple", "Apple", "Banana"));

    [Fact]
    public void Mixed_basket_applies_each_items_rule()
    {
        // 2 apples 70 + 1 banana 20 + 3 melons 100 + 4 limes 45
        var total = Total("Lime", "Apple", "Melon", "Lime", "Banana", "Melon", "Lime", "Apple", "Melon", "Lime");
        Assert.Equal(235, total);
    }

    [Theory]
    [InlineData("Melon", "Apple", "Melon", "Lime", "Lime", "Lime")]
    [InlineData("Lime", "Melon", "Lime", "Apple", "Lime", "Melon")]
    [InlineData("Apple", "Lime", "Lime", "Lime", "Melon", "Melon")]
    public void Item_order_does_not_matter(params string[] items) => Assert.Equal(115, Total(items));

    [Theory]
    [InlineData("Pear")]
    [InlineData("apple")]
    [InlineData("")]
    public void Unknown_item_throws_naming_the_item(string item)
    {
        var ex = Assert.Throws<UnknownItemException>(() => Total("Apple", item));
        Assert.Equal(item, ex.ItemName);
    }

    [Fact]
    public void Uses_the_injected_catalogue()
    {
        var checkout = new Checkout(new Dictionary<string, IPricingRule> { ["Pear"] = MultiBuyRule.ThreeForTwo(40) });
        Assert.Equal(80, checkout.Total(["Pear", "Pear", "Pear"]));
        Assert.Throws<UnknownItemException>(() => checkout.Total(["Apple"]));
    }

    [Fact]
    public void Null_arguments_are_rejected()
    {
        Assert.Throws<ArgumentNullException>(() => new Checkout(null!));
        Assert.Throws<ArgumentNullException>(() => new Checkout(Catalogue.Standard()).Total(null!));
    }
}
