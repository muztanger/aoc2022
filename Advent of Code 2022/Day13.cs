using System.ComponentModel;
using System.Security.Cryptography;
using System.Text.Json;

namespace Advent_of_Code_2022;

enum Order { Right, NotRight, Continue };

[TestClass]
public class Day13
{
    
    public class Item
    {
        public Item? Parent { get; set; } = null;

        public Item(Item? parent = null)
        {
            Parent = parent;
        }
    }

    public class IntItem : Item
    {
        public int X { get; set; } = -1;
        public IntItem() { }
        public override string ToString()
        {
            return X.ToString();
        }
    }

    public class ListItem : Item
    {
        public List<Item> Items { get; set; } = new();
        public ListItem() { }

        public override string ToString()
        {
            return $"[{string.Join(", ", Items)}]";
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var result = new List<int>();
        var pair = new JsonElement[2];
        var index = 0;
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line)) continue;

            pair[index % 2] = JsonDocument.Parse(line).RootElement;

            if (index > 0 && index % 2 == 1)
            {
                // Compare!
                Console.WriteLine($"== Pair {index / 2 + 1} ==");
                Order value = Compare(pair[0], pair[1]);
                Console.WriteLine(value);
                if (value == Order.Right)
                {
                    result.Add(index / 2 + 1);
                }
            }

            index++;    
        }
        return result.Sum().ToString();
    }

    private static Order Compare(JsonElement c1, JsonElement c2)
    {
        if (c1.ValueKind == JsonValueKind.Number && c2.ValueKind == JsonValueKind.Number)
        {
            if (c1.GetInt32() > c2.GetInt32())
            {
                return Order.NotRight;
            }
            else if (c1.GetInt32() < c2.GetInt32())
            {
                return Order.Right;
            }
            else
            {
                return Order.Continue;
            }
        }
        else if (c1.ValueKind == JsonValueKind.Number && c2.ValueKind == JsonValueKind.Array)
        {
            var a1 = new int[1] { c1.GetInt32() };
            var l1 = JsonSerializer.SerializeToElement(a1, typeof(int[]));

            Order compare = Compare(l1, c2);
            if (compare != Order.Continue)
            {
                return compare;
            }
        }
        else if (c1.ValueKind == JsonValueKind.Array && c2.ValueKind == JsonValueKind.Number)
        {
            var a2 = new int[1] {c2.GetInt32()};
            var l2 = JsonSerializer.SerializeToElement(a2, typeof(int[]));
            Order compare = Compare(c1, l2);
            if (compare != Order.Continue)
            {
                return compare;
            }
        }
        else if (c1.ValueKind == JsonValueKind.Array && c2.ValueKind == JsonValueKind.Array)
        {
            int minLength = Math.Min(c1.GetArrayLength(), c2.GetArrayLength());
            for (int i = 0; i < minLength; i++)
            {
                var compare = Compare(c1[i], c2[i]);
                if (compare != Order.Continue)
                {
                    return compare;
                }
            }
            if (c1.GetArrayLength() > c2.GetArrayLength())
            {
                return Order.NotRight;
            }
            else if (c1.GetArrayLength() < c2.GetArrayLength())
            {
                return Order.Right;
            }
        }
        return Order.Continue;
    }

    private static string Part2(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        foreach (var line in input)
        {
        }
        return result.ToString();
    }
    
    [TestMethod]
    public void Day13_Part1_Example01()
    {
        var input = """
            [1,1,3,1,1]
            [1,1,5,1,1]

            [[1],[2,3,4]]
            [[1],4]

            [9]
            [[8,7,6]]

            [[4,4],4,4]
            [[4,4],4,4,4]

            [7,7,7,7]
            [7,7,7]

            []
            [3]

            [[[]]]
            [[]]

            [1,[2,[3,[4,[5,6,7]]]],8,9]
            [1,[2,[3,[4,[5,6,0]]]],8,9]
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day13_Part1_Example02()
    {
        var input = """
            [[[],[3,2,10]]]
            [[[],2,[],1,[4,1]],[3,[[8,4,0,7,8],4,2]],[[9,[2,1,8,2],[6,0,3,1,1]],4],[10,2,2]]
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day13_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day13)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day13_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day13_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day13_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day13)));
        Assert.AreEqual("", result);
    }
    
}
