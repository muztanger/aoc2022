namespace Advent_of_Code_2022;

[TestClass]
public class Day18
{
    private static string Part1(IEnumerable<string> input)
    {
        var cubes = new HashSet<Pos3<int>>();
        foreach (var line in input)
        {
            var (x, y, z) = line.Split(',').Select(i => int.Parse(i)).ToArray();
            cubes.Add(new Pos3<int>(x, y, z));
        }
        var adj = new List<Pos3<int>>()
        {
            new(0,0,1),
            new(0,1,0),
            new(1,0,0),
            new(0,0,-1),
            new(0,-1,0),
            new(-1,0,0)
        };
        
        var result = 0;
        foreach (var cube in cubes)
        {
            foreach (var dt in adj)
            {
                if (!cubes.Contains(cube + dt))
                {
                    result++;
                }
            }
        }

        return result.ToString();
    }

    private static string Part2(IEnumerable<string> input)
    {
        var cubes = new HashSet<Pos3<int>>();
        foreach (var line in input)
        {
            var (x, y, z) = line.Split(',').Select(i => int.Parse(i)).ToArray();
            cubes.Add(new Pos3<int>(x, y, z));
        }
        var box = new Box3<int>(cubes.ToArray());

        var adj = new List<Pos3<int>>()
        {
            new(1,0,0),
            new(0,1,0),
            new(0,0,1),
            new(-1,0,0),
            new(0,-1,0),
            new(0,0,-1),
        };

        var count = 0;
        var path = new List<Pos3<int>>();
        foreach (var cube in cubes)
        {
            foreach (var dp in adj)
            {
                Pos3<int> p = cube + dp;
                if (!cubes.Contains(p) && !IsTrapped(p, ref path))
                {
                    count++;
                }
                path.Clear();
            }
        }

        bool IsTrapped(Pos3<int> p, ref List<Pos3<int>> path)
        {
            if (!box.IsInside(p)) return false;

            path.Add(p);

            foreach (var dp in adj)
            {
                var q = p + dp;
                if (!cubes.Contains(q) && !path.Contains(q))
                {
                    if (!IsTrapped(q, ref path)) return false;
                }
            }
            return true;
        }

        return count.ToString();
    }

    [TestMethod]
    public void Day18_Part1_Example01()
    {
        var input = """
            1,1,1
            2,1,1
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("10", result);
    }
    
    [TestMethod]
    public void Day18_Part1_Example02()
    {
        var input = """
            2,2,2
            1,2,2
            3,2,2
            2,1,2
            2,3,2
            2,2,1
            2,2,3
            2,2,4
            2,2,6
            1,2,5
            3,2,5
            2,1,5
            2,3,5
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("64", result);
    }
    
    [TestMethod]
    public void Day18_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day18)));
        Assert.AreEqual("4628", result);
    }
    
    [TestMethod]
    public void Day18_Part2_Example01()
    {
        var input = """
            2,2,2
            1,2,2
            3,2,2
            2,1,2
            2,3,2
            2,2,1
            2,2,3
            2,2,4
            2,2,6
            1,2,5
            3,2,5
            2,1,5
            2,3,5
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("58", result);
    }
    
    [TestMethod]
    public void Day18_Part2_Example02()
    {
        var input = """
            1,1,1
            2,1,1
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("10", result);
    }

    [TestMethod]
    public void Day18_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day18)));
        Assert.AreNotEqual("2572", result); // answer for someone else??
        Assert.AreEqual("2582", result);
    }
    
}
