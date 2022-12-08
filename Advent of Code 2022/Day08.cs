using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks.Dataflow;

namespace Advent_of_Code_2022;

[TestClass]
public class Day08
{
    [Flags]
    enum Visible
    {
        None =   0b0000,
        Left =   0b0001,
        Right =  0b0010,
        Top =    0b0100,
        Bottom = 0b1000,
    }

    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var visible = new List<List<Visible>>();
        var grid = new List<List<int>>();
        foreach (var line in input)
        {
            grid.Add(line.Select(x => int.Parse(x.ToString())).ToList());
        }
        var N = grid[0].Count;
        foreach (var line in grid)
        {
            visible.Add(Enumerable.Repeat(Visible.None, N).ToList());
        }
        // check from left
        for (int j = 0; j < N; j++)
        {
            var x = -1;
            for (int i = 0; i < N; i++)
            {
                if (grid[j][i] > x)
                {
                    x = grid[j][i];
                    visible[j][i] = Visible.Left;
                }
            }
        }
        // check from right
        for (int j = 0; j < N; j++)
        {
            var x = -1;
            for (int i = N - 1; i >=0; i--)
            {
                if (grid[j][i] > x)
                {
                    x = grid[j][i];
                    visible[j][i] |= Visible.Right;
                }
            }
        }
        // check from top
        for (int i = 0; i < N; i++)
        {
            var x = -1;
            for (int j = 0; j < N; j++)
            {
                if (grid[j][i] > x)
                {
                    x = grid[j][i];
                    visible[j][i] |= Visible.Top;
                }
            }
        }
        // check from bottom
        for (int i = 0; i < N; i++)
        {
            var x = -1;
            for (int j = N-1; j >= 0; j--)
            {
                if (grid[j][i] > x)
                {
                    x = grid[j][i];
                    visible[j][i] |= Visible.Bottom;
                }
            }
        }
        var sum = 0;
        foreach (var line in visible)
        {
            sum += line.Where(x => x != Visible.None).Count();
            Console.WriteLine(string.Join("|", line));
        }
        return sum.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var visible = new List<List<Visible>>();
        var grid = new List<List<long>>();
        foreach (var line in input)
        {
            grid.Add(line.Select(x => long.Parse(x.ToString())).ToList());
        }
        var N = grid[0].Count;
        var maximest = 0L;
        var maxipos = new Pos<int>(int.MinValue, int.MinValue);
        for (var j = 0; j < N; j++)
        {
            for (var i = 0; i < N; i++)
            {
                long LookRight()
                {
                    var x = grid[j][i];
                    var isValley = true;
                    var max = x;
                    var count = 0L;
                    for (var k = i + 1; k < N; k++)
                    {
                        var y = grid[j][k];
                        CheckTrees(ref isValley, max, ref count, y);
                        if (!isValley) break;
                        max = Math.Max(max, y);
                    }
                    return count;
                }

                long LookLeft()
                {
                    var x = grid[j][i];
                    var max = x;
                    var count = 0L;
                    var isValley = true;
                    for (var k = i - 1; k >= 0; k--)
                    {
                        var y = grid[j][k];
                        CheckTrees(ref isValley, max, ref count, y);
                        if (!isValley) break;
                        max = Math.Max(max, y);
                    }
                    return count;
                }

                long LookDown()
                {
                    var x = grid[j][i];
                    var max = x;
                    var count = 0L;
                    var isValley = true;
                    for (var k = j + 1; k < N; k++)
                    {
                        var y = grid[k][i];
                        CheckTrees(ref isValley, max, ref count, y);
                        if (!isValley) break;
                        max = Math.Max(max, y);
                    }
                    return count;
                }

                long LookUp()
                {
                    var x = grid[j][i];
                    var max = x;
                    var count = 0L;
                    var isValley = true;
                    for (var k = j - 1; k >= 0; k--)
                    {
                        var y = grid[k][i];
                        CheckTrees(ref isValley, max, ref count, y);
                        if (!isValley) break;
                        max = Math.Max(max, y);
                    }
                    return count;
                }

                var score = LookLeft() * LookRight() * LookDown() * LookUp();
                Console.WriteLine($"i,j={i},{j}: grid={grid[j][i]} score={score}: left={LookLeft()} * right={LookRight()} * down={LookDown()} * up={LookUp()}");
                if (score > maximest)
                {
                    maxipos.x = i;
                    maxipos.y = j;
                    maximest= score;
                }
            }
        }
        //var sum = 0;
        //foreach (var line in visible)
        //{
        //    sum += line.Where(x => x != Visible.None).Count();
        //    Console.WriteLine(string.Join("|", line));
        //}
        Console.WriteLine($"maximest={maximest} maxipos={maxipos}");
        return maximest.ToString();
    }

    private static void CheckTrees(ref bool isValley, long max, ref long count, long y)
    {
        if (isValley)
        {
            count++;
            if (y >= max)
            {
                isValley = false;
            }
        }
        else if (y >= max)
        {
            count++;
        }
    }

    [TestMethod]
    public void Day08_Part1_Example01()
    {
        var input = """
            30373
            25512
            65332
            33549
            35390
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("21", result);
    }
    
    [TestMethod]
    public void Day08_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day08)));
        Assert.AreEqual("1684", result);
    }
    
    [TestMethod]
    public void Day08_Part2_Example01()
    {
        var input = """
            30373
            25512
            65332
            33549
            35390
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("8", result);
    }
    
    [TestMethod]
    public void Day08_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day08)));
        Assert.AreNotEqual("1135260", result);
        Assert.AreNotEqual("1500165", result);
        Assert.AreNotEqual("1054170", result);
        Assert.AreEqual("486540", result);
    }
    
}
