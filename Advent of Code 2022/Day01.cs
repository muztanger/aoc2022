namespace Advent_of_Code_2022;

[TestClass]
public class Day01
{
    private long Calc(IEnumerable<string> input, int n)
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
        return elfs.OrderByDescending(x => x).Take(n).Sum();
    }
    
    private readonly string _example = """
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

    [TestMethod]
    public void Day01_Part1_Example01()
    {
        var result = Calc(Common.GetLines(_example), 1);
        Assert.AreEqual(24000L, result);
    }
    
    [TestMethod]
    public void Day01_Part1()
    {
        var result = Calc(Common.DayInput(nameof(Day01)), 1);
        Assert.AreEqual(72017L, result);
    }
    
    [TestMethod]
    public void Day01_Part2_Example01()
    {
        var result = Calc(Common.GetLines(_example), 3);
        Assert.AreEqual(45000L, result);
    }
    
    [TestMethod]
    public void Day01_Part2()
    {
        var result = Calc(Common.DayInput(nameof(Day01)), 3);
        Assert.AreEqual(212520L, result);
    }
}
