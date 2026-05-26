// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

//var pool = new SeededRandomPool(1, 100, seed: 12345);

var bag = new Mustard.ShuffleBag(Mustard.Seed.Restore("Hello"));

for (int i = 0; i < 5; i++)
{
	Console.WriteLine(bag.Next());
}