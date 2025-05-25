using FluentAssertions;

using SmellyCodeRefined;

namespace SmellyCodeTests;

public class DiscountServiceTests
{
	[Fact]
	public void Should_ApplyHighOrderDiscount_When_ManyLargeRecentOrders()
	{
		var orders = new List<(DateTime, decimal)>
		{
			(DateTime.Now.AddDays(-30), 200),
			(DateTime.Now.AddDays(-60), 200),
			(DateTime.Now.AddDays(-90), 200),
			(DateTime.Now.AddDays(-120), 200),
			(DateTime.Now.AddDays(-150), 200),
			(DateTime.Now.AddDays(-180), 200)
		};
		var customer = CreateCustomer("Alice", 4, true, orders);

		var discount = DiscountService.DoStuff(customer, DateTime.UtcNow);

		discount.Percentage.Should().Be(0.10m);
	}

	[Fact]
	public void Should_ApplyNameAndRatingBonusDiscount_When_CustomerNameStartsWithVAndHasManyOrders()
	{
		var orders = new List<(DateTime, decimal)>();
		for (var i = 0; i < 11; i++)
		{
			orders.Add((DateTime.Now.AddDays(-i * 30), 150));
		}

		var customer = CreateCustomer("Victor", 5, true, orders);

		var discount = DiscountService.DoStuff(customer, DateTime.UtcNow);

		discount.Percentage.Should().BeGreaterThan(0.10m);
		discount.Percentage.Should().BeApproximately(0.25m, 0.001m);
	}

	[Fact]
	public void Should_NotCrash_When_NoOrders()
	{
		var customer = CreateCustomer("Test", 5, true, new List<(DateTime, decimal)>());

		var discount = DiscountService.DoStuff(customer, DateTime.UtcNow);

		discount.Percentage.Should().Be(0m);
	}

	[Fact]
	public void Should_PenalizeLowActivityLowRatingCustomer()
	{
		var orders = new List<(DateTime, decimal)>
		{
			(DateTime.Now.AddDays(-10), 30)
		};
		var customer = CreateCustomer("LowGuy", 1, true, orders);

		var discount = DiscountService.DoStuff(customer, DateTime.UtcNow);

		discount.Percentage.Should().BeLessThan(0m); // Negative discount (penalty)
	}

	[Fact]
	public void Should_ReturnZeroDiscount_When_CustomerIsInactive()
	{
		var customer = CreateCustomer("Bob", 5, false, new List<(DateTime, decimal)>());

		var discount = DiscountService.DoStuff(customer, DateTime.UtcNow);

		discount.Percentage.Should().Be(0m);
	}

	private Customer CreateCustomer(
		string name,
		int rating,
		bool isActive,
		List<(DateTime, decimal)> orderData)
	{
		var customer = new Customer(Guid.NewGuid(), name, new Rating(rating), isActive);
		foreach (var (date, amount) in orderData)
		{
			customer.AddOrder(new Order(Guid.NewGuid(), date, new Money(amount)));
		}
		return customer;
	}
}