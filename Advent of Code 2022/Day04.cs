namespace Advent_of_Code_2022;

[TestClass]
public partial class Day04
{
    static bool IsFullyContain(Pos<int> p1, Pos<int> p2)
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

    static bool IsOverlap(Pos<int> p1, Pos<int> p2)
    {
        var x = Math.Max(p1.x, p2.x);
        var y = Math.Min(p1.y, p2.y);
        return x <= y;
    }

    private static string Calc(IEnumerable<string> input, bool isPart1)
    {
        var re = LineRegex();
        var result = 0;
        foreach (var line in input)
        {
            var (elf1, elf2) = line.Split(',');

            Pos<int> GetPos(string input)
            {
                var match = re.Match(input);
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                return new Pos<int>(x, y);
            }
            Pos<int> pos1 = GetPos(elf1);
            Pos<int> pos2 = GetPos(elf2);

            if (isPart1 && IsFullyContain(pos1, pos2))
            {
                result++;
            }
            else if (!isPart1 && IsOverlap(pos1, pos2))
            {
                result++;
            }
        }
        return result.ToString();
    }


    private readonly string _example = """
        2-4,6-8
        2-3,4-5
        5-7,7-9
        2-8,3-7
        6-6,4-6
        2-6,4-8
        """;

    [TestMethod]
    public void Day04_Part1_Example01()
    {
        var result = Calc(Common.GetLines(_example), isPart1: true);
        Assert.AreEqual("2", result);    
    }
    
    [TestMethod]
    public void Day04_Part1()
    {
        var result = Calc(Common.DayInput(nameof(Day04)), isPart1: true);
        Assert.AreEqual("605", result);
    }
    
    [TestMethod]
    public void Day04_Part2_Example01()
    {
        var result = Calc(Common.GetLines(_example), false);
        Assert.AreEqual("4", result);
    }
    
    [TestMethod]
    public void Day04_Part2()
    {
        var result = Calc(Common.DayInput(nameof(Day04)), false);
        Assert.AreEqual("914", result);
    }

    [GeneratedRegex("([0-9]+)-([0-9]+)")]
    private static partial Regex LineRegex();
}
