# ShuffleBag

Fisher–Yates Shuffle アルゴリズムを使用した、C#向けの再現可能なシャッフルバッグ実装です。

`ShuffleBag` は、指定された範囲の値をランダムな順序で生成しますが、全ての値が出現するまでは同じ値が重複しません。また、`Seed` を使用することで生成順序を完全に再現できます。

ゲームロジック、プロシージャル生成、テスト、シミュレーションなど、再現性が求められる用途に適しています。

## 特徴

* シード値による再現可能な乱数列
* 全ての値が出現するまで重複しない
* Fisher–Yates Shuffle アルゴリズムを使用
* 任意の範囲を指定可能
* シード文字列の保存・復元に対応

## 使用例

```csharp id="i4u3ok"
var seed = Seed.Create(20);

var bag = new ShuffleBag(seed, 1, 10);

for (int i = 0; i < 10; i++)
{
    Console.WriteLine(bag.Next());
}
```

出力例:

```text id="yl4l1x"
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

全ての値が一度ずつ出現した後、Fisher–Yates Shuffle によって自動的に再シャッフルされます。

## 再現可能な結果

```csharp id="c55tvf"
var seed = Seed.Create(20);

var bagA = new ShuffleBag(seed);
var bagB = new ShuffleBag(seed);
```

同じシードを使用したインスタンスは、同じ順序の値を生成します。

## シードの保存と復元

```csharp id="pn3p8h"
var seed = Seed.Create(20);

// 保存
string value = seed.Value;

// 復元
var restored = Seed.Restore(value);

var bag = new ShuffleBag(restored);
```

## 範囲指定

デフォルトでは `1～100` の値を使用します。

```csharp id="4ghf0y"
var bag = new ShuffleBag(seed, 1, 6);
```

この場合、1～6 の値が一度ずつ出現した後に再シャッフルされます。

## 主な用途

* ローグライクゲーム
* プロシージャル生成
* 再現性が必要なゲームシステム
* テストやデバッグ
* 重複を避けたいランダム選択

## ライセンス

zlib/libpng License
