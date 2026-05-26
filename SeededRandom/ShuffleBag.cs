// zlib/libpng License
//
// Copyright (c) 2025 RabitBox
//
// This software is provided 'as-is', without any express or implied warranty.
// In no event will the authors be held liable for any damages arising from the use of this software.
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it freely,
// subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software.
//    If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Mustard
{
	public class ShuffleBag
	{
		private readonly int _min;
		private readonly int _max;

		private List<int>	_values;
		private int			_index;
		private Random		_random;
		private Seed		_seed;

		public Seed Current => _seed;

		public ShuffleBag(Seed seed, int min = 1, int max = 100)
		{
			if(min >= max) 
				throw new ArgumentException("min must be < max");

			_min	= min;
			_max	= max;
			_values = Enumerable.Range(_min, _max - _min + 1).ToList();
			_index	= 0;
			_random = new Random(seed.Hash);
			_seed	= seed;

			Reshuffle();
		}

		public int Next()
		{
			if (_index >= _values.Count) Reshuffle();
			return _values[_index++];
		}

		public void Reset(Seed seed)
		{
			_values = Enumerable.Range(_min, _max - _min + 1).ToList();
			_index	= 0;
			_random = new Random(seed.Hash);
			_seed	= seed;

			Reshuffle();
		}

		private void Reshuffle()
		{
			// Fisher–Yates shuffle を使用
			for (int i = _values.Count - 1; i > 0; i--)
			{
				int j = _random.Next(i + 1);

				int tmp = _values[i];
				_values[i] = _values[j];
				_values[j] = tmp;
			}

			_index = 0;
		}
	}

	public readonly struct Seed : IEquatable<Seed>
	{
		private const string Charset = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const int DefaultLength = 20;

		public string Value { get; }
		public int Hash { get; }
		public static Seed Default => Create(DefaultLength);

		private Seed(string value)
		{
			Value = value;
			Hash = ComputeHash(value);
		}

		public static Seed Create(int length)
		{
			if (length <= 0)
				throw new ArgumentOutOfRangeException(nameof(length));

			var buffer = new byte[length];
			RandomNumberGenerator.Fill(buffer);

			var sb = new StringBuilder(length);
			for (int i = 0; i < length; i++)
			{
				int index = buffer[i] % Charset.Length;
				sb.Append(Charset[index]);
			}

			return new Seed(sb.ToString());
		}

		public static Seed Restore(string value)
		{
			Validate(value);
			return new Seed(value);
		}

		private static void Validate(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Seed cannot be null or empty.");

			if (value.Any(c => !Charset.Contains(c)))
				throw new ArgumentException("Seed contains invalid characters.");
		}

		private static int ComputeHash(string value)
		{
			using var sha = SHA256.Create();
			var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
			return BitConverter.ToInt32(hash, 0);
		}

		#region override
		public override string ToString() => Value;
		public bool Equals(Seed other) => Value == other.Value;
		public override bool Equals(object obj) => obj is Seed other && Equals(other);
		public override int GetHashCode() => Value.GetHashCode();
		public static bool operator ==(Seed left, Seed right) => left.Equals(right);
		public static bool operator !=(Seed left, Seed right) => !left.Equals(right);
		#endregion
	}
}
