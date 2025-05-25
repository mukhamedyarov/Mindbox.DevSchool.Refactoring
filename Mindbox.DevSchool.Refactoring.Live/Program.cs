namespace SmellyCodeRefined;

internal class Program
{
	private static void Main()
	{
		var customer = new Customer(Guid.NewGuid(), "Victor", new Rating(5), true);
		customer.AddOrder(new Order(Guid.NewGuid(), DateTime.Now.AddMonths(-1), new Money(200)));
		customer.AddOrder(new Order(Guid.NewGuid(), DateTime.Now.AddMonths(-2), new Money(300)));
		customer.AddOrder(new Order(Guid.NewGuid(), DateTime.Now.AddMonths(-3), new Money(250)));
		customer.AddOrder(new Order(Guid.NewGuid(), DateTime.Now.AddMonths(-4), new Money(150)));
		customer.AddOrder(new Order(Guid.NewGuid(), DateTime.Now.AddMonths(-5), new Money(120)));
		customer.AddOrder(new Order(Guid.NewGuid(), DateTime.Now.AddMonths(-6), new Money(100)));
		customer.AddOrder(new Order(Guid.NewGuid(), DateTime.Now.AddMonths(-7), new Money(200)));

		var discount = DiscountService.CalculateDiscount(customer, DateTime.UtcNow);

		Console.WriteLine($"Discount: {discount}");
	}
}