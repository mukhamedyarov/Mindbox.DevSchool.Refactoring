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