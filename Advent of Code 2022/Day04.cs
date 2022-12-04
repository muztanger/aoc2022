using System.Text.RegularExpressions;

namespace Advent_of_Code_2022;

[TestClass]
public class Day04
{
    static bool OneFullyContainsOther(Pos<int> p1, Pos<int> p2)
    {
        if (p1.x <= p2.x && p1.y >= p2.y)
        {
            return true;
        }
        if (p2.x <= p1.x && p2.y >= p1.y)
        {
            return true;
        }
        return false;
    }

    static bool Overlap(Pos<int> p1, Pos<int> p2)
    {
        var x = Math.Max(p1.x, p2.x);
        var y = Math.Min(p1.y, p2.y);
        var result = x <= y;
        Console.WriteLine($"p1={p1} p2={p2} result={result}");
        return result;
    }

    private static string Part1(IEnumerable<string> input)
    {
        var re = new Regex(@"([0-9]+)-([0-9]+)");
        var result = 0;
        foreach (var line in input)
        {
            var split = line.Split(',');
            var m1 = re.Match(split[0]);
            var x1 = int.Parse(m1.Groups[1].Value);
            var y1 = int.Parse(m1.Groups[2].Value);
            var pos1 = new Pos<int>(x1, y1);
            var m2 = re.Match(split[1]);
            var x2 = int.Parse(m2.Groups[1].Value);
            var y2 = int.Parse(m2.Groups[2].Value);
            var pos2 = new Pos<int>(x2, y2);
            if (OneFullyContainsOther(pos1, pos2))
            {
                result++;
            }

        }
        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var re = new Regex(@"([0-9]+)-([0-9]+)");
        var result = 0;
        foreach (var line in input)
        {
            var split = line.Split(',');
            var m1 = re.Match(split[0]);
            var x1 = int.Parse(m1.Groups[1].Value);
            var y1 = int.Parse(m1.Groups[2].Value);
            var pos1 = new Pos<int>(x1, y1);
            var m2 = re.Match(split[1]);
            var x2 = int.Parse(m2.Groups[1].Value);
            var y2 = int.Parse(m2.Groups[2].Value);
            var pos2 = new Pos<int>(x2, y2);
            if (Overlap(pos1, pos2))
            {
                result++;
            }

        }
        return result.ToString();
    }
    
    [TestMethod]
    public void Day04_Part1_Example01()
    {
        var input = """
            2-4,6-8
            2-3,4-5
            5-7,7-9
            2-8,3-7
            6-6,4-6
            2-6,4-8
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);    
    }
    
    [TestMethod]
    public void Day04_Part1_Example02()
    {
        var input = """

            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day04_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day04)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day04_Part2_Example01()
    {
        var input = """
            2-4,6-8
            2-3,4-5
            5-7,7-9
            2-8,3-7
            6-6,4-6
            2-6,4-8
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day04_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day04_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day04)));
        Assert.AreNotEqual("294", result);
        Assert.AreEqual("", result);
    }
    
}
