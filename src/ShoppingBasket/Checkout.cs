namespace ShoppingBasket;

/// <summary>Calculates the price of a basket using the pricing rules in an injected catalogue.</summary>
public sealed class Checkout(IReadOnlyDictionary<string, IPricingRule> catalogue)
{
    // Copied with an ordinal comparer so matching is always exact and later changes to the caller's dictionary have no effect.
    private readonly Dictionary<string, IPricingRule> _catalogue =
        new(catalogue ?? throw new ArgumentNullException(nameof(catalogue)), StringComparer.Ordinal);

    /// <summary>Total price of <paramref name="items"/> in pence. Order of items does not matter.</summary>
    /// <exception cref="UnknownItemException">An item is not in the catalogue.</exception>
    public int Total(IEnumerable<string> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        return items.CountBy(item => item).Sum(line => RuleFor(line.Key).Price(line.Value));
    }

    private IPricingRule RuleFor(string item) =>
        _catalogue.TryGetValue(item, out var rule) ? rule : throw new UnknownItemException(item);
}
