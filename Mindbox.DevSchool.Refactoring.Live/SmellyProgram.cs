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
}

public class DiscountService
{
	public Discount DoStuff(Customer c, DateTime dateTimeUtcNow)
	{
		decimal d = 0;
		if (c.IsActive && c.Rating.Stars > 3 && c.Orders.Count > 0)
		{
			var x = 0;
			var y = 0m;
			foreach (var o in c.Orders)
			{
				if ((dateTimeUtcNow - o.CreatedAt).TotalDays < 365)
				{
					if (o.Total.Amount > 100)
					{
						x++;
						y += o.Total.Amount;
					}
				}
			}

			if (x > 5 && y > 1000)
			{
				d += 0.1m;
			}

			if (c.Name.StartsWith("V") && c.Orders.Count > 10 && c.Rating.Stars == 5 ||
			    c.Orders.Count > 20 && !c.Name.Contains("test") && c.IsActive && c.Rating.Stars >= 4)
			{
				d += 0.15m;
			}
			else
			{
				if (c.Orders.Count < 3)
				{
					if (c.Rating.Stars <= 2)
					{
						d -= 0.05m;
					}
					else
					{
						if (c.Orders.Count == 2)
						{
							if (c.Orders[0].Total.Amount < 50 && c.Orders[1].Total.Amount < 50)
							{
								d -= 0.02m;
							}
						}
					}
				}
			}
		}

		return new Discount(d);
	}
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

		var discountService = new DiscountService();
		var discount = discountService.DoStuff(customer, DateTime.UtcNow);

		Console.WriteLine($"Discount: {discount}");
	}
}