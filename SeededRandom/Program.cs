// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

var pool = new SeededRandomPool(1, 100, seed: 12345);

for (int i = 0; i < 5; i++)
{
	Console.WriteLine(pool.Next());
}