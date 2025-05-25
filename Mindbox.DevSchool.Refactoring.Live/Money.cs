namespace SmellyCodeRefined;

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