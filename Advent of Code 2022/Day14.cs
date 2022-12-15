namespace Advent_of_Code_2022;

[TestClass]
public class Day14
{
    private static string Part1(IEnumerable<string> input)
    {
        var units = new Dictionary<Pos<int>, char>();

        var start = new Pos<int>(500, 0);
        units[start] = '+';

        var area = new Box<int>(start, start);
        foreach (var line in input)
        {
            var split = line.Split(' ');
            Pos<int>? last = null;
            for (int i = 0; i < split.Length; i += 2)
            {
                var P = split[i].Split(',');
                var p = new Pos<int>(int.Parse(P[0]), int.Parse(P[1]));
                if (last is not null)
                {
                    var dp = last.Sign(p);
                    var q = last + dp;
                    while (q != p)
                    {
                        units[q] = '#';
                        area.IncreaseToPoint(q);
                        q += dp;
                    }
                }
                area.IncreaseToPoint(p);
                units[p] = '#';
                last = p;
            }
        }
        var sands = new HashSet<Pos<int>>();
        PrintCave(units, area, sands);

        var sandFall = true;
        while (sandFall)
        {

            var sand = start;
            var tries = new List<Pos<int>> { new(0, 1), new(-1, 1), new(1,1) };
            var hasMoved = true;
            while (hasMoved)
            {
                hasMoved = false;
                foreach (var dp in tries)
                {
                    var q = sand + dp;
                    if (sands.Contains(q))
                    {
                        continue;
                    }
                    if (units.ContainsKey(q))
                    {
                        continue;
                    }
                    sand = q;
                    hasMoved = true;
                    break;
                }
                if (sand.y > area.Max.y)
                {
                    // into the abyss!
                    sandFall = false;
                    break;
                }
            }
            sands.Add(sand);

            //Console.WriteLine();
            //Console.WriteLine("Units of sand: " + sands.Count);
            //PrintCave(units, area, sands);
        }

        PrintCave(units, area, sands);

        return (sands.Count - 1).ToString();
    }

    private static void PrintCave(Dictionary<Pos<int>, char> units, Box<int> area, HashSet<Pos<int>> sands)
    {
#if DEBUG
        var cave = new StringBuilder();
        for (int y = area.Min.y; y <= area.Max.y; y++)
        {
            if (y != area.Min.y)
            {
                cave.AppendLine();
            }
            for (int x = area.Min.x; x <= area.Max.x; x++)
            {
                var p = new Pos<int>(x, y);
                if (units.TryGetValue(p, out var c))
                {
                    cave.Append(c);
                }
                else if (sands.Contains(p))
                {
                    cave.Append('o');
                }
                else
                {
                    cave.Append('.');
                }
            }
        }
        Console.WriteLine(cave);
#endif
    }

    private static string Part2(IEnumerable<string> input)
    {
        var units = new Dictionary<Pos<int>, char>();

        var start = new Pos<int>(500, 0);
        units[start] = '+';

        var area = new Box<int>(start, start);
        foreach (var line in input)
        {
            var split = line.Split(' ');
            Pos<int>? last = null;
            for (int i = 0; i < split.Length; i += 2)
            {
                var P = split[i].Split(',');
                var p = new Pos<int>(int.Parse(P[0]), int.Parse(P[1]));
                if (last is not null)
                {
                    var dp = last.Sign(p);
                    var q = last + dp;
                    while (q != p)
                    {
                        units[q] = '#';
                        area.IncreaseToPoint(q);
                        q += dp;
                    }
                }
                area.IncreaseToPoint(p);
                units[p] = '#';
                last = p;
            }
        }
        var sands = new HashSet<Pos<int>>();
        PrintCave(units, area, sands);

        var floor = area.Max.y + 1;

        var sandFall = true;
        while (sandFall)
        {

            var sand = start;
            var tries = new List<Pos<int>> { new(0, 1), new(-1, 1), new(1, 1) };
            var hasMoved = true;
            var moveCount = 0;
            while (hasMoved)
            {
                hasMoved = false;
                foreach (var dp in tries)
                {
                    var q = sand + dp;
                    if (sands.Contains(q))
                    {
                        continue;
                    }
                    if (units.ContainsKey(q))
                    {
                        continue;
                    }
                    if (sand.y == floor)
                    {
                        // hit the floor
                        break;
                    }
                    sand = q;
                    hasMoved = true;
                    moveCount++;
                    break;
                }
            }
            sands.Add(sand);
            area.IncreaseToPoint(sand);

            if (sand == start)
            {
                // Now it is safe!
                sandFall = false;
            }
            //Console.WriteLine();
            //Console.WriteLine("Units of sand: " + sands.Count);
            //PrintCave(units, area, sands);
        }

        PrintCave(units, area, sands);

        return (sands.Count).ToString();
    }


    [TestMethod]
    public void Day14_Part1_Example01()
    {
        var input = """
            498,4 -> 498,6 -> 496,6
            503,4 -> 502,4 -> 502,9 -> 494,9
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("24", result);
    }
    
    [TestMethod]
    public void Day14_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day14)));
        Assert.AreEqual("913", result);
    }
    
    [TestMethod]
    public void Day14_Part2_Example01()
    {
        var input = """
            498,4 -> 498,6 -> 496,6
            503,4 -> 502,4 -> 502,9 -> 494,9
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("93", result);
    }
    
    [TestMethod]
    public void Day14_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day14)));
        Assert.AreEqual("30762", result);
    }
    
}
