using System.ComponentModel;

namespace Advent_of_Code_2022;


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
        var result = new StringBuilder();
        
        var index = 0;
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line)) continue;

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
            if (current.Parent is ListItem parentWithList)
            {
                parentWithList.Items.Add(current);
            }

            //lists[index % 2] = 

            index++;    
        }
        return result.ToString();
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
            <TODO>
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
