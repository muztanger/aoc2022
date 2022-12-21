namespace Advent_of_Code_2022;

public enum Op { Say, Add, Sub, Multiply, Divide }

[TestClass]
public class Day21
{

    public class Monkey
    {
        public string Name { get; set; }
        public Op Op { get; set; } = Op.Say;
        public List<Monkey> WaitFor = new List<Monkey>();
        public long Number { get; set; }

        internal long Yell()
        {
            switch (Op)
            {
                case Op.Say:
                    return Number;
                case Op.Add:
                    return WaitFor[0].Yell() + WaitFor[1].Yell();
                case Op.Sub:
                    return WaitFor[0].Yell() - WaitFor[1].Yell();
                case Op.Multiply:
                    return WaitFor[0].Yell() * WaitFor[1].Yell();
                case Op.Divide:
                    return WaitFor[0].Yell() / WaitFor[1].Yell();
                default:
                    Assert.Fail();
                    break;
            }
            return int.MinValue;
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var operations = new Dictionary<char, Op>
        {
            { '+', Op.Add },
            { '-', Op.Sub },
            { '*', Op.Multiply },
            { '/', Op.Divide }, };
        var result = new StringBuilder();
        var monkeys = new List<Monkey>();
        foreach (var line in input)
        {
            var split = line.Split();
            var name = split[0][..^1];
            if (split.Length == 4)
            {
                monkeys.Add(new Monkey { Name = name, Op = operations[split[2][0]] });
            }
            else if (split.Length == 2)
            {
                monkeys.Add(new Monkey { Name = name, Number = long.Parse(split[1])});
            }
            else
            {
                Assert.Fail();
            }
        }
        // add monkeys to monkeys
        foreach (var line in input)
        {
            var split = line.Split();
            if (split.Length != 4) continue;

            var name = split[0][..^1];
            var monkey = monkeys.First(m => m.Name == name);
            monkey.WaitFor.Add(monkeys.First(m => m.Name == split[1]));
            monkey.WaitFor.Add(monkeys.First(m => m.Name == split[3]));
            Assert.AreEqual(2, monkey.WaitFor.Count);
        }

        var root = monkeys.First(m => m.Name == "root");
        ;
        return root.Yell().ToString();
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
    public void Day21_Part1_Example01()
    {
        var input = """
            root: pppw + sjmn
            dbpl: 5
            cczh: sllz + lgvd
            zczc: 2
            ptdq: humn - dvpt
            dvpt: 3
            lfqf: 4
            humn: 5
            ljgn: 2
            sjmn: drzm * dbpl
            sllz: 4
            pppw: cczh / lfqf
            lgvd: ljgn * ptdq
            drzm: hmdt - zczc
            hmdt: 32
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day21_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day21_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day21)));
        Assert.AreNotEqual("1189209258", result);
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day21_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day21_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day21_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day21)));
        Assert.AreEqual("", result);
    }
    
}
