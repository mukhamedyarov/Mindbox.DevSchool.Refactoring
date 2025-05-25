namespace Mindbox.DevSchool.Refactoring;

internal class Program
{
	public static async Task Main(string[] args)
	{
		var users = await GetUsersAsync();

		await File.WriteAllTextAsync("users.txt", string.Join('\n', users), CancellationToken.None);

		var x = new X();
		x.Do();
	}

	private static Task<string[]> GetUsersAsync()
	{
		return Task.FromResult(new[]
		{
			"Alice,22",
			"admin,30",
			"Andrew,19",
			"bob,17",
			"Anna,21",
			"Axel,25",
			"Axyz,26",
			"   ,20",
			"Aden,18",
			"Alex,abc"
		});
	}
}

internal class X
{
	private List<string> data = new();

	public void Do()
	{
		Console.WriteLine("Processing users...");

		var raw = File.ReadAllText("users.txt");
		var lines = raw.Split('\n');

		foreach (var line in lines)
		{
			var parts = line.Split(',');

			if (parts.Length < 2)
				continue;

			var name = parts[0];
			int age;
			if (!int.TryParse(parts[1], out age))
				continue;

			if (!string.IsNullOrWhiteSpace(name))
			{
				if (char.IsUpper(name[0]))
				{
					if (age > 18)
					{
						if (name.StartsWith("A") && name.Length > 3 && !name.Contains("xyz") && age % 2 == 0 &&
						    name.ToLower() != "admin")
						{
							Console.WriteLine($"Sending email to valid user: {name}, age: {age}...");
						}
						else
						{
							NotifyAdminAboutInvalidUser(name);
						}
					}
					else
					{
						NotifyAdminAboutInvalidUser(name);
					}
				}
				else
				{
					NotifyAdminAboutInvalidUser(name);
				}
			}
			else
			{
				NotifyAdminAboutInvalidUser(name);
			}
		}

		Console.WriteLine("Done.");
	}

	private void NotifyAdminAboutInvalidUser(string name)
	{
		Console.WriteLine($"[ALERT] Admin notified about: {name}");
	}
}