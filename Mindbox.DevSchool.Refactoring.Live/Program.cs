namespace OrderReporting;

internal class SmellyProgram
{
	private static void Main(string[] args)
	{
		var p = new P();
		p.P1(); // 4. Непонятные имена
	}
}

internal class P
{
	private List<string> orders = new();

	public void P1()
	{
		Console.WriteLine("Generating order report...");

		// 1. Длинный метод
		var raw = GetOrdersFromDb(); // 2. Высокий уровень абстракции в низкоуровневом методе
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
				// 6. Сложное условие
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
							LogInvalid(id); // 3. Метод работает корректно только в контексте вызова из P1
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
		// Эмуляция источника данных
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