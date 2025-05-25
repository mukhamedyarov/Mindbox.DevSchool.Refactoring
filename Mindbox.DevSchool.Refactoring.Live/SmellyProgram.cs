namespace OrderReporting;

internal class SmellyProgram
{
	private static void Main(string[] args)
	{
		var p = new P();
		p.P1();
	}
}

internal class P
{
	private List<string> orders = new();

	public void P1()
	{
		Console.WriteLine("Generating order report...");

		
		var raw = GetOrdersFromDb(); 
		foreach (var r in raw)
		{
			var parts = r.Split('-');
			if (parts.Length != 3)
				continue;

			var id = parts[0];
			var product = parts[1];
			var quantityString = parts[2];

			if (int.TryParse(quantityString, out var quantity))
			{
				if (!string.IsNullOrEmpty(id) && product.Length > 2 && quantity > 0 && !product.Contains("XYZ"))
				{
					if (product.ToLower().StartsWith("a"))
					{
						if (quantity < 100)
						{
							Console.WriteLine($"Order OK: {id} - {product} x{quantity}");
						}
						else
						{
							LogInvalid(id); 
						}
					}
					else
					{
						LogInvalid(id);
					}
				}
				else
				{
					LogInvalid(id);
				}
			}
		}

		Console.WriteLine("Report complete.");
	}

	private List<string> GetOrdersFromDb() =>
		new()
		{
			"O1-apple-5",
			"O2-avocado-105",
			"O3-banana-2",
			"O4-apple-0",
			"O5-XYZ-10"
		};

	private void LogInvalid(string id)
	{
		Console.WriteLine($"Invalid order: {id}");
	}
}