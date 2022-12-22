namespace Advent_of_Code_2022;

public enum Op { Say, Add, Sub, Multiply, Divide, Equals }

[TestClass]
public class Day21
{

    public class Monkey
    {
        public string Name { get; set; }

        public bool IsHuman { get; set; } = false;
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

        private long Eval(long humn, ref bool? isEqual, ref long? x1, ref long? x2)
        {
            switch (Op)
            {
                case Op.Equals:
                    x1 = GetWaitFor(0, ref isEqual, ref x1, ref x2);
                    x2 = GetWaitFor(1, ref isEqual, ref x1, ref x2);
                    isEqual = x1 == x2;
                    // skicka upp svaren och använd någon slags derivata för att pricka svaret!
                    return humn;
                case Op.Say:
                    return Number;
                case Op.Add:
                    return GetWaitFor(0, ref isEqual, ref x1, ref x2) + GetWaitFor(1, ref isEqual, ref x1, ref x2);
                case Op.Sub:
                    return GetWaitFor(0, ref isEqual, ref x1, ref x2) - GetWaitFor(1, ref isEqual, ref x1, ref x2);
                case Op.Multiply:
                    return GetWaitFor(0, ref isEqual, ref x1, ref x2) * GetWaitFor(1, ref isEqual, ref x1, ref x2);
                case Op.Divide:
                    return GetWaitFor(0, ref isEqual, ref x1, ref x2) / GetWaitFor(1, ref isEqual, ref x1, ref x2);
                default:
                    Assert.Fail();
                    break;
            }

            long GetWaitFor(int index, ref bool? isEqual, ref long? x1, ref long? x2)
            {
                return WaitFor[index].IsHuman ? humn : WaitFor[index].Eval(humn, ref isEqual, ref x1, ref x2);
            }

            return humn;
        }

        public bool Equals(long humn, out long result, ref long? x1, ref long? x2)
        {
            bool? isEqual = null;
            result = Eval(humn, ref isEqual, ref x1, ref x2);
            Assert.IsNotNull(isEqual);
            return isEqual.Value;
        }

    }

    private static string Part1(IEnumerable<string> input)
    {
        var operations = new Dictionary<char, Op>
        {
            { '+', Op.Add },
            { '-', Op.Sub },
            { '*', Op.Multiply },
            { '/', Op.Divide },
            { '=', Op.Equals },
        };
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
        return root.Yell().ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var operations = new Dictionary<char, Op>
        {
            { '+', Op.Add },
            { '-', Op.Sub },
            { '*', Op.Multiply },
            { '/', Op.Divide }, 
            { '=', Op.Equals },
        };
        var monkeys = new List<Monkey>();
        foreach (var line in input)
        {
            var split = line.Split();
            var name = split[0][..^1];
            if (split.Length == 4)
            {
                monkeys.Add(new Monkey { Name = name, Op = "root".Equals(name) ? Op.Equals : operations[split[2][0]], IsHuman = name.Equals("humn") });
            }
            else if (split.Length == 2)
            {
                monkeys.Add(new Monkey { Name = name, Number = long.Parse(split[1]), IsHuman = name.Equals("humn") });
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

        var start = monkeys.First(m => m.Name == "root");
        int i = 0;
        long humn = 1;

        for (humn = 3759566892644 - 10; humn <= 3759566892644 + 10; humn++)
        {
            long? x1 = 0L;
            long? x2 = 0L;
            var isEqual = start.Equals(humn, out var result, ref x1, ref x2);
            if (isEqual)
            {
                return result.ToString();
            }
        }

        // TODO Make this algorithm work for small difference
        var results = new List<(long, long, long)>();
        while (i++ < 100)
        {
            long? x1 = 0L;
            long? x2 = 0L;
            var isEqual = start.Equals(humn, out var result, ref x1, ref x2);
            if (isEqual)
            {
                return result.ToString();
            }
            else
            {
                Assert.IsTrue(x1.HasValue);
                Assert.IsTrue(x2.HasValue);
                results.Add((humn, x1.Value, x2.Value));

                // f(h1) = y1
                // f(h2) = y2
                // 
                // dh = h2 - h1
                // dy = y2 - y1
                // 
                // dh * c = dy
                // c = dy / dh
                //
                // y = 0 ?
                // y2 + dy = 0
                // y2 + dh * c = 0
                // y2 = - dh * c
                // 
                // dh = -y2 / c
                // dh3 = -y2 / (dy / dh) = - y2 * dh / dy

                if (results.Count > 1)
                {
                    var y1 = results[^2].Item2 - results[^2].Item3;
                    var y2 = results[^1].Item2 - results[^1].Item3;
                    var dh = results[^1].Item1 - results[^2].Item1;
                    long den = y2 - y1;
                    if (den != 0)
                    {
                        long dh2 = -y2 * dh / den;
                        humn += dh2;
                        Console.WriteLine($"y1={y1} y2={y2} dh={dh} dh2={dh2} humn2={humn}");
                    }
                    else
                    {
                        humn += 1;
                    }
                }
                else if (results.Count > 0)
                {
                    long dx = x2.Value - x1.Value;
                    humn += dx;
                }
                else
                {
                    humn++;
                }

            }
        }
        Console.WriteLine(string.Join(", ", results));
        return "failure";
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
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("301", result);
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
