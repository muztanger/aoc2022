using System.Security.AccessControl;

namespace Advent_of_Code_2022;

[TestClass]
public class Day12
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var grid = new List<List<int>>();
        var start = new Pos<int>(0, 0);
        var end = new Pos<int>(0, 0);
        var y = 0;
        foreach (var line in input)
        {
            List<int> row = new List<int>();
            grid.Add(row);
            var x = 0;
            foreach (var c in line)
            {
                if (c == 'S') // startPos
                {
                    start = new Pos<int>(x, y);
                    row.Add(0);
                }
                else if (c == 'E') // endPos
                {
                    end = new Pos<int>(x, y);
                    row.Add('z' - 'a');
                }
                else
                {
                    row.Add(c - 'a');
                }
                x++;
            }
            y++;
        }

        List<Pos<int>> path = new List<Pos<int>>();
        List<List<bool>> visited = Enumerable.Repeat(Enumerable.Repeat(false, grid.First().Count).ToList(), grid.Count).ToList();
        var z = ShortestPath(grid, start, end, ref path, ref visited);

        PrintGrid(grid, start, end);

        return z.ToString();
    }

    private static List<Pos<int>> walks = new List<Pos<int>> { new(0, 1), new(0, 1), new(-1, 0), new(0, -1) };
    private static int ShortestPath(List<List<int>> grid, Pos<int> start, Pos<int> end, ref List<Pos<int>> path, ref List<List<bool>> visited)
    {
        if (start == end)
        {
            return path.Count;
        }

        var min = int.MaxValue;
        foreach (var dp in walks)
        {
            var p = start + dp;
            if (p.x >= 0 && p.x < grid.First().Count && p.y >= 0 && p.y < grid.Count)
            {
                if (!visited[p.y][p.x] && grid[start.y][start.x] - grid[p.y][p.x] >= -1)
                {
                    //visited[p.y][p.x] = true;
                    var pPath = new List<Pos<int>>(path);
                    pPath.Add(p);
                    min = Math.Min(min, ShortestPath(grid, p, end, ref pPath, ref visited));
                }
            }
        }
        return min;
    }

    private static void PrintGrid(List<List<int>> grid, Pos<int> start, Pos<int> end)
    {
        int y;
        var tostr = new StringBuilder();
        y = 0;
        foreach (var row in grid)
        {
            var x = 0;
            if (y != 0)
            {
                tostr.AppendLine();
            }
            foreach (var c in row)
            {
                Pos<int> p = new Pos<int>(x, y);
                if (start == p)
                {
                    tostr.Append('S');
                }
                else if (end == p)
                {
                    tostr.Append('E');
                }
                else
                {
                    tostr.Append(Convert.ToChar('a' + c));
                }
                x++;
            }
            y++;
        }
        Console.WriteLine(tostr.ToString());
    }

    private static string Part2(IEnumerable<string> input)
    {
        return "EEK";
    }
    
    [TestMethod]
    public void Day12_Part1_Example01()
    {
        var input = """
            Sabqponm
            abcryxxl
            accszExk
            acctuvwj
            abdefghi
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual(input, result);
    }
    
    [TestMethod]
    public void Day12_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day12_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day12)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day12_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day12_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day12_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day12)));
        Assert.AreEqual("", result);
    }
    
}
