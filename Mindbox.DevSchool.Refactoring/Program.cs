namespace SmellyOrderApp;

public class Product
{

	public Product(string code, string description, decimal price)
	{
		if (string.IsNullOrEmpty(code)) throw new ArgumentException("Code required");
		if (price < 0) throw new ArgumentException("Price can't be negative");
		Code = code;
		Description = description;
		Price = price;
	}

	public string Code { get; }
	public string Description { get; }
	public decimal Price { get; }
}

public class OrderLine
{

	public OrderLine(Product product, int quantity)
	{
		if (quantity <= 0) throw new ArgumentException("Quantity must be > 0");
		Product = product;
		Quantity = quantity;
	}

	public Product Product { get; }
	public int Quantity { get; }
}

public class Order
{

	public Order(Guid id, DateTime createdAt, bool isPriority)
	{
		Id = id;
		CreatedAt = createdAt;
		IsPriority = isPriority;
		Lines = new List<OrderLine>();
	}

	public DateTime CreatedAt { get; }
	public Guid Id { get; }
	public bool IsPriority { get; }
	public List<OrderLine> Lines { get; }

	public void AddLine(OrderLine line)
	{
		Lines.Add(line);
	}
}

public class OrderService
{
	public decimal CalculateTotal(Order order)
	{
		decimal total = 0;

		if (order != null && order.Lines.Count > 0)
		{
			foreach (var line in order.Lines)
			{
				if (line.Quantity > 0)
				{
					if (line.Product != null)
					{
						total += line.Product.Price * line.Quantity;

						if (order.IsPriority)
						{
							if (line.Quantity > 10)
							{
								total -= total * 0.05m;
							}
						}
					}
				}
			}

			if (order.CreatedAt.DayOfWeek == DayOfWeek.Sunday || order.CreatedAt.DayOfWeek == DayOfWeek.Saturday)
			{
				if (total > 1000)
				{
					total *= 0.9m;
				}
			}
			else
			{
				if (total > 2000)
				{
					total *= 0.85m;
				}
			}
		}
		else
		{
			total = 0;
		}

		return total;
	}
}