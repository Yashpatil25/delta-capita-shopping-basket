namespace ShoppingBasket;

/// <summary>Thrown when the basket contains an item that is not in the catalogue.</summary>
public sealed class UnknownItemException(string itemName)
    : Exception($"Unknown item '{itemName}'. Item names are case-sensitive.")
{
    public string ItemName { get; } = itemName;
}
