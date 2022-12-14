using System.ComponentModel;
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
        var pair = new Item[2];
        var index = 0;
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line)) continue;

            var moop = JsonDocument.Parse(line); //TODO

            var lists = new List<Item>();
            Item root = new ListItem();
            var current = root;
            var val = new StringBuilder();
            var stack = new Stack<Item>();
            foreach (var c in line)
            {
                
                Assert.IsTrue(current != null);
                switch (c)
                {
                    case '[':
                        current = new ListItem() { Parent = current };
                        break;
                    case ']':
                        {
                            if (current is IntItem integer)
                            {
                                integer.X = int.Parse(val.ToString());
                                val.Clear();
                            }
                            if (current.Parent is ListItem list)
                            {
                                list.Items.Add(current);
                            }
                            current = current.Parent;
                        }
                        break;
                    case ',':
                        {
                            if (current is IntItem integer)
                            {
                                integer.X = int.Parse(val.ToString());
                                val.Clear();
                            }
                            if (current.Parent is ListItem list)
                            {
                                list.Items.Add(current);
                            }
                            current = current.Parent;
                        }
                        break;
                    case char d when d >= '0' && d <= '9':
                        val.Append(c);
                        if (current is not IntItem)
                        {
                            current = new IntItem() { Parent = current };
                        }
                        break;
                    default:
                        Assert.Fail(c.ToString());
                        break;
                }
            }
            root = current;

            pair[index % 2] = root;

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

    private static Order Compare(Item c1, Item c2)
    {
        if (c1 is IntItem i1 && c2 is IntItem i2)
        {
            if (i1.X > i2.X)
            {
                return Order.NotRight;
            }
            else if (i1.X < i2.X)
            {
                return Order.Right;
            }
            else
            {
                return Order.Continue;
            }
        }
        else if (c1 is IntItem && c2 is ListItem)
        {
            var l1 = new ListItem { Items = new() { c1 } };
            Order compare = Compare(l1, c2);
            if (compare != Order.Continue)
            {
                return compare;
            }
        }
        else if (c1 is ListItem && c2 is IntItem)
        {
            var l2 = new ListItem { Items = new() { c2 } };
            Order compare = Compare(c1, l2);
            if (compare != Order.Continue)
            {
                return compare;
            }
        }
        else if (c1 is ListItem l1 && c2 is ListItem l2)
        {
            for (int i = 0; i < Math.Min(l1.Items.Count, l2.Items.Count); i++)
            {
                var compare = Compare(l1.Items[i], l2.Items[i]);
                if (compare != Order.Continue)
                {
                    return compare;
                }
            }
            if (l1.Items.Count > l2.Items.Count)
            {
                return Order.NotRight;
            }
            else if (l1.Items.Count < l2.Items.Count)
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
