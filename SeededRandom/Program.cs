// See https://aka.ms/new-console-template for more information

var bag = new Mustard.ShuffleBag(Mustard.Seed.Restore("Hello"));

for (int i = 0; i < 5; i++)
{
	Console.WriteLine(bag.Next());
}