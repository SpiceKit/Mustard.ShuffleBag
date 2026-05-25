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

namespace Mustard
{
	public class SeededRandom
	{
		private readonly int _min;
		private readonly int _max;

		private List<int> _values;
		private Random _random;
		private int _index;

		public SeededRandom(Seed seed, int min = 1, int max = 100)
		{
			if (min >= max)
				throw new ArgumentException("min must be < max");

			_min = min;
			_max = max;

			_random = new Random(seed.Hash);
			_values = Enumerable.Range(_min, _max - _min + 1).ToList();

			Reshuffle();
		}

		public int Next()
		{
			if (_index >= _values.Count)
			{
				Reshuffle();
			}

			return _values[_index++];
		}

		public void Reset(Seed seed)
		{
			_random = new Random(seed.Hash);
			_values = Enumerable.Range(_min, _max - _min + 1).ToList();

			Reshuffle();
		}

		private void Reshuffle()
		{
			// Fisher–Yates shuffle を使用
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
}
