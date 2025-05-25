namespace SmellyCodeRefined;

public static class DiscountService
{
	public static Discount CalculateDiscount(Customer customer, DateTime dateTimeUtcNow)
	{
		var discount = GetDiscount(customer, dateTimeUtcNow);
		var penalty = GetPenalty(customer);

		return new Discount(discount - penalty);
	}

	private static decimal CalculateDiscountBasedOnOrderHistory(Customer customer, DateTime dateTimeUtcNow)
	{
		var ordersForLastYear = customer.Orders.Where(o => o.YoungerThanYear(dateTimeUtcNow));
		var expensiveOrders = ordersForLastYear.Where(o => o.Total.Amount > 100).ToArray();
		var expensiveOrdersTotalSum = expensiveOrders.Sum(o => o.Total.Amount);

		return expensiveOrders.Length > 5 && expensiveOrdersTotalSum > 1000
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

	private static decimal GetDiscount(Customer customer, DateTime dateTimeUtcNow)
	{
		if (!CanCustomerGetDiscount(customer))
			return 0m;

		var orderHistoryBasedDiscount = CalculateDiscountBasedOnOrderHistory(customer, dateTimeUtcNow);
		var personalDiscount = CalculatePersonalDiscount(customer);

		return orderHistoryBasedDiscount + personalDiscount;
	}

	private static decimal GetPenalty(Customer customer)
	{
		if (!CanCustomerGetPenalty(customer))
			return 0;

		return CalculatePenalty(customer);
	}
}