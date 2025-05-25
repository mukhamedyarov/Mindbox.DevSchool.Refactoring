namespace SmellyCodeRefined;

public class Rating
{

	public Rating(int stars)
	{
		if (stars is < 0 or > 5)
			throw new ArgumentException("Rating must be between 0 and 5");
		Stars = stars;
	}

	public int Stars { get; }
}

public class Money
{

	public Money(decimal amount)
	{
		if (amount < 0)
			throw new ArgumentException("Money can't be negative");
		Amount = amount;
	}

	public decimal Amount { get; }
}

public class Discount
{

	public Discount(decimal percentage)
	{
		if (percentage < -0.5m || percentage > 1.0m)
			throw new ArgumentException("Discount must be between -50% and 100%");
		Percentage = percentage;
	}

	public decimal Percentage { get; }

	public override string ToString() => $"{Percentage:P}";
}

public class Customer
{

	public Customer(Guid id, string name, Rating rating, bool isActive)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Name cannot be empty");
		Id = id;
		Name = name;
		Rating = rating;
		Orders = new List<Order>();
		IsActive = isActive;
	}

	public Guid Id { get; }
	public bool IsActive { get; }
	public string Name { get; }
	public List<Order> Orders { get; }
	public Rating Rating { get; }

	public void AddOrder(Order order)
	{
		if (order == null) throw new ArgumentNullException(nameof(order));
		Orders.Add(order);
	}
}

public class Order
{

	public Order(Guid id, DateTime createdAt, Money total)
	{
		if (total == null) throw new ArgumentNullException(nameof(total));
		Id = id;
		CreatedAt = createdAt;
		Total = total;
	}

	public DateTime CreatedAt { get; }
	public Guid Id { get; }
	public Money Total { get; }

	public bool YoungerThanYear(DateTime dateTimeUtcNow) => (dateTimeUtcNow - CreatedAt).TotalDays < 365;
}

public static class DiscountService
{
	public static Discount CalculateDiscount(Customer customer, DateTime dateTimeUtcNow)
	{
		decimal d = 0;

		if (CanCustomerGetDiscount(customer))
		{
			d += CalculateDiscountBasedOnOrderHistory(customer, dateTimeUtcNow);
			d += CalculatePersonalDiscount(customer);
		}

		if (CanCustomerGetPenalty(customer))
		{
			if (customer.Rating.Stars <= 2)
			{
				d -= 0.05m;
			}
			else if (customer.Orders.Count == 2)
			{
				if (customer.Orders[0].Total.Amount < 50 && customer.Orders[1].Total.Amount < 50)
				{
					d -= 0.02m;
				}
			}
		}

		return new Discount(d);
	}

	private static decimal CalculatePersonalDiscount(Customer customer)
	{
		return customer.Name.StartsWith("V") && customer.Orders.Count > 10 && customer.Rating.Stars == 5 ||
			customer.Orders.Count > 20 && !customer.Name.Contains("test") && customer.IsActive && customer.Rating.Stars >= 4
				? 0.15m
				: 0m;
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

	private static bool CanCustomerGetPenalty(Customer customer) => customer.Orders.Count < 3;

	private static bool CanCustomerGetDiscount(Customer customer) => customer.IsActive && customer.Rating.Stars > 3 && customer.Orders.Count > 0;
}

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