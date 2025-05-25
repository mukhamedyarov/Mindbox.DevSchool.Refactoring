namespace SmellyCodeRefined;

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