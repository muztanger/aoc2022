namespace Advent_of_Code_2022;

[TestClass]
public class Day01
{
    private static string Part1(IEnumerable<string> input)
    {
        var elfs = new List<long>();
        var sum = 0L;
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                elfs.Add(sum);
                sum = 0L;
            }
            else
            {
                sum += long.Parse(line);
            }
        }
        elfs.Add(sum);
        return elfs.Max().ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var elfs = new List<long>();
        var sum = 0L;
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                elfs.Add(sum);
                sum = 0L;
            }
            else
            {
                sum += long.Parse(line);
            }
        }
        elfs.Add(sum);
        return elfs.Select(x => x).OrderByDescending(x => x).Take(3).Sum().ToString();
    }
    
    [TestMethod]
    public void Day01_Part1_Example01()
    {
        var input = """
            1000
            2000
            3000

            4000

            5000
            6000

            7000
            8000
            9000

            10000
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day01_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day01_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day01)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day01_Part2_Example01()
    {
        var input = """
            1000
            2000
            3000
            
            4000
            
            5000
            6000
            
            7000
            8000
            9000
            
            10000
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day01_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day01_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day01)));
        Assert.AreEqual("", result);
    }
    
}
