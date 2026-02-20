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
using System.Security.Cryptography;
using System.Text;

namespace Mustard
{
	public readonly struct Seed : IEquatable<Seed>
	{
		private const string Charset = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const int DefaultLength = 20;

		public string Value { get; }
		public int Hash { get; }
		public int Length => Value.Length;

		private Seed(string value)
		{
			Value = value;
			Hash = ComputeHash(value);
		}

		/// <summary>
		/// デフォルト生成
		/// </summary>
		/// <returns></returns>
		public static Seed Create()
			=> Create(DefaultLength);

		/// <summary>
		/// 長さ指定生成
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
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

		/// <summary>
		/// 文字列からSeedを復元
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Seed Restore(string value)
		{
			Validate(value);
			return new Seed(value);
		}

		/// <summary>
		/// 整合性チェック
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentException"></exception>
		private static void Validate(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Seed cannot be null or empty.");

			if (value.Any(c => !Charset.Contains(c)))
				throw new ArgumentException("Seed contains invalid characters.");
		}

		/// <summary>
		/// Seed値からHashを算出
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static int ComputeHash(string value)
		{
			using var sha = SHA256.Create();
			var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
			return BitConverter.ToInt32(hash, 0);
		}

		public override string ToString() => Value;

		public bool Equals(Seed other) => Value == other.Value;
		public override bool Equals(object obj) => obj is Seed other && Equals(other);
		public override int GetHashCode() => Value.GetHashCode();

		public static bool operator ==(Seed left, Seed right) => left.Equals(right);
		public static bool operator !=(Seed left, Seed right) => !left.Equals(right);
	}
}