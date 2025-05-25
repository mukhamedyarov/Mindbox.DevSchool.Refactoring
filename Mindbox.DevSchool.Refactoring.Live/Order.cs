namespace SmellyCodeRefined;

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