namespace Advent_of_Code_2022;

[TestClass]
public class Day09
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var h = new Pos<int>(0, 0);
        var t = new Pos<int>(0, 0);
        var counts = new Dictionary<Pos<int>, int>();
        counts[h] = 1;
        var dir = new Dictionary<char, Pos<int>> { { 'R', new(1, 0) }, { 'L', new(-1, 0) }, { 'U', new(0, -1) }, { 'D', new(0, 1) } };
        var box = new Box<int>(new Pos<int>(0, -4), new Pos<int>(4, 0));
        void Print(Pos<int> h, Pos<int> t)
        {
            for (int y = -5; y <= 5; y++)
            {
                var line = new StringBuilder();
                for (int x = -5; x <= 5; x++)
                {
                    var p = new Pos<int>(x, y);
                    if (p == h)
                    {
                        line.Append('H');
                    }
                    else if (p == t)
                    {
                        line.Append('T');
                    }
                    else
                    {
                        line.Append('.');
                    }
                }
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
        Print(h, t);
        foreach (var line in input)
        {
            Console.WriteLine(line);
            var (moveStr, moveCountStr) = line.Split();
            var moveCount = int.Parse(moveCountStr);
            var move = dir[moveStr[0]];
            for (int i = 0; i < moveCount; i++)
            {
                h += move;
                if (!t.Adjacent(h))
                {
                    var candidates = new List<(int, Pos<int>)>();
                    foreach (var dt in new List<Pos<int>>() {
                        new(0, 1),
                        new(1, 0),
                        new(1, 1),
                        new(0, -1),
                        new(-1, 0),
                        new(-1, -1),
                        new(1, -1),
                        new(-1, 1),
                    })
                    {
                        var nt = t + dt;
                        if (nt.Adjacent(h))
                        {
                            candidates.Add((h.Manhattan(nt), nt));
                        }
                    }
                    t = candidates.OrderBy(x => x.Item1).First().Item2;
                    counts.TryGetValue(t, out int count);
                    count += 1;
                    counts[t] = count;
                }
                Print(h, t);
            }
        }
        return counts.Count.ToString();
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
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("meep", result);
    }
    
    [TestMethod]
    public void Day09_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day09_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day09)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day09_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day09_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day09_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day09)));
        Assert.AreEqual("", result);
    }
    
}
