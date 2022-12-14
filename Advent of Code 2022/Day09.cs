namespace Advent_of_Code_2022;

[TestClass]
public class Day09
{
    private static string Calc(IEnumerable<string> input, int N, bool isDebug = false)
    {
        var result = new StringBuilder();
        var s = new Pos<int>(0, 0);
        var ts = Enumerable.Repeat(s, N).ToList();
        var counts = new Dictionary<Pos<int>, int>();
        counts[s] = 1;
        var moves = new Dictionary<char, Pos<int>> { { 'R', new(1, 0) }, { 'L', new(-1, 0) }, { 'U', new(0, -1) }, { 'D', new(0, 1) } };
        var box = new Box<int>(new Pos<int>(0, -4), new Pos<int>(4, 0));
        void Print(List<Pos<int>> ts)
        {
            if (!isDebug) return;
            for (int y = -16; y <= 12; y++)
            {
                var line = new StringBuilder();
                for (int x = -12; x <= 12; x++)
                {
                    var p = new Pos<int>(x, y);
                    if (p == ts.First())
                    {
                        line.Append('H');
                    }
                    else
                    {
                        var found = false;
                        var index = 1;
                        foreach (var t in ts.Skip(1))
                        {
                            if (p == t)
                            {
                                found = true;
                                line.Append(index);
                                break;
                            }
                            index++;
                        }
                        if (!found)
                        {
                            if (p == s)
                            {
                                line.Append('s');
                            } else
                            {
                                line.Append('.');
                            }
                        }
                    }
                }
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
        var dts = new List<Pos<int>>() {
                            new(0, 1),
                            new(1, 0),
                            new(1, 1),
                            new(0, -1),
                            new(-1, 0),
                            new(-1, -1),
                            new(1, -1),
                            new(-1, 1)
        };
        Print(ts);
        foreach (var line in input)
        {
            if (isDebug) Console.WriteLine(line);
            var (moveStr, moveCountStr) = line.Split();
            var moveCount = int.Parse(moveCountStr);
            var move = moves[moveStr[0]];
            for (int i = 0; i < moveCount; i++)
            {
                ts[0] += move;
                for (var j = 1; j < ts.Count; j++)
                {
                    var t = ts[j];
                    var h = ts[j - 1];
                    if (!t.Adjacent(h))
                    {
                        var candidates = new List<(int, Pos<int>)>();
                        foreach (var dt in dts)
                        {
                            var nt = t + dt;
                            if (nt.Adjacent(h))
                            {
                                candidates.Add((h.Manhattan(nt), nt));
                            }
                        }
                        t = candidates.OrderBy(x => x.Item1).First().Item2;
                        if (j == ts.Count - 1)
                        {
                            counts.TryGetValue(t, out int count);
                            count += 1;
                            counts[t] = count;
                        }
                        ts[j] = t;
                    }
                }

            }
            Print(ts);
        }
        if (isDebug)
        {
            foreach (var c in counts)
            {
                Console.WriteLine($"{c.Key}: {c.Value}");
            }
        }
        return counts.Count.ToString();
    }


    [TestMethod]
    public void Day09_Part1_Example01()
    {
        var input = """
            R 4
            U 4
            L 3
            D 1
            R 4
            D 1
            L 5
            R 2
            """;
        var result = Calc(Common.GetLines(input), 2);
        Assert.AreEqual("13", result);
    }
    
    [TestMethod]
    public void Day09_Part1()
    {
        var result = Calc(Common.DayInput(nameof(Day09)), 2);
        Assert.AreEqual("6023", result);
    }
    
    [TestMethod]
    public void Day09_Part2_Example01()
    {
        var input = """
            R 4
            U 4
            L 3
            D 1
            R 4
            D 1
            L 5
            R 2
            """;
        var result = Calc(Common.GetLines(input), 10);
        Assert.AreEqual("1", result);
    }
    
    [TestMethod]
    public void Day09_Part2_Example02()
    {
        var input = """
            R 5
            U 8
            L 8
            D 3
            R 17
            D 10
            L 25
            U 20
            """;
        var result = Calc(Common.GetLines(input), 10);
        Assert.AreEqual("36", result);
    }
    
    [TestMethod]
    public void Day09_Part2()
    {
        var result = Calc(Common.DayInput(nameof(Day09)), 10);
        Assert.AreEqual("2533", result);
    }
    
}
