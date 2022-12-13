namespace Advent_of_Code_2022;


enum Type { Integer, List};

[TestClass]
public class Day13
{
    
    record Item(Type type, int X, List<int> List);

    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        
        var signals = new List<List<Item>>();
        var index = 0;
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line)) continue;

            var stack = new Stack<(List<Item>, Type)>();
            var list = new List<Item>();
            var val = new StringBuilder();
            var type = Type.List;
            foreach (var c in line)
            {
                switch (c)
                {
                    case '[':
                        stack.Push((list, type));
                        list = new List<Item>();
                        type = Type.List;
                        break;
                    case ']':
                        // end number or end list
                        if (type == Type.Integer)
                        {
                            list.Add(new Item(Type.Integer, int.Parse(val.ToString()), new()));
                            val.Clear();
                        }
                        signals.Add(list);
                        (list, type) = stack.Pop();
                        break;
                    case ',':
                        // end number or end list
                        if (type == Type.Integer)
                        {
                            list.Add(new Item(Type.Integer, int.Parse(val.ToString()), new()));
                            val.Clear();
                        }
                        break;
                    case char d when d >= '0' && d <= '9':
                        val.Append(c);
                        type = Type.Integer;
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }

            //signals[index % 2] = 

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
            <TODO>
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
