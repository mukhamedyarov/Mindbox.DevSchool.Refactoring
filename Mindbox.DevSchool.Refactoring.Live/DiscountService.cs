namespace SmellyCodeRefined;

public static class DiscountService
{
	public static Discount CalculateDiscount(Customer customer, DateTime dateTimeUtcNow)
	{
		decimal discount = 0;

		if (CanCustomerGetDiscount(customer))
		{
			discount += CalculateDiscountBasedOnOrderHistory(customer, dateTimeUtcNow);
			discount += CalculatePersonalDiscount(customer);
		}

		if (CanCustomerGetPenalty(customer))
		{
			discount -= CalculatePenalty(customer);
		}

		return new Discount(discount);
	}

	private static decimal CalculateDiscountBasedOnOrderHistory(Customer customer, DateTime dateTimeUtcNow)
	{
		var ordersForLastYear = customer.Orders.Where(o => o.YoungerThanYear(dateTimeUtcNow));
		var highTotalOrders = ordersForLastYear.Where(o => o.Total.Amount > 100).ToArray();
		var totalsSumOfHighTotalOrders = highTotalOrders.Sum(o => o.Total.Amount);

		return highTotalOrders.Length > 5 && totalsSumOfHighTotalOrders > 1000
			? 0.1m
			: 0m;
	}

	private static decimal CalculatePenalty(Customer customer)
	{
		var customerHasLowRating = customer.Rating.Stars <= 2;
		if (customerHasLowRating)
			return 0.05m;

		var firstTwoOrdersAreCheap = customer.Orders.Count >= 2
			&& customer.Orders[0].Total.Amount < 50
			&& customer.Orders[1].Total.Amount < 50;

		if (firstTwoOrdersAreCheap)
			return 0.02m;

		return 0;
	}

	private static decimal CalculatePersonalDiscount(Customer customer)
	{
		var isVCustomer = customer.Name.StartsWith("V");
		var canVCustomerGetDiscount = customer.Orders.Count > 10 && customer.Rating.Stars == 5;

		var isTestCustomer = customer.Name.Contains("test");
		var canNotTestCustomerGetDiscount = customer.Orders.Count > 20 && customer.IsActive && customer.Rating.Stars >= 4;

		return isVCustomer && canVCustomerGetDiscount || !isTestCustomer && canNotTestCustomerGetDiscount
			? 0.15m
			: 0m;
	}

	private static bool CanCustomerGetDiscount(Customer customer) =>
		customer.IsActive && customer.Rating.Stars > 3 && customer.Orders.Count > 0;

	private static bool CanCustomerGetPenalty(Customer customer) => customer.Orders.Count < 3;
}