
/// <summary>
/// Fisher–Yates shuffle を使用したランダムプール
/// </summary>
public class SeededRandomPool
{
	private readonly int _min;
	private readonly int _max;

	private List<int> _values;
	private Random _random;
	private int _index;

	public SeededRandomPool(int min, int max, int seed)
	{
		if (min > max)
			throw new ArgumentException("min must be <= max");

		_min = min;
		_max = max;

		Initialize(seed);
	}

	public int Next()
	{
		if (_index >= _values.Count)
		{
			Reshuffle();
		}

		return _values[_index++];
	}

	public void Reset(int seed) => Initialize(seed);

	private void Initialize(int seed)
	{
		_random = new Random(seed);
		_values = new List<int>(_max - _min + 1);

		for (int i = _min; i <= _max; i++)
		{
			_values.Add(i);
		}

		Reshuffle();
	}

	private void Reshuffle()
	{
		for (int i = _values.Count - 1; i > 0; i--)
		{
			int j = _random.Next(i + 1);

			int temp = _values[i];
			_values[i] = _values[j];
			_values[j] = temp;
		}

		_index = 0;
	}
}
