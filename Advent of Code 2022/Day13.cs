using System.ComponentModel;
using System.Security.Cryptography;
using System.Text.Json;

namespace Advent_of_Code_2022;

enum Order { Right, NotRight, Continue };

[TestClass]
public class Day13
{
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


    class JsonElementComparer : IComparer<JsonElement>
    {
        public int Compare(JsonElement x, JsonElement y)
        {
            var result = Day13.Compare(x, y);
            return result switch
            {
                Order.Continue => 0,
                Order.Right => -1,
                Order.NotRight => 1,
                _ => throw new NotImplementedException(),
            };
        }
    }

    private static string Part2(IEnumerable<string> input)
    {
        var list = new List<JsonElement>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            list.Add(JsonDocument.Parse(line).RootElement);       
        }

        const string divider1 = "[[2]]";
        list.Add(JsonDocument.Parse(divider1).RootElement);
        
        const string divider2 = "[[6]]";
        list.Add(JsonDocument.Parse(divider2).RootElement);

        list.Sort(new JsonElementComparer());
        var index = 1;
        var distressSignal = 1;
        foreach (var elem in list)
        {
            if (elem.GetRawText().Equals(divider1))
            {
                distressSignal *= index;
            }
            else if (elem.GetRawText().Equals(divider2))
            {
                distressSignal *= index;
            }
            index++;
        }

        return distressSignal.ToString();
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
        Assert.AreEqual("13", result);
    }
    
    [TestMethod]
    public void Day13_Part1_Example02()
    {
        var input = """
            [[[],[3,2,10]]]
            [[[],2,[],1,[4,1]],[3,[[8,4,0,7,8],4,2]],[[9,[2,1,8,2],[6,0,3,1,1]],4],[10,2,2]]
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("0", result);
    }
    
    [TestMethod]
    public void Day13_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day13)));
        Assert.AreEqual("5659", result);
    }
    
    [TestMethod]
    public void Day13_Part2_Example01()
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
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("140", result);
    }
    
    [TestMethod]
    public void Day13_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day13)));
        Assert.AreEqual("22110", result);
    }
    
}
