# Shuffle Bag

A deterministic shuffle bag implementation for C# based on the Fisher–Yates shuffle algorithm.

`ShuffleBag` generates values in a randomized order without repetition until every value in the range has been used. The sequence is fully reproducible using a `Seed`, making it suitable for gameplay systems, procedural generation, testing, and simulations.

## Features

* Deterministic random sequence using a seed
* No duplicates until the entire range has been exhausted
* Uses the Fisher–Yates shuffle algorithm
* Supports custom ranges
* Seed strings can be saved and restored

## Example

```csharp
var seed = Seed.Create(20);

var bag = new ShuffleBag(seed, 1, 10);

for (int i = 0; i < 10; i++)
{
    Console.WriteLine(bag.Next());
}
```

Output:

```text
4
9
1
7
3
10
2
5
8
6
```

Every value appears exactly once before the bag is automatically reshuffled using Fisher–Yates.

## Reproducible Results

```csharp
var seed = Seed.Create(20);

var bagA = new ShuffleBag(seed);
var bagB = new ShuffleBag(seed);
```

Both instances will produce the same sequence.

## Save and Restore

```csharp
var seed = Seed.Create(20);

// Save
string value = seed.Value;

// Restore
var restored = Seed.Restore(value);

var bag = new ShuffleBag(restored);
```

## Range

The default range is `1..100`.

```csharp
var bag = new ShuffleBag(seed, 1, 6);
```

This behaves like a shuffled dice bag, producing each value from 1 to 6 exactly once before repeating.

## Use Cases

* Roguelike games
* Procedural generation
* Deterministic gameplay systems
* Testing and debugging
* Random selection without repetition

## License

zlib/libpng License
