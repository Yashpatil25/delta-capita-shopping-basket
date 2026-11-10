using System.Globalization;
using System.Text;
using ShoppingBasket;

// Usage: ShoppingBasket.Console Apple Apple Banana   ->   Total: £0.90
Console.OutputEncoding = Encoding.UTF8;

try
{
    var pence = new Checkout(Catalogue.Standard()).Total(args);
    Console.WriteLine($"Total: £{(pence / 100m).ToString("0.00", CultureInfo.InvariantCulture)}");
    return 0;
}
catch (UnknownItemException ex)
{
    Console.Error.WriteLine(ex.Message);
    return 1;
}
