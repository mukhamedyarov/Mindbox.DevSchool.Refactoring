namespace SmellyCodeRefined;

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